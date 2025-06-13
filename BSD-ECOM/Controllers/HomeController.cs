using BSD_ECOM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BSD_ECOM.ViewModel;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using System.ComponentModel.Design;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Threading;
using BSD_ECOM.Areas.Admin.Models;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace BSD_ECOM.Controllers
{
    public class HomeController : Controller
    {
        ClsUtility util = new ClsUtility();

        DataBaseAccess Db = new DataBaseAccess();
        IndexViewModel viewmodel = new IndexViewModel();
        string message = "";
        int cart = 0;
        private IMemoryCache _cache;
        string sqlquery, status;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public IActionResult Index_New()
        {
            return View();
        }
        public IActionResult Index()
        {
            GetSiteNameAndCode();
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            //string companyid ="6";

            string cachename = companyid + "main";

            DataSet maincat = new DataSet();
            maincat = Db.FrontPage(companyid);
            //bool isExist = _cache.TryGetValue(cachename, out maincat);
            //if (!isExist)
            //{
            //    maincat = Db.FrontPage(companyid);
            //    var cacheEntryOptions = new MemoryCacheEntryOptions()
            //       .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            //    _cache.Set(cachename, maincat, cacheEntryOptions);


            //}
            //else
            //{
            //    maincat = (DataSet)_cache.Get(cachename);
            //}

            TempData["maindata"] = maincat;
            List<Locality_service> loc = new List<Locality_service>();
            List<Models.SubCategory> subCategories = new List<Models.SubCategory>();
            List<MainBanner> mainBanners = new List<MainBanner>();
            List<FeatureBanner> fbanner = new List<FeatureBanner>();
            List<ItemStroe> itemStroes = new List<ItemStroe>();
            List<welfare> welfares = new List<welfare>();
            List<Itemdetails> Itemdetails = new List<Itemdetails>();
            if (maincat.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[0].Rows)
                {
                    loc.Add(new Locality_service
                    {
                        id = Convert.ToInt32(dr["Loc_id"]),
                        Loc_serviceName = Convert.ToString(dr["Name"]),
                        content = Convert.ToString(dr["content"]),
                        image = Convert.ToString(dr["img"]),
                    });
                }
            }
            viewmodel.locality = loc;

            if (maincat.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[1].Rows)
                {
                    subCategories.Add(new Models.SubCategory
                    {
                        cat_id = Convert.ToInt32(dr["cat_id"]),
                        cat_name = Convert.ToString(dr["cat_name"]),
                        Image = Convert.ToString(dr["Image"]),
                    });
                }
            }
            viewmodel.subcategories = subCategories;
            if (maincat.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[2].Rows)
                {
                    mainBanners.Add(new MainBanner
                    {
                        id = Convert.ToInt32(dr["id"]),
                        HBanner = Convert.ToString(dr["TBanner1"]),
                        MUrl = Convert.ToString(dr["TUrl1"]),
                        typeid = Convert.ToInt32(dr["typeid"]),
                    });
                }
            }
            viewmodel.mainBanners = mainBanners;

            if (maincat.Tables[3].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[3].Rows)
                {
                    itemStroes.Add(new ItemStroe
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        ItemName = Convert.ToString(dr["ItemName"]),
                        Frontimage = Convert.ToString(dr["image"]),
                        Backimage = Convert.ToString(dr["image1"]),
                        Rightimage = Convert.ToString(dr["image2"]),
                        Leftimage = Convert.ToString(dr["image3"]),
                        URLName = Convert.ToString(dr["URLName"]),
                        CategoryId = Convert.ToInt32(dr["CategoryId"]),
                        category_name = Convert.ToString(dr["category_name"]),
                        send_query = Convert.ToInt32(dr["send_query"]),
                        price = Convert.ToInt32(dr["price"]),
                        productTag = Convert.ToString(dr["productTag"]),
                        types = Convert.ToInt32(dr["types"]),
                        itemdetailsId = Convert.ToInt32(dr["ItemId"]),

                        Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        
                        Disamt = Convert.ToDouble(dr["Disamt"]),
                        discount = Convert.ToDouble(dr["discount"]),
                        quantity = Convert.ToInt32(dr["stockqty"]),


                    });
                }
            }
            viewmodel.itemStroes = itemStroes;






            if (maincat.Tables[4].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[4].Rows)
                {
                    welfares.Add(new welfare
                    {
                        id = Convert.ToInt32(dr["id"]),
                        Welfare_Name = Convert.ToString(dr["Welfare_Name"]),
                        img = Convert.ToString(dr["img"]),
                        contactNo = Convert.ToString(dr["contactNo"]),
                        Email = Convert.ToString(dr["Email"]),
                        content = Convert.ToString(dr["content"]),
                    });
                }
            }
            viewmodel.Welfares = welfares;

            if (maincat.Tables[6].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[6].Rows)
                {
                    Itemdetails.Add(new Itemdetails
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        itemId = Convert.ToInt32(dr["item_id"]),
                        Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                        Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        Disamt = Convert.ToDouble(dr["Disamt"]),
                        unitId = Convert.ToInt32(dr["unit_id"])
                    }); ;
                }
            }
            viewmodel.ItemdetailsS = Itemdetails;
            ViewBag.companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            ViewBag.customerid = HttpContext.Session.GetInt32("customerid").ToString();
            ViewBag.itemid = HttpContext.Session.GetInt32("SiteId").ToString();


            List<SelectListItem> items = new List<SelectListItem>();
            DataSet ds = util.BindDropDown("select id,state_name from tblstate");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    items.Add(new SelectListItem { Value = dr["id"].ToString(), Text = dr["state_name"].ToString() });

                }
                ViewBag.Bindstate = items;

            }
            return View(viewmodel);

           // return View(viewmodel);
        }



        [HttpPost]

        public JsonResult openPopupregistration(String Email, string mobile,string password)
        {

            DataTable dt = new DataTable();


            //sqlquery = "SELECT * FROM tblregistration WHERE email = '" + Email + "' and MobileNo ='" + mobile + "' ";
            sqlquery = "SELECT * FROM tblregistration WHERE email = '" + Email + "' ";




            DataSet ds = util.TableBind(sqlquery);
            if (ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder sBody = new StringBuilder();
                string MailStatus = "";
                var DateTime = new DateTime();
                DateTime = DateTime.Now;
                sBody.Append("<!DOCTYPE html><html><body>");
                sBody.Append("<p>Dear " + HttpContext.Session.GetString("customerfirstname") + HttpContext.Session.GetString("customerlastname") +  Environment.NewLine);
                sBody.Append("<p>Thank you for registering with BSD Online Store! We’re thrilled to have you on board" + Environment.NewLine);
                sBody.Append("<p>You can now explore our products, services, and exclusive offers designed to meet your needs. Here are your registration details:" + Environment.NewLine);
                sBody.Append("<p>User Name: " + Email + "" + Environment.NewLine);
                sBody.Append("<p>Password: " + password + ", " + Environment.NewLine);
                // sBody.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
                //sBody.Append("<p>Contact Info: " + Mobile + "" + Environment.NewLine);
                //sBody.Append("<p>Email: " + email + "" + Environment.NewLine);
                sBody.Append("<p>" + Environment.NewLine);
                sBody.Append("<p>If you have any questions or need assistance, feel free to contact us at 9871092024 or email us at support@bsdinfotech.com." + Environment.NewLine);
                // sBody.Append("<p>Start exploring today by logging into your account: [Login Link]" + Environment.NewLine);
                sBody.Append("<p>Welcome to the BSD family!</p>");
                sBody.Append("<p>Best Regards</p>");
                sBody.Append("<p><b>The BSD Infotech Online Store Team</b></p>");
                sBody.Append("</body></html>");
                MailStatus = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + Email + "", "", "", "Subject : Welcome to BSD INFOTECH – Registration Successful!", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");


                StringBuilder sBody1 = new StringBuilder();
                string MailStatus1 = "";
                // var DateTime = new DateTime();
                DateTime = DateTime.Now;
                sBody1.Append("<!DOCTYPE html><html><body>");
                sBody1.Append("<p>Dear Team," + Environment.NewLine);
                sBody1.Append("<p>A new user has successfully registered on our platform. Below are the details: " + Environment.NewLine);
                sBody1.Append("<p>User Name: " + Email + "" + Environment.NewLine);
                sBody1.Append("<p>Password: " + password + ", " + Environment.NewLine);
                sBody1.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
                sBody1.Append("<p>Contact Info: " + mobile + "" + Environment.NewLine);
                sBody1.Append("<p>" + Environment.NewLine);
                sBody1.Append("<p>Please ensure their account setup is complete and ready for use. If there are any pending verifications or actions required, kindly address them promptly." + Environment.NewLine);
                sBody1.Append("<p>Let’s provide them with the best experience!</p>");
                sBody1.Append("<p>Best Regards,</p>");
                sBody1.Append("<p><b>BSD Team</b></p>");
                sBody1.Append("</body></html>");
                string email1 = "bsddemos@gmail.com";
                MailStatus1 = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : New User Registration Notification", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");
            }
            else
            {

                


            }
            return Json(JsonConvert.SerializeObject(dt));
        }



        [HttpPost]
        public JsonResult openPopup(String Email, string mobile,int Itemid)
        {

            DataTable dt = new DataTable();

            
                //sqlquery = "SELECT * FROM tblregistration WHERE email = '"+ Email + "' and MobileNo ='"+ mobile + "' ";
                sqlquery = "SELECT * FROM tblregistration WHERE email = '"+ Email + "' ";




            DataSet ds = util.TableBind(sqlquery);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return Json(JsonConvert.SerializeObject(dt));
        }


        [HttpPost]
        public JsonResult Replyconfrimuser(int Userid)
        {

            DataTable dt = new DataTable();


           
            sqlquery = "SELECT * FROM tblEnquiry WHERE id = '" + Userid + "' ";




            DataSet ds = util.TableBind(sqlquery);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return Json(JsonConvert.SerializeObject(dt));
        }


        [HttpPost]

        public JsonResult enquiry2(string Fullname, string Email, string mobile, string state, string city, string Message, string ItemName, int Itemid, int user_id,int code)
        {

            //DataTable dt2 = util.execQuery("Select reg_id, first_name + ' ' +last_name  as fullname, email, MobileNo from [tblregistration] where reg_id ='" + HttpContext.Session.GetInt32("customerid").ToString() + "'");
            Message = Message == null ? "0" : Message;
            //if (Db.IU_Enquiry2(util.FixQuotes(Fullname.Trim()), Email.Trim(), util.FixQuotes(mobile.Trim()), util.FixQuotes(state.Trim()), util.FixQuotes(city.Trim()), util.FixQuotes(Message.Trim()), Itemid, user_id,code))
            //{
                Db.sendAdminmail2(util.FixQuotes(Fullname.Trim()), Email.Trim(), util.FixQuotes(mobile.Trim()), util.FixQuotes(state.Trim()), util.FixQuotes(city.Trim()), util.FixQuotes(Message.Trim()), Itemid,code);
                message = "send otp";
            //}
            DataTable dt1 = util.execQuery("Select * from [tblstate] where ID='" + state + "'");
            var Data = new { message = message};
            if (message == "send otp")
            {

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("<!DOCTYPE html>");
                sb.AppendLine("<html lang=\"en\">");
                sb.AppendLine("<head>");
                sb.AppendLine("    <meta charset=\"UTF-8\">");
                sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
                sb.AppendLine("    <title>Email Verification</title>");
                sb.AppendLine("</head>");
                sb.AppendLine("<body style=\"font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;\">");
                sb.AppendLine("    <div style=\"width: 100%; max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 4px 8px rgba(0,0,0,0.1);\">");
                sb.AppendLine("        <div style=\"text-align: center; background-color: #4CAF50; padding: 10px 0; border-radius: 8px 8px 8px 8px;\">");
                sb.AppendLine("            <h2 style=\"color: white; margin: 0; font-size: 24px;\">Email Verification</h2>");
                sb.AppendLine("        </div>");
                sb.AppendLine("        <div style=\"text-align: center; padding: 20px;\">");
                sb.AppendLine("            <p style=\"font-size: 16px; color: #333333;\">Your email verification code is:</p>");
                sb.AppendLine("            <div style=\"font-size: 32px; font-weight: bold; color: #4CAF50; padding: 10px 0; letter-spacing: 5px;\">" + code + "</div>");
                sb.AppendLine("            <p style=\"font-size: 16px; color: #333333;\">If you did not request a code, you can ignore this email.</p>");
                sb.AppendLine("        </div>");
                sb.AppendLine("        <div style=\"text-align: center; font-size: 14px; color: #777777; margin-top: 20px;\">");
                sb.AppendLine("            <p>Thank you,<br><strong>BSD Infotech</strong></p>");
                sb.AppendLine("        </div>");
                sb.AppendLine("    </div>");
                sb.AppendLine("</body>");
                sb.AppendLine("</html>");


                util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + Email + "", "", "", "Subject : You Have Received Verification Code", sb.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");

            }
            return Json(Data);
        }




        [HttpPost]

        public JsonResult enquiry1(string Fullname, string Email, string mobile, string state, string city, string Message, string ItemName, int Itemid, int user_id, int code,int status)
        {

            HttpContext.Session.SetString("customerfirstname", Fullname);
            HttpContext.Session.SetString("customeremail", Email);
            HttpContext.Session.SetString("customerMobileNo", mobile);
            HttpContext.Session.SetString("StateID", state);
            HttpContext.Session.SetString("Cityid", city);
            HttpContext.Session.SetString("code", code.ToString());
            string password = "12345@";
           // password = util.cryption(password.Trim()) ;

            //DataTable dt4 = util.execQuery("Select reg_id, first_name + ' ' +last_name  as fullname, email, MobileNo from [tblregistration] where email ='" + Email + "' and MobileNo='"+ mobile + "' ");
            DataTable dt4 = util.execQuery("Select reg_id, first_name + ' ' +last_name  as fullname, email, MobileNo from [tblregistration] where email ='" + Email + "'");
            Message = Message == null ? "0" : Message;
            if (Db.IU_Enquiry1(util.FixQuotes(Fullname.Trim()), Email.Trim(), util.FixQuotes(mobile.Trim()), util.FixQuotes(state.Trim()), util.FixQuotes(city.Trim()), util.FixQuotes(Message.Trim()),  Itemid, user_id, code, status, util.cryption(password.Trim())))
              
            {
                Db.sendAdminmail1(util.FixQuotes(Fullname.Trim()), Email.Trim(), util.FixQuotes(mobile.Trim()), util.FixQuotes(state.Trim()), util.FixQuotes(city.Trim()), util.FixQuotes(Message.Trim()), Itemid, code);
                message = "send enquiry";
            }

            if (dt4.Rows.Count <= 0)
            {
                
                openPopupregistration(Email, mobile, password);
            }
           

            DataTable dt1 = util.execQuery("Select * from [tblstate] where ID='" + state + "'");
            DataTable dt2 = util.execQuery("Select * from [tblcity] where ID='" + city + "'");
            DataTable dt3 = util.execQuery("select b.ItemName from tblEnquiry  a join tblItemStore b on a.Itemid=b.ID where a.Itemid='" + Itemid + "'");
            var Data = new { message = message };

            if (message == "send enquiry")
            {
                StringBuilder sBody = new StringBuilder();
                string MailStatus = "";
                var DateTime = new DateTime();
                DateTime = DateTime.Now;
                sBody.Append("<!DOCTYPE html><html><body>");
               // sBody.Append("<p>Dear Support Team," + Environment.NewLine);
                //sBody.Append("<p>We have received your query. We will get back to you soon " + Environment.NewLine);
                sBody.Append("<p>Dear " + Fullname /*HttpContext.Session.GetString("customerfirstname") + " " + HttpContext.Session.GetString("customerlastname")*/ + Environment.NewLine);
                sBody.Append("<p>" + Environment.NewLine);
                sBody.Append("<p>Thank you for reaching out to us regarding " + dt3.Rows[0]["ItemName"].ToString() + ". We appreciate your interest in our offerings!" + Environment.NewLine);
                sBody.Append("<p> Here are the details of the product you inquired about:" + Environment.NewLine);
                sBody.Append("<p>Product Name: " + dt3.Rows[0]["ItemName"].ToString() + "" + Environment.NewLine);
                sBody.Append("<p>Email: " + Email + "" + Environment.NewLine);
                sBody.Append("<p>If you have any specific requirements or further questions, please don’t hesitate to ask. We’re here to assist you and ensure you have all the information needed to make an informed decision.</p>");
                sBody.Append("<p>Looking forward to serving you!</p>");
                sBody.Append("<p>" + Environment.NewLine);

                //sBody.Append("<p>If you have any urgent queries, you can also reach us at [contact information].</p>");
                //sBody.Append("<p>" + Environment.NewLine);

                //sBody.Append("<p>Looking forward to assisting you further!</p>");
                sBody.Append("<p>" + Environment.NewLine);

                sBody.Append("<p>Best Regards</p>");
                sBody.Append("<p><b>Team BSD</b></p>");
                sBody.Append("</body></html>");
                MailStatus = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + Email + "", "", "", "Subject : : Thank You for Your Product Inquiry", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");
            }
            if (message == "send enquiry")
            {

                StringBuilder sBody1 = new StringBuilder();

                string MailStatus1 = "";
                var DateTime = new DateTime();
                DateTime = DateTime.Now;
                sBody1.Append("<!DOCTYPE html><html><body>");
                sBody1.Append("<p>Dear Team," + Environment.NewLine);
                sBody1.Append("<p> We have received a new product inquiry from a user. Below are the details:" + Environment.NewLine);
              
                //sBody1.Append("<p>Customer Id: " + HttpContext.Session.GetString("customerid") + "" + Environment.NewLine);
                sBody1.Append("<p>Customer Name: " + Fullname + Environment.NewLine);
                sBody1.Append("<p>Contact Info: " + mobile + "" + Environment.NewLine);
                sBody1.Append("<p>Product Name: " + dt3.Rows[0]["ItemName"].ToString() + "" + Environment.NewLine);
                sBody1.Append("<p>Email: " + Email + "" + Environment.NewLine);
               // sBody1.Append("<p>State: " + dt1.Rows[0]["state_name"].ToString() + "" + Environment.NewLine);
               // sBody1.Append("<p>City: " + dt2.Rows[0]["city"].ToString() + "" + Environment.NewLine);
               
                sBody1.Append("<p>Customer Message: " + Message + "" + Environment.NewLine);
                //sBody1.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
                sBody1.Append("<p>" + Environment.NewLine);
                sBody1.Append("<p>Please review the inquiry and respond to the customer promptly with the required details. Let’s ensure the customer has a seamless experience." + Environment.NewLine);
                sBody1.Append("<p>Best Regards</p>");
                sBody1.Append("<p><b>Team BSD</b></p>");
                sBody1.Append("</body></html>");
                string email1 = "bsddemos@gmail.com";
                MailStatus1 = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : New Product Inquiry Notification", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");

            }
            return Json(Data);
        }

        [HttpPost]
        public JsonResult ShowItemView(int id, int itemdetailsid)
        {
            //int itemdetailsid = 0;
            //ViewBag.Id = id;
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet dsitemView = Db.Itemquickview(id, companyid, itemdetailsid);
            List<ItemStroe> itemStroes = new List<ItemStroe>();
            if (dsitemView.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsitemView.Tables[0].Rows)
                {
                    itemStroes.Add(new ItemStroe
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        ItemName = Convert.ToString(dr["ItemName"]),
                        Frontimage = Convert.ToString(dr["image"]),
                        Backimage = Convert.ToString(dr["image1"]),
                        Leftimage = Convert.ToString(dr["image3"]),
                        Rightimage = Convert.ToString(dr["image2"]),
                        URLName = Convert.ToString(dr["URLName"]),
                        MRP = Convert.ToString(dr["MRP"]),
                        Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                        Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        category_name = Convert.ToString(dr["category_name"]),
                        Disamt = Convert.ToDouble(dr["Disamt"]),
                        discount = Convert.ToDouble(dr["discount"]),
                        productTag = Convert.ToString(dr["productTag"]),
                        send_query = Convert.ToInt32(dr["send_query"]),
                        price = Convert.ToInt32(dr["price"]),
                        quantity = Convert.ToInt32(dr["balanced"]),

                    });
                }
            }
            viewmodel.itemStroes = itemStroes;
            
            return Json(itemStroes);
        }

        [HttpGet]
        [Route("Product/{id}/{itemdetailsId}/{name}/{CategoryId}")]
        //  [Route("Product/{id}/{name}")]
        public IActionResult ProductDetails()
        {
            GetSiteNameAndCode();
            List<ItemStroe> itemStroes = new List<ItemStroe>();
            List<ItemStroerelated> related = new List<ItemStroerelated>();
            List<CustomerReview> CustomerReviews = new List<CustomerReview>();
            List<Itemdetails> Itemdetails = new List<Itemdetails>();
            List<Models.SubCategory> scat = new List<Models.SubCategory>();
            string id = RouteData.Values["id"].ToString();
            int CategoryId = Convert.ToInt32(RouteData.Values["CategoryId"]);
            int itemdetailsid = 0;
            //int itemdetailsid = Convert.ToInt32(RouteData.Values["itemdetailsId"]);
            //string itemdetailsid = Convert.ToString(RouteData.Values["itemdetailsId"]);
            //HttpContext.Session.SetString("itemdetailsId", itemdetailsid);
            ViewBag.itemidet = itemdetailsid;
            string name = RouteData.Values["name"].ToString();
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            int Pid = Convert.ToInt32(id);
            DataSet Ds = Db.ItemView(Pid, companyid, itemdetailsid, CategoryId);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Ds.Tables[0].Rows)
                {
                    itemStroes.Add(new ItemStroe
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        ItemName = Convert.ToString(dr["ItemName"]),
                        Frontimage = Convert.ToString(dr["image"]),
                        Backimage = Convert.ToString(dr["image1"]),
                        Rightimage = Convert.ToString(dr["image2"]),
                        Leftimage = Convert.ToString(dr["image3"]),
                        Video = Convert.ToString(dr["Image4"]),
                        Pdf = Convert.ToString(dr["Image5"]),

                        URLName = Convert.ToString(dr["URLName"]),
                        MRP = Convert.ToString(dr["MRP"]),
                        Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                        Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        category_name = Convert.ToString(dr["category_name"]),
                        Disamt = Convert.ToDouble(dr["Disamt"]),
                        discount = Convert.ToDouble(dr["discount"]),
                        productTag = Convert.ToString(dr["productTag"]),
                        description = Convert.ToString(dr["Description"]),
                        additional = Convert.ToString(dr["additional"]),
                        ingredients = Convert.ToString(dr["ingredients"]),
                        sbalance = Convert.ToInt32(dr["balanced"]),
                        skucode = Convert.ToString(dr["SKUCode"]),
                        //itemdetailsId = Convert.ToInt32(dr["Unitid"]),
                        itemdetailsId = Convert.ToInt32(dr["ItemId"]),
                        unitid = Convert.ToInt32(dr["UNIT_ID"]),
                        send_query = Convert.ToInt32(dr["send_query"]),
                        price = Convert.ToInt32(dr["price"]),
                        quantity = Convert.ToInt32(dr["stockqty"]),
                    });
                }
            }
            viewmodel.itemStroes = itemStroes;
            if (Ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in Ds.Tables[1].Rows)
                {
                    CustomerReviews.Add(new CustomerReview
                    {
                        Review_id = Convert.ToInt32(dr["Review_id"]),
                        Name = Convert.ToString(dr["customerName"]),
                        Email = Convert.ToString(dr["customerEmail"]),
                        Review = Convert.ToString(dr["customerReview"]),
                        Rating = Convert.ToInt32(dr["customerrating"]),
                        Item_id = Convert.ToInt32(dr["Item_id"]),
                        entrydate = Convert.ToString(dr["entrydate"]),
                    });
                }
            }
            viewmodel.Reviews = CustomerReviews;
            if (Ds.Tables[4].Rows.Count > 0)
            {
                foreach (DataRow dr in Ds.Tables[4].Rows)
                {
                    Itemdetails.Add(new Itemdetails
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        itemId = Convert.ToInt32(dr["item_id"]),
                        Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                        Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        Disamt = Convert.ToDouble(dr["Disamt"]),
                    });
                }
            }
            viewmodel.ItemdetailsS = Itemdetails;

            DataSet maincat = Db.GetMainCategory(companyid);
            if (maincat.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[2].Rows)
                {
                    scat.Add(new Models.SubCategory
                    {
                        cat_id = Convert.ToInt32(dr["cat_id"]),
                        Main_cat_id = Convert.ToInt32(dr["Main_cat_id"]),
                        cat_name = Convert.ToString(dr["cat_name"]),
                        cat_status = Convert.ToBoolean(dr["cat_status"]),
                        company_id = Convert.ToInt32(dr["Company_id"]),
                        category_id = Convert.ToInt32(dr["category_id"]),
                        Image = Convert.ToString(dr["Image"]),
                    });
                }
            }
            viewmodel.subcategories = scat;


            if (Ds.Tables[5].Rows.Count > 0)
            {
                foreach (DataRow dr in Ds.Tables[5].Rows)
                {
                    related.Add(new ItemStroerelated
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        ItemName = Convert.ToString(dr["ItemName"]),
                        Frontimage = Convert.ToString(dr["image"]),
                        Backimage = Convert.ToString(dr["image1"]),
                        Rightimage = Convert.ToString(dr["image2"]),
                        Leftimage = Convert.ToString(dr["image3"]),
                        URLName = Convert.ToString(dr["URLName"]),
                        //itemrealted=
                        //MRP = Convert.ToString(dr["MRP"]),
                        //Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                        Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        category_name = Convert.ToString(dr["category_name"]),
                        Disamt = Convert.ToDouble(dr["Disamt"]),
                        discount = Convert.ToDouble(dr["discount"]),
                        productTag = Convert.ToString(dr["productTag"]),
                        //description = Convert.ToString(dr["Description"]),
                        //additional = Convert.ToString(dr["additional"]),
                        //ingredients = Convert.ToString(dr["ingredients"]),
                        //sbalance = Convert.ToInt32(dr["balanced"]),
                        //skucode = Convert.ToString(dr["SKUCode"]),
                        ////////////itemdetailsId = Convert.ToInt32(dr["itemdetid"]),
                        itemdetailsId = Convert.ToInt32(dr["ItemId"]),
                        CategoryId = Convert.ToInt32(dr["CategoryId"]),
                        price = Convert.ToInt32(dr["price"]),
                        send_query = Convert.ToInt32(dr["send_query"]),
                        types = Convert.ToInt32(dr["types"]),
                        //unitid = Convert.ToInt32(dr["UNIT_ID"]),
                          quantity = Convert.ToInt32(dr["stockqty"]),
                    });
                }
            }
            viewmodel.ItemStroerelateds = related;
            List<SelectListItem> items = new List<SelectListItem>();
            DataSet ds = util.BindDropDown("select id,state_name from tblstate");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    items.Add(new SelectListItem { Value = dr["id"].ToString(), Text = dr["state_name"].ToString() });

                }
                ViewBag.Bindstate = items;
            }
            return View(viewmodel);
        }

        [HttpGet]
        [Route("Category/{id}/{name}/{type}/{Main_cat_id}")]
        public IActionResult AllProduct(int cat_id,string itemname)
        {

            GetSiteNameAndCode();
            int id = Convert.ToInt32(RouteData.Values["id"]);
            string Id = Convert.ToString(RouteData.Values["id"]);
            HttpContext.Session.SetString("Id", Id);
            int itemdetails = 0;
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            ViewBag.user_id = HttpContext.Session.GetInt32("customerid").ToString();
            string name = RouteData.Values["name"].ToString();
            string type = RouteData.Values["type"].ToString();
            string Main_cat_id = RouteData.Values["Main_cat_id"].ToString();
            HttpContext.Session.SetString("type", type);
            HttpContext.Session.SetString("Main_cat_id", Main_cat_id);
            ViewBag.CategoryName = name;
            ViewBag.type = type;
            ViewBag.catid = id;
            DataTable dt = util.execQuery("select Main_cat_name, category_name from tblitem_Main_category M join tblcategory C on M.Main_cat_id = C.Main_cat_id where C.category_id = '" + id + "'");
            if (dt.Rows.Count > 0)
            {
                ViewBag.main_cat_name = dt.Rows[0]["Main_cat_name"];
                ViewBag.cat_name = dt.Rows[0]["category_name"];
            }

            List<ItemStroe> itemStroes = new List<ItemStroe>();
            List<Itemdetails> Itemdetails = new List<Itemdetails>();
            DataSet maincat = new DataSet();
            if (cat_id==0 && itemname != null)
            {
                //sqlquery = "exec Sp_itemseachbycatagroye @type1=3,@companyid=" + companyid + ",@itemname='"+itemname+"'";
                sqlquery = " select  isnull(ts.Id,0)as Id ,max(isnull(ts.ItemName,''))as ItemName,max(isnull(ts.image,''))as image,max(isnull(ts.image1,''))as image1,max(isnull(ts.image2,''))as image2,max(isnull(ts.image3,''))as image3,max(isnull(ts.URLName,''))as URLName,max(isnull(mcat.Main_cat_name,''))as category_name,max(isnull(ts.CategoryID,0))as CategoryID,max(isnull(td.discount,0))as discount,max(isnull(ts.productTag,''))as productTag,max(isnull(ts.Description,''))as Description, max(isnull(ts.ingredients,''))as ingredients,max(isnull(ts.additional,''))as additional,max(SKUCode)SKUCode,max(isnull(UNIT_ID,0))UNIT_ID,case when (select count(*) from  tblitemdetails tdd where tdd.item_id =  ts.id )=1 then max(unit_id) else 0 end  types,max((isnull(td.Unit_Rate,0.00)-(isnull(td.discount,0.00)*isnull(td.Unit_Rate,0.00))/100))as Disamt,max(isnull(td.Unit_Rate,0))as Unit_Rate,max(isnull(td.stockqty,0))as stockqty,entrydate,max(isnull(ts.ItemId,0)) ItemId,max(isnull(ts.price,0))as price,max(isnull(ts.send_query,0))as send_query,MAX(mcat.Main_cat_id)Main_cat_id from tblItemStore ts left join tblitemdetails td on td.item_id =  ts.id join tblitem_Main_category mcat on mcat.Main_cat_id=ts.GroupID where ts.STATUS=1 and ts.Active = 1 and ts.company_id=" + companyid + " and ItemName   LIKE '%" + itemname + "%'   group by  ts.Id,entrydate order by  ItemName ASC; select td.id, isnull(td.Unit_Qty,0)as Unit_Qty,isnull(td.Unit_Rate,0)as Unit_Rate, (td.Unit_Rate-(td.discount*td.Unit_Rate)/100)as Disamt,item_id ,unit_id  from tblitemdetails td, tblItemStore ts where ts.Id =  td.item_id and ts.STATUS=1 and ts.company_id=" + companyid + " and  ts.CategoryID=" + cat_id + " ";

                maincat = util.TableBind(sqlquery);
            }
            else if (cat_id != 0 && itemname != null)
            {
               // sqlquery = "exec Sp_itemseachbycatagroye @type1=4,@companyid=" + companyid + ",@id=" + cat_id + ",@itemname='" + itemname + "'";
                sqlquery = " select  isnull(ts.Id,0)as Id ,max(isnull(ts.ItemName,''))as ItemName,max(isnull(ts.image,''))as image,max(isnull(ts.image1,''))as image1,max(isnull(ts.image2,''))as image2,max(isnull(ts.image3,''))as image3,max(isnull(ts.URLName,''))as URLName,max(isnull(mcat.Main_cat_name,''))as category_name,max(isnull(ts.CategoryID,0))as CategoryID,max(isnull(td.discount,0))as discount,max(isnull(ts.productTag,''))as productTag,max(isnull(ts.Description,''))as Description, max(isnull(ts.ingredients,''))as ingredients,max(isnull(ts.additional,''))as additional,max(SKUCode)SKUCode,max(isnull(UNIT_ID,0))UNIT_ID,case when (select count(*) from  tblitemdetails tdd where tdd.item_id =  ts.id )=1 then max(unit_id) else 0 end  types,max((isnull(td.Unit_Rate,0.00)-(isnull(td.discount,0.00)*isnull(td.Unit_Rate,0.00))/100))as Disamt,max(isnull(td.Unit_Rate,0))as Unit_Rate,max(isnull(td.stockqty,0))as stockqty,entrydate,max(isnull(ts.ItemId,0)) ItemId,max(isnull(ts.price,0))as price,max(isnull(ts.send_query,0))as send_query,MAX(mcat.Main_cat_id)Main_cat_id from tblItemStore ts left join tblitemdetails td on td.item_id =  ts.id join tblitem_Main_category mcat on mcat.Main_cat_id=ts.GroupID where ts.STATUS=1 and ts.Active = 1 and ts.company_id=" + companyid + " and ItemName   LIKE '%" + itemname + "%' AND CategoryID=" + cat_id + "  group by  ts.Id,entrydate  order by  ItemName ASC ;select td.id, isnull(td.Unit_Qty,0)as Unit_Qty,isnull(td.Unit_Rate,0)as Unit_Rate,(td.Unit_Rate-(td.discount*td.Unit_Rate)/100)as Disamt,item_id ,unit_id  from tblitemdetails td, tblItemStore ts where ts.Id =  td.item_id and ts.STATUS=1 and ts.company_id=" + companyid + " and  ts.CategoryID=" + cat_id + " ";
                maincat = util.TableBind(sqlquery);
            }
            else
            {
                 maincat = Db.ItemViewcatewise(id, companyid, itemdetails, 1, 0, 0, type);
            }

           
            if (maincat.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[0].Rows)
                {
                    itemStroes.Add(new ItemStroe
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        ItemName = Convert.ToString(dr["ItemName"]),
                        Frontimage = Convert.ToString(dr["image"]),
                        Backimage = Convert.ToString(dr["image1"]),
                        Rightimage = Convert.ToString(dr["image2"]),
                        Leftimage = Convert.ToString(dr["image3"]),
                        URLName = Convert.ToString(dr["URLName"]),
                        itemdetailsId = Convert.ToInt32(dr["ItemId"]),
                        //MRP = Convert.ToString(dr["MRP"]),
                        //Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                        Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        category_name = Convert.ToString(dr["category_name"]),
                        Disamt = Convert.ToDouble(dr["Disamt"]),
                        discount = Convert.ToDouble(dr["discount"]),
                        productTag = Convert.ToString(dr["productTag"]),
                        types = Convert.ToInt32(dr["types"]),
                        send_query = Convert.ToInt32(dr["send_query"]),
                        price = Convert.ToInt32(dr["price"]),
                        CategoryId = Convert.ToInt32(dr["CategoryId"]),
                        quantity = Convert.ToInt32(dr["stockqty"]),
                    });
                }
            }
            viewmodel.itemStroes = itemStroes;

            if (maincat.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[1].Rows)
                {
                    Itemdetails.Add(new Itemdetails
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        itemId = Convert.ToInt32(dr["item_id"]),
                        Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                        Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        Disamt = Convert.ToDouble(dr["Disamt"]),
                        unitId = Convert.ToInt32(dr["unit_id"]),
                      
                    });
                }
            }
            viewmodel.ItemdetailsS = Itemdetails;
            List<SelectListItem> items = new List<SelectListItem>();
            DataSet ds = util.BindDropDown("select id,state_name from tblstate");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    items.Add(new SelectListItem { Value = dr["id"].ToString(), Text = dr["state_name"].ToString() });

                }
                ViewBag.Bindstate = items;

            }
            return View(viewmodel);
        }
        [HttpGet]
        public IActionResult SearchItem(string catid, string txtSearch)
        {
            GetSiteNameAndCode();
            int itemdetails = 0;
            int id = 0;
            string searchdata = "";
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            // int id = Convert.ToInt32(RouteData.Values["id"]);
            //string name = RouteData.Values["data"].ToString();
            ViewBag.CategoryName = txtSearch;
            ViewBag.catid = catid;
            List<ItemStroe> itemStroes = new List<ItemStroe>();
            List<Itemdetails> Itemdetails = new List<Itemdetails>();
            DataSet maincat = Db.ItemViewcatewisesearch(catid, companyid, itemdetails, 1, 0, 0, txtSearch);
            if (maincat.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[0].Rows)
                {
                    itemStroes.Add(new ItemStroe
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        ItemName = Convert.ToString(dr["ItemName"]),
                        Frontimage = Convert.ToString(dr["image"]),
                        Backimage = Convert.ToString(dr["image1"]),
                        Rightimage = Convert.ToString(dr["image2"]),
                        Leftimage = Convert.ToString(dr["image3"]),
                        URLName = Convert.ToString(dr["URLName"]),
                        // MRP = Convert.ToString(dr["MRP"]),
                        //Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                        // Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        category_name = Convert.ToString(dr["category_name"]),
                        // Disamt = Convert.ToDouble(dr["Disamt"]),
                        //  discount = Convert.ToDouble(dr["discount"]),
                        productTag = Convert.ToString(dr["productTag"]),
                        types = Convert.ToInt32(dr["types"]),
                    });
                }
            }
            viewmodel.itemStroes = itemStroes;

            if (maincat.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[1].Rows)
                {
                    Itemdetails.Add(new Itemdetails
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        itemId = Convert.ToInt32(dr["item_id"]),
                        Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                        Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        Disamt = Convert.ToDouble(dr["Disamt"]),
                        unitId = Convert.ToInt32(dr["unit_id"])
                    }); ;
                }
            }
            viewmodel.ItemdetailsS = Itemdetails;

            return View(viewmodel);
        }

        [HttpPost]
        public IActionResult allproductsearchcatewise(string cateid, int typeid, double minamt = 0.00, double maxamt = 0.00, string data = "")
        {
            GetSiteNameAndCode();
            int itemdetails = 0;
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            //int id = Convert.ToInt32(RouteData.Values["id"]);
            /// string name = RouteData.Values["name"].ToString();
            //ViewBag.CategoryName = name;
            List<ItemStroe> itemStroes = new List<ItemStroe>();
            List<Itemdetails> Itemdetails = new List<Itemdetails>();
            DataSet maincat = Db.ItemViewcatewisesearch(cateid, companyid, itemdetails, typeid, minamt, maxamt, data);
            if (maincat.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[0].Rows)
                {
                    itemStroes.Add(new ItemStroe
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        ItemName = Convert.ToString(dr["ItemName"]),
                        Frontimage = Convert.ToString(dr["image"]),
                        Backimage = Convert.ToString(dr["image1"]),
                        Rightimage = Convert.ToString(dr["image2"]),
                        Leftimage = Convert.ToString(dr["image3"]),
                        URLName = Convert.ToString(dr["URLName"]),
                        // MRP = Convert.ToString(dr["MRP"]),
                        //Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                        // Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        category_name = Convert.ToString(dr["category_name"]),
                        // Disamt = Convert.ToDouble(dr["Disamt"]),
                        //  discount = Convert.ToDouble(dr["discount"]),
                        productTag = Convert.ToString(dr["productTag"]),
                        types = Convert.ToInt32(dr["types"]),
                    });
                }
            }
            viewmodel.itemStroes = itemStroes;

            if (maincat.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[1].Rows)
                {
                    Itemdetails.Add(new Itemdetails
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        itemId = Convert.ToInt32(dr["item_id"]),
                        Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                        Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        Disamt = Convert.ToDouble(dr["Disamt"]),
                        unitId = Convert.ToInt32(dr["unit_id"])
                    }); ;
                }
            }
            viewmodel.ItemdetailsS = Itemdetails;


            var Data = new { itemStroes = viewmodel.itemStroes, ItemdetailsS = viewmodel.ItemdetailsS, count = 2, message = "ok", displaymessage = "" };
            return Json(Data);

        }
        [HttpGet]
        [Route("cart")]
        public IActionResult Addtocart()
        {
            GetSiteNameAndCode();
            ViewBag.count = (HttpContext.Session.GetInt32("count"));
            if (Convert.ToInt32(ViewBag.count) > 0)
            {
                ViewBag.cart = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("CartItems"));
            }
            //ViewBag.count = (HttpContext.Session.GetInt32("count"));


            
            return View();
        }
        [HttpPost]
        public IActionResult allproductcatewise(int cateid, int typeid, string minamt, string maxamt)
        {
            GetSiteNameAndCode();
            int itemdetails = 0;
            string type = "4";
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            //int id = Convert.ToInt32(RouteData.Values["id"]);
            /// string name = RouteData.Values["name"].ToString();
            //ViewBag.CategoryName = name;
            List<ItemStroe> itemStroes = new List<ItemStroe>();
            List<Itemdetails> Itemdetails = new List<Itemdetails>();
            DataSet maincat = Db.ItemViewcatewise(cateid, companyid, itemdetails, typeid, Convert.ToDouble(minamt), Convert.ToDouble(maxamt), type);
            if (maincat.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[0].Rows)
                {
                    itemStroes.Add(new ItemStroe
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        ItemName = Convert.ToString(dr["ItemName"]),
                        Frontimage = Convert.ToString(dr["image"]),
                        Backimage = Convert.ToString(dr["image1"]),
                        Rightimage = Convert.ToString(dr["image2"]),
                        Leftimage = Convert.ToString(dr["image3"]),
                        URLName = Convert.ToString(dr["URLName"]),
                        //  MRP = Convert.ToString(dr["MRP"]),
                        // Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                        Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        category_name = Convert.ToString(dr["category_name"]),
                        CategoryId = Convert.ToInt32(dr["CategoryId"]),
                        Disamt = Convert.ToDouble(dr["Disamt"]),
                        //  discount = Convert.ToDouble(dr["discount"]),
                        productTag = Convert.ToString(dr["productTag"]),
                        types = Convert.ToInt32(dr["types"]),
                        send_query = Convert.ToInt32(dr["send_query"]),
                        price = Convert.ToInt32(dr["price"]),
                    });
                }
            }
            viewmodel.itemStroes = itemStroes;

            if (maincat.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[2].Rows)
                {
                    Itemdetails.Add(new Itemdetails
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        itemId = Convert.ToInt32(dr["item_id"]),
                        //Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                        Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        Disamt = Convert.ToDouble(dr["Disamt"]),
                        unitId = Convert.ToInt32(dr["unit_id"])
                    }); ;
                }
            }
            viewmodel.ItemdetailsS = Itemdetails;

            


            var Data = new { itemStroes = viewmodel.itemStroes, ItemdetailsS = viewmodel.ItemdetailsS, count = 2, message = "ok", displaymessage = "" };
            return Json(Data);

        }



        [HttpPost]
        public IActionResult allproductcatewiseasc(int cateid, int typeid, string minamt, string maxamt , int val)
        {
            GetSiteNameAndCode();
            int itemdetails = 0;
            //string type = "4";
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            //int id = Convert.ToInt32(RouteData.Values["id"]);
            /// string name = RouteData.Values["name"].ToString();
            //ViewBag.CategoryName = name;
            List<ItemStroe> itemStroes = new List<ItemStroe>();
            List<Itemdetails> Itemdetails = new List<Itemdetails>();
            DataSet maincat = Db.ItemViewcatewiseasc(cateid, companyid, itemdetails, typeid, Convert.ToDouble(minamt), Convert.ToDouble(maxamt), val);
            if (maincat.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[0].Rows)
                {
                    itemStroes.Add(new ItemStroe
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        ItemName = Convert.ToString(dr["ItemName"]),
                        Frontimage = Convert.ToString(dr["image"]),
                        Backimage = Convert.ToString(dr["image1"]),
                        Rightimage = Convert.ToString(dr["image2"]),
                        Leftimage = Convert.ToString(dr["image3"]),
                        URLName = Convert.ToString(dr["URLName"]),
                        //  MRP = Convert.ToString(dr["MRP"]),
                        // Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                        Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        category_name = Convert.ToString(dr["category_name"]),
                        CategoryId = Convert.ToInt32(dr["CategoryId"]),
                        Disamt = Convert.ToDouble(dr["Disamt"]),
                        itemdetailsId = Convert.ToInt32(dr["ItemId"]),
                        //  discount = Convert.ToDouble(dr["discount"]),
                        productTag = Convert.ToString(dr["productTag"]),
                        types = Convert.ToInt32(dr["types"]),
                        send_query = Convert.ToInt32(dr["send_query"]),
                        price = Convert.ToInt32(dr["price"]),
                        quantity = Convert.ToInt32(dr["stockqty"]),
                    });
                }
            }
            viewmodel.itemStroes = itemStroes;

            if (maincat.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[2].Rows)
                {
                    Itemdetails.Add(new Itemdetails
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        itemId = Convert.ToInt32(dr["item_id"]),
                        //Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                        Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        Disamt = Convert.ToDouble(dr["Disamt"]),
                        unitId = Convert.ToInt32(dr["unit_id"])
                    }); ;
                }
            }
            viewmodel.ItemdetailsS = Itemdetails;




            var Data = new { itemStroes = viewmodel.itemStroes, ItemdetailsS = viewmodel.ItemdetailsS, count = 2, message = "ok", displaymessage = "" };
            return Json(Data);

        }




        [HttpGet]
        public JsonResult GetBrands()
        {
            List<Brand> brands = new List<Brand>();


            DataTable ds = util.execQuery("select brand_id,brand_name from tblbrand");

            if (ds.Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Rows)
                {
                    brands.Add(new  Brand
                    {
                        brand_id = Convert.ToInt32(dr["brand_id"]),
                        brand_name = Convert.ToString(dr["brand_name"]),
                    });
                }
            }

            return Json(brands);
        }

        [HttpPost]
        public IActionResult brandproduct(int cateid, int typeid,int brandcheckbox)
        {
            GetSiteNameAndCode();
            int itemdetails = 0;
            string type = "5";
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            //int id = Convert.ToInt32(RouteData.Values["id"]);
            /// string name = RouteData.Values["name"].ToString();
            //ViewBag.CategoryName = name;
            List<ItemStroe> itemStroes = new List<ItemStroe>();
            List<Itemdetails> Itemdetails = new List<Itemdetails>();
            DataSet maincat = Db.branditemwise(cateid, companyid, itemdetails, typeid, type,brandcheckbox);
            if (maincat.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[0].Rows)
                {
                    itemStroes.Add(new ItemStroe
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        ItemName = Convert.ToString(dr["ItemName"]),
                        Frontimage = Convert.ToString(dr["image"]),
                        Backimage = Convert.ToString(dr["image1"]),
                        Rightimage = Convert.ToString(dr["image2"]),
                        Leftimage = Convert.ToString(dr["image3"]),
                        URLName = Convert.ToString(dr["URLName"]),
                        //  MRP = Convert.ToString(dr["MRP"]),
                        // Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                        Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        category_name = Convert.ToString(dr["category_name"]),
                        // Disamt = Convert.ToDouble(dr["Disamt"]),
                        //  discount = Convert.ToDouble(dr["discount"]),
                        productTag = Convert.ToString(dr["productTag"]),
                        types = Convert.ToInt32(dr["types"]),
                        send_query = Convert.ToInt32(dr["send_query"]),
                        price = Convert.ToInt32(dr["price"]),
                    });
                }
            }
            viewmodel.itemStroes = itemStroes;

            if (maincat.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[1].Rows)
                {
                    Itemdetails.Add(new Itemdetails
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        itemId = Convert.ToInt32(dr["item_id"]),
                        Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                        Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                        Disamt = Convert.ToDouble(dr["Disamt"]),
                        unitId = Convert.ToInt32(dr["unit_id"])
                    }); ;
                }
            }
            viewmodel.ItemdetailsS = Itemdetails;


            var Data = new { itemStroes = viewmodel.itemStroes, ItemdetailsS = viewmodel.ItemdetailsS, count = 2, message = "ok", displaymessage = "" };
            return Json(Data);


            
        }

        [HttpPost]
        public IActionResult Addcart(int productId, int quantity, int itemdetid, string type = "")
        {
             GetSiteNameAndCode();
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();

            string messagealert = "";
            if (type == "5")
            {
                sqlquery = "update tblWatchlist set use_flg = 1  where ID=" + itemdetid + " and ProductID=" + productId + "";
                status = util.MultipleTransactions(sqlquery);
            }
            DataTable dtproduct = Db.getproduct(companyid, productId, itemdetid);
            if (quantity == 0)
            {
                quantity = 1;
            }
            Cart cart = new Cart();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("CartItems")))
            {
                List<Cart> carts = new List<Cart>();
                if (dtproduct.Rows.Count > 0)
                {
                    foreach (DataRow drproduct in dtproduct.Rows)
                    {
                        if (Convert.ToInt32(drproduct["stockqty"]) >= quantity)
                        {
                            cart.productid = Convert.ToInt32(drproduct["Id"]);
                            cart.itemdetid = Convert.ToInt32(itemdetid);
                            cart.productname = Convert.ToString(drproduct["ItemName"]);
                            cart.productimage = Convert.ToString(drproduct["image"]);
                            cart.productprice = Convert.ToDouble(drproduct["Disamt"]);
                            cart.unitdesc = Convert.ToString(drproduct["Unit_Qty"]);
                            cart.shipcharge = Convert.ToDouble(drproduct["ship_charge"]);
                            cart.CategoryId = Convert.ToString(drproduct["CategoryId"]);
                            cart.stockqty = Convert.ToString(drproduct["stockqty"]);
                            cart.quantity = quantity;
                            cart.totalprice += Convert.ToDouble((cart.quantity) * (cart.productprice));
                            carts.Add(cart);
                            messagealert = "Your product added in cart.";
                            HttpContext.Session.SetString("CartItems", JsonConvert.SerializeObject(carts));
                            HttpContext.Session.SetInt32("count", 1);
                        }
                        else
                        {
                            string stc = drproduct["stockqty"].ToString();
                            messagealert = "Only " + stc + " Stock aviable for this product";
                        }
                    }
                }

                //if (messagealert == "Your product added in cart.")
                //{



                //    StringBuilder sBody = new StringBuilder();
                //    string MailStatus = "";
                //    var DateTime = new DateTime();
                //    DateTime = DateTime.Now;
                //    sBody.Append("<!DOCTYPE html><html><body>");
                //    sBody.Append("<p>Dear " + HttpContext.Session.GetString("customerfirstname") + " " + HttpContext.Session.GetString("customerlastname") + "," + Environment.NewLine);
                //    sBody.Append("<p>" + Environment.NewLine);
                //    sBody.Append("<p>It looks like you left some items in your cart. Complete your purchase now before they're gone!" + Environment.NewLine);
                //    //sBody.Append("<p>User Name: " + email + "" + Environment.NewLine);
                //    //  sBody.Append("<p>Contact Info: " + Mobile + "" + Environment.NewLine);
                //    //sBody.Append("<p>Email: " + email + "" + Environment.NewLine);
                //    sBody.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
                //    sBody.Append("<p>" + Environment.NewLine);
                //    sBody.Append("<p>[Link to Cart]" + Environment.NewLine);
                //    sBody.Append("<p>" + Environment.NewLine);
                //    sBody.Append("<p>If you need any assistance, feel free to contact us." + Environment.NewLine);
                //    sBody.Append("<p>" + Environment.NewLine);
                //    sBody.Append("<p>Best Regards</p>");
                //    sBody.Append("<p><b>The BSD Infotech Online Store Team</b></p>");
                //    sBody.Append("</body></html>");
                //    MailStatus = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + HttpContext.Session.GetString("customeremail") + "", "", "", "Subject : Don't Forget About Your Cart!", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");

                //}
                //if (messagealert == "Your product added in cart.")
                //{

                //    StringBuilder sBody1 = new StringBuilder();
                //    string MailStatus1 = "";
                //    var DateTime = new DateTime();
                //    DateTime = DateTime.Now;
                //    sBody1.Append("<!DOCTYPE html><html><body>");
                //    sBody1.Append("<p>Dear Support Team," + Environment.NewLine);
                //    sBody1.Append("<p> Order Delivered " + Environment.NewLine);
                //    sBody1.Append("<p>" + Environment.NewLine);
                //    sBody1.Append("<p>Thank you for your continued trust in our services." + Environment.NewLine);
                //    sBody1.Append("<p>Best Regards</p>");
                //    sBody1.Append("<p><b>Support Team</b></p>");
                //    sBody1.Append("</body></html>");
                //    string email1 = "bsddemos@gmail.com";
                //    MailStatus1 = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject :  Don't Forget About Your Cart!", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");


                //}
                

            }
            else
            {
                List<Cart> carts = (List<Cart>)JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("CartItems"));
                bool cechkpexist = false;
                int cartscount = carts.Count;
                int i = 0, j = 0;
                if (dtproduct.Rows.Count > 0)
                {
                    foreach (DataRow drproduct in dtproduct.Rows)
                    {
                        for (i = 0; i < cartscount; i++)
                        {
                            // carts.

                            if (carts[i].productid == productId && carts[i].itemdetid == itemdetid)
                            {

                                cechkpexist = true;
                                if (type == "2")
                                {
                                    if (Convert.ToInt32(drproduct["stockqty"]) >= (quantity + 1))
                                    {
                                        carts[i].quantity += 1;
                                        carts[i].productprice = Convert.ToInt32(drproduct["Disamt"]);
                                        carts[i].totalprice = Convert.ToDouble((carts[i].quantity) * (carts[i].productprice));
                                        messagealert = "";
                                    }
                                    else
                                    {
                                        string stc = drproduct["stockqty"].ToString();

                                        messagealert = "Only " + stc + " Stock aviable for this product";
                                    }

                                }
                                else if (type == "1")
                                {
                                    if (carts[i].quantity > 1)
                                        carts[i].quantity -= 1;
                                    carts[i].productprice = Convert.ToInt32(drproduct["Disamt"]);
                                    carts[i].totalprice = Convert.ToDouble((carts[i].quantity) * (carts[i].productprice));
                                    messagealert = "";
                                    cechkpexist = true;
                                }
                                else if (type == "3")
                                {
                                    carts.RemoveAt(i);

                                    HttpContext.Session.SetInt32("count", Convert.ToInt32((HttpContext.Session.GetInt32("count") - 1)));
                                    messagealert = "";
                                    break;
                                }
                                else
                                {
                                    messagealert = "Your Product already added in cart.";
                                }


                            }
                        }

                        if (cechkpexist == false)
                        {
                            if (Convert.ToInt32(drproduct["stockqty"]) >= quantity)
                            {
                                cart.productid = Convert.ToInt32(drproduct["Id"]);
                                cart.itemdetid = Convert.ToInt32(itemdetid);
                                cart.productname = Convert.ToString(drproduct["ItemName"]);
                                cart.productimage = Convert.ToString(drproduct["image"]);
                                cart.productprice = Convert.ToInt32(drproduct["Disamt"]);
                                cart.unitdesc = Convert.ToString(drproduct["Unit_Qty"]);
                                cart.shipcharge = Convert.ToDouble(drproduct["ship_charge"]);
                                cart.stockqty = Convert.ToString(drproduct["stockqty"]);
                                cart.quantity = quantity;
                                cart.totalprice += Convert.ToDouble((cart.quantity) * (cart.productprice));
                                carts.Add(cart);
                                HttpContext.Session.SetInt32("count", Convert.ToInt32((HttpContext.Session.GetInt32("count") + 1)));
                                messagealert = "Your Product added in cart.";
                            }
                            else
                            {
                                string stc = drproduct["stockqty"].ToString();

                                messagealert = "Only " + stc + " Stock available for this product";
                            }
                        }
                    }
                }
                HttpContext.Session.SetString("CartItems", JsonConvert.SerializeObject(carts));
            }
            int count = Convert.ToInt32(HttpContext.Session.GetInt32("count"));
            if (count > 0)

            {
                var dt1 = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("CartItems"));
                var Data = new { carts = dt1, count = count, message = "add cart added.", displaymessage = messagealert,type=type };
                return Json(Data);
            }
            else
            {
                var dt1 = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("CartItems"));
                var Data = new { carts = "", count = count, message = "add cart not added.", displaymessage = messagealert, type = type };
                return Json(Data);
            }


        }

        [HttpPost]
        public JsonResult sendmailafter15minutes()
        {
            StringBuilder sBody = new StringBuilder();
            string MailStatus = "";
            var DateTime = new DateTime();
            DateTime = DateTime.Now;
            sBody.Append("<!DOCTYPE html><html><body>");
            sBody.Append("<p>Dear " + HttpContext.Session.GetString("customerfirstname") + " " + HttpContext.Session.GetString("customerlastname") + "," + Environment.NewLine);
            sBody.Append("<p>" + Environment.NewLine);
            sBody.Append("<p>It looks like you left some items in your cart. Complete your purchase now before they're gone!" + Environment.NewLine);
            //sBody.Append("<p>User Name: " + email + "" + Environment.NewLine);
            //  sBody.Append("<p>Contact Info: " + Mobile + "" + Environment.NewLine);
            //sBody.Append("<p>Email: " + email + "" + Environment.NewLine);
            //sBody.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
            //sBody.Append("<p>" + Environment.NewLine);
            //sBody.Append("<p>[Link to Cart]" + Environment.NewLine);
            sBody.Append($"<a href='https://localhost:44345/cart'>Click here</a>");
            sBody.Append("<p>" + Environment.NewLine);
            sBody.Append("<p>If you need any assistance, feel free to contact us." + Environment.NewLine);
            sBody.Append("<p>" + Environment.NewLine);
            sBody.Append("<p>Best Regards,</p>");
            sBody.Append("<p><b>The BSD Shop Team</b></p>");
            sBody.Append("</body></html>");
            MailStatus = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + HttpContext.Session.GetString("customeremail") + "", "", "", "Subject : Don't Forget About Your Cart!", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");


            //StringBuilder sBody1 = new StringBuilder();
            //string MailStatus1 = "";
            
            //sBody1.Append("<!DOCTYPE html><html><body>");
            //sBody1.Append("<p>Dear Support Team," + Environment.NewLine);
            //sBody1.Append("<p> Order Delivered " + Environment.NewLine);
            //sBody1.Append("<p>" + Environment.NewLine);
            //sBody1.Append("<p>Thank you for your continued trust in our services." + Environment.NewLine);
            //sBody1.Append("<p>Best Regards</p>");
            //sBody1.Append("<p><b>Support Team</b></p>");
            //sBody1.Append("</body></html>");
            //string email1 = "bsddemos@gmail.com";
            //MailStatus1 = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject :  Don't Forget About Your Cart!", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");




            return Json(MailStatus);

        }





        [HttpPost]
        public JsonResult GetItemName(int Type)
        {
            DataTable dt = new DataTable();
            //sqlquery = "select Main_cat_id,Main_cat_name  from tblitem_Main_category ";
            sqlquery = "select category_id,category_name,Main_cat_id  from tblcategory  ";
            DataSet ds = util.TableBind(sqlquery);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return Json(JsonConvert.SerializeObject(dt));
        }


        [HttpPost]
        public JsonResult Hdfc(String item)
        {
            DataTable dt = new DataTable();
           
            sqlquery = "select category_id,category_name,Main_cat_id  from tblcategory WHERE  category_name='"+ item + "' ";
            DataSet ds = util.TableBind(sqlquery);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return Json(JsonConvert.SerializeObject(dt));
        }


        [HttpPost]
        public JsonResult PrOsearchid(String item, int cid)
        {
            
            DataTable dt = new DataTable();


            //sqlquery = "SELECT ID ,ItemName,CategoryID FROM tblItemStore   WHERE ItemName='" + item + "' AND CategoryID=" + cid + " ";
            if (cid == 0) 
            { 
                sqlquery = "SELECT a.ID ,a.ItemName,a.CategoryID ,a.groupid,Main_cat_name FROM tblItemStore a join tblitem_Main_category b on a.groupid=b.Main_cat_id WHERE  ItemName   LIKE '%" + item + "%'"; 
            }else
            {
                sqlquery = "SELECT a.ID ,a.ItemName,a.CategoryID ,a.groupid,Category_name FROM tblItemStore a join tblCategory b on a.CategoryID=b.Category_id WHERE  ItemName LIKE '%" + item + "%'AND CategoryID=" + cid + " ";
            }
           
            
            DataSet ds = util.TableBind(sqlquery);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return Json(JsonConvert.SerializeObject(dt));
        }




        [HttpPost]
        public JsonResult Shortbynameasc( int cid)
        {

            DataTable dt = new DataTable();


            //sqlquery = "SELECT ID ,ItemName,CategoryID FROM tblItemStore   WHERE ItemName='" + item + "' AND CategoryID=" + cid + " ";
            if (cid == 0)
            {
                //sqlquery = "SELECT a.ID ,a.ItemName,a.CategoryID ,a.groupid,Main_cat_name FROM tblItemStore a join tblitem_Main_category b on a.groupid=b.Main_cat_id WHERE  ItemName   LIKE '%" + item + "%'";
                sqlquery = "SELECT a.ID ,a.ItemName,a.CategoryID ,a.groupid,Main_cat_name FROM tblItemStore a join tblitem_Main_category b on a.groupid=b.Main_cat_id order by   ItemName   asc";
            }
            else
            {
                sqlquery = "SELECT a.ID ,a.ItemName,a.CategoryID ,a.groupid,Category_name FROM tblItemStore a join tblCategory b on a.CategoryID=b.Category_id WHERE    CategoryID=" + cid + " order by ItemName asc  ";
            }


            DataSet ds = util.TableBind(sqlquery);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return Json(JsonConvert.SerializeObject(dt));
        }


        public JsonResult itemrate(int id, int itemid)
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet dsitemView = Db.Itemrate(id, companyid, itemid);
            List<Itemdetails> Itemdetails = new List<Itemdetails>();
            foreach (DataRow dr in dsitemView.Tables[0].Rows)
            {
                Itemdetails.Add(new Itemdetails
                {
                    Id = Convert.ToInt32(dr["id"]),
                    itemId = Convert.ToInt32(dr["item_id"]),
                    Unit_Qty = Convert.ToString(dr["Unit_Qty"]),
                    Unit_Rate = Convert.ToString(dr["Unit_Rate"]),
                    Disamt = Convert.ToDouble(dr["Disamt"]),
                    discount = Convert.ToDouble(dr["discount"]),
                });
            }
            viewmodel.ItemdetailsS = Itemdetails;
            return Json(Itemdetails);
        }
        [HttpPost]
        public IActionResult Viewcart()
        {
            GetSiteNameAndCode();
            DataTable dt = new DataTable();
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            StringBuilder str = new StringBuilder();
            DataSet dsviewcart = Db.ViewCart(companyid);
            int subtotal = 0;
            str.Append("<table id='cartView' cellpadding='0' cellspacing='0' border='0' width='100%'>".ToString());
            str.Append("<tr class='rd-brdr'>".ToString());
            str.Append("<th width='15%'>Image</th>".ToString());
            str.Append("<th width='40%'>Product</th>".ToString());
            str.Append("<th width='15%'>Price</th>".ToString());
            str.Append("<th width='15%'>Quantity</th>".ToString());
            str.Append("<th width='15%'>Total</th>".ToString());
            str.Append("<th width='15%'>Action</th>".ToString());
            str.Append(" </tr>".ToString());
            str.Append(" </tr>".ToString());
            if (dsviewcart.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsviewcart.Tables[0].Rows)
                {
                    str.Append("<tr>".ToString());
                    str.Append(" <td><img src='/images/Productimage/" + dr["pimage"].ToString() + "' Width='80px' Height='60px'/></td>").ToString();
                    str.Append("<td>".ToString());
                    str.Append("" + dr["itemname"].ToString() + "".ToString());
                    str.Append(" </td>".ToString());
                    str.Append(" <td>RS. " + dr["price"].ToString() + "</td>".ToString());
                    //str.Append("<td> " + dt.Rows[j]["Quantity"].ToString() + " </td>".ToString());
                    str.Append("<td><span class='input-group-btn'>").ToString();
                    //  str.Append("<button type='button' class='btn btn-default btn-number' style='float: left; margin-left: -40px; margin-right: 10px;'  data-type='minus' id='" + dr["productid"].ToString() + "'  data-field='" + (dr["productid"].ToString()) + "'>").ToString();
                    //str.Append("<span class='glyphicon glyphicon-minus'></span>").ToString();
                    // str.Append(" </button>").ToString();
                    str.Append("</span>").ToString();

                    str.Append("<input type='text' name='" + (dr["productid"].ToString()) + "' class='form-control input-number' id='" + dr["productid"].ToString() + "' value='" + dr["Quantity"].ToString() + "' style='width: 45px;margin-top: -34px;    float: left;' min='1' max='5'>").ToString();

                    //str.Append("<span class='input-group-btn'>").ToString();
                    // str.Append("<button type='button' class='btn btn-default btn-number' style='margin-left: 0px;    margin-top: -34px;    margin-right: 60px; float: right;'  data-type='plus' id='" + dr["productid"].ToString() + "' data-field='" + (dr["productid"].ToString()) + "'>").ToString();
                    // str.Append("<span class='glyphicon glyphicon-plus'></span>").ToString();
                    str.Append("</button>").ToString();
                    str.Append("</span></td>").ToString();
                    str.Append("<td>RS. " + dr["Total"].ToString() + "</td>".ToString());
                    //str.Append("<td><a class='btn btn-danger' href='cart?id=" + dr["productid"].ToString() + "'>Remove</a></td>".ToString());
                    str.Append("<td><a class='btn btn-danger' href='' onclick='DeleteProduct(" + dr["productid"] + ")'>Remove</a></td>".ToString());
                    //<span><i class='fa fa-times' aria-hidden='true'></i></span>
                    str.Append("</tr>".ToString());
                }
            }
            else
            {
                str.Append("<tr>".ToString());
                str.Append("<td colspan='4'>Empty Cart</td>".ToString());
                str.Append("<tr>".ToString());
            }
            str.Append("</table>".ToString());
            return this.Content(str.ToString());
        }
        [HttpPost]
        public JsonResult DeleteViewCart(int productid, int itemdetid)
        {
            GetSiteNameAndCode();
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();

            status = Db.deleteProductcart(companyid, productid, 0);
            var Data = new { msg = status };
            return Json(Data);
        }
        [HttpGet]
        [Route("welfaredetails/{id}")]
        // [Route("welfaredetails")]
        public IActionResult WelfareDetails()
        {
            GetSiteNameAndCode();
            int id = Convert.ToInt32(RouteData.Values["id"]);
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet maincat = Db.welfaredet(companyid, id);
            List<welfare> welfares = new List<welfare>();
            if (maincat.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[0].Rows)
                {
                    welfares.Add(new welfare
                    {
                        id = Convert.ToInt32(dr["id"]),
                        Welfare_Name = Convert.ToString(dr["Welfare_Name"]),
                        img = Convert.ToString(dr["img"]),
                        contactNo = Convert.ToString(dr["contactNo"]),
                        Email = Convert.ToString(dr["Email"]),
                        content = Convert.ToString(dr["content"]),
                    });
                }
            }
            viewmodel.Welfares = welfares;
            return View(viewmodel);
        }

        [HttpGet]

        public IActionResult blog()
        {
            GetSiteNameAndCode();
            //    int id = Convert.ToInt32(RouteData.Values["id"]);
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet maincat = Db.blogdet(companyid);
            List<Models.blog> welfares = new List<Models.blog>();
            if (maincat.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[0].Rows)
                {
                    welfares.Add(new Models.blog
                    {
                        id = Convert.ToInt32(dr["id"]),
                        ShortDesc = Convert.ToString(dr["ShortDesc"]),
                        img = Convert.ToString(dr["BlogImage"]),
                        LongDesc = Convert.ToString(dr["LongDesc"]),

                    });
                }
            }
            viewmodel.blogs = welfares;
            return View(viewmodel);
        }
        //[HttpGet]
        ////[Route("welfaredetails/{id}/{name}")]
        //[Route("welfaredetails")]
        //public IActionResult WelfareDetails()
        //{
        //    GetSiteNameAndCode();
        //    string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
        //    DataSet maincat = Db.FrontPage(companyid);
        //    List<welfare> welfares = new List<welfare>();
        //    if (maincat.Tables[4].Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in maincat.Tables[4].Rows)
        //        {
        //            welfares.Add(new welfare
        //            {
        //                id = Convert.ToInt32(dr["id"]),
        //                Welfare_Name = Convert.ToString(dr["Welfare_Name"]),
        //                img = Convert.ToString(dr["img"]),
        //                contactNo = Convert.ToString(dr["contactNo"]),
        //                Email = Convert.ToString(dr["Email"]),
        //                content = Convert.ToString(dr["content"]),
        //            });
        //        }
        //    }
        //    viewmodel.Welfares = welfares;
        //    return View(viewmodel);
        //}
        [HttpGet]
        [Route("locality-details/{id}/{name}")]
        public IActionResult localitydetails()
        {
            GetSiteNameAndCode();
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            string id = RouteData.Values["id"].ToString();
            string LocalityName = RouteData.Values["name"].ToString();
            ViewBag.sevename = LocalityName;
            int pincode = 0;
            HttpContext.Session.SetString("LocalityName", LocalityName);
            HttpContext.Session.SetString("Localityid", id);
            DataSet dsLocDetails = Db.LocalityDetails(companyid, id, pincode);
            List<Locality_service> loc = new List<Locality_service>();
            if (dsLocDetails.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsLocDetails.Tables[0].Rows)
                {
                    loc.Add(new Locality_service
                    {
                        id = Convert.ToInt32(dr["LocalityId"]),
                        Loc_serviceName = Convert.ToString(dr["Locality_Name"]),
                        content = Convert.ToString(dr["Locality_content"]),
                        image = Convert.ToString(dr["LocalityImage"]),
                        LocDetails_name = Convert.ToString(dr["LocDetails_name"]),
                        contact_name = Convert.ToString(dr["contact_name"]),
                        locdetails_Img = Convert.ToString(dr["locdetails_Img"]),
                        Locdetails_content = Convert.ToString(dr["Locdetails_content"]),
                        pincode = Convert.ToString(dr["pincode"]),
                        Loc_address = Convert.ToString(dr["Loc_address"]),
                        mobile_no = Convert.ToString(dr["mobile"]),
                        phone = Convert.ToString(dr["phone"]),
                        Loc_detailsId = Convert.ToInt32(dr["Loc_detailsId"]),
                    });
                }
            }
            viewmodel.locality = loc;
            return View(viewmodel);
        }
        [HttpPost]
        public JsonResult LocalityServicesEnquiry(int LocalityId, int locdetailsId, string Fullname, string phone, string email, string description)
        {
            string functionReturnValue = null;
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            string domainname = HttpContext.Session.GetString("SiteName");
            if (Db.Insert_LocalityServices_enquiry(LocalityId, locdetailsId, Fullname, phone, email, description, companyid))
            {
                functionReturnValue = Db.sendAdminmail(util.FixQuotes(Fullname.Trim()), "", email.Trim(), util.FixQuotes(phone.Trim()), util.FixQuotes(description.Trim()), "Locality Service Enquiry", companyid, domainname);
                if (functionReturnValue == "Sent")
                {
                    message = "send enquiry.";
                }
                else
                {
                    // message = "send enquiry.";
                    message = "Invalid mail.";
                }
            }
            var Data = new { message = message };
            return Json(Data);
        }

        [HttpGet]
        public JsonResult LocalityBindByPincode(int pincode)
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            string id = HttpContext.Session.GetString("Localityid");
            //string LocalityName = RouteData.Values["name"].ToString();
            //HttpContext.Session.SetString("LocalityName", LocalityName);
            DataSet dsLocDetails = Db.LocalityDetails(companyid, id, pincode);
            List<Locality_service> loc = new List<Locality_service>();
            if (dsLocDetails.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in dsLocDetails.Tables[1].Rows)
                {
                    loc.Add(new Locality_service
                    {
                        id = Convert.ToInt32(dr["LocalityId"]),
                        Loc_serviceName = Convert.ToString(dr["Locality_Name"]),
                        content = Convert.ToString(dr["Locality_content"]),
                        image = Convert.ToString(dr["LocalityImage"]),
                        LocDetails_name = Convert.ToString(dr["LocDetails_name"]),
                        contact_name = Convert.ToString(dr["contact_name"]),
                        locdetails_Img = Convert.ToString(dr["locdetails_Img"]),
                        Locdetails_content = Convert.ToString(dr["Locdetails_content"]),
                        pincode = Convert.ToString(dr["pincode"]),
                        Loc_address = Convert.ToString(dr["Loc_address"]),
                        mobile_no = Convert.ToString(dr["mobile"]),
                        phone = Convert.ToString(dr["phone"]),
                        Loc_detailsId = Convert.ToInt32(dr["Loc_detailsId"]),
                    });
                }
            }
            viewmodel.locality = loc;
            return Json(viewmodel);
        }
        [HttpPost]
        public JsonResult SaveReview(int Item_id, string Customer_review, string Customer_name, string Customer_Email, int rating)
        {
            DataTable dtreview = new DataTable();
            int companyid = Convert.ToInt32(HttpContext.Session.GetInt32("SiteId"));
            if (Db.SaveCustomer_Review(Item_id, util.FixQuotes(Customer_review.Trim()), util.FixQuotes(Customer_name.Trim()), util.FixQuotes(Customer_review.Trim()), rating, companyid))
            {
                message = "Review Submitted";
            }
            else
            {
                message = "Review Submission Failed";
            }
            var Data = new { message = message };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult Savewishlist(string Item_id, string itemdetid)
        {
            
            DataTable dtreview = new DataTable();
            //int companyid = Convert.ToInt32(HttpContext.Session.GetInt32("SiteId"));
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            string customerid = HttpContext.Session.GetInt32("customerid").ToString();

            DataTable dtlogo = util.execQuery("select * from [tblWatchlist] where ProductID='" + Item_id + "' and use_flg=0");

            if (dtlogo.Rows.Count > 0)
            {

                message = "Item already in wishlist";

            }
            else
            {
                if (Db.addwishlist(Item_id, itemdetid, companyid, customerid))
                {
                    message = "Add Your item in Wishlist";
                }

                else
                {
                    message = "Wishlist Submission Failed";
                }
            }
            var Data = new { message = message };
            return Json(Data);
        }
        [HttpPost]
        public JsonResult delwishlist(string Item_id, string itemdetid)
        {
            DataTable dtreview = new DataTable();
            //int companyid = Convert.ToInt32(HttpContext.Session.GetInt32("SiteId"));
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            string customerid = HttpContext.Session.GetInt32("customerid").ToString();

            if (Db.delwishlist(Item_id, itemdetid, companyid, customerid))
            {
                message = "deleted";
            }
            else
            {
                message = "Wishlist Submission Failed";
            }
            var Data = new { message = message };
            return Json(Data);
        }

        [HttpGet]
        [Route("wishlist")]
        public IActionResult wishlist()
        {
            GetSiteNameAndCode();
            List<ItemStroe> itemStroes = new List<ItemStroe>();
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            string customerid = HttpContext.Session.GetInt32("customerid").ToString();
            DataSet maincat = Db.wishlistitem(customerid, companyid);
            if (maincat.Tables.Count > 0)
            {
                if (maincat.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in maincat.Tables[0].Rows)
                    {
                        itemStroes.Add(new ItemStroe
                        {
                            Id = Convert.ToInt32(dr["id"]),
                            ItemName = Convert.ToString(dr["ItemName"]),
                            Frontimage = Convert.ToString(dr["image"]),
                            Backimage = Convert.ToString(dr["image1"]),
                            Rightimage = Convert.ToString(dr["image2"]),
                            Leftimage = Convert.ToString(dr["image3"]),
                            // URLName = Convert.ToString(dr["URLName"]),
                            Disamt = Convert.ToDouble(dr["Disamt"]),
                            sbalance = Convert.ToInt32(dr["balanced"]),
                            //itemdetailsId = Convert.ToInt32(dr["itemdetid"]),
                            itemdetailsId = Convert.ToInt32(dr["ItemId"]),
                            // productTag = Convert.ToString(dr["productTag"]),
                        });
                    }
                }
                viewmodel.itemStroes = itemStroes;

                List<SelectListItem> items = new List<SelectListItem>();
                DataSet ds = util.BindDropDown("select id,state_name from tblstate");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        items.Add(new SelectListItem { Value = dr["id"].ToString(), Text = dr["state_name"].ToString() });

                    }
                    ViewBag.Bindstate = items;

                }
            }
            return View(viewmodel);

        }

        //    [HttpGet]
        //[Route("CheckOut")]
        //public IActionResult CheckOut()
        //{
        //    customer cs = new customer();
        //    GetSiteNameAndCode();
        //    if (string.IsNullOrEmpty(HttpContext.Session.GetString("customerid")))
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    else
        //    {
        //        cs.id = HttpContext.Session.GetString("customerid");
        //        cs.firstname = HttpContext.Session.GetString("customerfirstname");
        //        cs.lastname = HttpContext.Session.GetString("customerlastname");
        //        cs.MobileNo = HttpContext.Session.GetString("customerMobileNo");
        //        cs.email = HttpContext.Session.GetString("customeremail");
        //        Country();
        //        ViewBag.cart = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("CartItems"));
        //        ViewBag.count = (HttpContext.Session.GetInt32("count"));
        //    }
        //    return View(cs);
        //}
        //[HttpPost]
        //public JsonResult checkout(int id, double amount, string firstname, string lastname, string address1, string address2, int countryId, int stateId, int city, string pincode, string phone, string email, string addtionalinfo, string Sfirstname, string Slastname, string Saddress1, string Saddress2, int ScountryId, int SstateId, int ScityId, string Spincode, string paymentmode, string smobile, string Semail)
        //{
        //    int customerid = Convert.ToInt32(HttpContext.Session.GetInt32("customerid"));
        //    int companyid = Convert.ToInt32(HttpContext.Session.GetInt32("SiteId"));
        //    string dob = "";
        //    int discount = 0, walletAmtdetect = 0;
        //    if (Db.IU_Checkout(id, address1, address2, city, pincode, countryId, stateId, ScityId, ScountryId, SstateId, ScityId, Saddress1, Saddress2, smobile, Sfirstname, firstname, addtionalinfo))
        //    {
        //        if (Db.IU_Order(id, amount, Sfirstname, Slastname, Saddress1, Saddress2, SstateId, ScityId, Spincode, ScountryId, smobile, Semail, firstname, lastname, address1, address2, stateId, city, pincode, countryId, phone, email, dob, paymentmode, discount, walletAmtdetect, companyid))
        //        {
        //            message = "order place save.";
        //        }
        //    }
        //    var Data = new { message = message };
        //    return Json(Data);
        //}

        //[HttpGet]
        //[Route("/thankyou")]
        //public IActionResult ThankYou()
        //{
        //    return View();
        //  }
        [HttpPost]
        public IActionResult proceedcheckout()
        {
            //string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            // string customerid = HttpContext.Session.GetString("customerid");
            //DataTable dt = util.execQuery("select * from tblorder where user_id ='" + HttpContext.Session.GetString("customerid") + "' and order_id = '" + id + "'");



            if (string.IsNullOrEmpty(HttpContext.Session.GetString("customerid")))
            {
                message = "Please Login.";
            }

            var Data = new { message = message };
            //if(message == "")
            //{





            //    StringBuilder sBody = new StringBuilder();
            //    string MailStatus = "";
            //    var DateTime = new DateTime();
            //    DateTime = DateTime.Now;
            //    sBody.Append("<!DOCTYPE html><html><body>");
            //    sBody.Append("<p>Dear "+ HttpContext.Session.GetString("customerfirstname") + HttpContext.Session.GetString("customerlastname") + Environment.NewLine);
            //    sBody.Append("<p>Thank you for your purchase! Your order has been successfully placed." + Environment.NewLine);
            //    sBody.Append("<p>You will receive another email once your order has been dispatched." + Environment.NewLine);
            //    sBody.Append("<p>You can view your order details and track your shipment here:" + Environment.NewLine);
            //    sBody.Append("<p>[Order Tracking Link]</p>");
            //    sBody.Append("<p>We appreciate your business!</p>");
            //    sBody.Append("<p>Best Regards</p>");
            //    sBody.Append("<p><b>The BSD Infotech Online Store Team</b></p>");
            //    sBody.Append("</body></html>");
            //    MailStatus = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + HttpContext.Session.GetString("customeremail") +"", "", "", "Subject : Order Confirmation - ", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");
            //}
            //if (message == "")
            //{
            //    //DataTable dtproduct = Db.getproduct(companyid, productId, itemdetid);
            //    string CartItems = HttpContext.Session.GetString("CartItems");
            //    JArray objects = JArray.Parse((CartItems == "" ? "[]" : CartItems));
            //    string productname = objects[0]["productname"].ToString();
            //    StringBuilder sBody1 = new StringBuilder();

            //    string MailStatus1 = "";
            //    var DateTime = new DateTime();
            //    DateTime = DateTime.Now;
            //    sBody1.Append("<!DOCTYPE html><html><body>");
            //    sBody1.Append("<p>Dear Support Team," + Environment.NewLine);
            //    sBody1.Append("<p> A new query has been generated. Please check the below details " + Environment.NewLine);
            //    sBody1.Append("<p>Customer Id: " + HttpContext.Session.GetString("customerid") + "" + Environment.NewLine);
            //    sBody1.Append("<p>Customer First Name: " + HttpContext.Session.GetString("customerfirstname") + "" + Environment.NewLine);
            //    sBody1.Append("<p>Customer Last Name: " + HttpContext.Session.GetString("customerlastname") + "" + Environment.NewLine);
            //    sBody1.Append("<p>Contact Info: " + HttpContext.Session.GetString("customerMobileNo") + "" + Environment.NewLine);
            //    sBody1.Append("<p>Email: " + HttpContext.Session.GetString("customeremail") + "" + Environment.NewLine);
            //    sBody1.Append("<p>Product Name: " + productname + "" + Environment.NewLine);
            //    sBody1.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
            //    sBody1.Append("<p>" + Environment.NewLine);
            //    sBody1.Append("<p>Thank you for your continued trust in our services." + Environment.NewLine);
            //    sBody1.Append("<p>Best Regards</p>");
            //    sBody1.Append("<p><b>Support Team</b></p>");
            //    sBody1.Append("</body></html>");
            //    string email1 = "bsddemos@gmail.com";
            //    MailStatus1 = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : Customer Query", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");

            //}
            return Json(Data);
        }
        #region Footer
        #region About
        [HttpGet]
        public IActionResult About()
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet dsAbout = Db.Footer(companyid);
            List<Models.About> abouts = new List<Models.About>();
            if (dsAbout.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsAbout.Tables[0].Rows)
                {
                    abouts.Add(new Models.About
                    {
                        id = Convert.ToInt32(dr["id"]),
                        AboutText = Convert.ToString(dr["About"]),
                    });
                }
            }
            viewmodel.abouts = abouts;
            return View(viewmodel);
        }
        #endregion

        #region DeliveryInformation
        [HttpGet]
        [Route("DeliveryInformation")]
        public IActionResult DeliveryInformation()
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet dsdel = Db.Footer(companyid);
            List<Models.DeliveryInformation> del_info = new List<Models.DeliveryInformation>();
            if (dsdel.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in dsdel.Tables[1].Rows)
                {
                    del_info.Add(new Models.DeliveryInformation
                    {
                        id = Convert.ToInt32(dr["id"]),
                        Delivery = Convert.ToString(dr["Delivery_info"]),
                    });
                }
            }
            viewmodel.deliveryInfo = del_info;
            return View(viewmodel);
        }
        #endregion

        #region PaymentInformation
        [HttpGet]
        [Route("PaymentInformation")]
        public IActionResult PaymentInformation()
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet dspayment = Db.Footer(companyid);
            List<paymentInfo> paymentinfos = new List<paymentInfo>();
            if (dspayment.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow dr in dspayment.Tables[2].Rows)
                {
                    paymentinfos.Add(new paymentInfo
                    {
                        id = Convert.ToInt32(dr["id"]),
                        paymentcontent = Convert.ToString(dr["paymentContent"]),
                    });
                }
            }
            viewmodel.paymentInfos = paymentinfos;
            return View(viewmodel);
        }
        #endregion

        #region FAQ
        [HttpGet]
        [Route("FAQ")]
        public IActionResult FAQ()
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet dsFAQ = Db.Footer(companyid);
            List<Models.FAQ> fAQs = new List<Models.FAQ>();
            if (dsFAQ.Tables[3].Rows.Count > 0)
            {
                foreach (DataRow dr in dsFAQ.Tables[3].Rows)
                {
                    fAQs.Add(new Models.FAQ
                    {
                        id = Convert.ToInt32(dr["id"]),
                        FAQText = Convert.ToString(dr["FAQ"]),
                    });
                }
            }
            viewmodel.fAQs = fAQs;
            return View(viewmodel);
        }
        #endregion

        #region contact
        [HttpGet]
        [Route("contact")]
        public IActionResult contact()
        {

            GetSiteNameAndCode();



            return View();
        }
        [HttpPost]
        public JsonResult enquiry(string Firstname, string Lastname, string Email, string mobile, string Message)
        {
            if (Db.IU_Enquiry(util.FixQuotes(Firstname.Trim()), util.FixQuotes(Lastname.Trim()), Email.Trim(), util.FixQuotes(mobile.Trim()), util.FixQuotes(Message.Trim())))
            {
                Db.sendAdminmail(util.FixQuotes(Firstname.Trim()), util.FixQuotes(Lastname.Trim()), Email.Trim(), util.FixQuotes(mobile.Trim()), util.FixQuotes(Message.Trim()));
                message = "send enquiry";
            }
            var Data = new { message = message };

            if (message == "send enquiry")
            {
                StringBuilder sBody = new StringBuilder();
                string MailStatus = "";
                var DateTime = new DateTime();
                DateTime = DateTime.Now;
                sBody.Append("<!DOCTYPE html><html><body>");
                //sBody.Append("<p>Dear Support Team," + Environment.NewLine);
                //sBody.Append("<p>We have received your query. We will get back to you soon " + Environment.NewLine);
                //sBody.Append("<p>Dear " + HttpContext.Session.GetString("customerfirstname") + " " + HttpContext.Session.GetString("customerlastname") + Environment.NewLine);
                sBody.Append("<p>Dear " + Firstname + ' ' + Lastname + Environment.NewLine);
                sBody.Append("<p>" + Environment.NewLine);
                sBody.Append("<p>Thank you for reaching out to us. We have received your message, and our support team will get back to you shortly. Here are the details of your query:" + Environment.NewLine);
                sBody.Append("<p>Customer Name: " + Firstname + ' ' + Lastname + Environment.NewLine);
                sBody.Append("<p>Contact Info: " + mobile + "" + Environment.NewLine);
                sBody.Append("<p>Email: " + Email + "" + Environment.NewLine);
                sBody.Append("<p>Customer Message/Query: " + Message + "" + Environment.NewLine);
                //sBody.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
                sBody.Append("<p>We will do our best to respond to your inquiry within 10AM to 6PM business hours. If you have any further questions, feel free to contact us at support@bsdinfotech.com or call us at 9871092024." + Environment.NewLine);
                sBody.Append("<p>Best Regards</p>");
                sBody.Append("<p><b>The BSD Infotech</b></p>");
                sBody.Append("</body></html>");
                MailStatus = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + Email + "", "", "", "Subject : Thank You for Contacting Us", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");
            }
            if (message == "send enquiry")
            {

                StringBuilder sBody1 = new StringBuilder();

                string MailStatus1 = "";
                var DateTime = new DateTime();
                DateTime = DateTime.Now;
                sBody1.Append("<!DOCTYPE html><html><body>");
                sBody1.Append("<p>Dear BSD," + Environment.NewLine);
                sBody1.Append("<p> A new user has submitted a query through the \"Contact Us\" form on your platform. Below are the details of the user's inquiry: " + Environment.NewLine);
               
                //sBody1.Append("<p>Customer Id: " + HttpContext.Session.GetString("customerid") + "" + Environment.NewLine);
                sBody1.Append("<p>Customer Name: " + Firstname + ' ' + Lastname + Environment.NewLine);
                sBody1.Append("<p>Contact Info: " + mobile + "" + Environment.NewLine);
                sBody1.Append("<p>Email: " + Email + "" + Environment.NewLine);
                //sBody1.Append("<p>state: " + dt1.Rows[0]["state_name"].ToString() + "" + Environment.NewLine);
                //sBody1.Append("<p>city: " + city + "" + Environment.NewLine);
                //sBody1.Append("<p>Product Name: " + ItemName + "" + Environment.NewLine);
                sBody1.Append("<p>Customer Message/Query: " + Message + "" + Environment.NewLine);
                sBody1.Append("<p>Enquiry Date and Time: " + DateTime + "" + Environment.NewLine);
                sBody1.Append("<p>" + Environment.NewLine);
                sBody1.Append("<p>Please review the inquiry and respond to the user as soon as possible. If you need any assistance or further clarification, feel free to reach out to us." + Environment.NewLine);
                sBody1.Append("<p>Best Regards</p>");
                sBody1.Append("<p><b>BSD Shop</b></p>");
                sBody1.Append("</body></html>");
                string email1 = "bsddemos@gmail.com";
                MailStatus1 = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : New User Inquiry on Your Platform", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");

            }

            return Json(Data);
        }
        #endregion

        #region terms
        [HttpGet]
        [Route("terms")]
        public IActionResult terms()
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet dsterms = Db.Footer(companyid);
            List<terms> terms = new List<terms>();
            if (dsterms.Tables[4].Rows.Count > 0)
            {
                foreach (DataRow dr in dsterms.Tables[4].Rows)
                {
                    terms.Add(new terms
                    {
                        id = Convert.ToInt32(dr["id"]),
                        termstext = Convert.ToString(dr["terms"]),
                    });
                }
            }
            viewmodel.term = terms;
            return View(viewmodel);
        }


        #endregion

        #region privacy-policy
        [HttpGet]
        [Route("privacy-policy")]
        public IActionResult privacy_policy()
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet dsPrivacy = Db.Footer(companyid);
            List<privacy_policy> privacy_Policies = new List<privacy_policy>();
            if (dsPrivacy.Tables[5].Rows.Count > 0)
            {
                foreach (DataRow dr in dsPrivacy.Tables[5].Rows)
                {
                    privacy_Policies.Add(new privacy_policy
                    {
                        id = Convert.ToInt32(dr["id"]),
                        privacy = Convert.ToString(dr["privacypolish"]),
                    });
                }
            }
            viewmodel.privacy_Policies = privacy_Policies;
            return View(viewmodel);
        }
        #endregion

        #region shipping-return
        [HttpGet]
        [Route("shipping-return")]
        public IActionResult shippingreturn()
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet dsSr = Db.Footer(companyid);
            List<shippingreturns> sr = new List<shippingreturns>();
            if (dsSr.Tables[6].Rows.Count > 0)
            {
                foreach (DataRow dr in dsSr.Tables[6].Rows)
                {
                    sr.Add(new shippingreturns
                    {
                        id = Convert.ToInt32(dr["id"]),
                        shipping = Convert.ToString(dr["shipping"]),
                    });
                }
            }
            viewmodel.shippingreturns = sr;
            return View(viewmodel);
        }
        #endregion

        #endregion

        //#region Customer
        //[HttpGet]
        //[Route("Login")]
        //public IActionResult Login()
        //{
        //    return View();
        ////}
        //[HttpPost]
        //public IActionResult CustomerLogin(string email, string mobile, string password)
        //{
        //    DataTable dtlogin = Db.CustomerLogin(email, mobile, password);
        //    if (dtlogin.Rows.Count > 0)
        //    {
        //        HttpContext.Session.SetString("customerid", dtlogin.Rows[0]["reg_id"].ToString());
        //        HttpContext.Session.SetString("customerfirstname", dtlogin.Rows[0]["first_name"].ToString());
        //        HttpContext.Session.SetString("customerlastname", dtlogin.Rows[0]["last_name"].ToString());
        //        HttpContext.Session.SetString("customeremail", dtlogin.Rows[0]["email"].ToString());
        //        HttpContext.Session.SetString("customerMobileNo", dtlogin.Rows[0]["MobileNo"].ToString());
        //        message = "Success Login.";
        //    }
        //    else
        //    {
        //        message = "Invalid User.";
        //    }
        //    var Data = new { message = message };
        //    return Json(Data);
        //}
        //[HttpPost]
        //public JsonResult CustomerRegistration()
        //{
        //    return Json("");
        //}

        //[HttpGet]
        //[Route("CustomerAccount")]
        //public IActionResult CustomerAccount()
        //{
        //    return View();
        //}
        // #endregion

        #region Dropdownbind
        public IActionResult Country()
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet ds = new DataSet();
            ds = Db.BindCountry(companyid);
            List<SelectListItem> country = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                country.Add(new SelectListItem { Text = dr["country"].ToString(), Value = dr["id"].ToString() });
            }
            ViewBag.country = country;
            return View(country);
        }
        [HttpGet]
        public JsonResult BindState(int id)
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet dsstate = new DataSet();
            dsstate = Db.BindState(id, companyid);
            List<SelectListItem> State = new List<SelectListItem>();
            foreach (DataRow dr in dsstate.Tables[0].Rows)
            {
                State.Add(new SelectListItem { Text = dr["state_name"].ToString(), Value = dr["id"].ToString() });
            }
            ViewBag.Category = State;
            return Json(State);
        }
        [HttpGet]
        public JsonResult BindCity(int id)
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet dscity = new DataSet();
            dscity = Db.BindCity(id, companyid);
            List<SelectListItem> city = new List<SelectListItem>();
            foreach (DataRow dr in dscity.Tables[0].Rows)
            {
                city.Add(new SelectListItem { Text = dr["city"].ToString(), Value = dr["id"].ToString() });
            }
            ViewBag.city = city;
            return Json(city);
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [NonAction]
        public void GetSiteNameAndCode()
        {
            string url = Request.Host.ToString();
            //string url = "www.indiastatdistrictagri.com";
            string dfds = Request.PathBase;
            if (url.ToLower().Contains("localhost"))
            {
                //DataTable dtSite = Db.GetDomain("wellnesstillulast");
                DataTable dtSite = Db.GetDomain("shop.bsddemos.in");
                if (dtSite.Rows.Count > 0)
                {

                    HttpContext.Session.SetString("SiteName", dtSite.Rows[0]["SiteName"].ToString());
                    HttpContext.Session.SetInt32("SiteId", Convert.ToInt32(dtSite.Rows[0]["SiteCode"]));
                    HttpContext.Session.SetString("Comp_Address", dtSite.Rows[0]["Comp_Address"].ToString());
                    HttpContext.Session.SetString("MobileNo", dtSite.Rows[0]["MobileNo"].ToString());
                    HttpContext.Session.SetString("EmailID", dtSite.Rows[0]["EmailID"].ToString());
                    HttpContext.Session.SetString("logo", dtSite.Rows[0]["logo"].ToString());


                }

                //HttpContext.Session.SetString("SiteName", "Bsd infotech");
                //HttpContext.Session.SetInt32("SiteId", 6);
                //HttpContext.Session.SetString("Logo", "/assets/imgs/theme/logo.svg");
                //HttpContext.Session.SetString("Comp_Address", "373, Pocket J-D, Hari Enclave, Hari Nagar, New Delhi, Delhi 110064");
                //HttpContext.Session.SetString("MobileNo", "999999999");
                //HttpContext.Session.SetString("EmailID", "ajay@bsdinfoteh.com");
                //HttpContext.Session.SetInt32("SiteId", 0);


            }
            else if (url.ToLower().Contains("shop.bsddemos.in"))
            {

                DataTable dtSite = Db.GetDomain("shop.bsddemos.in");
                if (dtSite.Rows.Count > 0)
                {

                    HttpContext.Session.SetString("SiteName", dtSite.Rows[0]["SiteName"].ToString());
                    HttpContext.Session.SetInt32("SiteId", Convert.ToInt32(dtSite.Rows[0]["SiteCode"]));
                    HttpContext.Session.SetString("Comp_Address", dtSite.Rows[0]["Comp_Address"].ToString());
                    HttpContext.Session.SetString("MobileNo", dtSite.Rows[0]["MobileNo"].ToString());
                    HttpContext.Session.SetString("EmailID", dtSite.Rows[0]["EmailID"].ToString());
                    HttpContext.Session.SetString("logo", dtSite.Rows[0]["logo"].ToString());


                }
                //HttpContext.Session.SetString("SiteName", "wellnesstillulast");
                //HttpContext.Session.SetInt32("SiteId", 6);
                //HttpContext.Session.SetString("Logo", "/assets/imgs/theme/logo.svg");
                //HttpContext.Session.SetInt32("SiteId", 0);


            }
            else
            {
                string[] urlInfo = url.Split('.');
                string domainName = "";
                if (url.ToLower().Contains(".local"))
                    domainName = urlInfo[0];
                else
                {
                    if (urlInfo.Length < 3)
                    {
                        domainName = urlInfo[0];
                    }
                    else
                    {
                        domainName = urlInfo[1];
                    }
                }
                //  DataTable dtSite = stateInfoService.GetSiteDetails(domainName);
                //   DataTable dtSite = Db.GetDomain(domainName);
                DataTable dtSite = Db.GetDomain("shop.bsddemos.in");
                if (dtSite.Rows.Count > 0)
                {

                    HttpContext.Session.SetString("SiteName", dtSite.Rows[0]["SiteName"].ToString());
                    HttpContext.Session.SetInt32("SiteId", Convert.ToInt32(dtSite.Rows[0]["SiteCode"]));
                    HttpContext.Session.SetString("Comp_Address", dtSite.Rows[0]["Comp_Address"].ToString());
                    HttpContext.Session.SetString("MobileNo", dtSite.Rows[0]["MobileNo"].ToString());
                    HttpContext.Session.SetString("EmailID", dtSite.Rows[0]["EmailID"].ToString());
                    HttpContext.Session.SetString("logo", dtSite.Rows[0]["logo"].ToString());


                }
                else
                {

                    HttpContext.Session.SetInt32("SiteType", 6);
                    HttpContext.Session.SetInt32("SiteId", 6);

                }
            }
        }
    }
}

