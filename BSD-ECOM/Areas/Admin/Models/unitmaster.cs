using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSD_ECOM.Areas.Admin.Models
{
    public class unitmaster
    {
        public int unit_id { get; set; }
        public int company_id { get; set; }
        public int create_user { get; set; }
        public string unit_name { get; set; }
        public bool unit_status { get; set; }
    }
}
