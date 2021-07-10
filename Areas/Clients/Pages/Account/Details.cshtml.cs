using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVA3_MVC_AGENCIA.Areas.Clients.Models;
using EVA3_MVC_AGENCIA.Data;
using EVA3_MVC_AGENCIA.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EVA3_MVC_AGENCIA.Areas.Clients.Pages.Account
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private LClients _client;
        public DetailsModel(
            ApplicationDbContext context)
        {
            _client = new LClients(context);
        }
        public void OnGet(int id)
        {
            var data = _client.getTClients(null, id);
            if (0 < data.Count)
            {
                Input = new InputModel
                {
                    DataClient = data.ToList().Last(),
                };
            }
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            public InputModelRegister DataClient { get; set; }
        }
    }
}
