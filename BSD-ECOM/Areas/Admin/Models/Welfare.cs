using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSD_ECOM.Areas.Admin.Models
{
    public class Welfare
    {
        public int id { get; set; }
        public string Welfare_Name { get; set; }
        public string img { get; set; }
        public string contactNo { get; set; }
        public string Email { get; set; }
        public string content { get; set; }
        public int createruser { get; set; }
        public bool welfarestatus { get; set; }
        public string welfaremaster_name { get;set; }
        public int welmaster_id { get; set; }
    }
}
