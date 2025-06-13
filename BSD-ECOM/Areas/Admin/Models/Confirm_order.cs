using System.Net.NetworkInformation;

namespace BSD_ECOM.Areas.Admin.Models
{
    public class Confirm_order
    {
        internal int status;

        public int id { get; set; }
        public int useridid { get; set; }

        public string EnquiryNo { set; get; }
        public string full_name {  get; set; }

        public string mobile {  get; set; }

        public string email { get; set; }

        public string payment_mode { get; set; }

        public string shipping_add { get; set; }

        public string jsondetailsdata { get; set; }

        public int item_id { get; set; }

        public string item_name { get; set; }

        public int rate { get; set; }   

        public int quantity {  get; set; }

        public string order_no {  get; set; }
        public int type {  get; set; }
        public int status_flag {  get; set; }
        public int Paymentflagid {  get; set; }
        public string Paymentflag { get; internal set; }
    }
}
