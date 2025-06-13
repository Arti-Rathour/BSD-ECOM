using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BSD_ECOM.Areas.Admin.Models
{
    public class MStCMSBanner_srv
    {
        public int ID { get; set; }
        public int CompanyId { get; set; }
        public string TBanner1 { get; set; }
        public string TBanner2 { get; set; }
        public string TBanner3 { get; set; }
        public string FBanner1 { get; set; }
        public string FBanner2 { get; set; }
        public string FBanner3 { get; set; }
        public string HBanner { get; set; }
        public int ENtryUser { get; set; }
        public bool Status { get; set; }
        public string TUrl1 { get; set; }
        public string TUrl2 { get; set; }
        public string TUrl3 { get; set; }
        public string FUrl1 { get; set; }
        public string FUrl2 { get; set; }
        public string FUrl3 { get; set; }
        public string MUrl { get; set; }
        public int companyId { get; set; }
        public string category_banner { get; set; }
        public string category_bannerUrl { get; set; }

        public IFormFile filenmes1 { get; set; }
        public IFormFile filenmes2 { get; set; }
        public IFormFile filenmes3 { get; set; }
        public IFormFile filenmes4 { get; set; }
        public IFormFile filenmes5 { get; set; }
        public IFormFile filenmes6 { get; set; }
    }




  
}
