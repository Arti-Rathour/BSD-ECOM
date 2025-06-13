using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BSD_ECOM.Areas.Admin.Models
{
    public class Courier
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Contact_Person { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
        public string MobileNumber { get; set; }
        public int CompanyId { get; set; }
    }
}
