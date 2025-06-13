using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BSD_ECOM.Areas.Admin.Models
{
    public class LocalityDetails
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string contact_name { get; set; }
        public string content { get; set; }
        public string Loc_address { get; set; }
        public string img { get; set; }
        public IFormFile imgs { get; set; }
        public int pincode { get; set; }
        public string pin { get; set; }
        public int mobile_no { get; set; }

        public string mobile_no1 { get; set; }
        public int phone { get; set; }
        public string phone1 { get; set; }
        public int Loc_id { get; set; }
        public string Loc_Name { get; set; }
        public bool loc_status { get; set; }
        public int company_id { get; set; }
        public int createuser { get; set; }
    }
}
