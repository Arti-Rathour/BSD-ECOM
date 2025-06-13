
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSD_ECOM.Areas.Admin.Models
{
    public class blog
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string BlogImage { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public int Status { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKeyWord { get; set; }


    }
}
