using EVA3_MVC_AGENCIA.Areas.Executives.Models;
using EVA3_MVC_AGENCIA.Controllers;
using EVA3_MVC_AGENCIA.Data;
using EVA3_MVC_AGENCIA.Library;
using EVA3_MVC_AGENCIA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVA3_MVC_AGENCIA.Areas.Executives.Controllers
{
    [Area("Executives")]
    public class ExecutivesController : Controller
    {
        private SignInManager<IdentityUser> _signInManager;
        private LExecutive _executive;
        private static DataPaginador<InputModelRegister> models;

        public ExecutivesController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _executive = new LExecutive(userManager, signInManager, roleManager, context);
        }
        public IActionResult Executives(int id, String filtrar, int registros)
        {
            if (_signInManager.IsSignedIn(User))
            {
                Object[] objects = new Object[3];
                var data = _executive.getTExecutivesAsync(filtrar, 0);
                if (0 < data.Result.Count)
                {
                    var url = Request.Scheme + "://" + Request.Host.Value;
                    objects = new LPaginador<InputModelRegister>().paginador(data.Result,
                        id, registros, "Executives", "Executives", "Executives", url);
                }
                else
                {
                    objects[0] = "No hay datos que mostrar";
                    objects[1] = "No hay datos que mostrar";
                    objects[2] = new List<InputModelRegister>();
                }
                models = new DataPaginador<InputModelRegister>
                {
                    List = (List<InputModelRegister>)objects[2],
                    Pagi_info = (String)objects[0],
                    Pagi_navegacion = (String)objects[1],
                    Input = new InputModelRegister(),
                };
                return View(models);

            }
            else
            {
                return Redirect("/");
            }

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
