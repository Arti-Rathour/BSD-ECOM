using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSD_ECOM.Areas.Admin.Models
{
    public class DisplayItem
    {
        public int ID { get; set; }
        public int cat_id { get; set; }
        public int category_id { get; set; }
        public int GroupID { get; set; }
        public string ItemName { get; set; }

        public string SuperCategory { get; set; }

        public string Item { get; set; }
        public string Main_cat_name { get; set; }
        public string cat_name { get; set; }
        public string Status { get; set; }
        public string display { get; set; }

    }
}
