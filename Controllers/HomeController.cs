using EVA3_MVC_AGENCIA.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EVA3_MVC_AGENCIA.Areas.Executives.Models;
using EVA3_MVC_AGENCIA.Library;
using EVA3_MVC_AGENCIA.Data;
using EVA3_MVC_AGENCIA.Areas.Principal.Controllers;

namespace EVA3_MVC_AGENCIA.Controllers
{
    public class HomeController : Controller
    {
        //IServiceProvider _serviceProvider;
        private static InputModelLogin _model;
        private LExecutive _executive;
        private SignInManager<IdentityUser> _signInManager;

        public HomeController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IServiceProvider serviceProvider)
        {
            //_serviceProvider = serviceProvider;
            _signInManager = signInManager;
            _executive = new LExecutive(userManager, signInManager, roleManager, context);
        }

        public async Task<IActionResult> Index()
        {
            //await CreateRolesAsync(_serviceProvider);
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(PrincipalController.Principal), "Principal");
            }
            else
            {
                if (_model != null)
                {
                    return View(_model);
                }
                else
                {
                    return View();
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> Index(InputModelLogin model)
        {
            _model = model;
            if (ModelState.IsValid)
            {
                var result = await _executive.ExecutiveLoginAsync(model);
                if (result.Succeeded)
                {
                    return Redirect("/Principal/Principal");
                }
                else
                {
                    _model.ErrorMessage = "Correo o contraseña inválidos.";
                    return Redirect("/");
                }
            }
            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _model.ErrorMessage = error.ErrorMessage;
                    }
                }
                return Redirect("/");
            }

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private async Task CreateRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            String[] rolesName = { "Admin", "Senior", "Junior", "Jefe"};
            foreach (var item in rolesName)
            {
                var roleExist = await roleManager.RoleExistsAsync(item);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(item));
                }
            }
        }
    }
}
