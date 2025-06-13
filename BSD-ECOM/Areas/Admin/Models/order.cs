using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSD_ECOM.Areas.Admin.Models
{
    public class order
    {
        public int order_id { get; set; }
        public string first_name { get; set; }
        public string mobile { get; set; }
        public string address1 { get; set; }
        public int user_id { get; set; }
        public string ItemName { get; set; }
        public string brand_name { get; set; }

    }


    public class Orders
    {
        // public int ID { set; get; }
        public string UserName { set; get; }
        public string Email { set; get; }
        public string Mobileno { set; get; }
        public string order_no { set; get; }
       
        public string quantity { set; get; }
        public string itemName { set; get; }
        public string itemimage { set; get; }
        public string unit_qty { set; get; }
        public string order_date { set; get; }

        public int order_id { set; get; }

        public int amount { set; get; }

        public int subtotal { set; get; }

        public int unit_rate { set; get; }
        public int Courior { set; get; }

        // public int subtitemimageotal { set; get; }

        public int item_id { set; get; }


        public string payment_mode { set; get; }

        public int CompanyID { set; get; }
    }
    public class Cancelled_Orders
    {

        public string UserName { set; get; }
        public string order_no { set; get; }
        public int order_id { set; get; }
        public int amount { set; get; }
        public string Address { set; get; }
        public string MobileNo { set; get; }
        public string payment_mode { set; get; }
        public string order_date { set; get; }
        public string Resion { set; get; }
        public int CompanyID { set; get; }
        public string quantity { set; get; }
        public string itemName { set; get; }
        public string itemimage { set; get; }
        public string unit_qty { set; get; }
        public int subtotal { set; get; }
        public int unit_rate { set; get; }
        public int item_id { set; get; }
        public int ID { set; get; }
        public int Cancelled_User { set; get; }
        public string cal_date { set; get; }
        public string cancel_type { get; set; }

    }

    public class Returned_Orders
    {

        public int User_id { set; get; }
        public string UserName { set; get; }
        public string order_no { set; get; }
        public int order_id { set; get; }
        public int amount { set; get; }
        public string Address { set; get; }
        public string MobileNo { set; get; }
        public string payment_mode { set; get; }
        public string order_date { set; get; }
        public string Resion { set; get; }
        public int CompanyID { set; get; }
        public string quantity { set; get; }
        public string itemName { set; get; }
        public string itemimage { set; get; }
        public string unit_qty { set; get; }
        public int subtotal { set; get; }
        public int unit_rate { set; get; }
        public int item_id { set; get; }
        public int ID { set; get; }
        public int Cancelled_User { set; get; }
        public string cal_date { set; get; }
        public string Return_date { set; get; }
        public string Return_User { set; get; }
        public string Return_Type { set; get; }
        public string Approvestatus { set; get; }

        




    }
    public class ORT_Dispatch
    {


        public string UserName { set; get; }

        public string order_no { set; get; }

        public int order_id { set; get; }

        public int amount { set; get; }

        public string Address { set; get; }

        public string MobileNo { set; get; }

        public string dispatch_flg { set; get; }

        public string payment_mode { set; get; }

        public string dispatch_date { set; get; }

        public string order_date { set; get; }

        public string Resion { set; get; }

        public int CompanyID { set; get; }


        public string quantity { set; get; }

        public string itemName { set; get; }



        public string itemimage { set; get; }



        public string unit_qty { set; get; }

        public int subtotal { set; get; }

        public int unit_rate { set; get; }

        // public int subtitemimageotal { set; get; }

        public int item_id { set; get; }


        public int cour_id { set; get; }

        public int cour_no { set; get; }



        public string cour_remarks { set; get; }







    }
    public class OR_to_Deliver
    {
        public string UserName { set; get; }

        public string order_no { set; get; }

        public int order_id { set; get; }

        public int amount { set; get; }

        public string Address { set; get; }

        public string MobileNo { set; get; }

        public int dispatch_flg { set; get; }

        public string payment_mode { set; get; }

        public string dispatch_date { set; get; }

        public string order_date { set; get; }

        public string Resion { set; get; }

        public int CompanyID { set; get; }

        public string quantity { set; get; }

        public string itemName { set; get; }

        public string itemimage { set; get; }

        public string unit_qty { set; get; }

        public int subtotal { set; get; }

        public int unit_rate { set; get; }

        // public int subtitemimageotal { set; get; }

        public int item_id { set; get; }

    }
    public class StockMaster
    {
        public int id { get; set; }
        public int itemdetailsid { get; set; }
        public int item_id { get; set; }
        public int cat_id { get; set; }
        public string ItemName { get; set; }
        public string Main_cat_name { get; set; }
        public string cat_name { get; set; }
        public string Unit_Qty { get; set; }
        public string stockqty { get; set; }
    }

}
