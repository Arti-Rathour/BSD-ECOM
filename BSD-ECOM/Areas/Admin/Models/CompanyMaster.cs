using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace BSD_ECOM.Areas.Admin.Models
{
    public class CompanyMaster
    {
        public int Comp_Id { get; set; }
        public string printingType { get; set; }
        public string CompanyName { get; set; }
        public string domainname { get; set; }
        public string ShortName { get; set; }
        public string EmailID { get; set; }
        public string PhoneNo { get; set; }
        public string Comp_Address { get; set; }
        public string MobileNo { get; set; }
        public string CityName { get; set; }
        public string FaxNo { get; set; }
        public string GstNo { get; set; }
        public string PinCode { get; set; }
        public string webs { get; set; }
        public string gline1 { get; set; }
        public string gline2 { get; set; }
        public string TinNo { get; set; }
        public int sub_chgs_Type { get; set; }
        public string PrintingID { get; set; }
        public string Logo { get; set; }
        public IFormFile Logos { get; set; }
        public bool comp_status { get; set; }

    }
}
