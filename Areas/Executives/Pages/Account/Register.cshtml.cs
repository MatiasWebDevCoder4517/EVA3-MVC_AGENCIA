using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EVA3_MVC_AGENCIA.Areas.Executives.Models;
using EVA3_MVC_AGENCIA.Library;
using EVA3_MVC_AGENCIA.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace EVA3_MVC_AGENCIA.Areas.Executives.Pages.Account
{
    [Authorize]
    [Area("Executives")]
    public class RegisterModel : PageModel
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;
        private LExecutivesRoles _usersRole;
        private static InputModel _dataInput;
        private Uploadimage _uploadimage;
        private static InputModelRegister _dataExecutive1, _dataExecutive2;
        private IWebHostEnvironment _environment;
        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _environment = environment;
            _usersRole = new LExecutivesRoles();
            _uploadimage = new Uploadimage();
        }
        public void OnGet(int id)
        {
            _dataExecutive2 = null;
            if (id.Equals(0))
            {
                _dataExecutive2 = null;
            }
            if (_dataInput != null || _dataExecutive1 != null || _dataExecutive2 != null)
            {
                if (_dataInput != null)
                {
                    Input = _dataInput;
                    Input.rolesLista = _usersRole.getRoles(_roleManager);
                    Input.AvatarImage = null;
                }
                else
                {
                    if (_dataExecutive1 != null || _dataExecutive2 != null)
                    {
                        if (_dataExecutive2 != null)
                            _dataExecutive1 = _dataExecutive2;
                        Input = new InputModel
                        {
                            Id = _dataExecutive1.Id,
                            Name = _dataExecutive1.Name,
                            LastName = _dataExecutive1.LastName,
                            NID = _dataExecutive1.NID,
                            Email = _dataExecutive1.Email,
                            Image = _dataExecutive1.Image,
                            PhoneNumber = _dataExecutive1.IdentityUser.PhoneNumber,
                            rolesLista = getRoles(_dataExecutive1.Role),
                        };
                        if (_dataInput != null)
                        {
                            Input.ErrorMessage = _dataInput.ErrorMessage;
                        }
                    }
                }
            }
            else
            {
                Input = new InputModel
                {
                    rolesLista = _usersRole.getRoles(_roleManager)
                };
            }

            _dataExecutive2 = _dataExecutive1;
            _dataExecutive1 = null;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel : InputModelRegister
        {
            public IFormFile AvatarImage { get; set; }
            //[TempData]
            //public string ErrorMessage { get; set; }
            public List<SelectListItem> rolesLista { get; set; }
        }
        public async Task<IActionResult> OnPost(String dataExecutive)
        {
            if (dataExecutive == null)
            {
                if (_dataExecutive2 == null)
                {
                    if (User.IsInRole("Admin"))
                    {
                        if (await SaveAsync())
                        {
                            return Redirect("/Executives/Executives?area=Executives");
                        }
                        else
                        {
                            return Redirect("/Executives/Register");
                        }
                    }
                    else
                    {
                        return Redirect("/Executives/Executives?area=Executives");
                    }

                }
                else
                {
                    if (User.IsInRole("Admin"))
                    {
                        if (await UpdateAsync())
                        {
                            var url = $"/Users/Account/Details?id={_dataExecutive2.Id}";
                            _dataExecutive2 = null;
                            return Redirect(url);
                        }
                        else
                        {
                            return Redirect("/Executives/Register");
                        }
                    }
                    else
                    {
                        return Redirect("/Executives/Executives?area=Executives");
                    }

                }

            }
            else
            {
                _dataExecutive1 = JsonConvert.DeserializeObject<InputModelRegister>(dataExecutive);
                return Redirect("/Executives/Register?id=1");
            }

        }

        private async Task<bool> SaveAsync()
        {
            _dataInput = Input;
            var valor = false;
            if (ModelState.IsValid)
            {
                var executiveList = _userManager.Users.Where(u => u.Email.Equals(Input.Email)).ToList();
                if (executiveList.Count.Equals(0))
                {
                    var strategy = _context.Database.CreateExecutionStrategy();
                    await strategy.ExecuteAsync(async () => {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                var executive = new IdentityUser
                                {
                                    UserName = Input.Email,
                                    Email = Input.Email,
                                    PhoneNumber = Input.PhoneNumber
                                };
                                var result = await _userManager.CreateAsync(executive, Input.Password);
                                if (result.Succeeded)
                                {
                                    await _userManager.AddToRoleAsync(executive, Input.Role);
                                    var dataExecutive = _userManager.Users.Where(u => u.Email.Equals(Input.Email)).ToList().Last();
                                    var imageByte = await _uploadimage.ByteAvatarImageAsync(
                                        Input.AvatarImage, _environment, "images/images/mrRobot_avatar.png");
                                    var t_executive = new TExecutives
                                    {
                                        Name = Input.Name,
                                        LastName = Input.LastName,
                                        NID = Input.NID,
                                        Email = Input.Email,
                                        IdUser = dataExecutive.Id,
                                        Image = imageByte,
                                    };
                                    await _context.AddAsync(t_executive);
                                    _context.SaveChanges();

                                    transaction.Commit();
                                    _dataInput = null;
                                    valor = true;
                                }
                                else
                                {
                                    foreach (var item in result.Errors)
                                    {
                                        _dataInput.ErrorMessage = item.Description;
                                    }
                                    valor = false;
                                    transaction.Rollback();
                                }
                            }
                            catch (Exception ex)
                            {
                                _dataInput.ErrorMessage = ex.Message;
                                transaction.Rollback();
                                valor = false;
                            }
                        }
                    });
                }
                else
                {
                    _dataInput.ErrorMessage = $"El {Input.Email} ya esta registrado";
                    valor = false;
                }
            }
            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _dataInput.ErrorMessage += error.ErrorMessage;
                    }
                }
                valor = false;
            }

            return valor;
        }
        private List<SelectListItem> getRoles(String role)
        {
            List<SelectListItem> rolesLista = new List<SelectListItem>();
            rolesLista.Add(new SelectListItem
            {
                Text = role
            });
            var roles = _usersRole.getRoles(_roleManager);
            roles.ForEach(item => {
                if (item.Text != role)
                {
                    rolesLista.Add(new SelectListItem
                    {
                        Text = item.Text
                    });
                }
            });
            return rolesLista;
        }
        private async Task<bool> UpdateAsync()
        {
            var valor = false;
            byte[] imageByte = null;
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () => {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var identityUser = _userManager.Users.Where(u => u.Id.Equals(_dataExecutive2.ID)).ToList().Last();
                        identityUser.UserName = Input.Email;
                        identityUser.Email = Input.Email;
                        identityUser.PhoneNumber = Input.PhoneNumber;
                        _context.Update(identityUser);
                        await _context.SaveChangesAsync();

                        if (Input.AvatarImage == null)
                        {
                            imageByte = _dataExecutive2.Image;
                        }
                        else
                        {
                            imageByte = await _uploadimage.ByteAvatarImageAsync(Input.AvatarImage, _environment, "");
                        }
                        var t_executive = new TExecutives
                        {
                            ID = _dataExecutive2.Id,
                            Name = Input.Name,
                            LastName = Input.LastName,
                            NID = Input.NID,
                            Email = Input.Email,
                            IdUser = _dataExecutive2.ID,
                            Image = imageByte,
                        };
                        _context.Update(t_executive);
                        _context.SaveChanges();
                        if (_dataExecutive2.Role != Input.Role)
                        {
                            await _userManager.RemoveFromRoleAsync(identityUser, _dataExecutive2.Role);
                            await _userManager.AddToRoleAsync(identityUser, Input.Role);
                        }
                        transaction.Commit();

                        valor = true;
                    }
                    catch (Exception ex)
                    {
                        _dataInput.ErrorMessage = ex.Message;
                        transaction.Rollback();
                        valor = false;
                    }
                }
            });
            return valor;
        }
    }
}
