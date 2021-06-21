using EVA3_MVC_AGENCIA.Areas.Executives.Models;
using EVA3_MVC_AGENCIA.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVA3_MVC_AGENCIA.Library
{
    public class LExecutive : ListObject
    {
        public LExecutive(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _executivesRole = new LExecutivesRoles();
        }
        public async Task<List<InputModelRegister>> getTExecutivesAsync(String valor, int id)
        {
            List<TExecutives> listUser;
            List<SelectListItem> _listRoles;
            List<InputModelRegister> userList = new List<InputModelRegister>();
            if (valor == null && id.Equals(0))
            {
                listUser = _context.TExecutives.ToList();
            }
            else
            {
                if (id.Equals(0))
                {
                    listUser = _context.TExecutives.Where(u => u.NID.StartsWith(valor) || u.Name.StartsWith(valor) ||
              u.LastName.StartsWith(valor) || u.Email.StartsWith(valor)).ToList();
                }
                else
                {
                    listUser = _context.TExecutives.Where(u => u.ID.Equals(id)).ToList();
                }
            }
            if (!listUser.Count.Equals(0))
            {
                foreach (var item in listUser)
                {
                    _listRoles = await _executivesRole.getRole(_userManager, _roleManager, item.IdUser);
                    var user = _context.Users.Where(u => u.Id.Equals(item.IdUser)).ToList().Last();
                    userList.Add(new InputModelRegister
                    {
                        Id = item.ID,
                        ID = item.IdUser,
                        NID = item.NID,
                        Name = item.Name,
                        LastName = item.LastName,
                        Email = item.Email,
                        Role = _listRoles[0].Text,
                        Image = item.Image,
                        IdentityUser = user
                    });
                    _listRoles.Clear();
                }
            }
            return userList;
        }
        internal async Task<SignInResult> ExecutiveLoginAsync(InputModelLogin model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {

            }
            return result;
        }
    }
}
