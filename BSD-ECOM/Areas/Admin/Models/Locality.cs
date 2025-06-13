using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BSD_ECOM.Areas.Admin.Models
{
    public class Locality
    {
        public int Loc_id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public IFormFile Imgs { get; set; }
        public bool loc_status { get; set; }
        public string content { get; set; }

    }
}
