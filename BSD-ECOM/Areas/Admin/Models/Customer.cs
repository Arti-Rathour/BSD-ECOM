using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSD_ECOM.Areas.Admin.Models
{
    public class Customer
    {
        public int reg_id { get; set; }
        public string first_name { get; set; }
        public string email { get; set; }
        public string MobileNo { get; set; }
        public string StateID { get; set; }
        public string CityID { get; set; }
        public string dob { get; set; }
        public bool status { get; set; }
        public string password { get; internal set; }
    }
}
