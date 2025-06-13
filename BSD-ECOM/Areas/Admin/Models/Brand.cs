using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSD_ECOM.Areas.Admin.Models
{
    public class Brand
    {
        public int brand_id { get; set; }
        public string brand_name { get; set; }
        public int item_cat_id { get; set; }
        public bool brand_status { get; set; }
        public int user_id { get; set; }
        public string brandImage { get; set; }
        public string company_id { get; set; }
    }
}
