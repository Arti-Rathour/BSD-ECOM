using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSD_ECOM.Models
{
    public class Cart
    {
       public int productid { get; set; }
       public string productname { get; set; }
        public int itemdetid { get; set; }
        public int unitid { get; set; }
        public string pro_desc { get; set; }
       public double productprice { get; set; }
       public string productimage { get; set; }
        public int quantity { get; set; }
        public double totalprice { get; set; }
        public string unitdesc { get; set; }
        public double shipcharge { get; set; }
        public string CategoryId { get; set; }
        public string stockqty { get; internal set; }
        // public int count { get; set; }
    }
}
