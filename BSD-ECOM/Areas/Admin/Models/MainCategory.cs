using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSD_ECOM.Areas.Admin.Models
{
    public class MainCategory
    {
        public int Main_cat_id { get; set; }
        public string entry_date { get; set; }
        public string Main_cat_name { get; set; }
        public bool Main_cat_status { get; set; }
        public int services { get; set; }
        public string service { get; set; }
    }

    public class Category
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
        public bool cat_status { get; set; }
        public int Main_cat_id { get; set; }
        public string main_cat_name { get; set; }
        public int company_id { get; set; }
        public string entry_date { get; set; }
    }

    public class SubCategory
    {
        public int cat_id { get; set; }
        public string cat_name { get; set; }
        public bool cat_status { get; set; }
        public int Main_cat_id { get; set; }
        public string main_cat_name { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string Image { get; set; }
        public string company_name { get; set; }
        public string entry_date { get; set; }
    }


   


    public class AddNewProduct
    {
       
      

        public int ID { get; set; }
        public int itemid { get; set; }
        public string ItemName { get; set; }
        public string BarCode { get; set; }
        public int CostPrice { get; set; }
        public int MRP { get; set; }
        public int LastPurchasePrice { get; set; }
        public int UnitID { get; set; }
        public int GroupID { get; set; }
        public int SubGroupID { get; set; }
        public int Admin_Services { get; set; }
        public string SKUCode { get; set; }
        public bool Status { get; set; }
        public int Rating { get; set; }
        public int BrandID { get; set; }
        public int Accessories { get; set; }
        public int NewArrival { get; set; }
        public string Date { get; set; }
        public int BestSeller { get; set; }
        public string Description { get; set; }
       
        public string URLName { get; set; }
        public int Opening { get; set; }
        public string ProductImage { get; set; }
        public int Onsale { get; set; }
        public int MangeStock { get; set; }
        public int StockStatus { get; set; }
        public string Weight { get; set; }
        public string Dimension { get; set; }
        public string ShipCharge { get; set; }
        public string productTag { get; set; }
       
        public string ingredients { get; set; }
        public string additional { get; set; }
        public int balanced { get; set; }
        public string unit { get; set; }
        public bool Flag { get; set; }
        public int display { get; set; }
        public string type { get; set; }
        public int VendorID { get; set; }
        public int CategoryID { get; set; }
        public string HSNCode { get; set; }
        public int Service_Order_Per { get; set; }
        public int company_id { get; set; }
        public string Main_cat_name { get; set; }
        public string category_name { get; set; }
        public string cat_name { get; set; }
        public string brand_name { get; set; }
        public string HSNName { get; set; }
        public string image { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }        
        public string SubCategory_name { get; set; }
        public decimal Unit_Rate { get; set; }
        public string unitname { get; set; }
        public string quantity { get; set; }
        public int discount { get; set; }
        public int unit_id { get; set; }

        public int stock_qty { get; set; }

        public int Active { get; set; }

        public int Send_query {  get; set; }

        public int Price {  get; set; }
        public string Image4 { get; internal set; }
		public string Image5 { get; internal set; }
        public int typeid { get; internal set; }
        public string HBanner { get; internal set; }
        public string MUrl { get; internal set; }
    }

}
