using EVA3_MVC_AGENCIA.Areas.Clients.Models;
using EVA3_MVC_AGENCIA.Data;
using EVA3_MVC_AGENCIA.Library;
using EVA3_MVC_AGENCIA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVA3_MVC_AGENCIA.Areas.Clients.Controllers
{
    [Authorize]
    [Area("Clients")]
    public class ClientsController : Controller
    {
        private LClients _customer;
        private SignInManager<IdentityUser> _signInManager;
        private static DataPaginador<InputModelRegister> models;

        public ClientsController(
           SignInManager<IdentityUser> signInManager,
           ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _customer = new LClients(context);
        }
        public IActionResult Clients(int id, String filtrar)
        {
            if (_signInManager.IsSignedIn(User))
            {
                Object[] objects = new Object[3];
                var data = _customer.getTClients(filtrar, 0);
                if (0 < data.Count)
                {
                    var url = Request.Scheme + "://" + Request.Host.Value;
                    objects = new LPaginador<InputModelRegister>().paginador(data,
                        id, 10, "Clients", "Clients", "Clients", url);
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
    }
}
