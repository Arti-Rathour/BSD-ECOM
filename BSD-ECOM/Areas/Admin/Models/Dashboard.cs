using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSD_ECOM.Areas.Admin.Models
{
    public class Dashboard
    {
        public int Dispatchordercount { set; get; }
        public int Pendingordercount { set; get; }
        public int TotalOrderCount { set; get; }
        public int TotalDeliveredCount { set; get; }
        public int cancelordercount { set; get; }
        public int TotalReturnorder { set; get; }
        public int Inquiryordercount { set; get; }


    }
}
