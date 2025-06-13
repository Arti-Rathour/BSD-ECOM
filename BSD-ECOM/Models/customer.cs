using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSD_ECOM.Models
{
    public class customer
    {
        public string id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string MobileNo { get; set; }
        public string email { get; set; }
        public string displayName { get; set; }
        public string CountryID { get; set; }
        public string StateID { get; set; }
        public string CityID { get; set; }
        public string PinCode { get; set; }
        public string Address { get; set; }
        public string address2 { get; set; }
        public string Shi_CountryID { get; set; }
        public string Shi_StateID { get; set; }
        public string Shi_CityID { get; set; }
        public string Shi_PinCode { get; set; }
        public string Shi_Address { get; set; }
        public string s_address2 { get; set; }
        public string Shi_MobileNO { get; set; }
        public string bil_country { get; set; }
        public string ship_country { get; set; }
        public string bill_city { get; set; }
        public string ship_city { get; set; }
        public string bill_state { get; set; }
        public string ship_state { get; set; }
        public string bill_MobileNO { get; set; }
        
    }
    public class customerOrder
    {
        public int id { get; set; }
        public int customerid { get; set; }
        public string itemname { get; set; }
        public int itemid { get; set; }
        public string itemimage { get; set; }
        public string subtotal { get; set; }
        public string orderdate { get; set; }
        public  string payment_mode { get; set; }
        public string unit_qty { get; set; }
        public string unitrate { get; set; }
        public string order_no { get; set; }
        public double Amount { get; set; }

        public string status { get; set; }
        public string status_flg { get; set; }
        public int orderday { get; set; }
        public decimal Taxable_Value { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }

        public decimal IGSTAmt { get; set; }

    }
    public class wallet
    {
        public string credit { get; set; }
        public  string debit { get; set; }
        public string totalbalance { get; set; }
    }
}

