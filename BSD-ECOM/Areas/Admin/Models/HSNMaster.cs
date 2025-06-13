using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSD_ECOM.Areas.Admin.Models
{
    public class HSNMaster
    {
        public int ID { get; set; }
        public int CompanyId { get; set; }
        public int UserID { get; set; }
        public string HSNName { get; set; }
        public string HSNCode { get; set; }
        public bool Status { get; set; }
    }
}
