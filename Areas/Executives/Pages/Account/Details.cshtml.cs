using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVA3_MVC_AGENCIA.Areas.Executives.Models;
using EVA3_MVC_AGENCIA.Data;
using EVA3_MVC_AGENCIA.Library;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVA3_MVC_AGENCIA.Areas.Executives.Pages.Account
{
    public class DetailsModel : PageModel
    {
        private SignInManager<IdentityUser> _signInManager;
        private LExecutive _executive;
        public DetailsModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _executive = new LExecutive(userManager, signInManager, roleManager, context);
        }
        public void OnGet(int id)
        {
            var data = _executive.getTExecutivesAsync(null, id);
            if (0 < data.Result.Count)
            {
                Input = new InputModel
                {
                    DataExecutive = data.Result.ToList().Last(),
                };
            }
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            public InputModelRegister DataExecutive { get; set; }
        }
    }
}
