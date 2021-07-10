using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EVA3_MVC_AGENCIA.Areas.Clients.Models
{
    public class TClients
    {
        [Key]
        public int IdClient { set; get; }
        public string Nid { set; get; }
        public string Name { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string Direction { set; get; }
        public string Phone { set; get; }
        //public DateTime Date { set; get; }
        //public bool Credit { set; get; }
        public byte[] Image { get; set; }
        public List<TReports_clients> TReports_clients { get; set; }

    }
}
