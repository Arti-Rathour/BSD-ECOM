using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSD_ECOM.Models
{
    public class Category
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
        public bool cat_status { get; set; }
        public int Main_cat_id { get; set; }
        public string main_cat_name { get; set; }
        public int company_id { get; set; }
    }
    public class MainCategory
    {
        public int Main_cat_id { get; set; }
        public string Main_cat_name { get; set; }
        public bool Main_cat_status { get; set; }
        public int services { get; set; }
        public int companyId { get; set; }
        
    }
    public class Itemdetails
    {
        public int Id { get; set; }
        public int itemId { get; set; }
        public int unitId { get; set; }
        public int stockqty { get; set; }
        public string Unit_Qty { get; set; }
        public string Unit_Rate { get; set; }
        public string category_name { get; set; }
        public double Disamt { get; set; }
        public double discount { get; set; }

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
        public int company_id { get; set; }
    }

    public class Locality_service
    {
        public int id { get; set; }
        public int Loc_id { get; set; }
        public string content { get; set; }
        public string Loc_serviceName { get; set; }
        public string image { get; set; }
        public string LocDetails_name { get; set; }
        public string contact_name { get; set; }
        public string locdetails_Img { get; set; }
        public string Locdetails_content { get; set; }
        public string pincode { get; set; }
        public string Loc_address { get; set; }
        public string mobile_no { get; set; }
        public string phone { get; set; }
        public int Loc_detailsId { get; set; }
    }

    public class FeatureBanner
    {
        public int id { get; set; }
        public string TBanner1 { get; set; }
        public string TBanner2 { get; set; }
        public string TBanner3 { get; set; }
        public string TUrl1 { get; set; }
        public string TUrl2 { get; set; }
        public string TUrl3 { get; set; }
        public string category_banner { get; set; }
        public string category_bannerUrl { get; set; }
    }
    public class footerBanner
    {
        public int id { get; set; }
        public string Footer_banner { get; set; }
        public string footer_bannerUrl { get; set; }
    }
    public class companyInformation
    {
        public int id { get; set; }
        public string name { get; set; }
        public string add1 { get; set; }
        public string add2 { get; set; }
        public string faceboolurl { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string logo { get; set; }
    }
    public class FooterInformation
    {
        public int id { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public string image { get; set; }
        public int status { get; set; }
        public int companyid { get; set; }
    }
    public class MainBanner
    {
        public int id { get; set; }
        public string HBanner { get; set; }
        public string MUrl { get; set; }
        public int typeid { get; set; }
    }
    public class socaildata
    {
        public int id { get; set; }
        public string url { get; set; }
        public string image { get; set; }
    }

    public class ItemStroe
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string MRP { get; set; }
        public string Frontimage { get; set; }
        public string Backimage { get; set; }
        public string Rightimage { get; set; }
        public string Leftimage { get; set; }
        public string URLName { get; set; }
        public string Unit_Qty { get; set; }
        public string Unit_Rate { get; set; }
        public string category_name { get; set; }
        public double Disamt { get; set; }
        public double discount { get; set; }
        public string productTag { get; set; }
        public string description { get; set; }
        public string ingredients { get; set; }
        public string additional { get; set; }
        public int subcategory_Id { get; set; }
        public string SubCategory_Name { get; set; }
        public int itemdetailsId{get;set;}
        public int types { get; set; }
        public int sbalance { get; set; }
        public string skucode { get; set; }
        public int unitid { get; set; }

        public int send_query {  get; set; }

        public int price {  get; set; }
        public int CategoryId { get; internal set; }
        public string Video { get; internal set; }
        public string Pdf { get; internal set; }
        public int quantity { get; internal set; }
    }

    public class ItemStroerelated
    {
        internal int price;

        public int Id { get; set; }
        public int quantity { get; set; }
        public string ItemName { get; set; }
        public string MRP { get; set; }
        public string Frontimage { get; set; }
        public string Backimage { get; set; }
        public string Rightimage { get; set; }
        public string Leftimage { get; set; }
        public string URLName { get; set; }
        public string Unit_Qty { get; set; }
        public string Unit_Rate { get; set; }
        public string category_name { get; set; }
        public double Disamt { get; set; }
        public double discount { get; set; }
        public string productTag { get; set; }
        public string description { get; set; }
        public string ingredients { get; set; }
        public string additional { get; set; }
        public int subcategory_Id { get; set; }
        public string SubCategory_Name { get; set; }
        public int itemdetailsId { get; set; }
        public int types { get; set; }
        public int sbalance { get; set; }
        public string skucode { get; set; }
        public int unitid { get; set; }
        public int CategoryId { get; internal set; }
        public int send_query { get; internal set; }
    }
    public class CustomerReview
    {
      public int Review_id { get; set; }
      public string Name { get; set; }
      public string Email { get; set; }
      public string Review { get; set; }
      public int Rating { get; set; }
      public int Isapproved { get; set; }
      public int Item_id { get; set; }
        public string entrydate { get; set; }
    }
    public class About
    {
        public int id { get; set; }
        public string AboutText { get; set; }
    }
    public class DeliveryInformation
    {
        public int id { get; set; }
        public string Delivery { get; set; }
    }
    public class paymentInfo
    {
        public int id { get; set; }
        public string paymentcontent { get; set; }
    }
    public class FAQ
    {
        public int id { get; set; }
        public string FAQText { get; set; }
    }
    public class terms
    {
        public int id { get; set; }
        public string termstext { get; set; }
    }
    public class shippingreturns {
        public int id { get; set; }
        public string shipping { get; set; }
    }
    public class privacy_policy
    {
        public int id { get; set; }
        public string privacy { get; set; }
    }

    public class welfare
    {
        public int id { get; set; }
        public string Welfare_Name { get; set; }
        public string img { get; set; }
        public string contactNo { get; set; }
        public string Email { get; set; }
        public string content { get; set; }
        public int companyid { get; set; }
        public int BallotNo { get; set; }
        public string url { get; set; }
        public string PostFor { get; set; }
    }

    public class blog
    {
        public int id { get; set; }
        public string ShortDesc { get; set; }
        public string img { get; set; }
        public string LongDesc { get; set; }
        public int companyid { get; set; }
    }

    public class unit
    {
        public int id { get; set; }
        public string unnitrate { get; set; }
    }

}
