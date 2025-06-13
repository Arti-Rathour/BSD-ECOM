using Microsoft.AspNetCore.Mvc;
using BSD_ECOM.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.Design;
using System.Security.Policy;
using Newtonsoft.Json.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using BSD_ECOM.Controllers;
using BSD_ECOM.Models;
using static System.Net.Mime.MediaTypeNames;


namespace BSD_ECOM.Areas.Admin.Controllers
{

    [Area("Admin")]
    [AllowAnonymous]
    public class Admin : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public Admin(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }



        ClsUtility Utility = new ClsUtility();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        SqlCommand sqcmd;
        string sqlquery = "";
        string message = "";
        string status = "";
        string ActualBalance = "";
        string Statement = "";
        string TotalStock = "";

        #region AdminLogin
        public IActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AdminLogin(AdminLogin Login)
        {
            DataTable dt = Utility.execQuery("select * from tblAdminLogin WHERE (Email='" + Login.Email + "' AND passwords='" + Login.password.Trim() + "') or (Mobile_no='" + Login.Mob_No + "' AND passwords='" + Login.password.Trim() + "')");
            if (dt.Rows.Count > 0)
            {
                HttpContext.Session.SetString("AdminId", dt.Rows[0]["AdminId"].ToString());
                HttpContext.Session.SetString("AdminName", dt.Rows[0]["Name"].ToString());
                HttpContext.Session.SetString("AdminEmail", dt.Rows[0]["Email"].ToString());
                HttpContext.Session.SetString("AdminMobile_no", dt.Rows[0]["Mobile_no"].ToString());
                HttpContext.Session.SetString("Company_Id", dt.Rows[0]["Company_Id"].ToString());

                string CompanyId = HttpContext.Session.GetString("Company_Id");
                DataTable dtlogo = Utility.execQuery("select * from tblCompanyInfo where Comp_Id='" + CompanyId + "'");
                HttpContext.Session.SetString("Logo", dtlogo.Rows[0]["Logo"].ToString());
                HttpContext.Session.SetString("CompanyName", dtlogo.Rows[0]["CompanyName"].ToString());
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.Message = "Email and password Invalid";
            }
            return View();
        }
        #endregion

        #region DAshboard
        public IActionResult Dashboard()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Dashboard Dashboard = new Dashboard();
            sqlquery = "exec Sp_Dashboard '" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Dashboard.Dispatchordercount = Convert.ToInt32(dr["Dispatchordercount"]);
                Dashboard.Pendingordercount = Convert.ToInt32(dr["Pendingordercount"]);
                Dashboard.TotalOrderCount = Convert.ToInt32(dr["TotalOrderCount"]);
                Dashboard.cancelordercount = Convert.ToInt32(dr["cancelordercount"]);
                Dashboard.TotalReturnorder = Convert.ToInt32(dr["TotalReturnorder"]);
                Dashboard.TotalDeliveredCount = Convert.ToInt32(dr["TotalDeliveredCount"]);
                Dashboard.Inquiryordercount = Convert.ToInt32(dr["Inquiryordercount"]);
            }
            return View(Dashboard);
        }


        public IActionResult Vendor_Mapping()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            ViewBag.District = Utility.PopulateDropDown("select id,vendar_name from tbl_vender", Utility.cs);
            return View();

        }


       





        #endregion

        #region MainCategory
        public IActionResult MainCategory()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            ServiceBind();
            return View();
        }

        public IActionResult Welfaredetails()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            PopulateWelfare();
            return View();
        }
        [HttpPost]
        public JsonResult MainCategory(int id, string SuperCategory, int ServicesId, int Status)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            if (id == 0)
            {
                sqlquery = "exec sp_IU_Item_Main_Category " + id + ",'" + SuperCategory + "'," + Status + "," + userid + "," + ServicesId + "," + CompanyId + "";
                string status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Main Category added.";
                }
                else
                {
                    message = "Main Category not added.";
                }
            }
            else
            {
                sqlquery = "exec sp_IU_Item_Main_Category " + id + ",'" + SuperCategory + "'," + Status + "," + userid + "," + ServicesId + "";
                string status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Main Category added.";
                }
                else
                {
                    message = "Main Category not added.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult Vendorreport(int id, string vendor_name, string vendor_emailid, string vendor_mobileno, string vendor_address, string companyname)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            if (id == 0)
            {
                sqlquery = "exec [shop].[sp_tbl_vender1] " + id + ",'" + vendor_name + "','" + vendor_emailid + "','" + vendor_mobileno + "','" + vendor_address + "','" + companyname + "','INSERT','" + CompanyId + "'";
                string status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Vendor_Page added.";
                }
                else
                {
                    message = "Vendor_Page not added.";
                }
            }
            else
            {
                sqlquery = "exec [shop].[sp_tbl_vender1] " + id + ",'" + vendor_name + "','" + vendor_emailid + "','" + vendor_mobileno + "','" + vendor_address + "','" + companyname + "','UPDATE'";
                string status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Vendor_Page added.";
                }
                else
                {
                    message = "Vendor_Page not added.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }


        [HttpPost]

        public JsonResult Confirm_order(Confirm_order order)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            DataTable dt = new DataTable();
            if (order.jsondetailsdata != "[]")
            {
                dt = Utility.JsonStringTodataTable(order.jsondetailsdata);
            }

            string first_name = "";
            string last_name = "";
            string[] fullname = order.full_name.Split(" ");
            if (fullname.Length > 1)
            {
                first_name = fullname[0];
                last_name = fullname[1];
            }
            

            sqlquery = "exec [shop].[sp_confirm_order] @id='" + order.id + "',@EnquiryNo='" + order.EnquiryNo + "',@first_name='" + first_name + "',@last_name='" + last_name + "',@mobile='" + order.mobile + "',@email='" + order.email + "',@payment_mode='" + order.payment_mode + "',@shipping_add='" + order.shipping_add + "',@user_id='" + order.useridid + "',@type='INSERT ORDER',@companyid='" + CompanyId + "'";
            string status = Utility.MultipleTransactions(sqlquery);
            DataTable dtable = Utility.execQuery("SELECT TOP 1 order_no FROM tblorder  ORDER BY order_id DESC");
            string orderno ="" ;


            if (dtable.Rows.Count > 0 && dtable.Rows[0]["order_no"] != DBNull.Value)
            {
                 orderno = dtable.Rows[0]["order_no"].ToString();

            }
            if (status == "Successfull")
            {


                message = "Enquiry converted to confirm order.";
             
                //if (dt.Rows.Count > 0)
                //{
                //    sqlquery = "";
                //    foreach (DataRow j in dt.Rows)
                //    {
                //        sqlquery += " exec [shop].[sp_confirm_order] @item_id='" + j["item_id"] + "',@rate='" + j["rate"] + "',@quantity=" + j["quantity"] + ",@type='INSERT ORDER DETAILS',@companyid='" + CompanyId + "'";
                //    }
                //    status = Utility.MultipleTransactions(sqlquery);
                //    if (status == "Successfull")
                //    {
                //        if (order.type == 1)
                //        {
                //            message = "Enquiry converted to confirm order.";
                //        }
                //        else if (order.type == 2)
                //        {
                //            message = "temporary converted to confirm order.";
                //        }

                     //DataTable dtable = Utility.execQuery("select max(order_no)order_no from tblorder");
                     //int orderno = 0;

               
                     //if (dtable.Rows.Count > 0 && dtable.Rows[0]["order_no"] != DBNull.Value)
                     //{
                     //        orderno = Convert.ToInt32(dtable.Rows[0]["order_no"]);
                     // }
                // DataTable dt1 = Utility.execQuery("select a.order_no,a.first_name,a.last_name,a.email,t.ItemName,d.quantity,d.rate from tblorder a join tblorderdetail d on a.order_id = d.order_id join tblItemStore t on d.item_id = t.ID where a.order_no=" + dtable.Rows[0]["order_no"] + " );


                //        StringBuilder sBody = new StringBuilder();
                //        string MailStatus = "";
                //        var DateTime = new DateTime();
                //        DateTime = DateTime.Now;
                //        sBody.Append("<!DOCTYPE html><html><body>");
                //        sBody.Append("<p>Dear " + dt1.Rows[0]["first_name"] + dt1.Rows[0]["last_name"] + Environment.NewLine);
                //        sBody.Append("<p>We are reconfirming your order  " + dt1.Rows[0]["order_no"] + " Please review the details and let us know if there are any changes needed. " + Environment.NewLine);
                //        sBody.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
                //        sBody.Append("<p>" + Environment.NewLine);
                //        sBody.Append("<p>ItemName: " + dt1.Rows[0]["ItemName"] + " Rate: " + dt1.Rows[0]["rate"] + " Quantity: " + dt1.Rows[0]["quantity"] + "" + Environment.NewLine);
                //        sBody.Append("<p>" + Environment.NewLine);
                //        sBody.Append("<p>If everything looks good, no further action is needed. We will proceed with processing your order." + Environment.NewLine);
                //        sBody.Append("<p>" + Environment.NewLine);
                //        sBody.Append("<p>Thank you for shopping with us!" + Environment.NewLine);
                //        sBody.Append("<p>Best regards</p>");
                //        sBody.Append("<p><b>,The BSD Infotech Online Store Team</b></p>");
                //        sBody.Append("</body></html>");
                //        MailStatus = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + dt1.Rows[0]["email"] + "", "", "", "Subject : Order Confirmation -" + dt1.Rows[0]["order_no"] + "", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");



                //        StringBuilder sBody1 = new StringBuilder();
                //        string MailStatus1 = "";
                //        // var DateTime = new DateTime();
                //        // DateTime = DateTime.Now;
                //        sBody1.Append("<!DOCTYPE html><html><body>");
                //        sBody1.Append("<p>Dear Support Team," + Environment.NewLine);
                //        sBody1.Append("<p> Cancel order " + Environment.NewLine);
                //        sBody1.Append("<p>" + Environment.NewLine);
                //        sBody1.Append("<p>Thank you for your continued trust in our services." + Environment.NewLine);
                //        sBody1.Append("<p>Best Regards</p>");
                //        sBody1.Append("<p><b>Support Team</b></p>");
                //        sBody1.Append("</body></html>");
                //        string email1 = "bsddemos@gmail.com";
                //        MailStatus1 = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : Cancel Order", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");

                //        //   message = "Cancel approve updated.";


                //    }
                //}
            }
            else
            {
                message = "Enquiry not converted to confirm order.";
            }


            var Data = new { message = message, id = order.id ,orderno= orderno };
            return Json(Data);
        }



        public JsonResult Confirm_orderfinal(Confirm_order order)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            DataTable dt = new DataTable();
            if (order.jsondetailsdata != "[]")
            {
                dt = Utility.JsonStringTodataTable(order.jsondetailsdata);
            }

            string first_name = "";
            string last_name = "";
            string[] fullname = order.full_name.Split(" ");
            if (fullname.Length > 1)
            {
                first_name = fullname[0];
                last_name = fullname[1];
            }

            //sqlquery = "exec [shop].[sp_confirm_order] @id='" + order.id + "',@first_name='" + first_name + "',@last_name='" + last_name + "',@mobile='" + order.mobile + "',@email='" + order.email + "',@payment_mode='" + order.payment_mode + "',@shipping_add='" + order.shipping_add + "',@user_id='" + userid + "',@type='INSERT ORDER',@companyid='" + CompanyId + "'";
            //string status = Utility.MultipleTransactions(sqlquery);
            DataTable dtable = Utility.execQuery("select max(order_no)order_no from tblorder");
            string orderno = "";


            if (dtable.Rows.Count > 0 && dtable.Rows[0]["order_no"] != DBNull.Value)
            {
                orderno = dtable.Rows[0]["order_no"].ToString();

            }
            if (status == "Successfull")
            {


                message = "Enquiry converted to confirm order.";

                //if (dt.Rows.Count > 0)
                //{
                //    sqlquery = "";
                //    foreach (DataRow j in dt.Rows)
                //    {
                //        sqlquery += " exec [shop].[sp_confirm_order] @item_id='" + j["item_id"] + "',@rate='" + j["rate"] + "',@quantity=" + j["quantity"] + ",@type='INSERT ORDER DETAILS',@companyid='" + CompanyId + "'";
                //    }
                //    status = Utility.MultipleTransactions(sqlquery);
                //    if (status == "Successfull")
                //    {
                //        if (order.type == 1)
                //        {
                //            message = "Enquiry converted to confirm order.";
                //        }
                //        else if (order.type == 2)
                //        {
                //            message = "temporary converted to confirm order.";
                //        }

                //DataTable dtable = Utility.execQuery("select max(order_no)order_no from tblorder");
                //int orderno = 0;


                //if (dtable.Rows.Count > 0 && dtable.Rows[0]["order_no"] != DBNull.Value)
                //{
                //        orderno = Convert.ToInt32(dtable.Rows[0]["order_no"]);
                // }
                // DataTable dt1 = Utility.execQuery("select a.order_no,a.first_name,a.last_name,a.email,t.ItemName,d.quantity,d.rate from tblorder a join tblorderdetail d on a.order_id = d.order_id join tblItemStore t on d.item_id = t.ID where a.order_no=" + dtable.Rows[0]["order_no"] + " );


                //        StringBuilder sBody = new StringBuilder();
                //        string MailStatus = "";
                //        var DateTime = new DateTime();
                //        DateTime = DateTime.Now;
                //        sBody.Append("<!DOCTYPE html><html><body>");
                //        sBody.Append("<p>Dear " + dt1.Rows[0]["first_name"] + dt1.Rows[0]["last_name"] + Environment.NewLine);
                //        sBody.Append("<p>We are reconfirming your order  " + dt1.Rows[0]["order_no"] + " Please review the details and let us know if there are any changes needed. " + Environment.NewLine);
                //        sBody.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
                //        sBody.Append("<p>" + Environment.NewLine);
                //        sBody.Append("<p>ItemName: " + dt1.Rows[0]["ItemName"] + " Rate: " + dt1.Rows[0]["rate"] + " Quantity: " + dt1.Rows[0]["quantity"] + "" + Environment.NewLine);
                //        sBody.Append("<p>" + Environment.NewLine);
                //        sBody.Append("<p>If everything looks good, no further action is needed. We will proceed with processing your order." + Environment.NewLine);
                //        sBody.Append("<p>" + Environment.NewLine);
                //        sBody.Append("<p>Thank you for shopping with us!" + Environment.NewLine);
                //        sBody.Append("<p>Best regards</p>");
                //        sBody.Append("<p><b>,The BSD Infotech Online Store Team</b></p>");
                //        sBody.Append("</body></html>");
                //        MailStatus = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + dt1.Rows[0]["email"] + "", "", "", "Subject : Order Confirmation -" + dt1.Rows[0]["order_no"] + "", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");



                //        StringBuilder sBody1 = new StringBuilder();
                //        string MailStatus1 = "";
                //        // var DateTime = new DateTime();
                //        // DateTime = DateTime.Now;
                //        sBody1.Append("<!DOCTYPE html><html><body>");
                //        sBody1.Append("<p>Dear Support Team," + Environment.NewLine);
                //        sBody1.Append("<p> Cancel order " + Environment.NewLine);
                //        sBody1.Append("<p>" + Environment.NewLine);
                //        sBody1.Append("<p>Thank you for your continued trust in our services." + Environment.NewLine);
                //        sBody1.Append("<p>Best Regards</p>");
                //        sBody1.Append("<p><b>Support Team</b></p>");
                //        sBody1.Append("</body></html>");
                //        string email1 = "bsddemos@gmail.com";
                //        MailStatus1 = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : Cancel Order", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");

                //        //   message = "Cancel approve updated.";


                //    }
                //}
            }
            else
            {
                message = "Enquiry not converted to confirm order.";
            }


            var Data = new { message = message, id = order.id, orderno = orderno };
            return Json(Data);
        }


        [HttpPost]

        public JsonResult Savereplyemail(int id, string dear, string cust, string thank, string best, string team)
        {
            string message = "";

            string status = "";
            string updateQuery = "";
            updateQuery = "UPDATE tblEnquiry SET status = 3 WHERE id = '" + id + "'";
            status = Utility.MultipleTransactions(updateQuery);

            string sqlquery = "SELECT * FROM tblEnquiry WHERE id = '" + id + "' ";

            DataSet ds = Utility.TableBind(sqlquery);


            if (ds.Tables[0].Rows.Count > 0)
            {
               

                StringBuilder sb = new StringBuilder();
                string MailStatus = "";

                sb.AppendLine("<!DOCTYPE html><html><body>");
                sb.AppendLine("<div style='font-family: Arial, sans-serif;'>");
                sb.AppendLine("<p>" + dear + "</p>");
                sb.AppendLine("<p>" + thank + "</p>");
                sb.AppendLine("<p>" + best + "</p>");
                sb.AppendLine("<p>" + team + "</p>");
                sb.AppendLine("</div>");
                sb.AppendLine("</body></html>");

             

                

                MailStatus = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + ds.Tables[0].Rows[0]["email"] + "", "bsddemos@gmail.com", "", "Subject :Reply of Contact Us", sb.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");

                if (string.IsNullOrEmpty(MailStatus))
                {
                    message = "Email sending failed.";
                }
                else
                {
                    message = "Reply sent successfully.";
                }
            }
            else
            {
                message = "No record found for the provided ID.";
            }



            var Data = new { message = message };
            return Json(Data);
        }





        [HttpPost]

        public JsonResult Confirm_orderemail(int id,int idd,string dear,string cust, string thank, string order, string orderno,string pleasereview, string total, string payment, /*string accountdetail, string bank, string account, string beneficiary, string branch, string ifsc, string paymentterm, string advance, string kindly, string email, string once, string choosing,*/ string ifeverything,string willwe,string shopping, string best, string team, int sr, int itemid, string itemname, string rate, string quantity, string courior, string tax, string totalid,string th1, string th2, string th3, string th4, string th5, string th6, string th7, Confirm_order orderr)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            //DataTable dt = new DataTable();
            //if (order.jsondetailsdata != "[]")
            //{
            //    dt = Utility.JsonStringTodataTable(order.jsondetailsdata);
            //}

            //if (dt.Rows.Count > 0)
            //{
            string status1 = "";
            string status3 = "";
            string updateQuery = "";
            string updateQuery2 = "";
            
            updateQuery = "UPDATE tblEnquiry SET status = 2 WHERE id = '" + id + "'";
            updateQuery2 = "UPDATE tblEnquiry SET status = 2 WHERE id = '" + idd + "'";

            string status2 = "";
          
            string updateQuery1 = "";

            updateQuery1 = "UPDATE tblorder SET status_flg = 7 WHERE order_id = '" + id + "'";

            
            status2 =Utility.MultipleTransactions(updateQuery1);
            status3 =Utility.MultipleTransactions(updateQuery2);
            status1 =Utility.MultipleTransactions(updateQuery);
            sqlquery = "";
                //foreach (DataRow j in dt.Rows)
                //{
                    sqlquery += " exec [shop].[sp_confirm_order] @item_id='" + itemid + "',@rate='" + rate + "',@quantity=" + quantity + ",@courior=" + courior + ",@tax=" + tax + ",@totalid=" + totalid + ",@type='INSERT ORDER DETAILS',@companyid='" + CompanyId + "'";


          
            //}
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                if (orderr.type == 1)
                {
                    message = "Enquiry converted to confirm order.";
                }
                else if (orderr.type == 2)
                {
                    message = "temporary converted to confirm order.";
                }

                DataTable dtable = Utility.execQuery("select max(order_no)order_no from tblorder");
                string ordernoid = "";


                if (dtable.Rows.Count > 0 && dtable.Rows[0]["order_no"] != DBNull.Value)
                {
                    ordernoid = dtable.Rows[0]["order_no"].ToString();

                }
                DataTable dt1 = Utility.execQuery("select a.order_no,a.mobile,a.first_name,a.last_name,a.email,t.ItemName,d.quantity,d.rate from tblorder a join tblorderdetail d on a.order_id = d.order_id join tblItemStore t on d.item_id = t.ID where a.order_no='" + dtable.Rows[0]["order_no"] + "'");



                StringBuilder sb = new StringBuilder();
                string MailStatus = "";
                var DateTime = new DateTime();
                DateTime = DateTime.Now;
                sb.AppendLine("<!DOCTYPE html><html><body>");
                sb.AppendLine("<div style='font-family: Arial, sans-serif;'>");
                //sb.AppendLine("<h5>Order Confirmation and Payment Details</h5>");
                sb.AppendLine("<p>" + dear + "</p>");
                sb.AppendLine("<p>" + thank + " </p>");
                //sb.AppendLine("<p>" + pleasereview + " </p>");

                sb.AppendLine("<h3>" + order + "</h3>");
                sb.AppendLine("<table style='border-collapse: collapse; width: 100%;'>");
                sb.AppendLine("<thead>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+  th1 +"</th>");
                sb.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+  th2 +"</th>");
                sb.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+  th3 +"</th>");
                sb.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+  th4 +"</th>");
                sb.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+  th5 +"</th>");
                sb.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+  th6 +"</th>");
                sb.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+  th7 +"</th>");
                sb.AppendLine("</tr>");
                sb.AppendLine("</thead>");
                sb.AppendLine("<tbody>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td style='border: 1px solid #ddd; padding: 8px;'>"+sr+"</td>");
                sb.AppendLine("<td style='border: 1px solid #ddd; padding: 8px;'>"+ itemname + "</td>");
                sb.AppendLine("<td style='border: 1px solid #ddd; padding: 8px;'>"+ rate + "</td>");
                sb.AppendLine("<td style='border: 1px solid #ddd; padding: 8px;'>"+quantity+"</td>");
                sb.AppendLine("<td style='border: 1px solid #ddd; padding: 8px;'>"+courior+"</td>");
                sb.AppendLine("<td style='border: 1px solid #ddd; padding: 8px;'>"+tax+"</td>");
                sb.AppendLine("<td style='border: 1px solid #ddd; padding: 8px;'>"+ totalid + "</td>");
                sb.AppendLine("</tr>");
                sb.AppendLine("</tbody>");
                sb.AppendLine("</table>");

                sb.AppendLine("<p>"+ total + " </p>");

                //sb.AppendLine("<h3>" + accountdetail + "</h3>");
                //sb.AppendLine("<p>"+bank+"</p>");
                //sb.AppendLine("<p>"+ account + "</p>");
                //sb.AppendLine("<p>"+ beneficiary + "</p>");
                //sb.AppendLine("<p>"+branch+"</p>");
                //sb.AppendLine("<p>"+ifsc+"</p>");

                //sb.AppendLine("<h3>"+ paymentterm + "</h3>");
                //sb.AppendLine("<p>"+ advance + "</p>");
                //sb.AppendLine("<p>"+ kindly + " "+ email +""+ once +"</p>");

                //sb.AppendLine("<p>"+ choosing + "</p>");
                sb.AppendLine("<p>"+ ifeverything + "</p>");
                sb.AppendLine("<p>"+ shopping + "</p>");
                sb.AppendLine("<p>"+best+"</p>");
                sb.AppendLine("<p>"+team+"</p>");
                sb.AppendLine("</div>");
                sb.AppendLine("</body></html>");
                MailStatus = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + dt1.Rows[0]["email"] + "", "", "", "Subject : Order Reonfirmation -" + dt1.Rows[0]["order_no"] + "", sb.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");






                StringBuilder sBody = new StringBuilder();
                string MailStatus1 = "";
                DateTime = DateTime.Now;
                sBody.Append("<!DOCTYPE html><html><body>");
                sBody.AppendLine("<div style='font-family: Arial, sans-serif;'>");
                sBody.AppendLine("<p>Dear BSD InfoTech,</p>");
                sBody.AppendLine("<p>We have received an order from a customer. Below are the details:</p>");

                sBody.AppendLine("<h3>Order Details:</h3>");
                sBody.AppendLine("<p>Order Number:"+ orderno +" </p>");
                sBody.AppendLine("<p>Customer Name:" + dt1.Rows[0]["first_name"] +" " + dt1.Rows[0]["last_name"] +"</p>");
                sBody.AppendLine("<p>Customer Contact:" + dt1.Rows[0]["mobile"] +"</p>");

                sBody.AppendLine("<table style='border-collapse: collapse; width: 100%; margin-top: 10px;'>");
                sBody.AppendLine("<thead>");
                sBody.AppendLine("<tr>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+  th1 +"</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+  th2 +"</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+  th3 +"</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+  th4 +"</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+  th5 +"</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+  th6 +"</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + th7 + "</th>");
                sBody.AppendLine("</tr>");
                sBody.AppendLine("</thead>");
                sBody.AppendLine("<tbody>");
                sBody.AppendLine("<tr>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+sr+"</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+itemname+"</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+rate+"</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+quantity+"</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'> "+courior+"</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>"+tax+"</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + totalid + "</th>");
                sBody.AppendLine("</tr>");
                sBody.AppendLine("</tbody>");
                sBody.AppendLine("</table>");

                sBody.AppendLine("<p>Total Amount Payable by Customer:"+totalid+"</p>");

                sBody.AppendLine("<p>The customer has been informed to make a 100% advance payment. Once we receive the payment confirmation and screenshot, we will notify you to proceed with the dispatch.</p>");

                sBody.AppendLine("<p>Please keep the item ready for shipping.</p>");

                sBody.AppendLine("<p>Thank you for your cooperation.</p>");

                sBody.AppendLine("<p>Best Regards,</p>");
                //sBody.AppendLine("<p>" + dt1.Rows[0]["first_name"] +" " + dt1.Rows[0]["last_name"] +" </p>");
                sBody.AppendLine("<p>BSD InfoTech</p>");
                sBody.AppendLine("</div>");
                sBody.Append("</body></html>");
                string email1 = "bsddemos@gmail.com";
                MailStatus1 = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : New Order Received - Payment Awaited", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");






                //StringBuilder sBody1 = new StringBuilder();
                //string MailStatus1 = "";
                //// var DateTime = new DateTime();
                //DateTime = DateTime.Now;
                //sBody1.Append("<!DOCTYPE html><html><body>");
                //sBody1.Append("<p>Dear Support Team," + Environment.NewLine);
                //sBody1.Append("<p> Cancel order " + Environment.NewLine);
                //sBody1.Append("<p>" + Environment.NewLine);
                //sBody1.Append("<p>Thank you for your continued trust in our services." + Environment.NewLine);
                //sBody1.Append("<p>Best Regards</p>");
                //sBody1.Append("<p><b>Support Team</b></p>");
                //sBody1.Append("</body></html>");
                //string email1 = "bsddemos@gmail.com";
                //MailStatus1 = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : New Order Received - Payment Awaited", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");






                //StringBuilder sBody = new StringBuilder();
                //    string MailStatus = "";
                //    var DateTime = new DateTime();
                //    DateTime = DateTime.Now;
                //    sBody.Append("<!DOCTYPE html><html><body>");
                //    sBody.Append("<p>" + sr + Environment.NewLine);

                //sBody.Append("<p>Dear " + dt1.Rows[0]["first_name"] + dt1.Rows[0]["last_name"] + Environment.NewLine);
                //sBody.Append("<p>We are reconfirming your order  " + dt1.Rows[0]["order_no"] + " Please review the details and let us know if there are any changes needed. " + Environment.NewLine);
                //sBody.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
                //sBody.Append("<p>" + Environment.NewLine);
                //sBody.Append("<p>ItemName: " + dt1.Rows[0]["ItemName"] + " Rate: " + dt1.Rows[0]["rate"] + " Quantity: " + dt1.Rows[0]["quantity"] + "" + Environment.NewLine);
                //sBody.Append("<p>" + Environment.NewLine);
                //sBody.Append("<p>If everything looks good, no further action is needed. We will proceed with processing your order." + Environment.NewLine);
                //sBody.Append("<p>" + Environment.NewLine);
                //sBody.Append("<p>Thank you for shopping with us!" + Environment.NewLine);
                //sBody.Append("<p>Best regards</p>");
                //sBody.Append("<p><b>,The BSD Infotech Online Store Team</b></p>");

                //sBody.Append("</body></html>");

                //MailStatus = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + dt1.Rows[0]["email"] + "", "", "", "Subject : Order Confirmation -" + dt1.Rows[0]["order_no"] + "", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");



                // StringBuilder sBody1 = new StringBuilder();
                // string MailStatus1 = "";
                //// var DateTime = new DateTime();
                // DateTime = DateTime.Now;
                // sBody1.Append("<!DOCTYPE html><html><body>");
                // sBody1.Append("<p>Dear Support Team," + Environment.NewLine);
                // sBody1.Append("<p> Cancel order " + Environment.NewLine);
                // sBody1.Append("<p>" + Environment.NewLine);
                // sBody1.Append("<p>Thank you for your continued trust in our services." + Environment.NewLine);
                // sBody1.Append("<p>Best Regards</p>");
                // sBody1.Append("<p><b>Support Team</b></p>");
                // sBody1.Append("</body></html>");
                // string email1 = "bsddemos@gmail.com";
                // MailStatus1 = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : Cancel Order", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");




            }
            //}

            else
                {
                message = "Enquiry not converted to confirm order.";
                }


            var Data = new { message = message, id = orderr.id };
            return Json(Data);
        }



        public JsonResult Confirm_orderemailfinal(int id,string custemail, string dear, string thank, string order, string orderno, string total, string payment, string accountdetail, string bank, string account, string beneficiary, string branch, string ifsc, string paymentterm, string advance, string kindly, string email, string once, string choosing, string best, string team, int sr, int itemid, string itemname, string rate, string quantity, string courior, string tax, string totalid, string th1, string th2, string th3, string th4, string th5, string th6, string th7, Confirm_order orderr)
        {
            sqlquery = "update tblorder set status_flg = 2  where order_id=" + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                if (orderr.type == 1)
                {
                    message = "Confirm Order converted to final order.";
                }
                else if (orderr.type == 2)
                {
                    message = "Confirm Order converted to final order.";
                }

                

                StringBuilder sb = new StringBuilder();
                string MailStatus = "";
                var DateTime = new DateTime();
                DateTime = DateTime.Now;
                sb.AppendLine("<!DOCTYPE html><html><body>");
                sb.AppendLine("<div style='font-family: Arial, sans-serif;'>");
                //sb.AppendLine("<h5>Order Confirmation and Payment Details</h5>");
                sb.AppendLine("<p>" + dear + "</p>");
                sb.AppendLine("<p>" + thank + " </p>");

                sb.AppendLine("<h3>" + order + "</h3>");
                sb.AppendLine("<table style='border-collapse: collapse; width: 100%;'>");
                sb.AppendLine("<thead>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + th1 + "</th>");
                sb.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + th2 + "</th>");
                sb.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + th3 + "</th>");
                sb.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + th4 + "</th>");
                sb.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + th5 + "</th>");
                sb.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + th6 + "</th>");
                sb.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + th7 + "</th>");
                sb.AppendLine("</tr>");
                sb.AppendLine("</thead>");
                sb.AppendLine("<tbody>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td style='border: 1px solid #ddd; padding: 8px;'>" + sr + "</td>");
                sb.AppendLine("<td style='border: 1px solid #ddd; padding: 8px;'>" + itemname + "</td>");
                sb.AppendLine("<td style='border: 1px solid #ddd; padding: 8px;'>" + rate + "</td>");
                sb.AppendLine("<td style='border: 1px solid #ddd; padding: 8px;'>" + quantity + "</td>");
                sb.AppendLine("<td style='border: 1px solid #ddd; padding: 8px;'>" + courior + "</td>");
                sb.AppendLine("<td style='border: 1px solid #ddd; padding: 8px;'>" + tax + "</td>");
                sb.AppendLine("<td style='border: 1px solid #ddd; padding: 8px;'>" + totalid + "</td>");
                sb.AppendLine("</tr>");
                sb.AppendLine("</tbody>");
                sb.AppendLine("</table>");

                sb.AppendLine("<p>" + total + " </p>");

                sb.AppendLine("<h3>" + accountdetail + "</h3>");
                sb.AppendLine("<p>" + bank + "</p>");
                sb.AppendLine("<p>" + account + "</p>");
                sb.AppendLine("<p>" + beneficiary + "</p>");
                sb.AppendLine("<p>" + branch + "</p>");
                sb.AppendLine("<p>" + ifsc + "</p>");

                sb.AppendLine("<h3>" + paymentterm + "</h3>");
                sb.AppendLine("<p>" + advance + "</p>");
                sb.AppendLine("<p>" + kindly + " " + email + "" + once + "</p>");

                sb.AppendLine("<p>" + choosing + "</p>");
                sb.AppendLine("<p>" + best + "</p>");
                sb.AppendLine("<p>" + team + "</p>");
                sb.AppendLine("</div>");
                sb.AppendLine("</body></html>");
                MailStatus = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + custemail + "" , "", "", "Subject : Final Order and Payment Details ", sb.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");







                StringBuilder sBody = new StringBuilder();
                string MailStatus1 = "";
                DateTime = DateTime.Now;
                sBody.Append("<!DOCTYPE html><html><body>");
                sBody.AppendLine("<div style='font-family: Arial, sans-serif;'>");
                sBody.AppendLine("<p>Dear Team,</p>");
                sBody.AppendLine("<p>The following final order details have been shared with the customer. Please ensure smooth coordination and prompt action upon payment confirmation.</p>");

                sBody.AppendLine("<h3>Customer Details:</h3>");
                sBody.AppendLine("<p>Order Number:" + orderno + " </p>");
                //sBody.AppendLine("<p>Customer Name:" + dt1.Rows[0]["first_name"] + " " + dt1.Rows[0]["last_name"] + "</p>");
                //sBody.AppendLine("<p>Customer Contact:" + dt1.Rows[0]["mobile"] + "</p>");

                sBody.AppendLine("<h3> Order Summary:</h3>");
                sBody.AppendLine("<table style='border-collapse: collapse; width: 100%; margin-top: 10px;'>");
                sBody.AppendLine("<thead>");
                sBody.AppendLine("<tr>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + th1 + "</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + th2 + "</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + th3 + "</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + th4 + "</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + th5 + "</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + th6 + "</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + th7 + "</th>");
                sBody.AppendLine("</tr>");
                sBody.AppendLine("</thead>");
                sBody.AppendLine("<tbody>");
                sBody.AppendLine("<tr>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + sr + "</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + itemname + "</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + rate + "</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + quantity + "</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'> " + courior + "</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + tax + "</th>");
                sBody.AppendLine("<th style='border: 1px solid #ddd; padding: 8px;'>" + totalid + "</th>");
                sBody.AppendLine("</tr>");
                sBody.AppendLine("</tbody>");
                sBody.AppendLine("</table>");

                sBody.AppendLine("<p>Total Amount Payable by Customer:" + totalid + "</p>");

                sBody.AppendLine("<h3>" + accountdetail + "</h3>");
                sBody.AppendLine("<p>" + bank + "</p>");
                sBody.AppendLine("<p>" + account + "</p>");
                sBody.AppendLine("<p>" + beneficiary + "</p>");
                sBody.AppendLine("<p>" + branch + "</p>");
                sBody.AppendLine("<p>" + ifsc + "</p>");

                sBody.AppendLine("<h3>" + paymentterm + "</h3>");

                sBody.AppendLine("<p>100% advance payment is required.</p>");
                sBody.AppendLine("<p>Request the customer to share the payment screenshot at info@bsdinfotech.com.</p>");
               
                sBody.AppendLine("<h3>Next Steps:</h3>");

                sBody.AppendLine("<p>Upon payment confirmation, prepare to dispatch the order promptly. Share the courier and AWB details with the customer.</p>");
                sBody.AppendLine("<p>Please monitor and ensure all processes are executed efficiently.</p>");

                //sBody.AppendLine("<p>Thank you for your cooperation.</p>");

                sBody.AppendLine("<p>Best Regards,</p>");
               
                sBody.AppendLine("<p>Team BSD</p>");
                sBody.AppendLine("</div>");
                sBody.Append("</body></html>");
                string email1 = "bsddemos@gmail.com";
                MailStatus1 = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : Final Order and Payment Details - "+ orderno + "", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");






                //StringBuilder sBody1 = new StringBuilder();
                //string MailStatus1 = "";
                //// var DateTime = new DateTime();
                //DateTime = DateTime.Now;
                //sBody1.Append("<!DOCTYPE html><html><body>");
                //sBody1.Append("<p>Dear Support Team," + Environment.NewLine);
                //sBody1.Append("<p> Cancel order " + Environment.NewLine);
                //sBody1.Append("<p>" + Environment.NewLine);
                //sBody1.Append("<p>Thank you for your continued trust in our services." + Environment.NewLine);
                //sBody1.Append("<p>Best Regards</p>");
                //sBody1.Append("<p><b>Support Team</b></p>");
                //sBody1.Append("</body></html>");
                //string email1 = "bsddemos@gmail.com";
                //MailStatus1 = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : New Order Received - Payment Awaited", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");






                //StringBuilder sBody = new StringBuilder();
                //    string MailStatus = "";
                //    var DateTime = new DateTime();
                //    DateTime = DateTime.Now;
                //    sBody.Append("<!DOCTYPE html><html><body>");
                //    sBody.Append("<p>" + sr + Environment.NewLine);

                //sBody.Append("<p>Dear " + dt1.Rows[0]["first_name"] + dt1.Rows[0]["last_name"] + Environment.NewLine);
                //sBody.Append("<p>We are reconfirming your order  " + dt1.Rows[0]["order_no"] + " Please review the details and let us know if there are any changes needed. " + Environment.NewLine);
                //sBody.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
                //sBody.Append("<p>" + Environment.NewLine);
                //sBody.Append("<p>ItemName: " + dt1.Rows[0]["ItemName"] + " Rate: " + dt1.Rows[0]["rate"] + " Quantity: " + dt1.Rows[0]["quantity"] + "" + Environment.NewLine);
                //sBody.Append("<p>" + Environment.NewLine);
                //sBody.Append("<p>If everything looks good, no further action is needed. We will proceed with processing your order." + Environment.NewLine);
                //sBody.Append("<p>" + Environment.NewLine);
                //sBody.Append("<p>Thank you for shopping with us!" + Environment.NewLine);
                //sBody.Append("<p>Best regards</p>");
                //sBody.Append("<p><b>,The BSD Infotech Online Store Team</b></p>");

                //sBody.Append("</body></html>");

                //MailStatus = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + dt1.Rows[0]["email"] + "", "", "", "Subject : Order Confirmation -" + dt1.Rows[0]["order_no"] + "", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");



                // StringBuilder sBody1 = new StringBuilder();
                // string MailStatus1 = "";
                //// var DateTime = new DateTime();
                // DateTime = DateTime.Now;
                // sBody1.Append("<!DOCTYPE html><html><body>");
                // sBody1.Append("<p>Dear Support Team," + Environment.NewLine);
                // sBody1.Append("<p> Cancel order " + Environment.NewLine);
                // sBody1.Append("<p>" + Environment.NewLine);
                // sBody1.Append("<p>Thank you for your continued trust in our services." + Environment.NewLine);
                // sBody1.Append("<p>Best Regards</p>");
                // sBody1.Append("<p><b>Support Team</b></p>");
                // sBody1.Append("</body></html>");
                // string email1 = "bsddemos@gmail.com";
                // MailStatus1 = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : Cancel Order", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");




            }
            //}

            else
            {
                message = "Confirm Order not converted to final order.";
            }


            var Data = new { message = message, id = orderr.id };
            return Json(Data);
        }

        [HttpPost]

        public JsonResult finalorder(int order_id)
        {
            sqlquery = "update tblorder set Paymentflag = 1  where order_id=" + order_id + "";
            status = Utility.MultipleTransactions(sqlquery);



            if (status == "Successfull") 
            {
                DataTable dt1 = Utility.execQuery("SELECT ISNULL(o.order_id, '') AS order_id,ISNULL(o.order_date, '') AS order_date, ISNULL(o.order_no, '') AS order_no, ISNULL(o.first_name + ' ' + o.last_name, '') AS UserName, ISNULL(o.email, '') AS email, ISNULL(o.mobile, '') AS mobileno, ISNULL(o.order_no, '') AS order_no, ISNULL(d.rate, '0') AS rate, ISNULL(o.payment_mode, '') AS payment_mode, ISNULL(d.quantity, '') AS UnitQty, ISNULL(d.amount, 0) AS amount, ISNULL(d.Tax, 0) AS Tax, ISNULL(d.Courior, 0) AS Courior, ISNULL(d.item_id, '') AS ID, ISNULL(i.ItemName, '') AS ItemName, o.user_id as createuser FROM tblorder o INNER JOIN tblorderdetail d on o.order_id = d.order_id join tblItemStore i on d.item_id = i.ID WHERE o.order_id ='" + order_id + "'");
              


                StringBuilder sBody = new StringBuilder();
                string MailStatus = "";
                var DateTime = new DateTime();
                DateTime = DateTime.Now;
                sBody.Append("<!DOCTYPE html><html><body>");
                sBody.Append("<p>Dear " + dt1.Rows[0]["UserName"] + Environment.NewLine);
                sBody.Append("<p>Thank you for Payment Receiving your order with us! We are excited to confirm that your order Payment has been successfully received." + Environment.NewLine);
                sBody.Append("<h3>Order Details:</h3>");
                sBody.Append("<p>Order Number: " + dt1.Rows[0]["order_no"] + "</p>");
                sBody.Append("<p>Order Date:" + dt1.Rows[0]["order_date"] + "</p>");
                sBody.Append("<p>Item Ordered: " + dt1.Rows[0]["ItemName"] + "</p>");
                sBody.Append("<p>Total Amount: " + dt1.Rows[0]["amount"] + "</p>");

                //sBody.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
                sBody.Append("<p>Your order will be processed and shipped soon. We will send you an update once your order has been dispatched, along with the tracking details." + Environment.NewLine);
                sBody.Append("<p>If you have any questions or need further assistance, feel free to contact us at support@bsdinfotech.com or 9871092024." + Environment.NewLine);
                sBody.Append("<p>" + Environment.NewLine);
                sBody.Append("<p>Thank you for choosing BSD Online Store!" + Environment.NewLine);
                //sBody.Append("<p>" + Environment.NewLine);
              //  sBody.Append("<p>Thank you for shopping with us!" + Environment.NewLine);
                sBody.Append("<p>" + Environment.NewLine);
                sBody.Append("<p>Best regards</p>");
                sBody.Append("<p><b>Team BSD</b></p>");
                sBody.Append("</body></html>");
                MailStatus = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + dt1.Rows[0]["email"] + "", "", "", "Subject :Order Payment - " + dt1.Rows[0]["order_no"] + "", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");



                StringBuilder sBody1 = new StringBuilder();
                string MailStatus1 = "";
                // var DateTime = new DateTime();
                // DateTime = DateTime.Now;
                sBody1.Append("<!DOCTYPE html><html><body>");
                sBody1.Append("<p>Dear Team," + Environment.NewLine);
                sBody1.Append("<p>A new order Payment has been Received by the following customer:" + Environment.NewLine);
                sBody1.Append("<p>Customer Name: " + dt1.Rows[0]["UserName"] + "</p>");
                sBody1.Append("<p>Order Number: " + dt1.Rows[0]["order_no"] + "</p>");
                sBody1.Append("<p>Order Date:" + dt1.Rows[0]["order_date"] + "</p>");
                sBody1.Append("<p>Item Ordered: " + dt1.Rows[0]["ItemName"] + "</p>");
                sBody1.Append("<p>Total Amount: " + dt1.Rows[0]["amount"] + "</p>");
                sBody1.Append("<p>" + Environment.NewLine);
                sBody1.Append("<p>Please process the order and begin preparation for dispatch. Ensure that the items are in stock and ready for shipment. Once the order is dispatched, provide the customer with tracking information." + Environment.NewLine);
                sBody1.Append("<p>If there are any issues with stock availability or processing, please notify the team immediately.</p>");
                sBody1.Append("<p>Best Regards</p>");
                sBody1.Append("<p><b>Team BSD</b></p>");
                sBody1.Append("</body></html>");
                string email1 = "bsddemos@gmail.com";
                MailStatus1 = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : New Order Payment Received - " + dt1.Rows[0]["order_no"] + "", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");

                message = "Payment Recieved";
            }

            var Data = new { message = message };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult ShowAllMainCategory(int ServicesId)
        {
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Models.MainCategory> MainCategory = new List<Models.MainCategory>();
            if (ServicesId == 0)
            {
                sqlquery = "exec Sp_Sel_tblitem_Main_category '" + Statement + "'," + CompanyId + "";
            }
            else
            {
                sqlquery = "exec Sp_Sel_tblitem_Main_category '" + Statement + "','" + CompanyId + "'," + 0 + "," + ServicesId + "";
            }
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                MainCategory.Add(new Models.MainCategory
                {
                    Main_cat_id = Convert.ToInt32(dr["Main_cat_id"]),
                    Main_cat_name = Convert.ToString(dr["Main_cat_name"]),
                    Main_cat_status = Convert.ToBoolean(dr["Main_cat_status"]),
                    service = Convert.ToString(dr["services"])
                });
            }
            return Json(MainCategory);
        }

        [HttpPost]
        public JsonResult ShowVendorReport(int id, string searchitem)
        {
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Vendor> vendor_report = new List<Vendor>();
            if (id == 0)
            {
                sqlquery = "exec [shop].[sp_tbl_vender1] @type ='" + Statement + "',@Company_id =" + CompanyId + ",@searchitem= '" + searchitem + "'";
            }
            else
            {
                sqlquery = "exec [shop].[sp_tbl_vender1] @type ='" + Statement + "',@Company_id ='" + CompanyId + "',@searchitem= '" + searchitem + "'," + 0 + "," + id + "";
            }
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                vendor_report.Add(new Vendor
                {
                    id = Convert.ToInt32(dr["id"]),
                    vendor_name = Convert.ToString(dr["vendar_name"]),
                    vendor_emailid = Convert.ToString(dr["vendar_emailid"]),
                    vendor_mobileno = Convert.ToString(dr["vendar_mobileno"]),
                    vendor_address = Convert.ToString(dr["vendar_address"]),
                    companyname = Convert.ToString(dr["company_name"]),
                });
            }
            return Json(vendor_report);
        }

        
        [HttpPost]
        public JsonResult ShowConfirmorder()
        {
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Confirm_order> order = new List<Confirm_order>();
            sqlquery = "exec [shop].[sp_confirm_order] @type='" + Statement + "',@companyid=" + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                order.Add(new Confirm_order
                {

                    id = Convert.ToInt32(dr["order_id"]),
                    order_no = Convert.ToString(dr["order_no"]),
                    full_name = Convert.ToString(dr["fullname"]),
                    email = Convert.ToString(dr["email"]),
                    mobile = Convert.ToString(dr["mobile"]),
                    payment_mode = Convert.ToString(dr["payment_mode"]),
                    shipping_add = Convert.ToString(dr["shipping_add"]),
                    item_name = Convert.ToString(dr["ItemName"]),
                    rate = Convert.ToInt32(dr["rate"]),
                    
                    Paymentflag = Convert.ToString(dr["Paymentflag"]),
                    Paymentflagid = Convert.ToInt32(dr["Paymentflagid"]),

                    quantity = Convert.ToInt32(dr["quantity"]),
                    status_flag = Convert.ToInt32(dr["status_flg"]),

                });
            }
            return Json(order);
        }


        [HttpPost]
        public JsonResult ShowDatadash( string orderno)
        {
            Statement = "DashboadSearch";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Confirm_order> order = new List<Confirm_order>();
            sqlquery = "exec [shop].[sp_confirm_order] @type='" + Statement + "',@companyid=" + CompanyId + ",@order='" + orderno + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                order.Add(new Confirm_order
                {

                    id = Convert.ToInt32(dr["order_id"]),
                    order_no = Convert.ToString(dr["order_no"]),
                    full_name = Convert.ToString(dr["fullname"]),
                    email = Convert.ToString(dr["email"]),
                    mobile = Convert.ToString(dr["mobile"]),
                    payment_mode = Convert.ToString(dr["payment_mode"]),
                    shipping_add = Convert.ToString(dr["shipping_add"]),
                    item_name = Convert.ToString(dr["ItemName"]),
                    rate = Convert.ToInt32(dr["rate"]),
                    quantity = Convert.ToInt32(dr["quantity"]),
                    status_flag = Convert.ToInt32(dr["status_flg"]),
                    status = Convert.ToInt32(dr["status"]),

                });
            }
            return Json(order);
        }
        [HttpPost]
        public JsonResult EditMainCategory(int main_cat_id)
        {
            Statement = "EDIT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Models.MainCategory MainCategory = new Models.MainCategory();
            sqlquery = "exec Sp_Sel_tblitem_Main_category '" + Statement + "'," + CompanyId + ", " + main_cat_id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                MainCategory.Main_cat_id = Convert.ToInt32(dr["Main_cat_id"]);
                MainCategory.Main_cat_name = Convert.ToString(dr["Main_cat_name"]);
                MainCategory.services = Convert.ToInt32(dr["services"]);
                MainCategory.Main_cat_status = Convert.ToBoolean(dr["Main_cat_status"]);

            }
            return Json(MainCategory);
        }


        [HttpPost]
        public JsonResult EditVendor(int id)
        {
            Statement = "EDIT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Vendor vendor = new Vendor();
            sqlquery = "exec [shop].[sp_tbl_vender1] @type ='" + Statement + "',@Company_id =" + CompanyId + ",@id = " + id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                vendor.id = Convert.ToInt32(dr["id"]);
                vendor.vendor_name = Convert.ToString(dr["vendar_name"]);
                vendor.vendor_emailid = Convert.ToString(dr["vendar_emailid"]);
                vendor.vendor_mobileno = Convert.ToString(dr["vendar_mobileno"]);
                vendor.vendor_address = Convert.ToString(dr["vendar_address"]);
                vendor.companyname = Convert.ToString(dr["company_name"]);

            }
            return Json(vendor);
        }

        [HttpPost]
        public JsonResult DeleteMainCategory(int main_cat_id)
        {
            Statement = "DELETE";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "exec Sp_Sel_tblitem_Main_category '" + Statement + "'," + CompanyId + "," + main_cat_id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }

        
        [HttpPost]
        public JsonResult DeleteMainBannerShowid(int id)
        {
           
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "delete from tblCMSBanner_new where typeid=1 and  ID = " + id + ""; 
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }

        

        [HttpPost]
        public JsonResult DeleteCatagroyeBannerShow(int id)
        {
          
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "delete from tblCMSBanner_new where typeid=3 and  ID = " + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }


        [HttpPost]
        public JsonResult DeleteFooter3BannerShow(int id)
        {

            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "delete from tblCMSBanner_new where typeid=2 and  ID = " + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }





        [HttpPost]
        public JsonResult DeleteVendor(int id)
        {
            Statement = "DELETE";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "exec [shop].[sp_tbl_vender1] @type ='" + Statement + "',@Company_id =" + CompanyId + ",@id = " + id + " ";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }
        public IActionResult MainCategoryReportFile(int main_cat_id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }

            Statement = "VIEW";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Models.MainCategory MainCategory = new Models.MainCategory();
            sqlquery = "exec Sp_Sel_tblitem_Main_category '" + Statement + "'," + CompanyId + ", " + main_cat_id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                MainCategory.Main_cat_id = Convert.ToInt32(dr["Main_cat_id"]);
                MainCategory.Main_cat_name = Convert.ToString(dr["Main_cat_name"]);
                MainCategory.service = Convert.ToString(dr["services"]);
                MainCategory.Main_cat_status = Convert.ToBoolean(dr["Main_cat_status"]);
            }
            return View("MainCategoryReportFile", MainCategory);
        }

        #endregion

        #region Category
        public IActionResult Category()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            BindMainItemCategory();
            return View();
        }
        [HttpPost]
        public JsonResult Category(int id, string Category, int SuperCategoryId, int Status)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            if (id == 0)
            {
                sqlquery = "exec sp_IU_Category " + id + ",'" + Category + "'," + Status + "," + userid + "," + SuperCategoryId + "," + CompanyId + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Category added.";
                }
                else
                {
                    message = "Category not added.";
                }
            }
            else
            {
                sqlquery = "exec sp_IU_Category " + id + ",'" + Category + "'," + Status + "," + userid + "," + SuperCategoryId + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Category added.";
                }
                else
                {
                    message = "Category not added.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult ShowAllCategory(int SuperCategoryId)
        {
            List<Models.Category> Category = new List<Models.Category>();
            Statement = "SELECT";
            int cat_id = 0; string category_name = "", Main_cat_name = "";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            if (SuperCategoryId == 0)
            {
                sqlquery = "exec Sp_Sel_Del_tblcategory '" + Statement + "'," + CompanyId + "";
            }
            else
            {
                sqlquery = "exec Sp_Sel_Del_tblcategory '" + Statement + "'," + CompanyId + "," + cat_id + ",'" + category_name + "','" + Main_cat_name + "'," + SuperCategoryId + "";
            }
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Category.Add(new Models.Category
                {
                    category_id = Convert.ToInt32(dr["category_id"]),
                    category_name = Convert.ToString(dr["category_name"]),
                    cat_status = Convert.ToBoolean(dr["cat_status"]),
                    main_cat_name = Convert.ToString(dr["Main_cat_name"])
                });
            }
            return Json(Category);
        }

        [HttpPost]
        public JsonResult EditCategory(int category_id)
        {
            Statement = "EDIT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Models.Category category = new Models.Category();
            sqlquery = "exec Sp_Sel_Del_tblcategory '" + Statement + "'," + CompanyId + ", " + category_id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                category.category_id = Convert.ToInt32(dr["category_id"]);
                category.category_name = Convert.ToString(dr["category_name"]);
                category.Main_cat_id = Convert.ToInt32(dr["Main_cat_id"]);
                category.main_cat_name = Convert.ToString(dr["Main_cat_name"]);
                category.cat_status = Convert.ToBoolean(dr["cat_status"]);

            }
            return Json(category);
        }
        [HttpPost]
        public JsonResult DeleteCategory(int category_id)
        {
            Statement = "DELETE";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "exec Sp_Sel_Del_tblcategory '" + Statement + "'," + CompanyId + "," + category_id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }
        public IActionResult CategoryReportFile(int category_id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }

            Statement = "VIEW";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Models.Category category = new Models.Category();
            sqlquery = "exec Sp_Sel_Del_tblcategory '" + Statement + "'," + CompanyId + ", " + category_id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                category.category_id = Convert.ToInt32(dr["category_id"]);
                category.category_name = Convert.ToString(dr["category_name"]);
                category.Main_cat_id = Convert.ToInt32(dr["Main_cat_id"]);
                category.main_cat_name = Convert.ToString(dr["Main_cat_name"]);
                category.cat_status = Convert.ToBoolean(dr["cat_status"]);

            }
            return View("CategoryReportFile", category);


        }
        #endregion

        #region SubCategory
        public IActionResult SubCategory()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            BindMainItemCategory();
            return View();
        }
        [HttpPost]
        public IActionResult SaveSubCategory()
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            string id = Request.Form["id"].ToString();
            string SuperCategoryId = Request.Form["SuperCategoryId"].ToString();
            string CategoryId = Request.Form["CategoryId"].ToString();
            string SubCategory = Request.Form["SubCategory"].ToString();
            string Status = Request.Form["Status"].ToString();
            string flg = Request.Form["flg"].ToString();
            string nefilname = Request.Form["filenmes"].ToString();
            string webRootPath = "";
            string filename = "";
            if (flg == "okg")
            {
                nefilname = "";
                IFormFile file = Request.Form.Files[0];
                string folderName = "wwwroot/images/SubCategory/" + CompanyId + "/";
                string extension = Path.GetExtension(file.FileName);
                filename = Path.GetFileNameWithoutExtension(file.FileName);
                //nefilname = filename;
                webRootPath = filename + DateTime.Now.ToString("yymmssff") + extension;
                nefilname = webRootPath;
                string newPath = Path.Combine(folderName, webRootPath);
                using (var fileStream = new FileStream(newPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            if (id == "0")
            {
                sqlquery = "exec sp_IU_ItemCategory " + id + ",'" + SubCategory + "'," + Status + "," + userid + "," + SuperCategoryId + ",'" + CategoryId + "','" + webRootPath + "'," + CompanyId + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Sub Category added.";
                }
                else
                {
                    message = "Sub Category not added.";
                }
            }
            else
            {
                sqlquery = "exec sp_IU_ItemCategory " + id + ",'" + SubCategory + "'," + Status + "," + userid + "," + SuperCategoryId + ",'" + CategoryId + "','" + nefilname + "'," + CompanyId + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Sub Category added.";
                }
                else
                {
                    message = "Sub Category not added.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult ShowSubCategory(int CategoryId, int MainCategoryId)
        {
            Statement = "SELECT";
            string cat_name = "", Main_cat_name = "", category_name = "";
            List<Models.SubCategory> SubCategory = new List<Models.SubCategory>();
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            if (CategoryId == 0 && MainCategoryId == 0)
            {
                sqlquery = "exec Sp_Sel_Del_IU_ItemCategory '" + Statement + "','" + CompanyId + "'";
            }
            else
            {
                sqlquery = "exec Sp_Sel_Del_IU_ItemCategory '" + Statement + "','" + CompanyId + "'," + CategoryId + ",'" + cat_name + "','" + Main_cat_name + "','" + category_name + "'," + MainCategoryId + "";
            }
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SubCategory.Add(new Models.SubCategory
                {
                    cat_id = Convert.ToInt32(dr["cat_id"]),
                    cat_name = Convert.ToString(dr["cat_name"]),
                    cat_status = Convert.ToBoolean(dr["cat_status"]),
                    Main_cat_id = Convert.ToInt32(dr["Main_cat_id"]),
                    main_cat_name = Convert.ToString(dr["Main_cat_name"]),
                    category_name = Convert.ToString(dr["category_name"]),
                    category_id = Convert.ToInt32(dr["category_id"]),
                    company_name = Convert.ToString(dr["CompanyName"]),
                    Image = Convert.ToString(dr["Image"]),

                });
            }
            return Json(SubCategory);
        }

        [HttpPost]
        public JsonResult EditSubCategory(int Sub_cat_id)
        {
            Statement = "EDIT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Models.SubCategory Subcategory = new Models.SubCategory();
            sqlquery = "exec Sp_Sel_Del_IU_ItemCategory '" + Statement + "'," + CompanyId + ", " + Sub_cat_id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Subcategory.cat_id = Convert.ToInt32(dr["cat_id"]);
                Subcategory.Main_cat_id = Convert.ToInt32(dr["Main_cat_id"]);
                Subcategory.category_id = Convert.ToInt32(dr["category_id"]);
                Subcategory.cat_name = Convert.ToString(dr["cat_name"]);
                Subcategory.Image = (dr["Image"]).ToString();
                Subcategory.cat_status = Convert.ToBoolean(dr["cat_status"]);

            }
            return Json(Subcategory);
        }
        [HttpPost]
        public JsonResult ViewSubCategory()
        {
            return Json("");
        }
        [HttpPost]
        public JsonResult DeleteSubCategory(int cat_id)
        {
            Statement = "DELETE";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "exec Sp_Sel_Del_IU_ItemCategory '" + Statement + "'," + CompanyId + "," + cat_id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }

        public IActionResult SubCategoryReportFile(int cat_id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            Statement = "VIEW";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Models.SubCategory Subcategory = new Models.SubCategory();
            sqlquery = "exec Sp_Sel_Del_IU_ItemCategory '" + Statement + "'," + CompanyId + ", " + cat_id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Subcategory.cat_id = Convert.ToInt32(dr["cat_id"]);
                Subcategory.main_cat_name = Convert.ToString(dr["main_cat_name"]);
                Subcategory.category_name = Convert.ToString(dr["category_name"]);
                Subcategory.cat_name = Convert.ToString(dr["cat_name"]);
                Subcategory.Image = (dr["Image"]).ToString();
                Subcategory.cat_status = Convert.ToBoolean(dr["cat_status"]);

            }
            return View("SubCategoryReportFile", Subcategory);
        }
        #endregion

        #region DropDown Bind
        [HttpGet]
        public IActionResult ServiceBind()
        {
            sqlquery = "select ID,ServiceType from tblServiceType";
            ds = Utility.BindDropDown(sqlquery);
            List<SelectListItem> Services = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Services.Add(new SelectListItem { Text = dr["ServiceType"].ToString(), Value = dr["ID"].ToString() });
            }
            ViewBag.Services = Services;
            return View(Services);
        }
        public IActionResult BindMainItemCategory()
        {
            sqlquery = "select Main_cat_id, Main_cat_name from tblitem_Main_category";
            ds = Utility.BindDropDown(sqlquery);
            List<SelectListItem> ItemCategory = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ItemCategory.Add(new SelectListItem { Text = dr["Main_cat_name"].ToString(), Value = dr["Main_cat_id"].ToString() });
            }
            ViewBag.ItemCategory = ItemCategory;
            return View(ItemCategory);
        }
        [HttpGet]
        public JsonResult BindCategory(int id)
        {
            sqlquery = "select category_id,category_name from tblcategory where cat_status=1 and  Main_cat_id=" + id + "";
            ds = Utility.BindDropDown(sqlquery);
            List<SelectListItem> Category = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Category.Add(new SelectListItem { Text = dr["category_name"].ToString(), Value = dr["category_id"].ToString() });
            }
            ViewBag.Category = Category;
            return Json(Category);
        }
        [HttpGet]
        public JsonResult BindSubCategory(int id)
        {
            sqlquery = "select cat_id,cat_name from tblitem_category where  cat_status=1 and category_id=" + id + "";
            ds = Utility.BindDropDown(sqlquery);
            List<SelectListItem> Category = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Category.Add(new SelectListItem { Text = dr["cat_name"].ToString(), Value = dr["cat_id"].ToString() });
            }
            ViewBag.Category = Category;
            return Json(Category);
        }

        public IActionResult PopulateSubCategory()
        {
            sqlquery = "select cat_id,cat_name from tblitem_category";
            ds = Utility.BindDropDown(sqlquery);
            List<SelectListItem> Category = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Category.Add(new SelectListItem { Text = dr["cat_name"].ToString(), Value = dr["cat_id"].ToString() });
            }
            ViewBag.SubCategory = Category;
            return View(Category);
        }
        public IActionResult PopulateCategory()
        {
            sqlquery = "select category_id,category_name from tblcategory";
            ds = Utility.BindDropDown(sqlquery);
            List<SelectListItem> Category = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Category.Add(new SelectListItem { Text = dr["category_name"].ToString(), Value = dr["category_id"].ToString() });
            }
            ViewBag.Category = Category;
            return View(Category);
        }

        [HttpGet]
        public IActionResult BindDuplicateProduct()
        {
            string userid = HttpContext.Session.GetString("AdminId");
            sqlquery = "select MAx(i.ID) as ID,i.ItemName from tblItemStore I , tblitem_category_Vendor vc where vc.Vendor_ID=6 and I.VendorID<>6 And i.SubGroupID=vc.SubCategory_ID And i.GroupID=vc.Main_cat_ID And i.CategoryID=vc.category_id  and i.Flag<>1 ANd i.Status=1 group by i.ItemName";
            ds = Utility.BindDropDown(sqlquery);
            List<SelectListItem> DuplicateProduct = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DuplicateProduct.Add(new SelectListItem { Text = dr["ItemName"].ToString(), Value = dr["ID"].ToString() });
            }
            ViewBag.DuplicateProduct = DuplicateProduct;
            return View(DuplicateProduct);
        }
        public IActionResult ItemTypeBind()
        {
            sqlquery = "select unit_id,unit_name from Unitmaster";
            ds = Utility.BindDropDown(sqlquery);
            List<SelectListItem> itemType = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                itemType.Add(new SelectListItem { Text = dr["unit_name"].ToString(), Value = dr["unit_id"].ToString() });
            }
            ViewBag.itemType = itemType;
            return View(itemType);
        }
        public IActionResult BrandBind()
        {
            sqlquery = "select brand_id, brand_name from tblbrand";
            ds = Utility.BindDropDown(sqlquery);
            List<SelectListItem> Brand = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Brand.Add(new SelectListItem { Text = dr["brand_name"].ToString(), Value = dr["brand_id"].ToString() });
            }
            ViewBag.Brand = Brand;
            return View(Brand);
        }
        public IActionResult BindHSN()
        {
            sqlquery = "select ID,HSNName from tblhsn";
            ds = Utility.BindDropDown(sqlquery);
            List<SelectListItem> HSN = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                HSN.Add(new SelectListItem { Text = dr["HSNName"].ToString(), Value = dr["ID"].ToString() });
            }
            ViewBag.HSN = HSN;
            return View(HSN);
        }
        [HttpGet]
        public IActionResult BindLocalityType()
        {
            string query = "  select Loc_id,Name from Mst_Locality";
            ds = Utility.BindDropDown(query);
            List<SelectListItem> LocalType = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                LocalType.Add(new SelectListItem { Text = dr["Name"].ToString(), Value = dr["Loc_id"].ToString() });
            }
            ViewBag.LocalType = LocalType;
            return View(LocalType);
        }
        public IActionResult CountryBind()
        {
            sqlquery = "select id,country from tblcountry";
            ds = Utility.BindDropDown(sqlquery);
            List<SelectListItem> Country = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Country.Add(new SelectListItem { Text = dr["country"].ToString(), Value = dr["id"].ToString() });
            }
            ViewBag.ItemCountry = Country;
            return View(Country);
        }
        public IActionResult StateBind()
        {
            sqlquery = "select id,state_name from tblstate";
            ds = Utility.BindDropDown(sqlquery);
            List<SelectListItem> State = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                State.Add(new SelectListItem { Text = dr["state_name"].ToString(), Value = dr["id"].ToString() });
            }
            ViewBag.ItemState = State;
            return View(State);
        }
        [HttpGet]
        public IActionResult Customer()
        {
            sqlquery = "select reg.reg_id as id,  reg.first_name+' '+reg.last_name as customer from tblregistration reg";
            ds = Utility.BindDropDown(sqlquery);
            List<SelectListItem> UserName = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                UserName.Add(new SelectListItem { Text = dr["customer"].ToString(), Value = dr["id"].ToString() });
            }
            ViewBag.ItemUserNane = UserName;
            return View(UserName);
        }
        [HttpGet]
        public JsonResult BindCourier()
        {
            sqlquery = "select ID,Contact_Person from tblCourier";
            ds = Utility.BindDropDown(sqlquery);
            List<SelectListItem> Courier = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Courier.Add(new SelectListItem { Text = dr["Contact_Person"].ToString(), Value = dr["ID"].ToString() });
            }
            ViewBag.Courier = Courier;
            return Json(Courier);
        }
        #endregion

        #region Product Master
        public IActionResult NewProduct()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            BindDuplicateProduct();
            BindMainItemCategory();
            ItemTypeBind();
            BrandBind();
            BindHSN();
            // PopulateCategory();
            //PopulateSubCategory();
            //Statement = "EDIT";
            //AddNewProduct product = new AddNewProduct();
            //sqlquery = "exec Sp_sel_del_tblItemStore '" + Statement + "','"+ CompanyId + "'," + id + "";
            //DataSet ds = Utility.TableBind(sqlquery);
            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    product.ID = Convert.ToInt32(dr["ID"]);
            //    product.ItemName = Convert.ToString(dr["ItemName"]);
            //    product.URLName = Convert.ToString(dr["URLName"]);
            //    product.GroupID = Convert.ToInt32(dr["GroupID"]);
            //    product.CategoryID = Convert.ToInt32(dr["CategoryID"]);
            //    product.SubGroupID = Convert.ToInt32(dr["SubGroupID"]);
            //    product.productTag = Convert.ToString(dr["productTag"]);
            //    product.HSNCode = Convert.ToString(dr["HSNCode"]);
            //    product.SKUCode = Convert.ToString(dr["SKUCode"]);
            //    product.CostPrice = Convert.ToInt32(dr["CostPrice"]);
            //    product.MRP = Convert.ToInt32(dr["MRP"]);
            //    product.StockStatus = Convert.ToInt32(dr["StockStatus"]);
            //    product.Dimension = Convert.ToString(dr["Dimension"]);
            //    product.ShipCharge = Convert.ToString(dr["ShipCharge"]);
            //    product.Description = Convert.ToString(dr["Description"]);
            //    product.additional = Convert.ToString(dr["additional"]);
            //    product.ingredients = Convert.ToString(dr["ingredients"]);
            //}
            return View();
        }
        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            BindDuplicateProduct();
            BindMainItemCategory();
            ItemTypeBind();
            BrandBind();
            BindHSN();
            PopulateCategory();
            PopulateSubCategory();
            Statement = "EDIT";
            AddNewProduct product = new AddNewProduct();
            sqlquery = "exec Sp_sel_del_tblItemStore '" + Statement + "','" + CompanyId + "'," + id + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                product.ID = Convert.ToInt32(dr["ID"]);
                product.ItemName = Convert.ToString(dr["ItemName"]);
                product.URLName = Convert.ToString(dr["URLName"]);
                product.GroupID = Convert.ToInt32(dr["GroupID"]);
                product.CategoryID = Convert.ToInt32(dr["CategoryID"]);
                product.SubGroupID = Convert.ToInt32(dr["SubGroupID"]);
                product.productTag = Convert.ToString(dr["productTag"]);
                product.HSNCode = Convert.ToString(dr["HSNCode"]);
                //product.SKUCode = Convert.ToString(dr["SKUCode"]);
                //product.CostPrice = Convert.ToInt32(dr["CostPrice"]);
                // product.MRP = Convert.ToInt32(dr["MRP"]);
                product.StockStatus = Convert.ToInt32(dr["stockqty"]);
                //product.Dimension = Convert.ToString(dr["Dimension"]);
                product.ShipCharge = Convert.ToString(dr["ship_charge"]);
                product.Description = Convert.ToString(dr["Description"]);
                product.additional = Convert.ToString(dr["additional"]);
                product.ingredients = Convert.ToString(dr["ingredients"]);
                product.BrandID = Convert.ToInt32(dr["BrandID"]);
                product.image = Convert.ToString(dr["image"]);
                product.image1 = Convert.ToString(dr["image1"]);
                product.image2 = Convert.ToString(dr["image2"]);
                product.image3 = Convert.ToString(dr["image3"]);
                product.Image4 = Convert.ToString(dr["Image4"]);
                product.Image5 = Convert.ToString(dr["Image5"]);
                product.Unit_Rate = Convert.ToDecimal(dr["Unit_Rate"]);
                product.unit_id = Convert.ToInt32(dr["unit_id"]);
                //product.unitname = Convert.ToString(dr["unit_name"]);
                product.quantity = Convert.ToString(dr["Unit_Qty"]);
                product.Active = Convert.ToInt32(dr["Active"]);
                product.Send_query = Convert.ToInt32(dr["send_query"]);
                product.Price = Convert.ToInt32(dr["price"]);
            }
            return View(product);
        }


        [HttpPost]
        public JsonResult ShowItemdetailsOnEditProduct(int id)
        {
            Statement = "showitemdetails";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<AddNewProduct> product = new List<AddNewProduct>();
            sqlquery = "exec Sp_sel_del_tblItemStore '" + Statement + "','" + CompanyId + "'," + id + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                product.Add(new AddNewProduct
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    itemid = Convert.ToInt32(dr["item_id"]),
                    StockStatus = Convert.ToInt32(dr["stockqty"]),
                    Unit_Rate = Convert.ToDecimal(dr["Unit_Rate"]),
                    unit_id = Convert.ToInt32(dr["unit_id"]),
                    unitname = Convert.ToString(dr["unit_name"]),
                    discount = Convert.ToInt32(dr["discount"]),
                    quantity = Convert.ToString(dr["Unit_Qty"]),
                    ShipCharge = Convert.ToString(dr["ship_charge"]),
                });
            }
            return Json(product);
        }


        
       



        [HttpPost]
        public IActionResult SaveNewProduct()
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            //int siud =  HttpContext.Session.GetInt32("SiteId");

          

            string webRootPath = ""; string webRootPath1 = ""; string webRootPath2 = ""; string webRootPath3 = ""; string webRootPath4 = ""; string webRootPath5 = "";

			string folderName = "wwwroot/images/Productimage/" + CompanyId + "/";
            string id = Request.Form["id"].ToString();
            string ProductName = Request.Form["ProductName"].ToString();
            string DupProductval = Request.Form["DupProductval"].ToString();
            string Url = Request.Form["Url"].ToString();
            string SuperCategoryVal = Request.Form["SuperCategoryVal"].ToString();
            string CategoryVal = Request.Form["CategoryVal"].ToString();
            string SubCategoryVal = Request.Form["SubCategoryVal"].ToString();
            string ProductTag = Request.Form["ProductTag"].ToString();
            string BrandVal = Request.Form["BrandVal"].ToString();
            string HSNVal = Request.Form["HSNVal"].ToString();
            string ItemTypeval = Request.Form["ItemTypeval"].ToString();
            string SKUCode = Request.Form["SKUCode"].ToString();
            string RegularPrice = Request.Form["RegularPrice"].ToString();
            string SalePrice = Request.Form["SalePrice"].ToString();
            string Stockstatus = Request.Form["Stockstatus"].ToString();
            string Weight = Request.Form["Weight"].ToString();
            string weightval = Request.Form["weightval"].ToString();
            string D_Length = Request.Form["D_Length"].ToString();
            string D_Width = Request.Form["D_Width"].ToString();
            string D_height = Request.Form["txtDimension3"].ToString();
            string ShipCharges = Request.Form["ShipCharges"].ToString();
            string ProductDesc = Request.Form["ProductDesc"].ToString();
            string AddInformation = Request.Form["AddInformation"].ToString();
            string Ingredients = Request.Form["Ingredients"].ToString();
            string hdnbalance = Request.Form["balance"].ToString();
            string hdnStock = Request.Form["hdnStock"].ToString();

            string Unitdetails = Request.Form["json"].ToString();
            string Featuredimageflg = Request.Form["Featuredimageflg"].ToString();
            string Featuredfilename = Request.Form["Featuredfilename"].ToString();

            string BackImageflg = Request.Form["BackImageflg"].ToString();
            string Backfilename = Request.Form["Backfilename"].ToString();

            string Leftimageflg = Request.Form["Leftimageflg"].ToString();
            string Leftfilename = Request.Form["Leftfilename"].ToString();

            string RightImageflg = Request.Form["RightImageflg"].ToString();
            string Rightfilename = Request.Form["Rightfilename"].ToString();

            string filename1Imageflg = Request.Form["filename1Imageflg"].ToString();
            string filename1Image = Request.Form["filename1Image"].ToString();

			string pdf1Imageflg = Request.Form["pdf1Imageflg"].ToString();
			string pdf1Image = Request.Form["pdf1Image"].ToString();

			string Active = Request.Form["Active"].ToString();
            string Send_query = Request.Form["Send_query"].ToString();
            string Price = Request.Form["Price"].ToString();


            int j = 0;






            if (Featuredimageflg == "okg")
            {
                Featuredfilename = "";
                IFormFile Featuredimg = Request.Form.Files[j];
                string extension = Path.GetExtension(Featuredimg.FileName);
                string filename = Path.GetFileNameWithoutExtension(Featuredimg.FileName);
                webRootPath = filename + DateTime.Now.ToString("yymmssff") + extension;
                //nefilname = filename;
                Featuredfilename = webRootPath;
                string newPath = Path.Combine(folderName, webRootPath);
                using (var fileStream = new FileStream(newPath, FileMode.Create))
                {
                    Featuredimg.CopyTo(fileStream);
                }
                j++;
            }

            if (BackImageflg == "okg")
            {
                Backfilename = "";
                IFormFile backimg = Request.Form.Files[j];
                string extension1 = Path.GetExtension(backimg.FileName);
                string filename1 = Path.GetFileNameWithoutExtension(backimg.FileName);
                webRootPath1 = filename1 + DateTime.Now.ToString("yymmssff") + extension1;
                //nefilname = filename;
                Backfilename = webRootPath1;
                string newPath1 = Path.Combine(folderName, webRootPath1);
                using (var fileStream = new FileStream(newPath1, FileMode.Create))
                {
                    backimg.CopyTo(fileStream);
                }
                j++;
            }


            if (Leftimageflg == "okg")
            {
                Leftfilename = "";
                IFormFile leftimg = Request.Form.Files[j];
                string extension2 = Path.GetExtension(leftimg.FileName);
                string filename2 = Path.GetFileNameWithoutExtension(leftimg.FileName);
                webRootPath2 = filename2 + DateTime.Now.ToString("yymmssff") + extension2;
                //nefilname = filename;
                Leftfilename = webRootPath2;
                string newPath2 = Path.Combine(folderName, webRootPath2);
                using (var fileStream = new FileStream(newPath2, FileMode.Create))
                {
                    leftimg.CopyTo(fileStream);
                }
                j++;
            }

            if (RightImageflg == "okg")
            {
                Rightfilename = "";
                IFormFile rightimg = Request.Form.Files[j];
                string extension3 = Path.GetExtension(rightimg.FileName);
                string filename3 = Path.GetFileNameWithoutExtension(rightimg.FileName);
                webRootPath3 = filename3 + DateTime.Now.ToString("yymmssff") + extension3;
                //nefilname = filename;
                Rightfilename = webRootPath3;
                string newPath3 = Path.Combine(folderName, webRootPath3);
                using (var fileStream = new FileStream(newPath3, FileMode.Create))
                {
                    rightimg.CopyTo(fileStream);
                }
                j++;
            }

            if (filename1Imageflg == "okg")
            {
                filename1Image = "";
                IFormFile videoimg = Request.Form.Files[j];
                string extension4 = Path.GetExtension(videoimg.FileName);
                string filename4 = Path.GetFileNameWithoutExtension(videoimg.FileName);
                webRootPath4 = filename4 + DateTime.Now.ToString("yymmssff") + extension4;
                //nefilname = filename;
                filename1Image = webRootPath4;
                string newPath4 = Path.Combine(folderName, webRootPath4);
                using (var fileStream = new FileStream(newPath4, FileMode.Create))
                {
                    videoimg.CopyTo(fileStream);
                }
                j++;
            }

			if (pdf1Imageflg == "okg")
			{
				pdf1Image = "";
				IFormFile pdfimg = Request.Form.Files[j];
				string extension5 = Path.GetExtension(pdfimg.FileName);
				string filename5 = Path.GetFileNameWithoutExtension(pdfimg.FileName);
				webRootPath5 = filename5 + DateTime.Now.ToString("yymmssff") + extension5;
				//nefilname = filename;
				pdf1Image = webRootPath5;
				string newPath5 = Path.Combine(folderName, webRootPath5);
				using (var fileStream = new FileStream(newPath5, FileMode.Create))
				{
					pdfimg.CopyTo(fileStream);
				}
				j++;
			}



			DataTable dt = new DataTable();
            if (Unitdetails != "[]")
            {
                 dt = Utility.JsonStringToDataTable(Unitdetails);
            }
            
            int row = dt.Rows.Count;
            int col = dt.Columns.Count;
            int i = 0;
            //string UnitId = Request.Form["UnitId"].ToString();
            //string  Qty = Request.Form["Qty"].ToString();
            //string discount = Request.Form["discount"].ToString();
            //string unitrate = Request.Form["unitrate"].ToString();
            //balance = Convert.ToDouble(hdnbalance);
            //TotalStock = hdnStock.Trim();
            //if (balance == 0)
            //{
            //    ActualBalance = Stockstatus.Trim();
            //}
            //else
            //{
            //    double Finalstock = Convert.ToDouble(Stockstatus.Trim()) - Convert.ToDouble(TotalStock);
            //    double FInalBalance = balance + Finalstock;
            //    ActualBalance = FInalBalance.ToString();
            //}
            string barcode = "";
            int rating = 0; int Accessories = 0; int status1 = 0; int newarrival = 0; int bestSeller = 0; int opening = 0;
            int onsale = 0; int managestock = 0; int unitid = 0, Type = 1; int LastPurchasePrice = 0; int balance = 0;
            string dimension = D_Length.Trim() + '-' + D_Width.Trim() + '-' + D_height.Trim();
            int Flag = 0;
            if (id != "0")
            {
                Flag = 1;
            }
            else
            {
                Flag = 0;
            }
            if (id == "0")
            {
                DataSet ds2 = Utility.TableBind("select * FROM tblItemStore where ItemName='" + ProductName + "' ");
                DataTable dt2 = ds2.Tables[0];
                if (dt2.Rows.Count > 0)
                {
                    message = "Name Already Exist";
                }
                else
                {
                    sqlquery = "exec Sp_IU_ItemStore '" + CompanyId + "'," + id + ",'" + ProductName + "','" + barcode + "'," + RegularPrice + "," + SalePrice + "," + LastPurchasePrice + "," + unitid + "," + SuperCategoryVal + "," + CategoryVal + "," + SubCategoryVal + "," + 0 + ",'" + userid + "','" + SKUCode + "'," + status1 + "," + rating + "," + Accessories + "," + newarrival + "," + bestSeller + ",'" + Url + "'," + opening + ",'" + ProductDesc + "','" + webRootPath + "','" + webRootPath1 + "','" + webRootPath2 + "','" + webRootPath3 + "','" + webRootPath4 + "','" + webRootPath5 + "','" + BrandVal + "','" + ProductTag + "'," + onsale + ",'" + managestock + "','" + Stockstatus + "','" + Weight + "','" + dimension + "','" + ShipCharges + "','" + Ingredients + "','" + AddInformation + "'," + balance + ",'" + weightval + "','" + HSNVal + "','" + Flag + "'," + Type + ",@Active=" + Active + ",@send_query='" + Send_query + "',@price='" + Price + "'";
                    status = Utility.MultipleTransactions(sqlquery);
                    if (status == "Successfull")
                    {
                        DataTable dt1 = Utility.execQuery("select max(id) from tblitemstore");
                        string sql = "";
                        if(row > 0)
                        {
                            for (i = 0; i < row; i++)
                            {
                                sql += Utility.MultipleTransactions("Insert into tblitemdetails(Unit_id,Unit_Qty,company_id,create_user, create_date,discount, Unit_Rate,item_id,ship_charge,stockqty)values(" + dt.Rows[i]["UnitId"].ToString() + ",'" + dt.Rows[i]["Qty"].ToString() + "'," + CompanyId + ",'" + userid + "',getdate()," + dt.Rows[i]["discount"].ToString() + "," + dt.Rows[i]["unitrate"].ToString() + ",'" + dt1.Rows[0][0].ToString() + "','" + dt.Rows[i]["shippingCharge"].ToString() + "','" + dt.Rows[i]["stockQuantity"].ToString() + "')");
                            }
                        }

                        message = "NewProduct added.";
                    }
                    else
                    {
                        message = "NewProduct not added.";
                    }
                }
            }
            else
            {
                DataSet ds2 = Utility.TableBind("select * FROM tblItemStore where ItemName='" + ProductName + "' and id !='" + id + "' ");
                DataTable dt2 = ds2.Tables[0];
                if (dt2.Rows.Count > 0)
                {
                    message = "Name Already Exist";
                }
                else
                {
                    sqlquery = "exec Sp_IU_ItemStore '" + CompanyId + "'," + id + ",'" + ProductName + "','" + barcode + "'," + RegularPrice + "," + SalePrice + "," + LastPurchasePrice + "," + unitid + "," + SuperCategoryVal + "," + CategoryVal + "," + SubCategoryVal + "," + 0 + ",'" + userid + "','" + SKUCode + "'," + status1 + "," + rating + "," + Accessories + "," + newarrival + "," + bestSeller + ",'" + Url + "'," + opening + ",'" + ProductDesc + "','" + Featuredfilename + "','" + Backfilename + "','" + Leftfilename + "','" + Rightfilename + "','" + filename1Image + "','" + pdf1Image + "','" + BrandVal + "','" + ProductTag + "'," + onsale + ",'" + managestock + "','" + Stockstatus + "','" + Weight + "','" + dimension + "','" + ShipCharges + "','" + Ingredients + "','" + AddInformation + "'," + balance + ",'" + weightval + "','" + HSNVal + "','" + Flag + "'," + Type + ",@Active = " + Active + ",@send_query='" + Send_query + "',@price='" + Price + "'";
                    string status = Utility.MultipleTransactions(sqlquery);
                    if (status == "Successfull")
                    {
                        // DataTable dt1 = Utility.execQuery("select max(id) from tblitemstore");
                        string sql = "";


                        for (i = 0; i < row; i++)
                        {
                            if (dt.Rows[i]["id"].ToString() == "" || dt.Rows[i]["id"].ToString() == null)
                            {



                                sql += Utility.MultipleTransactions("Insert into tblitemdetails(Unit_id,Unit_Qty,company_id,create_user, create_date,discount, Unit_Rate,item_id,ship_charge,stockqty)values(" + dt.Rows[i]["UnitId"].ToString() + ",'" + dt.Rows[i]["Qty"].ToString() + "'," + CompanyId + ",'" + userid + "',getdate()," + dt.Rows[i]["discount"].ToString() + "," + dt.Rows[i]["unitrate"].ToString() + ",'" + id + "','" + dt.Rows[i]["shippingCharge"].ToString() + "','" + dt.Rows[i]["stockQuantity"].ToString() + "')");

                            }
                            else
                            {

                                sql += Utility.MultipleTransactions("Update tblitemdetails set Unit_id=" + dt.Rows[i]["UnitId"].ToString() + ",Unit_Qty='" + dt.Rows[i]["Qty"].ToString() + "',company_id=" + CompanyId + ",modify_user='" + userid + "', modify_date=getdate(),discount=" + dt.Rows[i]["discount"].ToString() + ", Unit_Rate=" + dt.Rows[i]["unitrate"].ToString() + ",ship_charge='" + dt.Rows[i]["shippingCharge"].ToString() + "',stockqty='" + dt.Rows[i]["stockQuantity"].ToString() + "' where id=" + dt.Rows[i]["id"].ToString() + "");
                            }
                        }


                        message = "NewProduct added.";
                    }
                    else
                    {
                        message = "NewProduct not added.";
                    }
                }

            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }
        public IActionResult ProductList()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }


        public IActionResult Order_Report()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }


        public IActionResult Vendor_Report_Page()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            ViewBag.District = Utility.PopulateDropDown(" select id,vendar_name from tbl_vender", Utility.cs);
            return View();
        }

        [HttpPost]
        public JsonResult ShowOrderReport(int productStatus, string fromDate, string toDate)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<OR_to_Deliver> OrdersItem = new List<OR_to_Deliver>();
            Statement = "SELECT";
            string sqlquery = $"exec SP_OrderReport '{Statement}', '{CompanyId}', '{productStatus}', '{fromDate}', '{toDate}'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                OrdersItem.Add(new OR_to_Deliver
                {

                    order_id = Convert.ToInt32(dr["order_id"]),
                    order_no = Convert.ToString(dr["order_no"]),
                    UserName = Convert.ToString(dr["UserName"]),
                    amount = Convert.ToInt32(dr["amount"]),
                    MobileNo = Convert.ToString(dr["MobileNo"]),
                    order_date = Convert.ToString(dr["order_date"]),
                });
            }
            return Json(OrdersItem);
        }



        [HttpPost]
        public JsonResult ShowProductList(string searchitem,int statusactive)
        {
            Statement = "SELECT";

            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<AddNewProduct> product = new List<AddNewProduct>();
            sqlquery = "exec Sp_sel_del_tblItemStore '" + Statement + "',@companyid ='" + CompanyId + "',@searchitem= '" + searchitem + "',@status= '" + statusactive + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                product.Add(new AddNewProduct
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    image = Convert.ToString(dr["image"]),
                    ItemName = Convert.ToString(dr["ItemName"]),
                    //StockStatus = Convert.ToInt32(dr["stockqty"]),

                    GroupID = Convert.ToInt32(dr["GroupID"]),
                    Main_cat_name = Convert.ToString(dr["Main_cat_name"]),
                    CategoryID = Convert.ToInt32(dr["CategoryID"]),
                    category_name = Convert.ToString(dr["category_name"]),
                    SubGroupID = Convert.ToInt32(dr["SubGroupID"]),
                    SubCategory_name = Convert.ToString(dr["subgroupname"]),
                    productTag = Convert.ToString(dr["productTag"]),
                    Unit_Rate = Convert.ToDecimal(dr["Unit_Rate"]),
                    // Unit_Rate = Convert.ToInt32(dr["Unit_Rate"]),
                    Date = Convert.ToString(dr["EntryDate"]),
                    //BrandID = Convert.ToInt32(dr["BrandID"]),
                    Active = Convert.ToInt32(dr["Active"]),
                });
            }
            return Json(product);
        }


        [HttpPost]
        public JsonResult MainBannerShowid()
        {
           

            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<AddNewProduct> product = new List<AddNewProduct>();
            sqlquery = "select isnull(ID,0)as ID,typeid, isnull(TBanner1,'')as TBanner1,isnull(TUrl1,'')as TUrl1,status as  Active from tblCMSBanner_new where status=1  and typeid=1 and  companyId ='" + CompanyId + "'";
            DataSet ds = Utility.TableBind(sqlquery);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                product.Add(new AddNewProduct
                {
                    ID = Convert.ToInt32(dr["id"]),
                    HBanner = Convert.ToString(dr["TBanner1"]),
                    MUrl = Convert.ToString(dr["TUrl1"]),
                    typeid = Convert.ToInt32(dr["typeid"]),
                    Active = Convert.ToInt32(dr["Active"]),
                });
            }



            return Json(product);
        }


        [HttpPost]
        public JsonResult Footer3BannerShow()
        {


            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<AddNewProduct> product = new List<AddNewProduct>();
            sqlquery = "select isnull(ID,0)as ID,typeid, isnull(TBanner1,'')as TBanner1,isnull(TUrl1,'')as TUrl1,status as  Active from tblCMSBanner_new where status=1  and typeid=2 and  companyId ='" + CompanyId + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                product.Add(new AddNewProduct
                {
                    ID = Convert.ToInt32(dr["id"]),
                    HBanner = Convert.ToString(dr["TBanner1"]),
                    MUrl = Convert.ToString(dr["TUrl1"]),
                    typeid = Convert.ToInt32(dr["typeid"]),
                    Active = Convert.ToInt32(dr["Active"]),
                });
            }



            return Json(product);
        }





        [HttpPost]
        public JsonResult CatagroyeBannerShow()
        {


            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<AddNewProduct> product = new List<AddNewProduct>();
            sqlquery = "select isnull(ID,0)as ID,typeid, isnull(TBanner1,'')as TBanner1,isnull(TUrl1,'')as TUrl1,status as  Active from tblCMSBanner_new where status=1  and typeid=3 and  companyId ='" + CompanyId + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                product.Add(new AddNewProduct
                {
                    ID = Convert.ToInt32(dr["id"]),
                    HBanner = Convert.ToString(dr["TBanner1"]),
                    MUrl = Convert.ToString(dr["TUrl1"]),
                    typeid = Convert.ToInt32(dr["typeid"]),
                    Active = Convert.ToInt32(dr["Active"]),
                });
            }



            return Json(product);
        }


        [HttpPost]
        public JsonResult EditCatagroyeBannerShow( int id)
        {


            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<AddNewProduct> product = new List<AddNewProduct>();
            sqlquery = "select isnull(ID,0)as ID,typeid, isnull(TBanner1,'')as TBanner1,isnull(TUrl1,'')as TUrl1,status as  Active from tblCMSBanner_new where status=1  and typeid=3 and  companyId ='" + CompanyId + "'and  ID =" + id + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                product.Add(new AddNewProduct
                {
                    ID = Convert.ToInt32(dr["id"]),
                    HBanner = Convert.ToString(dr["TBanner1"]),
                    MUrl = Convert.ToString(dr["TUrl1"]),
                    typeid = Convert.ToInt32(dr["typeid"]),
                    Active = Convert.ToInt32(dr["Active"]),
                });
            }

            

            return Json(product);
        }


        [HttpPost]
        public JsonResult EditMainBannerShow(int id)
        {


            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<AddNewProduct> product = new List<AddNewProduct>();
            sqlquery = "select isnull(ID,0)as ID,typeid, isnull(TBanner1,'')as TBanner1,isnull(TUrl1,'')as TUrl1,status as  Active from tblCMSBanner_new where status=1  and typeid=1 and  companyId ='" + CompanyId + "'and  ID =" + id + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                product.Add(new AddNewProduct
                {
                    ID = Convert.ToInt32(dr["id"]),
                    HBanner = Convert.ToString(dr["TBanner1"]),
                    MUrl = Convert.ToString(dr["TUrl1"]),
                    typeid = Convert.ToInt32(dr["typeid"]),
                    Active = Convert.ToInt32(dr["Active"]),
                });
            }



            return Json(product);
        }





        [HttpPost]
        public JsonResult EditFooter3BannerShow(int id)
        {


            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<AddNewProduct> product = new List<AddNewProduct>();
            sqlquery = "select isnull(ID,0)as ID,typeid, isnull(TBanner1,'')as TBanner1,isnull(TUrl1,'')as TUrl1,status as  Active from tblCMSBanner_new where status=1  and typeid=2 and  companyId ='" + CompanyId + "'and  ID =" + id + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                product.Add(new AddNewProduct
                {
                    ID = Convert.ToInt32(dr["id"]),
                    HBanner = Convert.ToString(dr["TBanner1"]),
                    MUrl = Convert.ToString(dr["TUrl1"]),
                    typeid = Convert.ToInt32(dr["typeid"]),
                    Active = Convert.ToInt32(dr["Active"]),
                });
            }



            return Json(product);
        }


        [HttpPost]
        public JsonResult ShowProductList11(int vendor_id)
            {
            Statement = "SELECT1";

            string CompanyId = HttpContext.Session.GetString("Company_Id");

            sqlquery = "exec Sp_sel_del_tblItemStore @satement='" + Statement + "',@companyid='" + CompanyId + "',@Id='" + vendor_id + "' ";
            DataSet ds = Utility.TableBind(sqlquery);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                dt1 = ds.Tables[1];
            }
            var data = new { dt = JsonConvert.SerializeObject(dt), dt1 = JsonConvert.SerializeObject(dt1) };
            return Json(data);
        }



        public JsonResult Showvendoreport(int vendor_id)
        {
            Statement = "SELECT";

            // string CompanyId = HttpContext.Session.GetString("Company_Id");

            sqlquery = "exec sp_tbl_vendor_map @type='" + Statement + "',@vendor_id='" + vendor_id + "' ";
            DataSet ds = Utility.TableBind(sqlquery);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            var data = new { dt = JsonConvert.SerializeObject(dt) };
            return Json(data);
        }

        [HttpPost]
        public JsonResult VendorMapping(string jsondata)
        {
            Statement = "INSERT";

            string message = "";
            if (jsondata != "[]")
            {
                DataTable dtable = Utility.JsonStringToDataTable(jsondata);
                if (dtable.Rows.Count > 0)
                {
                    string query = "delete from tbl_vendor_map where vendor_id = '" + dtable.Rows[0]["vendor_id"] + "'";
                    string msg = Utility.MultipleTransactions(query);
                    for (int i = 0; i < dtable.Rows.Count; i++)
                    {
                        sqlquery += " exec sp_tbl_vendor_map @type='" + Statement + "', @vendor_id='" + dtable.Rows[i]["vendor_id"] + "',@item_id='" + dtable.Rows[i]["id"] + "',@vendor_price='" + dtable.Rows[i]["vendor_price"] + "'";
                    }
                    status = Utility.MultipleTransactions(sqlquery);


                    if (status == "Successfull")
                    {
                        message = "Item Mapped Successfully";
                    }
                    else
                    {
                        message = "Item Not Mapped";
                    }
                }
            }
            return Json(message);
        }

        public JsonResult StockReportList(string productname)
        {
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<AddNewProduct> product = new List<AddNewProduct>();
            sqlquery = "exec sp_stock_report '" + Statement + "'," + CompanyId + ", @productname='" + productname + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                product.Add(new AddNewProduct
                {
                    ID = Convert.ToInt32(dr["ID"]),

                    ItemName = Convert.ToString(dr["ItemName"]),

                    Unit_Rate = Convert.ToDecimal(dr["Unit_Rate"]),

                    stock_qty = Convert.ToInt32(dr["stockqty"]),


                });
            }
            return Json(product);
        }




        //public JsonResult Vendor_report(int id, string vendor_name, string vendor_email, string vendor_mobile, string vendor_address, string companyname)
        //{
        //    string message1 = "";
        //    DataTable dt = Utility.execQuery("select * from tblregistration where email='" + vendor_email + "' or MobileNo='" + vendor_mobile + "'");
        //    if (dt.Rows.Count > 0)
        //    {
        //        if (dt.Rows[0]["MobileNo"].ToString() == vendor_mobile)
        //        {
        //            message1 = "This Mobile number already exit.";
        //        }
        //        else if (dt.Rows[0]["email"].ToString() == vendor_email)
        //        {
        //            message1 = "This Email Id already exit.";
        //        }
        //        else
        //        {
        //            message1 = "This Email id and Mobile number already exit.";
        //        }
        //    }
        //    else
        //    {
        //        if (id == 0)
        //        {
        //            if (Db.CustomerRegistraion1(Utility.FixQuotes(vendor_name.Trim()), Utility.FixQuotes(vendor_email.Trim()), Utility.FixQuotes(vendor_mobile.Trim()), Utility.cryption(vendor_address.Trim()), Utility.FixQuotes(companyname.Trim())))
        //            {
        //                message = "Registration Success.";
        //            }

        //        }
        //        else
        //        {
        //            message = "Some thing wrong.";
        //        }
        //    }
        //    var Data = new { message = message, vendor_email = vendor_email, message1 = message1 };
        //    return Json(Data);
        //}


        [HttpPost]
        public JsonResult DeleteProductList(int id)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Statement = "DELETE";
            sqlquery = "exec Sp_sel_del_tblItemStore '" + Statement + "','" + CompanyId + "'," + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult DeleteUNITTYPE(int id)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            //  Statement = "DELETE";
            sqlquery = " DELETE tblitemdetails WHERE ID =" + id + "  ";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }

        //[HttpPost]
        //public IActionResult EditProducr(int id)
        //{
        //    Statement = "EDIT";
        //    string CompanyId = HttpContext.Session.GetString("Company_Id");
        //    AddNewProduct product = new AddNewProduct();
        //    sqlquery = "exec Sp_sel_del_tblItemStore '" + Statement + "','"+CompanyId+"'," + id + "";
        //    DataSet ds = Utility.TableBind(sqlquery);
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        product.ID = Convert.ToInt32(dr["ID"]);
        //        product.ItemName = Convert.ToString(dr["ItemName"]);
        //        product.URLName = Convert.ToString(dr["URLName"]);
        //        product.GroupID = Convert.ToInt32(dr["GroupID"]);
        //        product.CategoryID = Convert.ToInt32(dr["CategoryID"]);
        //        product.SubGroupID = Convert.ToInt32(dr["SubGroupID"]);
        //        product.productTag = Convert.ToString(dr["productTag"]);
        //        product.HSNCode = Convert.ToString(dr["HSNCode"]);
        //        product.SKUCode = Convert.ToString(dr["SKUCode"]);
        //        product.CostPrice = Convert.ToInt32(dr["CostPrice"]);
        //        product.MRP = Convert.ToInt32(dr["MRP"]);
        //        product.StockStatus = Convert.ToInt32(dr["StockStatus"]);
        //        product.Dimension = Convert.ToString(dr["Dimension"]);
        //        product.ShipCharge = Convert.ToString(dr["ShipCharge"]);
        //        product.Description = Convert.ToString(dr["Description"]);
        //        product.additional = Convert.ToString(dr["additional"]);
        //        product.ingredients = Convert.ToString(dr["ingredients"]);
        //    }
        //    return RedirectToAction("NewProduct",product);
        //}
        [HttpPost]
        public JsonResult GetAllProduct(int id)
        {
            List<AddNewProduct> product = new List<AddNewProduct>();
            sqlquery = "select * from tblItemStore where status=1 and ID='" + id + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                product.Add(new AddNewProduct
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    image = Convert.ToString(dr["image"]),
                    ItemName = Convert.ToString(dr["ItemName"]),
                    SKUCode = Convert.ToString(dr["SKUCode"]),
                    StockStatus = Convert.ToInt32(dr["StockStatus"]),
                    MRP = Convert.ToInt32(dr["MRP"]),
                    CategoryID = Convert.ToInt32(dr["CategoryID"]),
                    productTag = Convert.ToString(dr["productTag"]),
                    Date = Convert.ToString(dr["EntryDate"]),
                    BrandID = Convert.ToInt32(dr["BrandID"]),
                    LastPurchasePrice = Convert.ToInt32(dr["LastPurchasePrice"]),
                    GroupID = Convert.ToInt32(dr["GroupID"]),
                    SubGroupID = Convert.ToInt32(dr["GroupID"]),
                    Description = Convert.ToString(dr["Description"]),
                    URLName = Convert.ToString(dr["URLName"]),
                    ingredients = Convert.ToString(dr["ingredients"]),
                    additional = Convert.ToString(dr["additional"]),
                    unit = Convert.ToString(dr["unit"]),
                    HSNCode = Convert.ToString(dr["HSNCode"]),
                    CostPrice = Convert.ToInt32(dr["CostPrice"]),
                    Weight = Convert.ToString(dr["Weight"]),
                    ShipCharge = Convert.ToString(dr["ShipCharge"])
                });
            }
            return Json(product);
        }
        #endregion

        #region Brand
        public IActionResult Brand()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        [HttpPost]
        public IActionResult SaveBrand()
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            string brand_id = Request.Form["brand_id"].ToString();
            string brand_name = Request.Form["brand_name"].ToString();
            string item_cat_id = Request.Form["item_cat_id"].ToString();
            string Status = Request.Form["Status"].ToString();
            string flg = Request.Form["flg"].ToString();
            string nefilname = Request.Form["filenmes"].ToString();
            string webRootPath = "";
            string filename = "";
            if (flg == "okg")
            {
                nefilname = "";
                IFormFile file = Request.Form.Files[0];
                string folderName = "wwwroot/Images/Brand/" + CompanyId + "/";
                string extension = Path.GetExtension(file.FileName);
                filename = Path.GetFileNameWithoutExtension(file.FileName);
                //nefilname = filename;
                webRootPath = filename + DateTime.Now.ToString("yymmssff") + extension;
                nefilname = webRootPath;
                string newPath = Path.Combine(folderName, webRootPath);
                using (var fileStream = new FileStream(newPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            if (brand_id == "0")
            {
                sqlquery = "exec Sp_IU_Brand " + brand_id + ",'" + brand_name + "'," + item_cat_id + "," + Status + "," + userid + ",'" + webRootPath + "'," + CompanyId + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Brand added";
                }
                else
                {
                    message = "Brand not added.";
                }
            }
            else
            {
                sqlquery = "exec Sp_IU_Brand " + brand_id + ",'" + brand_name + "'," + item_cat_id + "," + Status + "," + userid + ",'" + nefilname + "'," + CompanyId + "";
                string status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Brand added";
                }
                else
                {
                    message = "Main Category not added.";
                }
            }
            var Data = new { message = message, brand_id = brand_id };
            return Json(Data);
        }
        [HttpPost]
        public JsonResult ShowAllBrand()
        {
            Statement = "SELECT";
            List<Brand> Brand = new List<Brand>();
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "exec Sp_show_Brand '" + Statement + "','" + CompanyId + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Brand.Add(new Brand
                {
                    brand_id = Convert.ToInt32(dr["brand_id"]),
                    brand_name = Convert.ToString(dr["brand_name"]),
                    brand_status = Convert.ToBoolean(dr["brand_status"]),
                    brandImage = Convert.ToString(dr["brandImage"])
                });
            }
            return Json(Brand);
        }
        [HttpPost]
        public JsonResult EditBrand(int brand_id)
        {
            Statement = "EDIT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Brand brand = new Brand();
            sqlquery = "exec Sp_show_Brand '" + Statement + "','" + CompanyId + "', " + brand_id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                brand.brand_id = Convert.ToInt32(dr["brand_id"]);
                brand.brand_name = Convert.ToString(dr["brand_name"]);
                brand.brand_status = Convert.ToBoolean(dr["brand_status"]);
                brand.brandImage = Convert.ToString(dr["brandImage"]);
            }
            return Json(brand);
        }
        [HttpPost]
        public JsonResult DeleteBrand(int brand_id)
        {
            Statement = "DELETE";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "exec Sp_show_Brand '" + Statement + "','" + CompanyId + "'," + brand_id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }
        #endregion

        #region LocalityDetails
        public IActionResult LocalityDetails()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            BindLocalityType();
            return View();
        }
        [HttpPost]
        public IActionResult SaveLocalityDetails()
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            string id = Request.Form["id"].ToString();
            string Name = Request.Form["Name"].ToString();
            string contact_name = Request.Form["contact_name"].ToString();
            string mobile_no = Request.Form["mobile_no"].ToString();
            string content = Request.Form["content"].ToString();
            string pincode = Request.Form["pincode"].ToString();
            string Loc_address = Request.Form["Loc_address"].ToString();
            string phone = Request.Form["phone"].ToString();
            string Loc_id = Request.Form["Loc_id"].ToString();
            string Status = Request.Form["Status"].ToString();
            string flg = Request.Form["flg"].ToString();
            string nefilname = Request.Form["filenmes"].ToString();
            string webRootPath = "";
            string filename = "";
            if (flg == "okg")
            {
                nefilname = "";
                IFormFile file = Request.Form.Files[0];
                string folderName = "wwwroot/Images/Locality_Img/" + CompanyId + "/";
                string extension = Path.GetExtension(file.FileName);
                filename = Path.GetFileNameWithoutExtension(file.FileName);
                //nefilname = filename;
                webRootPath = filename + DateTime.Now.ToString("yymmssfff") + extension;
                nefilname = webRootPath;
                string newPath = Path.Combine(folderName, webRootPath);
                using (var fileStream = new FileStream(newPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            if (id == "0")
            {
                Statement = "INSERT";
                sqlquery = "exec SP_tblLocality_details  '" + Statement + "'," + id + ",'" + Name + "','" + contact_name + "','" + webRootPath + "','" + content + "'," + pincode + ",'" + Loc_address + "'," + mobile_no + "," + phone + "," + Loc_id + "," + Status + "," + CompanyId + "," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Locality Details added";
                }
                else
                {
                    message = "Locality Details not added.";
                }
            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec SP_tblLocality_details  '" + Statement + "'," + id + ",'" + Name + "','" + contact_name + "','" + nefilname + "','" + content + "'," + pincode + ",'" + Loc_address + "'," + mobile_no + "," + phone + "," + Loc_id + "," + Status + "," + CompanyId + "," + userid + "";
                string status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Locality Details added";
                }
                else
                {
                    message = "Locality Details not added.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }
        [HttpPost]
        public JsonResult ShowLocalityDetails()
        {
            List<LocalityDetails> locality = new List<LocalityDetails>();
            sqlquery = "exec SP_tblLocality_details";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                locality.Add(new LocalityDetails
                {
                    id = Convert.ToInt32(dr["id"]),
                    Name = Convert.ToString(dr["Name"]),
                    contact_name = Convert.ToString(dr["contact_name"]),
                    img = Convert.ToString(dr["img"]),
                    content = Convert.ToString(dr["content"]),
                    pin = Convert.ToString(dr["pincode"]),
                    Loc_address = Convert.ToString(dr["Loc_address"]),
                    phone1 = Convert.ToString(dr["phone"]),
                    mobile_no1 = Convert.ToString(dr["mobile_no"]),
                    loc_status = Convert.ToBoolean(dr["loc_status"]),
                    Loc_Name = Convert.ToString(dr["Loc_Name"]),
                });
            }
            return Json(locality);
        }
        [HttpPost]
        public JsonResult EditLocalityDetails(int id)
        {
            Statement = "EDIT";
            LocalityDetails locality = new LocalityDetails();
            sqlquery = "exec SP_tblLocality_details '" + Statement + "', " + id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                locality.id = Convert.ToInt32(dr["id"]);
                locality.Name = Convert.ToString(dr["Name"]);
                locality.contact_name = Convert.ToString(dr["contact_name"]);
                locality.img = Convert.ToString(dr["img"]);
                locality.content = Convert.ToString(dr["content"]);
                locality.pincode = Convert.ToInt32(dr["pincode"]);
                locality.Loc_address = Convert.ToString(dr["Loc_address"]);
                locality.phone1 = Convert.ToString(dr["phone"]);
                locality.mobile_no1 = Convert.ToString(dr["mobile_no"]);
                locality.loc_status = Convert.ToBoolean(dr["loc_status"]);
                locality.Loc_id = Convert.ToInt32(dr["Loc_id"]);
            }
            return Json(locality);
        }
        [HttpPost]
        public JsonResult DeleteLocalityDetails(int id)
        {
            Statement = "DELETE";
            sqlquery = "exec SP_tblLocality_details '" + Statement + "'," + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }
        #endregion

        #region Locality
        public IActionResult Locality()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        [HttpPost]
        public IActionResult SaveLocality()
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            string Loc_id = Request.Form["Loc_id"].ToString();
            string Name = Request.Form["Name"].ToString();
            string Status = Request.Form["Status"].ToString();
            string content = Request.Form["content"].ToString();
            string flg = Request.Form["flg"].ToString();
            string nefilname = Request.Form["filenmes"].ToString();
            string webRootPath = "";
            string filename = "";
            if (flg == "okg")
            {
                nefilname = "";
                IFormFile file = Request.Form.Files[0];
                string folderName = "wwwroot/Images/Locality_Img";
                string extension = Path.GetExtension(file.FileName);
                filename = Path.GetFileNameWithoutExtension(file.FileName);
                //nefilname = filename;
                webRootPath = filename + DateTime.Now.ToString("yymmssfff") + extension;
                nefilname = webRootPath;
                string newPath = Path.Combine(folderName, webRootPath);

                using (var fileStream = new FileStream(newPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            if (Loc_id == "0")
            {
                Statement = "INSERT";
                sqlquery = "exec SP_Mst_Locality  '" + Statement + "'," + Loc_id + ",'" + Name + "','" + webRootPath + "'," + Status + "," + CompanyId + ",'" + content + "'," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Locality added";
                }
                else
                {
                    message = "Locality not added.";
                }
            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec SP_Mst_Locality  '" + Statement + "'," + Loc_id + ",'" + Name + "','" + nefilname + "'," + Status + "," + CompanyId + ",'" + content + "'," + userid + "";
                string status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Locality added";
                }
                else
                {
                    message = "Locality not added.";
                }
            }
            var Data = new { message = message, id = Loc_id };
            return Json(Data);
        }
        [HttpPost]
        public JsonResult ShowLocality()
        {
            List<Locality> locality = new List<Locality>();
            sqlquery = "exec SP_Mst_Locality";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                locality.Add(new Locality
                {
                    Loc_id = Convert.ToInt32(dr["Loc_id"]),
                    Name = Convert.ToString(dr["Name"]),
                    Img = Convert.ToString(dr["Img"]),
                    loc_status = Convert.ToBoolean(dr["loc_status"]),
                });
            }
            return Json(locality);
        }
        [HttpPost]
        public JsonResult EditLocality(int Loc_id)
        {
            Statement = "EDIT";
            Locality locality = new Locality();
            sqlquery = "exec SP_Mst_Locality '" + Statement + "', " + Loc_id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                locality.Loc_id = Convert.ToInt32(dr["Loc_id"]);
                locality.Name = Convert.ToString(dr["Name"]);
                locality.Img = Convert.ToString(dr["Img"]);
                locality.loc_status = Convert.ToBoolean(dr["loc_status"]);
                locality.content = Convert.ToString(dr["content"]);
            }
            return Json(locality);
        }
        [HttpPost]
        public JsonResult DeleteLocality(int Loc_id)
        {
            Statement = "DELETE";
            sqlquery = "exec SP_Mst_Locality '" + Statement + "'," + Loc_id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }
        #endregion

        #region DisplayItem
        public IActionResult DisplayItem()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            BindMainItemCategory();
            return View();
        }

        [HttpPost]
        public JsonResult ShowItemDisplay(string Supercat, string Item)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Statement = "SELECT";
            List<DisplayItem> item = new List<DisplayItem>();
            sqlquery = "exec SP_Show_tblItemStore '" + Statement + "','" + CompanyId + "','" + Supercat + "','" + Item + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                item.Add(new DisplayItem
                {

                    ID = Convert.ToInt32(dr["ID"]),
                    ItemName = Convert.ToString(dr["ItemName"]),
                    Main_cat_name = Convert.ToString(dr["Main_cat_name"]),
                    cat_name = Convert.ToString(dr["category_name"]),
                    Status = Convert.ToString(dr["Status"]),
                    display = Convert.ToString(dr["display"]),
                });
            }
            return Json(item);
        }

        [HttpPost]
        public IActionResult UpdateItemDisplay(string itemdisplayjson)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            //string order_id = Request.Form["order_id"].ToString();
            //string Status = Request.Form["chkStatus"].ToString();
            //string Unitdetails = Request.Form["json"].ToString();
            DataTable dt = Utility.JsonStringToDataTable(itemdisplayjson);
            int row = dt.Rows.Count;
            int col = dt.Columns.Count;
            int i = 0;
            if (dt.Rows.Count > 0)
            {


                for (i = 0; i < row; i++)
                {

                    if (dt.Rows[i]["chkdisplay"].ToString() == "1")
                    {
                        sqlquery += "Update tblItemStore set display=1 where ID= '" + dt.Rows[i]["ID"].ToString() + "'";
                    }
                    else
                    {
                        sqlquery += "Update tblItemStore set display=0 where ID= '" + dt.Rows[i]["ID"].ToString() + "'";
                    }

                }
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Item approve.";

                }
                else
                {
                    message = "Item approve not Successfully.";
                }
            }
            return Json(message);
        }
        #endregion

        #region MStCMSBanner
        public IActionResult MStCMSBanner_srv()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }


        [HttpPost]
        public ActionResult SaveCMSBanner(MStCMSBanner_srv CMSBanner)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            string item = Request.Form["item"].ToString();
            JArray objects = JArray.Parse(item == "" ? "[]" : item);
            string flg1 = Request.Form["flg1"].ToString();
            string flg2 = Request.Form["flg2"].ToString();
            string flg3 = Request.Form["flg3"].ToString();
            string flg4 = Request.Form["flg4"].ToString();
            string flg5 = Request.Form["flg5"].ToString();
            string flg6 = Request.Form["flg6"].ToString();

            string nefilname1 = Request.Form["filenmes1"].ToString();
            string nefilname2 = Request.Form["filenmes2"].ToString();
            string nefilname3 = Request.Form["filenmes3"].ToString();
            string nefilname4 = Request.Form["filenmes4"].ToString();
            string nefilname5 = Request.Form["filenmes5"].ToString();
            string nefilname6 = Request.Form["filenmes6"].ToString();
            string folderName = "wwwroot/images/Banner/" + CompanyId + "/";

            string webRootPath1 = "";
            string webRootPath2 = "";
            string webRootPath3 = "";
            string webRootPath4 = "";
            string webRootPath5 = "";
            string webRootPath6 = "";

            string filename1 = "";
            string filename2 = "";
            string filename3 = "";
            string filename4 = "";
            string filename5 = "";
            string filename6 = "";
            /*-----------------------------------------------------*/
            if (flg1 == "okg1")
            {
                if (CMSBanner.filenmes1 != null)
                {
                    nefilname1 = "";
                    filename1 = Path.GetFileNameWithoutExtension(CMSBanner.filenmes1.FileName);
                    string extension = Path.GetExtension(CMSBanner.filenmes1.FileName);
                    //string filename = Path.GetFileNameWithoutExtension(Studentimg.FileName);
                    webRootPath1 = filename1 + DateTime.Now.ToString("yymmssff") + extension;
                    //nefilname = filename;
                    nefilname1 = webRootPath1;
                    string newPath = Path.Combine(folderName, webRootPath1);
                    using (var fileStream = new FileStream(newPath, FileMode.Create))
                    {
                        CMSBanner.filenmes1.CopyTo(fileStream);
                    }
                }
                else
                {
                    webRootPath1 = "noPhoto.png";
                }
            }
            /*-----------------------------------------------------*/
            if (flg2 == "okg2")
            {
                if (CMSBanner.filenmes2 != null)
                {
                    nefilname2 = "";
                    filename2 = Path.GetFileNameWithoutExtension(CMSBanner.filenmes2.FileName);
                    string extension = Path.GetExtension(CMSBanner.filenmes2.FileName);
                    //string filename = Path.GetFileNameWithoutExtension(Studentimg.FileName);
                    webRootPath2 = filename2 + DateTime.Now.ToString("yymmssff") + extension;
                    //nefilname = filename;
                    nefilname2 = webRootPath2;
                    string newPath = Path.Combine(folderName, webRootPath2);
                    using (var fileStream = new FileStream(newPath, FileMode.Create))
                    {
                        CMSBanner.filenmes2.CopyTo(fileStream);
                    }
                }
                else
                {
                    webRootPath2 = "noPhoto.png";
                }
            }
            /*-----------------------------------------------------*/
            if (flg3 == "okg3")
            {
                if (CMSBanner.filenmes3 != null)
                {
                    nefilname3 = "";
                    filename3 = Path.GetFileNameWithoutExtension(CMSBanner.filenmes3.FileName);
                    string extension = Path.GetExtension(CMSBanner.filenmes3.FileName);
                    //string filename = Path.GetFileNameWithoutExtension(Studentimg.FileName);
                    webRootPath3 = filename3 + DateTime.Now.ToString("yymmssff") + extension;
                    //nefilname = filename;
                    nefilname3 = webRootPath3;
                    string newPath = Path.Combine(folderName, webRootPath3);
                    using (var fileStream = new FileStream(newPath, FileMode.Create))
                    {
                        CMSBanner.filenmes3.CopyTo(fileStream);
                    }
                }
                else
                {
                    webRootPath3 = "noPhoto.png";
                }
            }
            /*-----------------------------------------------------*/
            if (flg4 == "okg4")
            {
                if (CMSBanner.filenmes4 != null)
                {
                    nefilname4 = "";
                    filename4 = Path.GetFileNameWithoutExtension(CMSBanner.filenmes4.FileName);
                    string extension = Path.GetExtension(CMSBanner.filenmes4.FileName);
                    //string filename = Path.GetFileNameWithoutExtension(Studentimg.FileName);
                    webRootPath4 = filename4 + DateTime.Now.ToString("yymmssff") + extension;
                    //nefilname = filename;
                    nefilname4 = webRootPath4;
                    string newPath = Path.Combine(folderName, webRootPath4);
                    using (var fileStream = new FileStream(newPath, FileMode.Create))
                    {
                        CMSBanner.filenmes4.CopyTo(fileStream);
                    }
                }
                else
                {
                    webRootPath4 = "noPhoto.png";
                }
            }
            /*-----------------------------------------------------*/
            if (flg5 == "okg5")
            {
                if (CMSBanner.filenmes5 != null)
                {
                    nefilname5 = "";
                    filename5 = Path.GetFileNameWithoutExtension(CMSBanner.filenmes5.FileName);
                    string extension = Path.GetExtension(CMSBanner.filenmes5.FileName);
                    //string filename = Path.GetFileNameWithoutExtension(Studentimg.FileName);
                    webRootPath5 = filename5 + DateTime.Now.ToString("yymmssff") + extension;
                    //nefilname = filename;
                    nefilname5 = webRootPath5;
                    string newPath = Path.Combine(folderName, webRootPath5);
                    using (var fileStream = new FileStream(newPath, FileMode.Create))
                    {
                        CMSBanner.filenmes5.CopyTo(fileStream);
                    }
                }
                else
                {
                    webRootPath5 = "noPhoto.png";
                }
            }

            /*-----------------------------------------------------*/
            if (flg6 == "okg6")
            {
                if (CMSBanner.filenmes6 != null)
                {
                    nefilname6 = "";
                    filename6 = Path.GetFileNameWithoutExtension(CMSBanner.filenmes6.FileName);
                    string extension = Path.GetExtension(CMSBanner.filenmes6.FileName);
                    //string filename = Path.GetFileNameWithoutExtension(Studentimg.FileName);
                    webRootPath6 = filename6 + DateTime.Now.ToString("yymmssff") + extension;
                    //nefilname = filename;
                    nefilname6 = webRootPath6;
                    string newPath = Path.Combine(folderName, webRootPath6);
                    using (var fileStream = new FileStream(newPath, FileMode.Create))
                    {
                        CMSBanner.filenmes6.CopyTo(fileStream);
                    }
                }
                else
                {
                    webRootPath6 = "noPhoto.png";
                }

            }

            if (objects[0]["id"].ToString() == "0")
            {

                foreach (var items in objects)
                {

                    Statement = "insert";
                    string webRootPath = "";
                    string webRootPaths = webRootPath;
                    if (items["typeid"].ToString() == "1")
                    {
                        sqlquery += " exec SP_tblCMSBanner '" + Statement + "',@CompanyId='" + CompanyId + "',@ENtryUser='" + userid + "',@TBanner1='" + webRootPath5 + "',@Status=" + items["Status"] + ",@TUrl1='" + items["Url5"] + "',@typeid=" + items["typeid"] + "";
                    }
                    else if (items["typeid"].ToString() == "2")
                    {
                        string photos = "";
                        photos += webRootPath1 + ",";
                        photos += webRootPath2 + ",";
                        photos += webRootPath3;
                        string[] allphoto = photos.Split(",");
                        int i = 1;
                        foreach (var photo in allphoto)
                        {
                            if (photo != "noPhoto.png" && items["Url" + i + ""].ToString() != "")
                            {


                                sqlquery += " exec SP_tblCMSBanner '" + Statement + "',@CompanyId='" + CompanyId + "',@ENtryUser='" + userid + "',@TBanner1='" + photo + "',@Status=" + items["Status"] + ",@TUrl1='" + items["Url" + i + ""] + "',@typeid=" + items["typeid"] + "";

                            }
                            i++;

                        }

                    }
                    else if (items["typeid"].ToString() == "3")
                    {
                        sqlquery += " exec SP_tblCMSBanner '" + Statement + "',@CompanyId='" + CompanyId + "',@ENtryUser='" + userid + "',@TBanner1='" + webRootPath6 + "',@Status=" + items["Status"] + ",@TUrl1='" + items["Url6"] + "',@typeid=" + items["typeid"] + "";
                    }
                    else if (items["typeid"].ToString() == "4")
                    {
                        sqlquery += " exec SP_tblCMSBanner '" + Statement + "',@CompanyId='" + CompanyId + "',@ENtryUser='" + userid + "',@TBanner1='" + webRootPath4 + "',@Status=" + items["Status"] + ",@TUrl1='" + items["Url4"] + "',@typeid=" + items["typeid"] + "";
                    }

                }

                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Saved Successfully";
                }
                else
                {
                    message = "Data not Saved";
                }
            }
            else
            {
                foreach (var items in objects)
                {

                    Statement = "update";
                    string webRootPath = "";
                    string webRootPaths = webRootPath;
                    if (items["typeid"].ToString() == "1")
                    {
                        sqlquery += " exec SP_tblCMSBanner '" + Statement + "',@CompanyId='" + CompanyId + "',@ENtryUser='" + userid + "',@TBanner1='" + nefilname5 + "',@Status=" + items["Status"] + ",@TUrl1='" + items["Url5"] + "',@typeid=" + items["typeid"] + "  , @ID=" + items["id"] + "";
                    }
                    else if (items["typeid"].ToString() == "2")
                    {
                        string photos = "";
                        photos += nefilname1 + ",";
                        photos += nefilname2 + ",";
                        photos += nefilname3;
                        string[] allphoto = photos.Split(",");
                        int i = 1;
                        foreach (var photo in allphoto)
                        {
                            if (photo != "noPhoto.png" && items["Url" + i + ""].ToString() != "")
                            {


                                sqlquery += " exec SP_tblCMSBanner '" + Statement + "',@CompanyId='" + CompanyId + "',@ENtryUser='" + userid + "',@TBanner1='" + photo + "',@Status=" + items["Status"] + ",@TUrl1='" + items["Url" + i + ""] + "',@typeid=" + items["typeid"] + " , @ID=" + items["id"] + "";

                            }
                            i++;

                        }

                    }
                    else if (items["typeid"].ToString() == "3")
                    {
                        sqlquery += " exec SP_tblCMSBanner '" + Statement + "',@CompanyId='" + CompanyId + "',@ENtryUser='" + userid + "',@TBanner1='" + nefilname6 + "',@Status=" + items["Status"] + ",@TUrl1='" + items["Url6"] + "',@typeid=" + items["typeid"] + " , @ID=" + items["id"] + "";
                    }
                    else if (items["typeid"].ToString() == "4")
                    {
                        sqlquery += " exec SP_tblCMSBanner '" + Statement + "',@CompanyId='" + CompanyId + "',@ENtryUser='" + userid + "',@TBanner1='" + nefilname4 + "',@Status=" + items["Status"] + ",@TUrl1='" + items["Url4"] + "',@typeid=" + items["typeid"] + ", @ID=" + items["id"] + "";
                    }

                }


                //Statement = "update";
                //sqlquery = "SP_tblCMSBanner '" + Statement + "','" + CompanyId + "','" + status + "','" + nefilname1 + "'";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Updated Successfully";

                }
                else
                {
                    message = "Data Not Updated";
                }
            }
            var Data = new { message = message };
            return Json(Data);
        }





        //public IActionResult SaveMStCMSBanner_srv()
        //{
        //    string userid = HttpContext.Session.GetString("AdminId");
        //    string CompanyId = HttpContext.Session.GetString("Company_Id");
        //    //string folder = Path.Combine(string.Format("/wwwroot/images/Banner/"+ CompanyId));
        //    //if (!Directory.Exists(folder))
        //    //{
        //    //    Directory.CreateDirectory(folder);
        //    //    ViewBag.Message = "Folder " + CompanyId.ToString() + " created successfully!";
        //    //}
        //    //else
        //    //{
        //    //    ViewBag.Message = "Folder " + CompanyId.ToString() + "  already exists!";
        //    //}

        //    string ID = Request.Form["ID"].ToString();
        //    string TUrl1 = Request.Form["TUrl1"].ToString();
        //    string TUrl2 = Request.Form["TUrl2"].ToString();
        //    string TUrl3 = Request.Form["TUrl3"].ToString();
        //    string FUrl2 = Request.Form["FUrl2"].ToString();
        //    string MUrl = Request.Form["MUrl"].ToString();
        //    string category_bannerUrl = Request.Form["category_bannerUrl"].ToString();
        //    string Status = Request.Form["Status"].ToString();
        //    string flg = Request.Form["flg"].ToString();
        //    string bflg = Request.Form["bflg"].ToString();
        //    string tflg = Request.Form["tflg"].ToString();
        //    string fflg = Request.Form["fflg"].ToString();
        //    string mflg = Request.Form["mflg"].ToString();
        //    string cflg = Request.Form["cflg"].ToString();
        //    string nefilname = Request.Form["filenmes"].ToString();
        //    string bnefilname = Request.Form["bfilenmes"].ToString();
        //    string tnefilname = Request.Form["tfilenmes"].ToString();
        //    string fnefilname = Request.Form["ffilenmes"].ToString();
        //    string mnefilname = Request.Form["mfilenmes"].ToString();
        //    string cnefilname = Request.Form["cfilenmes"].ToString();
        //    string webRootPath = ""; string webRootPathb = ""; string twebRootPath = ""; string fwebRootPath = ""; string mwebRootPath = ""; string cwebRootPath = "";
        //    string folderName = "wwwroot/images/Banner/"+ CompanyId + "/";
        //    if (!Directory.Exists(folderName))
        //    {
        //        Directory.CreateDirectory(folderName);
        //    }
        //    if (flg == "okg")
        //    {
        //        nefilname = "";
        //        IFormFile file = Request.Form.Files[0];
        //        string extension = Path.GetExtension(file.FileName);
        //        string filename = Path.GetFileNameWithoutExtension(file.FileName);
        //        //nefilname = filename;
        //        webRootPath = filename + DateTime.Now.ToString("yymmssfff") + extension;
        //        nefilname = webRootPath;
        //        string newPath = Path.Combine(folderName, webRootPath);

        //        using (var fileStream = new FileStream(newPath, FileMode.Create))
        //        {
        //            file.CopyTo(fileStream);
        //        }
        //    }
        //    if (bflg == "okg")
        //    {
        //        bnefilname = "";
        //        IFormFile bfile = Request.Form.Files[1];
        //        string extension1 = Path.GetExtension(bfile.FileName);
        //        string bfilename = Path.GetFileNameWithoutExtension(bfile.FileName);
        //        //nefilname = filename;
        //        webRootPathb = bfilename + DateTime.Now.ToString("yymmssfff") + extension1;
        //        bnefilname = webRootPathb;
        //        string newPathb = Path.Combine(folderName, webRootPathb);
        //        using (var fileStream = new FileStream(newPathb, FileMode.Create))
        //        {
        //            bfile.CopyTo(fileStream);
        //        }
        //    }
        //    if (tflg == "okg")
        //    {
        //        tnefilname = "";
        //        IFormFile tfile = Request.Form.Files[2];
        //        string extension2 = Path.GetExtension(tfile.FileName);
        //        string tilename = Path.GetFileNameWithoutExtension(tfile.FileName);
        //        //nefilname = filename;
        //        twebRootPath = tilename + DateTime.Now.ToString("yymmssfff") + extension2;
        //        tnefilname = twebRootPath;
        //        string newPatht = Path.Combine(folderName, twebRootPath);
        //        using (var fileStream = new FileStream(newPatht, FileMode.Create))
        //        {
        //            tfile.CopyTo(fileStream);
        //        }
        //    }
        //    if (fflg == "okg")
        //    {
        //        fnefilname = "";
        //        IFormFile ffile = Request.Form.Files[3];
        //        string extension3 = Path.GetExtension(ffile.FileName);
        //        string ffilename = Path.GetFileNameWithoutExtension(ffile.FileName);
        //        //nefilname = filename;
        //        fwebRootPath = ffilename + DateTime.Now.ToString("yymmssfff") + extension3;
        //        fnefilname = fwebRootPath;
        //        string newPath1 = Path.Combine(folderName, fwebRootPath);
        //        using (var fileStream = new FileStream(newPath1, FileMode.Create))
        //        {
        //            ffile.CopyTo(fileStream);
        //        }
        //    }
        //    if (mflg == "okg")
        //    {
        //        mnefilname = "";
        //        IFormFile mfile = Request.Form.Files[4];
        //        string extension4 = Path.GetExtension(mfile.FileName);
        //        string mfilename = Path.GetFileNameWithoutExtension(mfile.FileName);
        //        //nefilname = filename;
        //         mwebRootPath = mfilename + DateTime.Now.ToString("yymmssfff") + extension4;
        //        mnefilname = mwebRootPath;
        //        string newPath2 = Path.Combine(folderName, mwebRootPath);
        //        using (var fileStream = new FileStream(newPath2, FileMode.Create))
        //        {
        //            mfile.CopyTo(fileStream);
        //        }
        //    }
        //    if (cflg == "okg")
        //    {
        //        cnefilname = "";
        //        IFormFile cfile = Request.Form.Files[5];
        //        string extension5 = Path.GetExtension(cfile.FileName);
        //        string cfilename = Path.GetFileNameWithoutExtension(cfile.FileName);
        //        //nefilname = filename;
        //        cwebRootPath = cfilename + DateTime.Now.ToString("yymmssfff") + extension5;
        //        cnefilname = cwebRootPath;
        //        string newPath3 = Path.Combine(folderName, cwebRootPath);
        //        using (var fileStream = new FileStream(newPath3, FileMode.Create))
        //        {
        //            cfile.CopyTo(fileStream);
        //        }
        //    }

        //    if (ID == "0")
        //    {
        //        Statement = "INSERT";
        //        sqlquery = "exec SP_tblCMSBanner '" + Statement + "'," + CompanyId + "," + ID + ",'" + webRootPath + "','" + webRootPathb + "','" + twebRootPath + "','" + fwebRootPath + "','" + mwebRootPath + "'," + userid + "," + Status + ",'" + TUrl1 + "','" + TUrl2 + "','" + TUrl3 + "','" + FUrl2 + "','" + MUrl + "','" + cwebRootPath + "','" + category_bannerUrl + "'";
        //        status = Utility.MultipleTransactions(sqlquery);
        //        if (status == "Successfull")
        //        {
        //            message = "Banner added";
        //        }
        //        else
        //        {
        //            message = "Banner not added.";
        //        }
        //    }
        //    else
        //    {
        //        Statement = "UPDATE";
        //        sqlquery = "exec SP_tblCMSBanner '" + Statement + "'," + CompanyId + "," + ID + ",'" + nefilname + "','" + bnefilname + "','" + tnefilname + "','" + fnefilname + "','" + mnefilname + "'," + userid + "," + Status + ",'" + TUrl1 + "','" + TUrl2 + "','" + TUrl3 + "','" + FUrl2 + "','" + MUrl + "','" + cnefilname + "','" + category_bannerUrl + "'";
        //        string status = Utility.MultipleTransactions(sqlquery);
        //        if (status == "Successfull")
        //        {
        //            message = "Banner added successfully.";
        //        }
        //        else
        //        {
        //            message = "Banner modify successfully.";
        //        }
        //    }
        //    var Data = new { message = message, id = ID };
        //    return Json(Data);
        //}
        [HttpPost]
        public JsonResult ShowBannerService()
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<MStCMSBanner_srv> Banner = new List<MStCMSBanner_srv>();
            //            Statement = "TBBANNER";
            Statement = "SELECT";
            sqlquery = "exec SP_tblCMSBanner '" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Banner.Add(new MStCMSBanner_srv
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    TBanner1 = Convert.ToString(dr["TBanner1"]),
                    TUrl1 = Convert.ToString(dr["TUrl2"]),
                    TBanner2 = Convert.ToString(dr["TBanner2"]),
                    TUrl2 = Convert.ToString(dr["TUrl2"]),
                    TBanner3 = Convert.ToString(dr["TBanner3"]),
                    TUrl3 = Convert.ToString(dr["TUrl3"]),
                    Status = Convert.ToBoolean(dr["Status"]),

                });
            }
            return Json(Banner);
        }
        [HttpPost]
        public JsonResult ShowFooterBanner()
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<MStCMSBanner_srv> Banner = new List<MStCMSBanner_srv>();
            Statement = "FOOTERBANNER";
            string Footersqlquery = "exec SP_tblCMSBanner '" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(Footersqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Banner.Add(new MStCMSBanner_srv
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    FBanner2 = Convert.ToString(dr["FBanner2"]),
                    FUrl2 = Convert.ToString(dr["FUrl2"]),
                    Status = Convert.ToBoolean(dr["Status"]),

                });
            }
            return Json(Banner);
        }
        [HttpPost]
        public JsonResult ShowMainBanner()
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<MStCMSBanner_srv> Banner = new List<MStCMSBanner_srv>();
            Statement = "MAINBANNER";
            string Mainsqlquery = "exec SP_tblCMSBanner '" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(Mainsqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Banner.Add(new MStCMSBanner_srv
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    HBanner = Convert.ToString(dr["HBanner"]),
                    MUrl = Convert.ToString(dr["MUrl"]),
                    Status = Convert.ToBoolean(dr["Status"]),
                });
            }
            return Json(Banner);
        }
        [HttpPost]
        public JsonResult ShowCategoryBanner()
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<MStCMSBanner_srv> Banner = new List<MStCMSBanner_srv>();
            Statement = "CATEGORYBANNER";
            string Categorysqlquery = "exec SP_tblCMSBanner '" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(Categorysqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Banner.Add(new MStCMSBanner_srv
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    category_banner = Convert.ToString(dr["category_banner"]),
                    category_bannerUrl = Convert.ToString(dr["category_bannerUrl"]),
                    Status = Convert.ToBoolean(dr["Status"]),
                });
            }
            return Json(Banner);
        }
        [HttpPost]
        public JsonResult EditBanner(int id)
        {
            Statement = "EDIT";
            MStCMSBanner_srv Banner = new MStCMSBanner_srv();
            sqlquery = "exec SP_tblCMSBanner '" + Statement + "', " + id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Banner.ID = Convert.ToInt32(dr["ID"]);
                Banner.TBanner1 = Convert.ToString(dr["TBanner1"]);
                Banner.TUrl1 = Convert.ToString(dr["TUrl1"]);
                Banner.TBanner2 = Convert.ToString(dr["TBanner2"]);
                Banner.TUrl2 = Convert.ToString(dr["TUrl2"]);
                Banner.TBanner3 = Convert.ToString(dr["TBanner3"]);
                Banner.TUrl3 = Convert.ToString(dr["TUrl3"]);
                Banner.FBanner2 = Convert.ToString(dr["FBanner2"]);
                Banner.FUrl2 = Convert.ToString(dr["FUrl2"]);
                Banner.HBanner = Convert.ToString(dr["HBanner"]);
                Banner.MUrl = Convert.ToString(dr["MUrl"]);
                Banner.category_banner = Convert.ToString(dr["category_banner"]);
                Banner.category_bannerUrl = Convert.ToString(dr["category_bannerUrl"]);
                Banner.Status = Convert.ToBoolean(dr["Status"]);
            }
            return Json(Banner);
        }
        #endregion

        #region Courier
        public IActionResult Courier()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        [HttpPost]
        public IActionResult SaveUpdateCourier()
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            string ID = Request.Form["ID"].ToString();
            string Name = Request.Form["Name"].ToString();
            string Contact_Person = Request.Form["Contact_Person"].ToString();
            string Email = Request.Form["Email"].ToString();
            string Address = Request.Form["Address"].ToString();
            string MobileNumber = Request.Form["MobileNumber"].ToString();
            string Status = Request.Form["Status"].ToString();
            if (ID == "0")
            {
                Statement = "INSERT";
                sqlquery = "exec Sp_IU_Courier '" + Statement + "'," + CompanyId + "," + ID + ",'" + Name + "','" + Contact_Person + "','" + Email + "','" + Address + "'," + Status + "," + userid + ",'" + MobileNumber + "'";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Courier added";
                }
                else
                {
                    message = "Courier not added.";
                }
            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec Sp_IU_Courier '" + Statement + "'," + CompanyId + "," + ID + ",'" + Name + "','" + Contact_Person + "','" + Email + "','" + Address + "'," + Status + "," + userid + ",'" + MobileNumber + "'";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Courier added";
                }
                else
                {
                    message = "Courier not added.";
                }
            }
            var Data = new { message = message, id = ID };
            return Json(Data);
        }
        [HttpPost]
        public JsonResult ShowCourier()
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Courier> courier = new List<Courier>();
            Statement = "SELECT";
            sqlquery = "exec Sp_IU_Courier '" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                courier.Add(new Courier
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    Name = Convert.ToString(dr["Name"]),
                    Contact_Person = Convert.ToString(dr["Contact_Person"]),
                    Email = Convert.ToString(dr["Email"]),
                    Address = Convert.ToString(dr["Address"]),
                    MobileNumber = Convert.ToString(dr["MobileNumber"]),
                    Status = Convert.ToBoolean(dr["Status"]),
                });
            }
            return Json(courier);
        }
        [HttpPost]
        public JsonResult EditCourier(int id)
        {
            Statement = "EDIT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Courier courier = new Courier();
            sqlquery = "exec Sp_IU_Courier '" + Statement + "'," + CompanyId + "," + id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                courier.ID = Convert.ToInt32(dr["ID"]);
                courier.Name = Convert.ToString(dr["Name"]);
                courier.Contact_Person = Convert.ToString(dr["Contact_Person"]);
                courier.Email = Convert.ToString(dr["Email"]);
                courier.Address = Convert.ToString(dr["Address"]);
                courier.MobileNumber = Convert.ToString(dr["MobileNumber"]);
                courier.Status = Convert.ToBoolean(dr["Status"]);
            }
            return Json(courier);
        }
        [HttpPost]
        public JsonResult DeleteCourier(int id)
        {
            Statement = "DELETE";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "exec Sp_IU_Courier '" + Statement + "','" + CompanyId + "'," + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }
        #endregion

        #region HSNMaster
        public IActionResult HSNMaster()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        [HttpPost]
        public IActionResult SaveUpdateHSNMaster()
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            string ID = Request.Form["ID"].ToString();
            string HSNName = Request.Form["HSNName"].ToString();
            string HSNCode = Request.Form["HSNCode"].ToString();
            string Status = Request.Form["Status"].ToString();
            if (ID == "0")
            {
                Statement = "INSERT";
                sqlquery = "exec sp_IU_HSNMaster '" + Statement + "'," + CompanyId + "," + ID + ",'" + HSNName + "','" + HSNCode + "'," + userid + "," + Status + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Hsn added";
                }
                else
                {
                    message = "Hsn not added.";
                }
            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec sp_IU_HSNMaster '" + Statement + "'," + CompanyId + "," + ID + ",'" + HSNName + "','" + HSNCode + "'," + userid + "," + Status + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Hsn added";
                }
                else
                {
                    message = "Hsn not added.";
                }
            }

            var Data = new { message = message, id = ID };
            return Json(Data);
        }
        [HttpPost]
        public JsonResult ShowHSNMaster()
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<HSNMaster> hsnmaster = new List<HSNMaster>();
            Statement = "SELECT";
            sqlquery = "exec sp_IU_HSNMaster '" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                hsnmaster.Add(new HSNMaster
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    HSNName = Convert.ToString(dr["HSNName"]),
                    HSNCode = Convert.ToString(dr["HSNCode"]),
                    Status = Convert.ToBoolean(dr["Status"]),
                });
            }
            return Json(hsnmaster);
        }
        [HttpPost]
        public JsonResult EditHSNMaster(int id)
        {
            Statement = "EDIT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            HSNMaster hsnmaster = new HSNMaster();
            sqlquery = "exec sp_IU_HSNMaster '" + Statement + "','" + CompanyId + "'," + id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                hsnmaster.ID = Convert.ToInt32(dr["ID"]);
                hsnmaster.HSNName = Convert.ToString(dr["HSNName"]);
                hsnmaster.HSNCode = Convert.ToString(dr["HSNCode"]);
                hsnmaster.Status = Convert.ToBoolean(dr["Status"]);
            }
            return Json(hsnmaster);
        }
        [HttpPost]
        public JsonResult DeleteHSNMaster(int id)
        {
            Statement = "DELETE";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "exec sp_IU_HSNMaster '" + Statement + "','" + CompanyId + "'," + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }
        #endregion

        #region unitmaster
        public IActionResult unitmaster()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        public IActionResult SaveUpdateunitmaster()
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            string unit_id = Request.Form["unit_id"].ToString();
            string unit_name = Request.Form["unit_name"].ToString();
            string Status = Request.Form["Status"].ToString();
            if (unit_id == "0")
            {
                Statement = "INSERT";
                sqlquery = "exec SP_unitmaster '" + Statement + "'," + CompanyId + "," + unit_id + ",'" + unit_name + "'," + userid + "," + Status + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Unit name added";
                }
                else
                {
                    message = "Unit name not added.";
                }
            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec SP_unitmaster '" + Statement + "'," + CompanyId + "," + unit_id + ",'" + unit_name + "'," + userid + "," + Status + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Unit name added";
                }
                else
                {
                    message = "Unit name not added.";
                }
            }

            var Data = new { message = message, id = unit_id };
            return Json(Data);
        }
        [HttpPost]
        public JsonResult Showunitmaster()
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<unitmaster> unit = new List<unitmaster>();
            Statement = "SELECT";
            sqlquery = "exec SP_unitmaster '" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                unit.Add(new unitmaster
                {
                    unit_id = Convert.ToInt32(dr["unit_id"]),
                    unit_name = Convert.ToString(dr["unit_name"]),
                    unit_status = Convert.ToBoolean(dr["unit_status"]),
                });
            }
            return Json(unit);
        }
        [HttpPost]
        public JsonResult Editunitmaster(int unit_id)
        {
            Statement = "EDIT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            unitmaster unit = new unitmaster();
            sqlquery = "exec SP_unitmaster '" + Statement + "'," + CompanyId + "," + unit_id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                unit.unit_id = Convert.ToInt32(dr["unit_id"]);
                unit.unit_name = Convert.ToString(dr["unit_name"]);
                unit.unit_status = Convert.ToBoolean(dr["unit_status"]);
            }
            return Json(unit);
        }
        [HttpPost]
        public JsonResult Deleteunitmaster(int unit_id)
        {
            Statement = "DELETE";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "exec SP_unitmaster '" + Statement + "'," + CompanyId + "," + unit_id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }
        #endregion

        #region Welfare
        public IActionResult Welfare()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        [HttpPost]
        public IActionResult SaveWelfare()
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            string id = Request.Form["id"].ToString();
            string Welfare_Name = Request.Form["Welfare_Name"].ToString();
            string contactNo = Request.Form["contactNo"].ToString();
            string Email = Request.Form["Email"].ToString();
            string content = Request.Form["content"].ToString();
            string Status = Request.Form["Status"].ToString();
            string flg = Request.Form["flg"].ToString();
            string nefilname = Request.Form["filenmes"].ToString();
            string welmaster_id = Request.Form["welmaster_id"].ToString();
            string webRootPath = "";
            string filename = "";
            string folderName = "wwwroot/Images/Welfare/" + CompanyId + "/";
            if (flg == "okg")
            {
                nefilname = "";
                IFormFile file = Request.Form.Files[0];
                string extension = Path.GetExtension(file.FileName);
                filename = Path.GetFileNameWithoutExtension(file.FileName);
                //nefilname = filename;
                webRootPath = filename + DateTime.Now.ToString("yymmssfff") + extension;
                nefilname = webRootPath;
                string newPath = Path.Combine(folderName, webRootPath);
                using (var fileStream = new FileStream(newPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            if (id == "0")
            {
                Statement = "INSERT";
                sqlquery = "exec SP_tblWelfare '" + Statement + "'," + CompanyId + "," + id + ",'" + Welfare_Name + "','" + webRootPath + "','" + contactNo + "','" + Email + "','" + content + "'," + userid + "," + Status + "," + welmaster_id + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Welfare added";
                }
                else
                {
                    message = "Welfare not added.";
                }
            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec SP_tblWelfare '" + Statement + "'," + CompanyId + "," + id + ",'" + Welfare_Name + "','" + nefilname + "','" + contactNo + "','" + Email + "','" + content + "'," + userid + "," + Status + "," + welmaster_id + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Welfare added";
                }
                else
                {
                    message = "Welfare not added.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }

        //public IActionResult SaveWelfare()
        //{
        //    string userid = HttpContext.Session.GetString("AdminId");
        //    string CompanyId = HttpContext.Session.GetString("Company_Id");
        //    string id = Request.Form["id"].ToString();
        //    string Welfare_Name = Request.Form["Welfare_Name"].ToString();
        //    string contactNo = Request.Form["contactNo"].ToString();
        //    string Email = Request.Form["Email"].ToString();
        //    string content = Request.Form["content"].ToString();
        //    string Status = Request.Form["Status"].ToString();
        //    string flg = Request.Form["flg"].ToString();
        //    string nefilname = Request.Form["filenmes"].ToString();
        //    string webRootPath = "";
        //    string filename = "";

        //    string folderName = "wwwroot/Images/Welfare/"+ CompanyId + "/";

        //    if (flg == "okg")
        //    {
        //        nefilname = "";
        //        IFormFile file = Request.Form.Files[0];
        //        string extension = Path.GetExtension(file.FileName);
        //        filename = Path.GetFileNameWithoutExtension(file.FileName);
        //        //nefilname = filename;
        //        webRootPath = filename + DateTime.Now.ToString("yymmssfff") + extension;
        //        nefilname = webRootPath;
        //        string newPath = Path.Combine(folderName, webRootPath);
        //        using (var fileStream = new FileStream(newPath, FileMode.Create))
        //        {
        //            file.CopyTo(fileStream);
        //        }
        //    }

        //    if (id == "0")
        //    {
        //        Statement = "INSERT";
        //        sqlquery = "exec SP_tblWelfare '" + Statement + "'," + CompanyId + "," + id + ",'" + Welfare_Name + "','" + webRootPath + "','" + contactNo + "','" + Email + "','" + content + "'," + userid + "," + Status + "";
        //        status = Utility.MultipleTransactions(sqlquery);
        //        if (status == "Successfull")
        //        {
        //            message = "Welfare added";
        //        }
        //        else
        //        {
        //            message = "Welfare not added.";
        //        }
        //    }
        //    else
        //    {
        //        Statement = "UPDATE";
        //        sqlquery = "exec SP_tblWelfare '" + Statement + "'," + CompanyId + "," + id + ",'" + Welfare_Name + "','" + nefilname + "','" + contactNo + "','" + Email + "','" + content + "'," + userid + "," + Status + "";
        //        status = Utility.MultipleTransactions(sqlquery);
        //        if (status == "Successfull")
        //        {
        //            message = "Welfare added";
        //        }
        //        else
        //        {
        //            message = "Welfare not added.";
        //        }
        //    }
        //    var Data = new { message = message, id = id };
        //    return Json(Data);
        //}

        [HttpPost]
        public JsonResult ShowWelfare()
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Welfare> fare = new List<Welfare>();
            Statement = "SELECT";
            sqlquery = "exec SP_tblWelfare '" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                fare.Add(new Welfare
                {
                    id = Convert.ToInt32(dr["id"]),
                    welfaremaster_name = Convert.ToString(dr["welfaremaster_name"]),
                    Welfare_Name = Convert.ToString(dr["Welfare_Name"]),
                    img = Convert.ToString(dr["img"]),
                    contactNo = Convert.ToString(dr["contactNo"]),
                    Email = Convert.ToString(dr["Email"]),
                    content = Convert.ToString(dr["content"]),
                    welfarestatus = Convert.ToBoolean(dr["welfarestatus"]),

                });
            }
            return Json(fare);
        }
        [HttpPost]
        public JsonResult EditWelfare(int id)
        {
            Statement = "EDIT";
            Welfare fare = new Welfare();
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "exec SP_tblWelfare '" + Statement + "'," + CompanyId + ", " + id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                fare.id = Convert.ToInt32(dr["id"]);
                fare.Welfare_Name = Convert.ToString(dr["Welfare_Name"]);
                fare.img = Convert.ToString(dr["img"]);
                fare.contactNo = Convert.ToString(dr["contactNo"]);
                fare.Email = Convert.ToString(dr["Email"]);
                fare.content = Convert.ToString(dr["content"]);
                fare.welfarestatus = Convert.ToBoolean(dr["welfarestatus"]);
                fare.welmaster_id = Convert.ToInt32(dr["master_id"]);
            }
            return Json(fare);
        }
        [HttpPost]
        public JsonResult DeleteWelfare(int id)
        {
            Statement = "DELETE";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "exec SP_tblWelfare '" + Statement + "'," + CompanyId + "," + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }
        public IActionResult welfaremaster()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        [HttpPost]
        public JsonResult IU_welfaremaster()
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            string id = Request.Form["id"].ToString();
            string Welfare_Name = Request.Form["Welfare_Name"].ToString();
            string contactNo = Request.Form["contactNo"].ToString();
            string Email = Request.Form["Email"].ToString();
            string content = Request.Form["content"].ToString();
            string Status = Request.Form["Status"].ToString();
            string flg = Request.Form["flg"].ToString();
            string nefilname = Request.Form["filenmes"].ToString();
            string webRootPath = "";
            string filename = "";
            string folderName = "wwwroot/Images/Welfare/" + CompanyId + "/";

            if (flg == "okg")
            {
                nefilname = "";
                IFormFile file = Request.Form.Files[0];
                string extension = Path.GetExtension(file.FileName);
                filename = Path.GetFileNameWithoutExtension(file.FileName);
                //nefilname = filename;
                webRootPath = filename + DateTime.Now.ToString("yymmssfff") + extension;
                nefilname = webRootPath;
                string newPath = Path.Combine(folderName, webRootPath);
                using (var fileStream = new FileStream(newPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            if (id == "0")
            {
                Statement = "INSERT";
                sqlquery = "exec SP_tblwelfare_master '" + Statement + "'," + CompanyId + "," + id + ",'" + Welfare_Name + "','" + webRootPath + "','" + contactNo + "','" + Email + "','" + content + "'," + userid + "," + Status + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Welfare added";
                }
                else
                {
                    message = "Welfare not added.";
                }
            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec SP_tblwelfare_master '" + Statement + "'," + CompanyId + "," + id + ",'" + Welfare_Name + "','" + nefilname + "','" + contactNo + "','" + Email + "','" + content + "'," + userid + "," + Status + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Welfare added";
                }
                else
                {
                    message = "Welfare not added.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult ShowWelfaremster()
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Welfare> fare = new List<Welfare>();
            Statement = "SELECT";
            sqlquery = "exec SP_tblwelfare_master '" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                fare.Add(new Welfare
                {
                    id = Convert.ToInt32(dr["id"]),
                    Welfare_Name = Convert.ToString(dr["Welfare_Name"]),
                    img = Convert.ToString(dr["img"]),
                    contactNo = Convert.ToString(dr["contactNo"]),
                    Email = Convert.ToString(dr["Email"]),
                    content = Convert.ToString(dr["content"]),
                    welfarestatus = Convert.ToBoolean(dr["welfarestatus"]),

                });
            }
            return Json(fare);
        }
        [HttpPost]
        public JsonResult EditWelfaremaster(int id)
        {
            Statement = "EDIT";
            Welfare fare = new Welfare();
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "exec SP_tblwelfare_master '" + Statement + "'," + CompanyId + ", " + id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                fare.id = Convert.ToInt32(dr["id"]);
                fare.Welfare_Name = Convert.ToString(dr["Welfare_Name"]);
                fare.img = Convert.ToString(dr["img"]);
                fare.contactNo = Convert.ToString(dr["contactNo"]);
                fare.Email = Convert.ToString(dr["Email"]);
                fare.content = Convert.ToString(dr["content"]);
                fare.welfarestatus = Convert.ToBoolean(dr["welfarestatus"]);
            }
            return Json(fare);
        }
        [HttpPost]
        public JsonResult DeleteWelfaremaster(int id)
        {
            Statement = "DELETE";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "exec SP_tblwelfare_master '" + Statement + "'," + CompanyId + "," + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }
        [HttpGet]
        public IActionResult PopulateWelfare()
        {
            sqlquery = "select isnull(id,0)as id,isnull(Welfare_Name,'')as Welfare_Name from tblwelfare_master where welfarestatus=1";
            ds = Utility.BindDropDown(sqlquery);
            List<SelectListItem> welfaremaster = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                welfaremaster.Add(new SelectListItem { Text = dr["Welfare_Name"].ToString(), Value = dr["id"].ToString() });
            }
            ViewBag.welfaremaster = welfaremaster;
            return View(welfaremaster);
        }

        #endregion

        #region About
        public IActionResult About()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();

        }
        [HttpPost]
        public IActionResult InsertUpdateAbout(string About, int id)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            if (id == 0)
            {
                Statement = "INSERT";
                sqlquery = "exec Sp_tblAbout '" + Statement + "'," + id + ",'" + About + "','" + CompanyId + "'," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "About added.";
                }
                else
                {
                    message = "About not added.";
                }
            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec Sp_tblAbout '" + Statement + "'," + id + ",'" + About + "'," + CompanyId + "," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "About added.";
                }
                else
                {
                    message = "About not added.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }

        [HttpGet]
        public IActionResult ShowAbout()
        {
            string sqlquery;
            List<Models.About> About = new List<Models.About>();
            try
            {
                sqlquery = "exec Sp_tblAbout";
                DataSet ds = Utility.TableBind(sqlquery);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    About.Add(new Models.About
                    {
                        ID = Convert.ToInt32(dr["id"]),
                        about = Convert.ToString(dr["About"]),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Json(About);
        }

        [HttpPost]
        public JsonResult DeleteAbout(int id)
        {
            Statement = "DELETE";
            sqlquery = "exec Sp_tblAbout '" + Statement + "'," + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult EditAbout(int id)
        {
            Statement = "EDIT";
            Models.About About = new Models.About();
            sqlquery = "exec Sp_tblAbout '" + Statement + "', " + id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                About.ID = Convert.ToInt32(dr["id"]);
                About.about = Convert.ToString(dr["About"]);
            }
            return Json(About);
        }

        #endregion

        #region Home
        public IActionResult Home()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        #endregion

        #region Delivery Information
        public IActionResult DeliveryInformation()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        [HttpPost]
        public IActionResult IUDeliveryInfo(string Delivery_info, int id)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            if (id == 0)

            {
                Statement = "INSERT";
                sqlquery = "exec Sp_deliveryInformation '" + Statement + "'," + id + ",'" + Delivery_info + "','" + CompanyId + "'," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Delivery Information added.";
                }
                else
                {
                    message = "Delivery Information not added.";
                }
            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec Sp_deliveryInformation '" + Statement + "'," + id + ",'" + Delivery_info + "'," + CompanyId + "," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Delivery Information added.";
                }
                else
                {
                    message = "Delivery Information not added.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }

        [HttpGet]
        public IActionResult ShowdeliveryInformation()
        {
            string sqlquery;
            List<Models.DeliveryInformation> DeliveryInformation = new List<Models.DeliveryInformation>();
            try
            {

                sqlquery = "exec Sp_deliveryInformation";
                DataSet ds = Utility.TableBind(sqlquery);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DeliveryInformation.Add(new Models.DeliveryInformation
                    {
                        ID = Convert.ToInt32(dr["id"]),
                        Delivery_Info = Convert.ToString(dr["delivery_info"]),
                    });
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Json(DeliveryInformation);
        }
        [HttpPost]
        public JsonResult DeleteDeliveryInformation(int id)
        {
            Statement = "DELETE";
            sqlquery = "exec Sp_deliveryInformation '" + Statement + "'," + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult EditDeliveryInformation(int id)
        {
            Statement = "EDIT";
            Models.DeliveryInformation DeliveryInformation = new Models.DeliveryInformation();
            sqlquery = "exec Sp_deliveryInformation '" + Statement + "', " + id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DeliveryInformation.ID = Convert.ToInt32(dr["id"]);
                DeliveryInformation.Delivery_Info = Convert.ToString(dr["Delivery_info"]);


            }
            return Json(DeliveryInformation);
        }


        #endregion

        #region Payment
        public IActionResult Payment()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        [HttpPost]
        public IActionResult IUPayment(string PaymentContent, int id)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            if (id == 0)

            {
                Statement = "INSERT";
                sqlquery = "exec Sp_Payments '" + Statement + "'," + id + ",'" + PaymentContent + "','" + CompanyId + "'," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Payments added.";
                }
                else
                {
                    message = "Payments not added.";
                }
            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec Sp_Payments '" + Statement + "'," + id + ",'" + PaymentContent + "'," + CompanyId + "," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Payments added.";
                }
                else
                {
                    message = "Payments not added.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }

        [HttpGet]
        public IActionResult ShowPayment()
        {
            string sqlquery;
            List<Payment> Payment = new List<Payment>();
            try
            {

                sqlquery = "exec Sp_Payments";
                DataSet ds = Utility.TableBind(sqlquery);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Payment.Add(new Payment
                    {
                        ID = Convert.ToInt32(dr["id"]),
                        payment = Convert.ToString(dr["PaymentContent"]),
                    });
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Json(Payment);
        }

        [HttpPost]
        public JsonResult EditPayment(int id)
        {
            Statement = "EDIT";
            Payment Payment = new Payment();
            sqlquery = "exec Sp_Payments '" + Statement + "', " + id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Payment.ID = Convert.ToInt32(dr["id"]);
                Payment.payment = Convert.ToString(dr["paymentcontent"]);


            }
            return Json(Payment);
        }

        [HttpPost]
        public JsonResult DeletePayment(int id)
        {
            Statement = "DELETE";
            sqlquery = "exec Sp_Payments '" + Statement + "'," + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }

        #endregion

        #region FAQ
        public IActionResult FAQ()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        [HttpPost]
        public IActionResult IUFAQ(string FAQ, int id)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            if (id == 0)
            {
                Statement = "INSERT";
                sqlquery = "exec Sp_FAQ '" + Statement + "'," + id + ",'" + FAQ + "','" + CompanyId + "'," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "FAQ added.";
                }
                else
                {
                    message = "FAQ not added.";
                }
            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec Sp_FAQ '" + Statement + "'," + id + ",'" + FAQ + "'," + CompanyId + "," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "FAQ added.";
                }
                else
                {
                    message = "FAQ not added.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }
        [HttpGet]
        public IActionResult ShowFAQ()
        {
            string sqlquery;
            List<Models.FAQ> FAQ = new List<Models.FAQ>();
            try
            {
                sqlquery = "exec Sp_FAQ";
                DataSet ds = Utility.TableBind(sqlquery);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    FAQ.Add(new Models.FAQ
                    {
                        ID = Convert.ToInt32(dr["id"]),
                        Faq = Convert.ToString(dr["faq"]),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Json(FAQ);
        }
        [HttpPost]
        public IActionResult DeleteFAQ(int id)
        {
            Statement = "DELETE";
            sqlquery = "exec Sp_FAQ '" + Statement + "'," + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);

        }

        [HttpPost]
        public JsonResult EditFAQ(int id)
        {
            Statement = "EDIT";
            Models.FAQ faq = new Models.FAQ();
            sqlquery = "exec Sp_FAQ '" + Statement + "', " + id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                faq.ID = Convert.ToInt32(dr["id"]);
                faq.Faq = Convert.ToString(dr["FAQ"]);
            }
            return Json(faq);
        }
        #endregion

        #region TERMS
        public IActionResult Terms()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        [HttpPost]
        public IActionResult IUTerms(string terms, int id)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            if (id == 0)

            {
                Statement = "INSERT";
                sqlquery = "exec Sp_Terms '" + Statement + "'," + id + ",'" + terms + "','" + CompanyId + "'," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Terms added.";
                }
                else
                {
                    message = "Terms not added.";
                }
            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec Sp_Terms '" + Statement + "'," + id + ",'" + terms + "'," + CompanyId + "," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Terms added.";
                }
                else
                {
                    message = "Terms not added.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }

        [HttpGet]
        public IActionResult ShowTerms()
        {
            string sqlquery;
            List<Terms> Terms = new List<Terms>();
            try
            {

                sqlquery = "exec Sp_Terms";
                DataSet ds = Utility.TableBind(sqlquery);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Terms.Add(new Terms
                    {
                        ID = Convert.ToInt32(dr["id"]),
                        terms = Convert.ToString(dr["terms"]),
                    });
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Json(Terms);
        }

        [HttpPost]
        public JsonResult DeleteTerms(int id)
        {
            Statement = "DELETE";
            sqlquery = "exec Sp_Terms '" + Statement + "'," + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult EditTerms(int id)
        {
            Statement = "EDIT";
            Terms terms = new Terms();
            sqlquery = "exec Sp_Terms '" + Statement + "', " + id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                terms.ID = Convert.ToInt32(dr["id"]);
                terms.terms = Convert.ToString(dr["terms"]);
            }
            return Json(terms);
        }
        #endregion

        #region PrivacyPolish
        public IActionResult PrivacyPolish()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        [HttpPost]
        public IActionResult IUPrivacyPolish(string privacypolish, int id)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            if (id == 0)

            {
                Statement = "INSERT";
                sqlquery = "exec Sp_tblprivacypolish '" + Statement + "','" + CompanyId + "'," + id + ",'" + privacypolish + "'," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "PrivacyPolish added.";
                }
                else
                {
                    message = "PrivacyPolish not added.";
                }
            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec Sp_tblprivacypolish '" + Statement + "','" + CompanyId + "'," + id + ",'" + privacypolish + "'," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "PrivacyPolish added.";
                }
                else
                {
                    message = "PrivacyPolish not added.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }

        [HttpGet]
        public IActionResult ShowPrivacyPolish()
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            string sqlquery;
            string statement = "SELECT";
            List<PrivacyPolish> PrivacyPolish = new List<PrivacyPolish>();
            try
            {
                sqlquery = "exec Sp_tblprivacypolish '" + statement + "', '" + CompanyId + "'";
                DataSet ds = Utility.TableBind(sqlquery);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    PrivacyPolish.Add(new PrivacyPolish
                    {
                        ID = Convert.ToInt32(dr["id"]),
                        Privacypolish = Convert.ToString(dr["privacypolish"]),
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Json(PrivacyPolish);
        }

        [HttpPost]
        public JsonResult DeletePrivacyPolish(int id)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            Statement = "DELETE";
            sqlquery = "exec Sp_tblprivacypolish '" + Statement + "','" + CompanyId + "'," + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult EditPrivacyPolish(int id)
        {
            Statement = "EDIT";
            PrivacyPolish privacyPolish = new PrivacyPolish();
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "exec Sp_tblprivacypolish '" + Statement + "','" + CompanyId + "'," + id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                privacyPolish.ID = Convert.ToInt32(dr["id"]);
                privacyPolish.Privacypolish = Convert.ToString(dr["privacypolish"]);
            }
            return Json(privacyPolish);
        }
        #endregion

        #region SHIPPING DESCRIPTION
        public IActionResult Shipping()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        [HttpPost]
        public IActionResult IUShipping(string shipping, int id)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            if (id == 0)

            {
                Statement = "INSERT";
                sqlquery = "exec Sp_shipping_return '" + Statement + "'," + id + ",'" + shipping + "','" + CompanyId + "'," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Shipping added.";
                }
                else
                {
                    message = "Shipping not added.";
                }
            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec Sp_shipping_return '" + Statement + "'," + id + ",'" + shipping + "'," + CompanyId + "," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Shipping added.";
                }
                else
                {
                    message = "Shipping not added.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }

        [HttpGet]
        public IActionResult ShowShipping()
        {
            string sqlquery;
            List<Shipping> Shipping = new List<Shipping>();
            try
            {
                sqlquery = "exec Sp_shipping_return";
                DataSet ds = Utility.TableBind(sqlquery);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Shipping.Add(new Shipping
                    {
                        ID = Convert.ToInt32(dr["id"]),
                        shipping = Convert.ToString(dr["shipping"]),
                    });
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Json(Shipping);
        }

        [HttpPost]
        public JsonResult DeleteShipping(int id)
        {
            Statement = "DELETE";
            sqlquery = "exec Sp_shipping_return '" + Statement + "'," + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult EditShippingDesc(int id)
        {
            Statement = "EDIT";
            Shipping shippings = new Shipping();
            //string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "exec Sp_shipping_return '" + Statement + "'," + id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                shippings.ID = Convert.ToInt32(dr["id"]);
                shippings.shipping = Convert.ToString(dr["shipping"]);
            }
            return Json(shippings);
        }
        #endregion

        #region Create User
        public IActionResult Createuser()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        [HttpPost]
        public IActionResult IUCreateuser(int ID, string Name, string Email, string MobileNo, string Password, int Type, int Status)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            if (ID == 0)
            {
                DataTable dt = Utility.SelectParticular("tblAdminLogin", "*", "Email='" + Email + "' or Mobile_no='" + MobileNo + "'");
                if (dt.Rows.Count > 0)
                {
                    //ViewBag.ErrorMsg = "Your mobile number or email id is already exit.";
                    message = "Your mobile number or email id is already exit.";
                }
                else
                {
                    Statement = "INSERT";
                    sqlquery = "exec Sp_Createuser '" + Statement + "','" + CompanyId + "'," + Type + "," + ID + ",'" + Name + "','" + Email + "','" + Password + "','" + MobileNo + "'," + Status + "," + userid + "";

                    status = Utility.MultipleTransactions(sqlquery);
                    if (status == "Successfull")
                    {
                        message = "User added.";
                    }
                    else
                    {
                        message = "User not added.";
                    }
                }
            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec Sp_Createuser '" + Statement + "','" + CompanyId + "'," + Type + "," + ID + ",'" + Name + "','" + Email + "','" + Password + "','" + MobileNo + "'," + Status + "," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "User modify successfully.";
                }
                else
                {
                    message = "User not modify successfully.";
                }
            }
            var Data = new { message = message, id = ID };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult ShowCreateuser()
        {
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Createuser> Createuser = new List<Createuser>();
            sqlquery = "exec Sp_Createuser '" + Statement + "', '" + CompanyId + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Createuser.Add(new Createuser
                {
                    CompanyID = Convert.ToInt32(dr["Company_Id"]),
                    Type = Convert.ToInt32(dr["type"]),
                    ID = Convert.ToInt32(dr["AdminId"]),
                    Name = Convert.ToString(dr["Name"]),
                    Email = Convert.ToString(dr["Email"]),
                    Password = Convert.ToString(dr["passwords"]),
                    MobileNo = Convert.ToString(dr["Mobile_no"]),
                    Status = Convert.ToInt32(dr["status"]),
                });
            }
            return Json(Createuser);
        }

        [HttpPost]
        public JsonResult EditCreateuser(int ID, int typeid)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Statement = "EDIT";
            Createuser Createuser = new Createuser();
            sqlquery = "exec Sp_Createuser '" + Statement + "','" + CompanyId + "'," + typeid + ",'" + ID + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Createuser.CompanyID = Convert.ToInt32(dr["Company_Id"]);
                Createuser.Type = Convert.ToInt32(dr["type"]);
                Createuser.ID = Convert.ToInt32(dr["AdminId"]);
                Createuser.Name = Convert.ToString(dr["Name"]);
                Createuser.Email = Convert.ToString(dr["Email"]);
                Createuser.Password = Convert.ToString(dr["passwords"]);
                Createuser.MobileNo = Convert.ToString(dr["Mobile_no"]);
                Createuser.Status = Convert.ToInt32(dr["status"]);

            }
            return Json(Createuser);
        }

        [HttpPost]
        public JsonResult DeleteCreateuser(int ID, int typeid)

        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Statement = "DELETE";
            sqlquery = "exec Sp_Createuser '" + Statement + "','" + CompanyId + "'," + typeid + ",'" + ID + "'";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }
        #endregion

        #region Orders
        public IActionResult Orders()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            //  UserNameBind();
            Customer();
            return View();
        }

        public IActionResult UserNameBind()
        {
            sqlquery = "select reg_id,first_name from tblregistration";
            ds = Utility.BindDropDown(sqlquery);
            List<SelectListItem> UserName = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                UserName.Add(new SelectListItem { Text = dr["first_name"].ToString(), Value = dr["reg_id"].ToString() });
            }
            ViewBag.ItemUserNane = UserName;
            return View(UserName);
        }

        //public IActionResult OrdersReport(int order_id )
        //{

        //    string CompanyId = HttpContext.Session.GetString("Company_Id");
        //    Statement = "VIEW";
        //    Orders Orders = new Orders();
        //    sqlquery = "exec Sp_Orders '" + Statement + "','" + CompanyId + "'," + order_id + "";
        //    DataSet ds = Utility.TableBind(sqlquery);
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {


        //       //Orders.order_id = Convert.ToInt32(dr["order_id"]);               
        //        Orders.UserName = Convert.ToString(dr["UserName"]);
        //        Orders.order_no = Convert.ToString(dr["order_no"]);
        //        Orders.payment_mode = Convert.ToString(dr["payment_mode"]);
        //        Orders.quantity = Convert.ToString(dr["quantity"]);
        //        Orders.itemName = Convert.ToString(dr["itemName"]);
        //        Orders.itemimage = Convert.ToString(dr["itemimage"]);
        //        Orders.unit_qty = Convert.ToString(dr["unit_qty"]);
        //        Orders.order_date = Convert.ToString(dr["order_date"]);
        //        Orders.amount = Convert.ToInt32(dr["amount"]);
        //        Orders.subtotal = Convert.ToInt32(dr["subtotal"]);
        //        Orders.unit_rate = Convert.ToInt32(dr["unit_rate"]);
        //        Orders.item_id = Convert.ToInt32(dr["item_id"]);

        //    }
        //    ViewBag.Orders = Orders;
        //    return View();

        //}

        [HttpGet]
        public JsonResult ShowOrders(string from_date, string to_date, int UserName)
        {
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Orders> Orders = new List<Orders>();
            sqlquery = "EXEC Sp_Orders '" + Statement + "', " + CompanyId + ", @from_date = '" + from_date + "', @to_date = '" + to_date + "', @user_id = '" + UserName + "'";

            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Orders.Add(new Orders
                {

                    order_id = Convert.ToInt32(dr["order_id"]),
                    UserName = Convert.ToString(dr["UserName"]),
                    Email = Convert.ToString(dr["Email"]),
                    Mobileno = Convert.ToString(dr["Mobileno"]),
                    order_no = Convert.ToString(dr["order_no"]),
                    amount = Convert.ToInt32(dr["amount"]),
                    payment_mode = Convert.ToString(dr["payment_mode"]),
                    itemName = Convert.ToString(dr["ItemName"]),
                    unit_qty = Convert.ToString(dr["UnitQty"]),

                });
            }
            return Json(Orders);
        }





        [HttpGet]
        public JsonResult OrdersReport(int order_id)
        {
            Statement = "VIEW";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Orders> Orders = new List<Orders>();
            sqlquery = "exec [Sp_Orders_new] '" + Statement + "'," + CompanyId + "," + order_id + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Orders.Add(new Orders
                {

                    // order_id = Convert.ToInt32(dr["order_id"]),
                    UserName = Convert.ToString(dr["UserName"]),
                    order_no = Convert.ToString(dr["order_no"]),
                    payment_mode = Convert.ToString(dr["payment_mode"]),
                    amount = Convert.ToInt32(dr["amount"]),
                    subtotal = Convert.ToInt32(dr["subtotal"]),
                    // MobileNo = Convert.ToString(dr["MobileNo"]),
                    order_date = Convert.ToString(dr["order_date"]),
                    itemName = Convert.ToString(dr["itemName"]),
                    quantity = Convert.ToString(dr["quantity"]),
                    unit_rate = Convert.ToInt32(dr["unit_rate"]),

                });
            }
            return Json(Orders);
        }

        [HttpGet]
        public JsonResult Dispatchview(int order_id)
        {
            Statement = "VIEWORDERDETAIL";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Orders> Orders = new List<Orders>();
            sqlquery = "exec [Sp_Orders_new] '" + Statement + "',@companyid=" + CompanyId + ",@order_id=" + order_id + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Orders.Add(new Orders
                {

                    // order_id = Convert.ToInt32(dr["order_id"]),
                   // UserName = Convert.ToString(dr["UserName"]),
                    order_no = Convert.ToString(dr["order_no"]),
                   // payment_mode = Convert.ToString(dr["payment_mode"]),
                    amount = Convert.ToInt32(dr["amount"]),
                   // subtotal = Convert.ToInt32(dr["subtotal"]),
                    // MobileNo = Convert.ToString(dr["MobileNo"]),
                  //  order_date = Convert.ToString(dr["order_date"]),
                    itemName = Convert.ToString(dr["itemName"]),
                    quantity = Convert.ToString(dr["quantity"]),
                    unit_rate = Convert.ToInt32(dr["unit_rate"]),
                    Courior = Convert.ToInt32(dr["Courior"]),

                });
            }
            return Json(Orders);
        }

        [HttpGet]
        public JsonResult SearchOrders(int user_id, string from_date, string to_date, int order_id)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Orders> Orders = new List<Orders>();
            Statement = "SERACH";
            sqlquery = "exec Sp_Orders '" + Statement + "','" + CompanyId + "'," + order_id + "," + user_id + ",'" + from_date + "','" + to_date + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Orders.Add(new Orders
                {
                    order_id = Convert.ToInt32(dr["order_id"]),
                    UserName = Convert.ToString(dr["UserName"]),
                    order_no = Convert.ToString(dr["order_no"]),
                    amount = Convert.ToInt16(dr["amount"]),
                    payment_mode = Convert.ToString(dr["payment_mode"]),

                });
            }
            return Json(Orders);



        }

        #endregion

        #region Cancelled Orders
        public IActionResult Cancelled_Orders()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            Customer();
            return View();
        }

        public IActionResult Cancelled_Orders_Report(int order_id)
        {

            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Statement = "VIEW";
            Cancelled_Orders Cancelled_Orders = new Cancelled_Orders();
            sqlquery = "exec Sp_Orders_cancelled '" + Statement + "','" + CompanyId + "'," + order_id + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {


                //ORT_Dispatch.order_id = Convert.ToInt32(dr["order_id"]);               
                Cancelled_Orders.UserName = Convert.ToString(dr["UserName"]);
                Cancelled_Orders.order_no = Convert.ToString(dr["order_no"]);
                Cancelled_Orders.Address = Convert.ToString(dr["Address"]);
                Cancelled_Orders.MobileNo = Convert.ToString(dr["MobileNo"]);
                Cancelled_Orders.cal_date = Convert.ToString(dr["cal_date"]);
                Cancelled_Orders.payment_mode = Convert.ToString(dr["payment_mode"]);
                Cancelled_Orders.quantity = Convert.ToString(dr["quantity"]);
                Cancelled_Orders.itemName = Convert.ToString(dr["itemName"]);
                Cancelled_Orders.itemimage = Convert.ToString(dr["itemimage"]);
                Cancelled_Orders.unit_qty = Convert.ToString(dr["unit_qty"]);
                Cancelled_Orders.order_date = Convert.ToString(dr["order_date"]);
                Cancelled_Orders.amount = Convert.ToInt32(dr["amount"]);
                Cancelled_Orders.subtotal = Convert.ToInt32(dr["subtotal"]);
                Cancelled_Orders.unit_rate = Convert.ToInt32(dr["unit_rate"]);
                Cancelled_Orders.item_id = Convert.ToInt32(dr["item_id"]);




            }
            // ViewBag.ORT_Dispatch = GetORT_DispatchReportLiast(order_id);
            return View("Cancelled_Orders_Report", Cancelled_Orders);



        }
        public IActionResult ApproveCancelOrderReport()
        {
            Customer();
            return View();
        }

        [HttpGet]
        public JsonResult ShowApproveCancelOrderReport(int user_id, string from_date, string to_date)
        {
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Cancelled_Orders> Cancelled_Orders = new List<Cancelled_Orders>();
            if (user_id == 0 && from_date == null && to_date == null)
            {
                sqlquery = "exec Sp_Approve_cancelled_order '" + Statement + "'," + CompanyId + "," + 0 + "," + user_id + ",'" + "" + "','" + "" + "'";
            }
            else if (from_date == null && to_date == null)
            {
                sqlquery = "exec Sp_Approve_cancelled_order '" + Statement + "'," + CompanyId + "," + 0 + "," + user_id + ",'" + "" + "','" + "" + "'";
            }
            else
            {
                sqlquery = "exec Sp_Approve_cancelled_order '" + Statement + "'," + CompanyId + "," + 0 + "," + user_id + ",'" + from_date + "','" + to_date + "'";
            }
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Cancelled_Orders.Add(new Cancelled_Orders
                {
                    //CompanyID = Convert.ToInt32(dr["companyid"]),
                    order_id = Convert.ToInt32(dr["order_id"]),
                    UserName = Convert.ToString(dr["userName"]),
                    order_no = Convert.ToString(dr["order_no"]),
                    amount = Convert.ToInt32(dr["amount"]),
                    order_date = Convert.ToString(dr["order_date"]),
                    cal_date = Convert.ToString(dr["cal_date"]),
                    Cancelled_User = Convert.ToInt32(dr["Cancelled_User"]),
                    cancel_type = Convert.ToString(dr["cancel_type"]),
                    payment_mode = Convert.ToString(dr["payment_mode"]),
                });
            }
            return Json(Cancelled_Orders);
        }
        public IActionResult ApproveReturnOrderReport()
        {
            Customer();
            return View();
        }
        [HttpGet]
        public JsonResult ShowReturnOrderReport(int user_id, string from_date, string to_date)
        {
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Returned_Orders> Returned_Orders = new List<Returned_Orders>();
            if (user_id == 0 && from_date == null && to_date == null)
            {
                sqlquery = "exec Sp_Returned_Orders_Report '" + Statement + "'," + CompanyId + "," + 0 + "," + user_id + ",'" + "" + "','" + "" + "'";
            }
            else if (from_date == null && to_date == null)
            {
                sqlquery = "exec Sp_Returned_Orders_Report '" + Statement + "'," + CompanyId + "," + 0 + "," + user_id + ",'" + "" + "','" + "" + "'";
            }
            else
            {
                sqlquery = "exec Sp_Returned_Orders_Report '" + Statement + "'," + CompanyId + "," + 0 + "," + user_id + ",'" + from_date + "','" + to_date + "'";
            }
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Returned_Orders.Add(new Returned_Orders
                {
                    //CompanyID = Convert.ToInt32(dr["companyid"]),
                    order_id = Convert.ToInt32(dr["order_id"]),
                    order_no = Convert.ToString(dr["order_no"]),
                    UserName = Convert.ToString(dr["UserName"]),
                    //User_id = Convert.ToInt32(dr["user_id"]),
                    amount = Convert.ToInt32(dr["amount"]),
                    order_date = Convert.ToString(dr["order_date"]),
                    Return_date = Convert.ToString(dr["Return_date"]),
                    Return_User = Convert.ToString(dr["ReturnUser"]),
                    Return_Type = Convert.ToString(dr["Return_Type"]),
                    payment_mode = Convert.ToString(dr["payment_mode"]),
                    Approvestatus = Convert.ToString(dr["Approvestatus"]),
                    //Resion = Convert.ToString(dr["Resion"]),
                });
            }
            return Json(Returned_Orders);
        }


        [HttpGet]
        public JsonResult ShowCancelled_Orders(string from_date, string to_date, int UserName)
        {
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Cancelled_Orders> Cancelled_Orders = new List<Cancelled_Orders>();
            sqlquery = "exec Sp_Orders_cancelled_new '" + Statement + "'," + CompanyId + ",@from_date='" + from_date + "', @to_date='" + to_date + "',@user_id=" + UserName + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Cancelled_Orders.Add(new Cancelled_Orders
                {
                    //CompanyID = Convert.ToInt32(dr["companyid"]),
                    order_id = Convert.ToInt32(dr["order_id"]),
                    UserName = Convert.ToString(dr["userName"]),
                    order_no = Convert.ToString(dr["order_no"]),
                    amount = Convert.ToInt32(dr["amount"]),
                    order_date = Convert.ToString(dr["order_date"]),
                    cal_date = Convert.ToString(dr["cal_date"]),
                    // Cancelled_User = Convert.ToInt32(dr["Cancelled_User"]),
                    cancel_type = Convert.ToString(dr["cancel_type"]),
                    payment_mode = Convert.ToString(dr["payment_mode"]),


                });
            }
            return Json(Cancelled_Orders);
        }

        [HttpGet]



        public JsonResult SearchCancelled_Orders(int user_id, string from_date, string to_date, int order_id)
        {
            Statement = "SEARCH";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Cancelled_Orders> Cancelled_Orders = new List<Cancelled_Orders>();
            sqlquery = "exec Sp_Orders_cancelled '" + Statement + "'," + CompanyId + "," + order_id + "," + user_id + ",'" + from_date + "','" + to_date + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Cancelled_Orders.Add(new Cancelled_Orders
                {
                    //CompanyID = Convert.ToInt32(dr["companyid"]),
                    order_id = Convert.ToInt32(dr["order_id"]),
                    UserName = Convert.ToString(dr["userName"]),
                    order_no = Convert.ToString(dr["order_no"]),
                    amount = Convert.ToInt32(dr["amount"]),
                    order_date = Convert.ToString(dr["order_date"]),
                    cal_date = Convert.ToString(dr["cal_date"]),
                    Cancelled_User = Convert.ToInt32(dr["Cancelled_User"]),
                    Resion = Convert.ToString(dr["Resion"]),
                    payment_mode = Convert.ToString(dr["payment_mode"]),


                });
            }
            return Json(Cancelled_Orders);
        }
        [HttpPost]
        public JsonResult UpdateCancel(int orderid, double amount, string Transactionnumber, string remarks, string paymentmodeTest)
        {
            sqlquery = "Update  tblorder set Cn_Transactionid='" + Transactionnumber + "',Cn_Remarks='" + remarks + "',Cn_Amount='" + amount + "',cn_paymentmode='" + paymentmodeTest + "' ,cancelpprove=1  where order_id=" + orderid + "";
            status = Utility.MultipleTransactions(sqlquery);
            


            if (status == "Successfull")
            {
           DataTable dt1 = Utility.execQuery("select c.*, a.ItemName from tblItemStore   a join tblorderdetail b on a.ID=b.item_id  join tblorder c on b.order_id=c.order_id where c.order_id ='" + orderid + "'");


                //StringBuilder sBody = new StringBuilder();
                //string MailStatus = "";
                //var DateTime = new DateTime();
                //DateTime = DateTime.Now;
                //sBody.Append("<!DOCTYPE html><html><body>");
                ////sBody.Append("<p>Dear Support Team," + Environment.NewLine);
                ////  sBody.Append("<p>Your Order Has Been Cancel " + Environment.NewLine);
                //sBody.Append("<p>Dear " + dt1.Rows[0]["first_name"] + dt1.Rows[0]["last_name"] + Environment.NewLine);
                //sBody.Append("<p>Your order " + dt1.Rows[0]["order_no"] + " has been successfully cancelled. If you have any questions or need further assistance, please contact our support team. " + Environment.NewLine);
                //sBody.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
                //sBody.Append("<p>" + Environment.NewLine);
                //sBody.Append("<p>Thank you for your continued trust in our services." + Environment.NewLine);
                //sBody.Append("<p>Best regards</p>");
                //sBody.Append("<p><b>,The BSD Infotech Online Store Team</b></p>");
                //sBody.Append("</body></html>");
                //MailStatus = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + dt1.Rows[0]["email"] + "", "", "", "Subject : Your Order " + dt1.Rows[0]["order_no"] + " Has Been Cancelled", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");



                // StringBuilder sBody1 = new StringBuilder();
                // string MailStatus1 = "";
                //// var DateTime = new DateTime();
                //// DateTime = DateTime.Now;
                // sBody1.Append("<!DOCTYPE html><html><body>");
                // sBody1.Append("<p>Dear Support Team," + Environment.NewLine);
                // sBody1.Append("<p> Cancel order " + Environment.NewLine);
                // sBody1.Append("<p>" + Environment.NewLine);
                // sBody1.Append("<p>Thank you for your continued trust in our services." + Environment.NewLine);
                // sBody1.Append("<p>Best Regards</p>");
                // sBody1.Append("<p><b>Support Team</b></p>");
                // sBody1.Append("</body></html>");
                // string email1 = "bsddemos@gmail.com";
                // MailStatus1 = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : Cancel Order", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");


                StringBuilder sBody = new StringBuilder();
                string MailStatus = "";
                var DateTime = new DateTime();
                DateTime = DateTime.Now;
                sBody.Append("<!DOCTYPE html><html><body>");
                sBody.Append("<p>Dear " + dt1.Rows[0]["first_name"] + " " + dt1.Rows[0]["last_name"] + "," + Environment.NewLine);
                sBody.Append("<p>" + Environment.NewLine);
                sBody.Append("<p>We regret to inform you that your order with the following details has been cancelled:" + Environment.NewLine);
                // sBody.Append("<h3>Order Details:</h3>");
                sBody.Append("<p>Order Number: " + dt1.Rows[0]["order_no"] + "</p>");
                sBody.Append("<p>Order Cancellation  Date:" + dt1.Rows[0]["cal_date"] + "</p>");
                sBody.Append("<p>Product Name: " + dt1.Rows[0]["ItemName"] + "</p>");
                // sBody.Append("<p>Tracking Number:  " + dt1.Rows[0]["cour_no"] + "</p>");
                //sBody.Append("<p>User Name: " + email + "" + Environment.NewLine);
                //sBody.Append("<p>Contact Info: " + Mobile + "" + Environment.NewLine);
                //sBody.Append("<p>Email: " + email + "" + Environment.NewLine);
                //sBody.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
                // sBody.Append("<p>Track your shipment here:" + Environment.NewLine);
                //  sBody.Append("<p>[Tracking Link]" + Environment.NewLine);
                sBody.Append("<p>If this cancellation was initiated by you, no further action is required. However, if this was unexpected, please contact us immediately at support@bsdinfotech.com or 9871092024. </p>");
                sBody.Append("<p>If you have already made any payment, please allow 4-5 business days for the refund (if applicable) to be processed.</p>");
                sBody.Append("<p>We apologize for any inconvenience caused and hope to serve you again in the future.</p>");
                sBody.Append("<p>Best Regards</p>");
                sBody.Append("<p><b>Team BSD</b></p>");
                sBody.Append("</body></html>");
                MailStatus = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + dt1.Rows[0]["email"] + "", "", "", "Subject : Order Cancellation Confirmation", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");



                message = "Cancel approve updated.";
            }

            var Data = new { message = message };
            return Json(Data);
        }

        #endregion

        public IActionResult Order_Delivery_Report()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            Customer();
            return View();
        }

        public JsonResult showOrder_delivery_Report()
        {
            Statement = "order_delivered_report";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<OR_to_Deliver> OR_to_Deliver = new List<OR_to_Deliver>();
            sqlquery = "exec Sp_Deliver_Orders '" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                OR_to_Deliver.Add(new OR_to_Deliver
                {

                    order_no = Convert.ToString(dr["order_no"]),
                    UserName = Convert.ToString(dr["UserName"]),
                    amount = Convert.ToInt32(dr["amount"]),
                    MobileNo = Convert.ToString(dr["mobile"]),
                    order_date = Convert.ToString(dr["order_date"]),
                    dispatch_date = Convert.ToString(dr["dispatch_date"]),
                    payment_mode = Convert.ToString(dr["payment_mode"]),


                });
            }
            return Json(OR_to_Deliver);
        }



        public IActionResult Confirm_order_page()
        {

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            Customer();
            return View();
        }

        //public IActionResult Vender_Page(int id, string vendar_name, string vendar_emailid, string vendar_mobile, string vendar_address, string company_name)
        //{
        //    string type = "INSERT";

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(Utility.CON))
        //        {
        //            conn.Open();
        //            using (SqlCommand sqcmd = new SqlCommand("sp_tbl_vender1", conn))
        //            {
        //                sqcmd.CommandType = CommandType.StoredProcedure;
        //                sqcmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar, 100)).Value = type;
        //                sqcmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
        //                sqcmd.Parameters.Add(new SqlParameter("@vendar_name", SqlDbType.VarChar, 100)).Value = vendar_name;
        //                sqcmd.Parameters.Add(new SqlParameter("@vendar_emailid", SqlDbType.VarChar, 200)).Value = vendar_emailid;
        //                sqcmd.Parameters.Add(new SqlParameter("@vendar_mobileno", SqlDbType.VarChar, 200)).Value = vendar_mobile;
        //                sqcmd.Parameters.Add(new SqlParameter("@vendar_address", SqlDbType.VarChar, 50)).Value = vendar_address;
        //                sqcmd.Parameters.Add(new SqlParameter("@company_name", SqlDbType.VarChar, 500)).Value = company_name;

        //                sqcmd.ExecuteNonQuery();
        //            }
        //        }

        //        return Ok(); // Return a 200 OK response if the operation is successful
        //    }
        //    catch (Exception ex)
        //    {

        //        return StatusCode(500, "Internal server error"); 
        //    }
        //}




        public IActionResult Vendor_Page()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            Customer();
            return View();
        }

        public IActionResult Stock_Report()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            Customer();
            return View();
        }




        public IActionResult Inquiry_Page()
        {

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            Customer();
            return View();
        }

        public IActionResult InquiryToConfirm()
        {

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
           // Customer();
            return View();
        }


        public JsonResult showCustomerInquiry(string companyid, int itemid)
        {
            DataTable dt = new DataTable();
            sqlquery = "select distinct A.id,A.EnquiryNo,A.Fullname,A.Email,A.Mobile, FORMAT(A.CreateDate, 'dd/MM/yyyy') Createdate,A.status , A.Itemid , A.Comments from tblEnquiry as A  where a.status=1 order by  a.id desc  "; //where company_id='" + companyid + "'";// and ts.id=" + itemid + "";
            //and td.id = " + itemdetid + "
            dt = Utility.execQuery(sqlquery);
            return Json(JsonConvert.SerializeObject(dt));

        }

        public JsonResult BindPopup(int id)
        {
            string sqlquery = "";
            DataTable dt1 = new DataTable();
            string query = "select A.id, A.EnquiryNo,A.Fullname,A.Email,A.Mobile,B.ItemName,D.Unit_Rate as Amount,A.createuser from tblEnquiry as A left join tblItemStore as B on A.Itemid = B.ID left outer join tblItemDetails as D on B.ID = D.item_id where A.id = " + id + "";
            ds = Utility.TableBind(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                ViewBag.customername = ds.Tables[0].Rows[0]["Fullname"];
                ViewBag.email = ds.Tables[0].Rows[0]["Email"];
                ViewBag.Mobile = ds.Tables[0].Rows[0]["Mobile"];
                sqlquery = "select B.ID,A.EnquiryNo, A.Fullname,A.Email,A.Mobile,B.ItemName,D.Unit_Rate as Amount,Unit_Qty ,A.createuser from tblEnquiry as A  join tblItemStore as B on A.Itemid = B.ID  join tblItemDetails as D on B.ID = D.item_id where A.createuser = '" + ds.Tables[0].Rows[0]["createuser"] + "'and A.id = " + id + "";
                DataSet ds1 = Utility.TableBind(sqlquery);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    dt1 = ds1.Tables[0];
                }
            }

            return Json(JsonConvert.SerializeObject(dt1));
        }



        public JsonResult BindPopup11(int id)
        {
            //string sqlquery = "";
            DataTable dt1 = new DataTable();
            string query = "SELECT ISNULL(o.order_id, '') AS order_id, ISNULL(o.user_id, '') AS user_id,ISNULL(o.order_no, '') AS order_no,ISNULL(o.first_name + ' ' + o.last_name, '') AS UserName,ISNULL(o.email, '') AS email,ISNULL(o.mobile, '') AS mobileno,ISNULL(o.order_no, '') AS order_no,ISNULL(d.rate, '0') AS rate,ISNULL(o.payment_mode, '') AS payment_mode, ISNULL(d.quantity, '') AS UnitQty,ISNULL(d.amount, 0) AS amount,ISNULL(d.Tax, 0) AS Tax,ISNULL(d.Courior, 0) AS Courior,ISNULL(d.item_id, '') AS ID, ISNULL(i.ItemName, '') AS ItemName, o.user_id as createuser FROM tblorder o INNER JOIN tblorderdetail d on o.order_id = d.order_id join tblItemStore i on d.item_id = i.ID WHERE o.order_id = " + id + "";
            ds = Utility.TableBind(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt1 = ds.Tables[0];
            }

            return Json(JsonConvert.SerializeObject(dt1));
        }


        public JsonResult bindtableid(int id)
        {
            //string sqlquery = "";
            DataTable dt1 = new DataTable();
            string query = "select B.ID, B.ItemName ,D.Unit_Rate as Amount,Unit_Qty from  tblItemStore as B left outer join tblItemDetails as D on B.ID = D.item_id where B.id = " + id + "";
            ds = Utility.TableBind(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt1 = ds.Tables[0];
            }

            return Json(JsonConvert.SerializeObject(dt1));
        }



        public JsonResult BindPopupAdd(int id)
        {
            DataTable dt1 = new DataTable();
            DataTable enquirydt = new DataTable();
            string query = "SELECT A.id, A.EnquiryNo, A.Fullname, A.Email, A.Mobile, A.createuser FROM tblEnquiry AS A WHERE A.id = " + id;
            ds = Utility.TableBind(query);

            if (ds.Tables[0].Rows.Count > 0)
            {
                dt1 = ds.Tables[0];

              
                string enquiryNoQuery = "select ID,ItemName from tblItemStore ";
                DataSet enquiryDs = Utility.TableBind(enquiryNoQuery);

                enquirydt = enquiryDs.Tables[0];



                //if (enquiryDs.Tables[0].Rows.Count > 0)
                //{
                //    ViewBag.ID = enquiryDs.Tables[0].Rows[0]["ID"].ToString();
                //    ViewBag.ItemName = enquiryDs.Tables[0].Rows[0]["ItemName"].ToString();
                //}
            }


            var Data = new { dt1 = JsonConvert.SerializeObject(dt1), enquirydt = JsonConvert.SerializeObject(enquirydt) };
            return Json(Data);
        }




        #region Returned Orders
        public IActionResult Returned_Orders()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            Customer();
            return View();
        }



        public IActionResult Returned_Orders_Report(int order_id)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Statement = "VIEW";
            Returned_Orders Returned_Orders = new Returned_Orders();
            sqlquery = "exec Sp_Returned_Orders '" + Statement + "','" + CompanyId + "'," + order_id + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //ORT_Dispatch.order_id = Convert.ToInt32(dr["order_id"]);               
                Returned_Orders.UserName = Convert.ToString(dr["UserName"]);
                Returned_Orders.order_no = Convert.ToString(dr["order_no"]);
                Returned_Orders.Address = Convert.ToString(dr["Address"]);
                Returned_Orders.MobileNo = Convert.ToString(dr["MobileNo"]);
                Returned_Orders.cal_date = Convert.ToString(dr["cal_date"]);
                Returned_Orders.payment_mode = Convert.ToString(dr["payment_mode"]);
                Returned_Orders.quantity = Convert.ToString(dr["quantity"]);
                Returned_Orders.itemName = Convert.ToString(dr["itemName"]);
                Returned_Orders.itemimage = Convert.ToString(dr["itemimage"]);
                Returned_Orders.unit_qty = Convert.ToString(dr["unit_qty"]);
                Returned_Orders.order_date = Convert.ToString(dr["order_date"]);
                Returned_Orders.amount = Convert.ToInt32(dr["amount"]);
                Returned_Orders.subtotal = Convert.ToInt32(dr["subtotal"]);
                Returned_Orders.unit_rate = Convert.ToInt32(dr["unit_rate"]);
                Returned_Orders.item_id = Convert.ToInt32(dr["item_id"]);
            }
            ViewBag.order = GetReturned_Orders_Report(order_id);
            return View("Returned_Orders_Report", Returned_Orders);
        }





        [HttpPost]

        public IActionResult UpdateReturned_Orders(string returnorderjson)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            //string order_id = Request.Form["order_id"].ToString();
            //string Status = Request.Form["chkStatus"].ToString();
            //            string Unitdetails = Request.Form["json"].ToString();
            DataTable dt = Utility.JsonStringToDataTable(returnorderjson);
            int row = dt.Rows.Count;
            int col = dt.Columns.Count;
            int i = 0;
            if (dt.Rows.Count > 0)
            {


                for (i = 0; i < row; i++)
                {

                    if (dt.Rows[i]["chkStatus"].ToString() == "1")
                    {

                        sqlquery += "Insert into tbluserwallet(User_id,amount,EntryDate,order_id,type,flag) Values(" + dt.Rows[i]["User_id"].ToString() + "," + dt.Rows[i]["amount"].ToString() + ",getdate()," + dt.Rows[i]["order_id"].ToString() + ",'Cr','4')";
                        sqlquery += "Update tblorder set refundapprove=1, where order_id= '" + dt.Rows[i]["order_id"].ToString() + "'";

                    }

                }
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Return approve.";

                }
                else
                {
                    message = "Return approve not Successfully. ";
                }
            }
            return Json(message);
        }


        public List<Returned_Orders> GetReturned_Orders_Report(int order_id)
        {
            List<Returned_Orders> Returned_Orders = new List<Returned_Orders>();
            string sql = "select isnull(o.amount,0)as amount , isnull(o.subtotal,0)as subtotal,isnull(tblod.quantity,'')as quantity,isnull(ts.itemName,'')as itemName, isnull(tblod.rate,0)as unit_rate from tblorder o join tblorderdetail tblod  on tblod.order_id = o.order_id  join tblitemstore ts on ts.id=tblod.item_id  join tblitemdetails td on td.item_id = ts.id where o.order_id= '" + order_id + "'";
            DataSet ds = Utility.TableBind(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Returned_Orders.Add(new Returned_Orders
                {
                    itemName = Convert.ToString(dr["itemName"]),
                    quantity = Convert.ToString(dr["quantity"]),
                    subtotal = Convert.ToInt32(dr["subtotal"]),
                    amount = Convert.ToInt32(dr["amount"]),
                });
            }
            return Returned_Orders;
        }


        [HttpGet]
        public JsonResult ShowReturned_Orders()
        {
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Returned_Orders> Returned_Orders = new List<Returned_Orders>();
            sqlquery = "exec Sp_Returned_Orders '" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Returned_Orders.Add(new Returned_Orders
                {
                    //CompanyID = Convert.ToInt32(dr["companyid"]),
                    order_id = Convert.ToInt32(dr["order_id"]),
                    order_no = Convert.ToString(dr["order_no"]),
                    UserName = Convert.ToString(dr["UserName"]),
                    //User_id = Convert.ToInt32(dr["user_id"]),
                    amount = Convert.ToInt32(dr["amount"]),
                    order_date = Convert.ToString(dr["order_date"]),
                    Return_date = Convert.ToString(dr["Return_date"]),
                    Return_User = Convert.ToString(dr["ReturnUser"]),
                    Return_Type = Convert.ToString(dr["Return_Type"]),
                    payment_mode = Convert.ToString(dr["payment_mode"]),
                    //Resion = Convert.ToString(dr["Resion"]),
                });
            }
            return Json(Returned_Orders);
        }



        [HttpGet]
        public JsonResult SearchReturned_Orders(int user_id, string from_date, string to_date, int order_id)
        {
            Statement = "SEARCH";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Returned_Orders> Returned_Orders = new List<Returned_Orders>();
            sqlquery = "exec Sp_Returned_Orders '" + Statement + "'," + CompanyId + "," + order_id + "," + user_id + ",'" + from_date + "','" + to_date + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Returned_Orders.Add(new Returned_Orders
                {
                    //CompanyID = Convert.ToInt32(dr["companyid"]),
                    order_id = Convert.ToInt32(dr["order_id"]),
                    order_no = Convert.ToString(dr["order_no"]),
                    UserName = Convert.ToString(dr["userName"]),
                    User_id = Convert.ToInt32(dr["user_id"]),
                    amount = Convert.ToInt32(dr["amount"]),
                    order_date = Convert.ToString(dr["order_date"]),
                    Return_date = Convert.ToString(dr["Return_date"]),
                    Return_User = Convert.ToString(dr["ReturnUser"]),
                    Return_Type = Convert.ToString(dr["Return_Type"]),
                    payment_mode = Convert.ToString(dr["payment_mode"]),
                    //Resion = Convert.ToString(dr["Resion"]),



                });
            }
            return Json(Returned_Orders);
        }
        [HttpPost]
        public JsonResult UpdateReturn(int orderid, double amount, string Transactionnumber, string remarks, string paymentmodeTest)
        {
            sqlquery = "Update  tblorder set Rt_Transactionid='" + Transactionnumber + "',Rt_Remarks='" + remarks + "',Rt_Amount='" + amount + "',rt_paymentmode='" + paymentmodeTest + "', refundapprove=1 where order_id=" + orderid + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                
                    DataTable dt1 = Utility.execQuery("select c.*, a.ItemName from tblItemStore   a join tblorderdetail b on a.ID=b.item_id  join tblorder c on b.order_id=c.order_id where c.order_id ='" + orderid + "'");



                StringBuilder sBody = new StringBuilder();
                string MailStatus = "";
                var DateTime = new DateTime();
                DateTime = DateTime.Now;
                sBody.Append("<!DOCTYPE html><html><body>");
                sBody.Append("<p>Dear " + dt1.Rows[0]["first_name"] + " " + dt1.Rows[0]["last_name"] + "," + Environment.NewLine);
                sBody.Append("<p>" + Environment.NewLine);
                sBody.Append("<p>We have received your request to return the following order:" + Environment.NewLine);
                // sBody.Append("<h3>Order Details:</h3>");
                sBody.Append("<p>Order Number: " + dt1.Rows[0]["order_no"] + "</p>");
                sBody.Append("<p>Order Return Date:" + dt1.Rows[0]["return_date"] + "</p>");
                sBody.Append("<p>Product Name: " + dt1.Rows[0]["ItemName"] + "</p>");
                // sBody.Append("<p>Tracking Number:  " + dt1.Rows[0]["cour_no"] + "</p>");
                //sBody.Append("<p>User Name: " + email + "" + Environment.NewLine);
                //sBody.Append("<p>Contact Info: " + Mobile + "" + Environment.NewLine);
                //sBody.Append("<p>Email: " + email + "" + Environment.NewLine);
                //sBody.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
                // sBody.Append("<p>Track your shipment here:" + Environment.NewLine);
                //  sBody.Append("<p>[Tracking Link]" + Environment.NewLine);
                sBody.Append("<p>Your return has been successfully processed. If a refund or replacement is applicable, it will be initiated as per our return policy: </p>");
                sBody.Append("<p>Refund Processing Time: [3-5] business days (if applicable).</ p>");
                sBody.Append("<p>Replacement Details: Under Process.</ p>");

                sBody.Append("<p>If you have any further questions or concerns, please feel free to contact us at support@bsdinfotech.com or 9871092024.<p>");
                sBody.Append("<p>Thank you for choosing BSD Online Store.</p>");
                sBody.Append("<p>Best Regards</p>");
                sBody.Append("<p><b>Team BSD</b></p>");
                sBody.Append("</body></html>");
                MailStatus = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + dt1.Rows[0]["email"] + "", "", "", "Subject : Order Return Confirmation", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");


                //StringBuilder sBody1 = new StringBuilder();
                //    string MailStatus1 = "";
                //   // var DateTime = new DateTime();
                //  //  DateTime = DateTime.Now;
                //    sBody1.Append("<!DOCTYPE html><html><body>");
                //    sBody1.Append("<p>Dear Support Team," + Environment.NewLine);
                //    sBody1.Append("<p> Return order " + Environment.NewLine);
                //    sBody1.Append("<p> Customer Bank Details " + Environment.NewLine);
                //    sBody1.Append("<p>Bank name: " + dt1.Rows[0]["Bank_name"] + ", " + Environment.NewLine);
                //    sBody1.Append("<p>IFSC name: " + dt1.Rows[0]["IFSC_code"] + ", " + Environment.NewLine);
                //    sBody1.Append("<p>Account No: " + dt1.Rows[0]["Account_no"] + ", " + Environment.NewLine);
                //    sBody1.Append("<p>Account Holder name: " + dt1.Rows[0]["Account_holder_name"] + ", " + Environment.NewLine);
                //    sBody1.Append("<p>" + Environment.NewLine);
                //    sBody1.Append("<p>Thank you for your continued trust in our services." + Environment.NewLine);
                //    sBody1.Append("<p>Best Regards</p>");
                //    sBody1.Append("<p><b>Support Team</b></p>");
                //    sBody1.Append("</body></html>");
                //    string email1 = "bsddemos@gmail.com";
                //    MailStatus1 = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : Return Order", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");

                    
                

                message = "Return approve updated";
            }
            else
            {
                message = "Return approve not updated";
            }

            var Data = new { message = message };
            return Json(Data);
        }


        #endregion

        #region Orders Ready to Dispatch
        public IActionResult ORT_Dispatch(int order_id,int Type)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            ViewBag.Type = Type;
            BindCourier();
            return View();
        }

        [HttpPost]
        public IActionResult UpdateDispatchOrder(int order_id, string from_date, string to_date, int cour_id, int cour_no, string cour_remarks)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");


            Statement = "UPDATE";
            string sqlquery = "exec Sp_Dispatch_Orders '" + Statement + "'," + CompanyId + "," + order_id + "," + 0 + "," + 0 + "," + cour_id + "," + cour_no + ",'" + cour_remarks + "'";
            status = Utility.MultipleTransactions(sqlquery);


            if (status == "Successfull")
            {

                DataTable dt1 = Utility.execQuery("select a.*,b.Contact_Person from tblorder a join tblCourier b on a.cour_id=b.ID where a.order_id = '" + order_id + "'");




              


                StringBuilder sBody = new StringBuilder();
                string MailStatus = "";
                var DateTime = new DateTime();
                DateTime = DateTime.Now;
                sBody.Append("<!DOCTYPE html><html><body>");
                sBody.Append("<p>Dear " + dt1.Rows[0]["first_name"] + " " + dt1.Rows[0]["last_name"] + "," + Environment.NewLine);
                sBody.Append("<p>" + Environment.NewLine);
                sBody.Append("<p>We are pleased to inform you that your order has been dispatched and is on its way to you." + Environment.NewLine);
                sBody.Append("<h3>Order Details:</h3>");
                sBody.Append("<p>Order Number: " + dt1.Rows[0]["order_no"] + "</p>");
                sBody.Append("<p>Dispatch Date:" + dt1.Rows[0]["dispatch_date"] + "</p>");
                sBody.Append("<p>Courier Name: " + dt1.Rows[0]["Contact_Person"] + "</p>");
                sBody.Append("<p>Tracking Number:  " + dt1.Rows[0]["cour_no"] + "</p>");
                //sBody.Append("<p>User Name: " + email + "" + Environment.NewLine);
                //sBody.Append("<p>Contact Info: " + Mobile + "" + Environment.NewLine);
                //sBody.Append("<p>Email: " + email + "" + Environment.NewLine);
                //sBody.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
               // sBody.Append("<p>Track your shipment here:" + Environment.NewLine);
              //  sBody.Append("<p>[Tracking Link]" + Environment.NewLine);
                sBody.Append("<p>You can track the status of your shipment using the provided tracking number on the courier’s website. </p>");
                sBody.Append("<p>If you have any questions or need assistance with your order, please don’t hesitate to contact us at support@bsdinfotech.com or 9871092024.</p>");
                sBody.Append("<p>Thank you for shopping with BSD Online Store!</p>");
                sBody.Append("<p>Best Regards</p>");
                sBody.Append("<p><b>Team BSD</b></p>");
                sBody.Append("</body></html>");
                MailStatus = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + dt1.Rows[0]["email"] +"" , "", "", "Subject : Your Order " + dt1.Rows[0]["order_no"] + " Has Been Dispatched", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");


          

                StringBuilder sBody1 = new StringBuilder();
                string MailStatus1 = "";
               // var DateTime = new DateTime();
              //  DateTime = DateTime.Now;
                sBody1.Append("<!DOCTYPE html><html><body>");
                sBody1.Append("<p>Dear Team," + Environment.NewLine);
                sBody1.Append("<p> The following order has been successfully dispatched: " + Environment.NewLine);
                sBody1.Append("<p>Customer Name:" + dt1.Rows[0]["first_name"] + " " + dt1.Rows[0]["last_name"] + "," + Environment.NewLine);
                sBody1.Append("<p>Order Number: " + dt1.Rows[0]["order_no"] + "" + Environment.NewLine);
                sBody1.Append("<p>Dispatch Date:" + dt1.Rows[0]["dispatch_date"] + "" + Environment.NewLine);
                sBody1.Append("<p>Courier Name: " + dt1.Rows[0]["Contact_Person"] + "" + Environment.NewLine);
                sBody1.Append("<p>Tracking Number:  " + dt1.Rows[0]["cour_no"] + "" + Environment.NewLine);
                sBody1.Append("<p>" + Environment.NewLine);
                sBody1.Append("<p>Please ensure the customer receives the tracking details and any further updates on their shipment." + Environment.NewLine);
                sBody1.Append("<p>Best Regards</p>");
                sBody1.Append("<p><b>Team BSD</b></p>");
                sBody1.Append("</body></html>");
                string email1 = "bsddemos@gmail.com";
                MailStatus1 = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : Order  " + dt1.Rows[0]["order_no"] + " Dispatched", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");


                message = "Your Order Dispatch Successfully.";
            }

            else
            {
                message = "Table not modify successfully.";
            }

            var Data = new { message = message };
            return Json(Data);
        }

        [HttpGet]
        public JsonResult ShowORT_Dispatch(int Type)
        {
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<ORT_Dispatch> ORT_Dispatch = new List<ORT_Dispatch>();
            sqlquery = "exec Sp_Dispatch_Orders '" + Statement + "'," + CompanyId + ",@Type=" + Type + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ORT_Dispatch.Add(new ORT_Dispatch
                {
                    //CompanyID = Convert.ToInt32(dr["companyid"]),
                    order_id = Convert.ToInt32(dr["order_id"]),
                    order_no = Convert.ToString(dr["order_no"]),
                    UserName = Convert.ToString(dr["userName"]),
                    //amount = Convert.ToInt32(dr["amount"]),
                    Address = Convert.ToString(dr["Address"]),
                    MobileNo = Convert.ToString(dr["MobileNo"]),
                    order_date = Convert.ToString(dr["order_date"]),
                    dispatch_date = Convert.ToString(dr["dispatch_date"]),
                    payment_mode = Convert.ToString(dr["payment_mode"]),
                    dispatch_flg = Convert.ToString(dr["dispatch_flg"]),

                });
            }
            return Json(ORT_Dispatch);
        }




        //[HttpGet]
        //public JsonResult ShowORT_DispatchPending(int Type)
        //{
        //    Statement = "SELECT";
        //    string CompanyId = HttpContext.Session.GetString("Company_Id");
        //    List<ORT_Dispatch> ORT_Dispatch = new List<ORT_Dispatch>();
        //    sqlquery = "exec Sp_Dispatch_Orders '" + Statement + "'," + CompanyId + ",@Type=" + Type + "";
        //    DataSet ds = Utility.TableBind(sqlquery);
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        ORT_Dispatch.Add(new ORT_Dispatch
        //        {
        //            //CompanyID = Convert.ToInt32(dr["companyid"]),
        //            order_id = Convert.ToInt32(dr["order_id"]),
        //            order_no = Convert.ToString(dr["order_no"]),
        //            UserName = Convert.ToString(dr["userName"]),
        //            //amount = Convert.ToInt32(dr["amount"]),
        //            Address = Convert.ToString(dr["Address"]),
        //            MobileNo = Convert.ToString(dr["MobileNo"]),
        //            order_date = Convert.ToString(dr["order_date"]),
        //            dispatch_date = Convert.ToString(dr["dispatch_date"]),
        //            payment_mode = Convert.ToString(dr["payment_mode"]),
        //            dispatch_flg = Convert.ToString(dr["dispatch_flg"]),

        //        });
        //    }
        //    return Json(ORT_Dispatch);
        //}
        [HttpGet]
        public JsonResult SearchORT_Dispatch(int order_id)
        {
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<ORT_Dispatch> ORT_Dispatch = new List<ORT_Dispatch>();
            sqlquery = "exec Sp_Dispatch_Orders '" + Statement + "'," + order_id + "," + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ORT_Dispatch.Add(new ORT_Dispatch
                {
                    //CompanyID = Convert.ToInt32(dr["companyid"]),
                    order_id = Convert.ToInt32(dr["order_id"]),
                    order_no = Convert.ToString(dr["order_no"]),
                    UserName = Convert.ToString(dr["userName"]),
                    amount = Convert.ToInt32(dr["amount"]),
                    Address = Convert.ToString(dr["Address"]),
                    MobileNo = Convert.ToString(dr["MobileNo"]),
                    order_date = Convert.ToString(dr["order_date"]),
                    dispatch_date = Convert.ToString(dr["dispatch_date"]),
                    payment_mode = Convert.ToString(dr["payment_mode"]),
                    dispatch_flg = Convert.ToString(dr["dispatch_flg"]),

                });
            }
            return Json(ORT_Dispatch);
        }



        #endregion

        #region Orders Ready to Deliver
        public IActionResult OR_to_Deliver()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }

            return View();
        }
        public IActionResult OR_to_DeliverReport(int order_id)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Statement = "VIEW";
            OR_to_Deliver OR_to_Deliver = new OR_to_Deliver();
            sqlquery = "exec Sp_Deliver_Orders '" + Statement + "','" + CompanyId + "'," + order_id + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //ORT_Dispatch.order_id = Convert.ToInt32(dr["order_id"]);               
                OR_to_Deliver.UserName = Convert.ToString(dr["UserName"]);
                OR_to_Deliver.order_no = Convert.ToString(dr["order_no"]);
                OR_to_Deliver.Address = Convert.ToString(dr["Address"]);
                OR_to_Deliver.MobileNo = Convert.ToString(dr["MobileNo"]);
                OR_to_Deliver.dispatch_date = Convert.ToString(dr["dispatch_date"]);
                OR_to_Deliver.payment_mode = Convert.ToString(dr["payment_mode"]);
                OR_to_Deliver.quantity = Convert.ToString(dr["quantity"]);
                OR_to_Deliver.itemName = Convert.ToString(dr["itemName"]);
                OR_to_Deliver.itemimage = Convert.ToString(dr["itemimage"]);
                OR_to_Deliver.unit_qty = Convert.ToString(dr["unit_qty"]);
                OR_to_Deliver.order_date = Convert.ToString(dr["order_date"]);
                OR_to_Deliver.amount = Convert.ToInt32(dr["amount"]);
                OR_to_Deliver.subtotal = Convert.ToInt32(dr["subtotal"]);
                OR_to_Deliver.unit_rate = Convert.ToInt32(dr["unit_rate"]);
                OR_to_Deliver.item_id = Convert.ToInt32(dr["item_id"]);

            }
            ViewBag.order = GetOR_to_Deliver(order_id);
            return View("OR_to_DeliverReport", OR_to_Deliver);

            //return View();
        }

        [HttpPost]
        public IActionResult UpdateOR_to_Deliver(string Deliverdisplayjson)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            //string order_id = Request.Form["order_id"].ToString();
            string customerid = HttpContext.Session.GetString("customerid");
            //string Status = Request.Form["chkStatus"].ToString();
            //            string Unitdetails = Request.Form["json"].ToString();
            DataTable dt = Utility.JsonStringToDataTable(Deliverdisplayjson);


            int row = dt.Rows.Count;
            int col = dt.Columns.Count;
            int i = 0;





            if (dt.Rows.Count > 0)
            {


                for (i = 0; i < row; i++)
                {

                    if (dt.Rows[i]["chkStatus"].ToString() == "1")
                    {

                        //sqlquery += "Insert into tbluserwallet(User_id,amount,EntryDate,order_id,type,flag) Values(" + dt.Rows[i]["User_id"].ToString() + "," + dt.Rows[i]["amount"].ToString() + ",getdate()," + dt.Rows[i]["order_id"].ToString() + ",'Cr','4')";

                        // DataTable dty = Utility.execQuery("select * from tblorder where user_id ='" + HttpContext.Session.GetString("customerid") + "' and order_id = '" +  + "'");

                        sqlquery += "Update tblorder set status_flg=4,deliver_date=getdate() where order_id= '" + dt.Rows[i]["order_id"].ToString() + "'";

                        status = Utility.MultipleTransactions(sqlquery);
                        if (status == "Successfull")
                        {

                            DataTable dt1 = Utility.execQuery("select c.first_name,c.last_name, c.order_no,c.deliver_date, a.ItemName,d.Contact_Person,c.cour_no,c.email  from tblItemStore   a join tblorderdetail b on a.ID=b.item_id  join tblorder c on b.order_id=c.order_id  join tblCourier d on c.cour_id=d.ID where c.order_id = '" + dt.Rows[i]["order_id"].ToString() + "'");

                            StringBuilder sBody = new StringBuilder();
                            string MailStatus = "";
                            var DateTime = new DateTime();
                            DateTime = DateTime.Now;
                            sBody.Append("<!DOCTYPE html><html><body>");
                            sBody.Append("<p>Dear " + dt1.Rows[0]["first_name"] + " " + dt1.Rows[0]["last_name"] + "," + Environment.NewLine);
                            sBody.Append("<p>" + Environment.NewLine);
                            sBody.Append("<p>We are happy to inform you that your order has been successfully delivered!" + Environment.NewLine);
                            sBody.Append("<h3>Order Details:</h3>");
                            sBody.Append("<p>Order Number: " + dt1.Rows[0]["order_no"] + "" + Environment.NewLine);
                            sBody.Append("<p>Delivery Date:" + dt1.Rows[0]["deliver_date"] + "" + Environment.NewLine);
                            sBody.Append("<p>Items Delivered: " + dt1.Rows[0]["ItemName"] + "" + Environment.NewLine);	
                            sBody.Append("<h3>Courier Details: </h3>");

                            sBody.Append("<p>Courier Name: " + dt1.Rows[0]["Contact_Person"] + "" + Environment.NewLine);
                            sBody.Append("<p>Tracking Number:  " + dt1.Rows[0]["cour_no"] + "" + Environment.NewLine);
                            //sBody.Append("<p>User Name: " + email + "" + Environment.NewLine);
                            //sBody.Append("<p>Contact Info: " + Mobile + "" + Environment.NewLine);
                            //sBody.Append("<p>Email: " + email + "" + Environment.NewLine);
                            //sBody.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
                            sBody.Append("<p>We hope you are satisfied with your purchase. If you have any questions or need assistance with your order, feel free to contact us at support@bsdinfotech.com or 9871092024." + Environment.NewLine);
                            sBody.Append("<p>Thank you for shopping with BSD Online Store!" + Environment.NewLine);
                            sBody.Append("<p>Best Regards</p>");
                            sBody.Append("<p><b>Team BSD</b></p>");
                            sBody.Append("</body></html>");
                            MailStatus = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + dt1.Rows[0]["email"] + "", "", "", "Subject : Your Order " + dt1.Rows[0]["order_no"] + " Has Been Successfully Delivered", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");

                        //}
                        //if (status == "Successfull")
                        //{

                            StringBuilder sBody1 = new StringBuilder();
                            string MailStatus1 = "";
                            //var DateTime = new DateTime();
                            DateTime = DateTime.Now;
                            sBody1.Append("<!DOCTYPE html><html><body>");
                            sBody1.Append("<p>Dear Team," + Environment.NewLine);
                            sBody1.Append("<p> The following order has been successfully delivered: " + Environment.NewLine);
                            sBody1.Append("<h3>Order Details:</h3>");
                            sBody1.Append("<p>Order Number: " + dt1.Rows[0]["order_no"] + "" + Environment.NewLine);
                            sBody1.Append("<p>Delivery Date:" + dt1.Rows[0]["deliver_date"] + "" + Environment.NewLine);
                            sBody1.Append("<p>Items Delivered: " + dt1.Rows[0]["ItemName"] + "" + Environment.NewLine);
                            sBody1.Append("<h3>Courier Details: </h3>");

                            sBody1.Append("<p>Courier Name: " + dt1.Rows[0]["Contact_Person"] + "" + Environment.NewLine);
                            sBody1.Append("<p>Tracking Number:  " + dt1.Rows[0]["cour_no"] + "" + Environment.NewLine);
                            sBody1.Append("<p>Please update the order status in our system to reflect successful delivery. If the customer has any further questions or concerns, ensure prompt follow-up." + Environment.NewLine);
                            sBody1.Append("<p>Thank you for your attention to this order." + Environment.NewLine);
                            sBody1.Append("<p>Best Regards</p>");
                            sBody1.Append("<p><b>Support Team</b></p>");
                            sBody1.Append("</body></html>");
                            string email1 = "bsddemos@gmail.com";
                            MailStatus1 = Utility.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : Order " + dt1.Rows[0]["order_no"] + " - Delivery Confirmation ", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");

                            message = "Your Order Delivered Successfully.";
                        }

                    }

                    //else
                    //{
                    //    sqlquery += "Update tblorder set status_flg=4 where order_id= '" + dt.Rows[i]["order_id"].ToString() + "'";

                    //}

                }




            }
            return Json(message);
        }





        public List<OR_to_Deliver> GetOR_to_Deliver(int order_id)
        {
            List<OR_to_Deliver> OR_to_Deliver = new List<OR_to_Deliver>();
            string sql = "select isnull(o.amount,0)as amount , isnull(o.subtotal,0)as subtotal,isnull(tblod.quantity,'')as quantity,isnull(ts.itemName,'')as itemName, isnull(tblod.rate,0)as unit_rate from tblorder o join tblorderdetail tblod  on tblod.order_id = o.order_id  join tblitemstore ts on ts.id=tblod.item_id  join tblitemdetails td on td.item_id = ts.id where o.order_id= '" + order_id + "'";
            DataSet ds = Utility.TableBind(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                OR_to_Deliver.Add(new OR_to_Deliver
                {
                    itemName = Convert.ToString(dr["itemName"]),
                    quantity = Convert.ToString(dr["quantity"]),
                    subtotal = Convert.ToInt32(dr["subtotal"]),
                    amount = Convert.ToInt32(dr["amount"]),
                });
            }
            return OR_to_Deliver;
        }

        [HttpGet]
        public JsonResult ShowOR_to_Deliver()
        {
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<OR_to_Deliver> OR_to_Deliver = new List<OR_to_Deliver>();
            sqlquery = "exec Sp_Deliver_Orders '" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                OR_to_Deliver.Add(new OR_to_Deliver
                {
                    //CompanyID = Convert.ToInt32(dr["companyid"]),
                    order_id = Convert.ToInt32(dr["order_id"]),
                    order_no = Convert.ToString(dr["order_no"]),
                    UserName = Convert.ToString(dr["userName"]),
                    amount = Convert.ToInt32(dr["amount"]),
                    MobileNo = Convert.ToString(dr["MobileNo"]),
                    order_date = Convert.ToString(dr["order_date"]),
                    dispatch_date = Convert.ToString(dr["dispatch_date"]),
                    payment_mode = Convert.ToString(dr["payment_mode"]),


                });
            }
            return Json(OR_to_Deliver);
        }

        [HttpGet]
        public JsonResult SearchOR_to_Deliver(string from_date, string to_date, int order_id)
        {
            Statement = "SEARCH";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<OR_to_Deliver> OR_to_Deliver = new List<OR_to_Deliver>();
            sqlquery = "exec Sp_Deliver_Orders '" + Statement + "'," + CompanyId + "," + order_id + ",'" + from_date + "','" + to_date + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                OR_to_Deliver.Add(new OR_to_Deliver
                {
                    //CompanyID = Convert.ToInt32(dr["companyid"]),
                    order_id = Convert.ToInt32(dr["order_id"]),
                    order_no = Convert.ToString(dr["order_no"]),
                    UserName = Convert.ToString(dr["userName"]),
                    amount = Convert.ToInt32(dr["amount"]),
                    MobileNo = Convert.ToString(dr["MobileNo"]),
                    order_date = Convert.ToString(dr["order_date"]),
                    dispatch_date = Convert.ToString(dr["dispatch_date"]),
                    payment_mode = Convert.ToString(dr["payment_mode"]),


                });
            }
            return Json(OR_to_Deliver);
        }

        #endregion

        #region Country
        public IActionResult Country()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            // CountryBind();
            return View();
        }

        [HttpPost]
        public IActionResult IUCountry(int id, string Country, string searchitem)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            if (id == 0)
            {

                Statement = "INSERT";
                sqlquery = "exec Sp_Country '" + Statement + "'," + CompanyId + "," + id + ",'" + Country + "'," + userid + "";

                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Country added.";
                }
                else
                {
                    message = "Country not added.";
                }

            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec Sp_Country '" + Statement + "'," + CompanyId + "," + id + ",'" + Country + "'," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Country modify successfully.";
                }
                else
                {
                    message = "Country not modify successfully.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }
        [HttpGet]
        public JsonResult ShowCountry(string searchitem)
        {
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Country> Country = new List<Country>();
            sqlquery = "exec Sp_Country '" + Statement + "', '" + CompanyId + "',@searchitem= '" + searchitem + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Country.Add(new Country
                {

                    ID = Convert.ToInt32(dr["id"]),
                    country = Convert.ToString(dr["country"]),

                });
            }
            return Json(Country);
        }

        [HttpPost]
        public JsonResult EditCountry(int id)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Statement = "EDIT";
            Country Country = new Country();
            sqlquery = "exec Sp_Country '" + Statement + "'," + CompanyId + "," + id + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Country.CompanyID = Convert.ToInt32(dr["companyid"]);
                Country.ID = Convert.ToInt32(dr["id"]);
                Country.country = Convert.ToString(dr["country"]);


            }
            return Json(Country);
        }
        [HttpPost]
        public JsonResult DeleteCountry(int id)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            Statement = "DELETE";
            sqlquery = "exec Sp_Country '" + Statement + "','" + CompanyId + "'," + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }
        #endregion

        #region State
        public IActionResult State()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            CountryBind();
            return View();
        }


        [HttpPost]
        public IActionResult IUState(int id, int country_id, string state_name, int Status)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            if (id == 0)
            {
                DataTable dt = Utility.SelectParticular("tblstate", "*", " or state='" + state_name + "' ");
                if (dt.Rows.Count > 0)
                {
                    ViewBag.ErrorMsg = "Your state or State is already exit.";
                }
                else
                {
                    Statement = "INSERT";
                    sqlquery = "exec Sp_State '" + Statement + "'," + CompanyId + "," + id + "," + country_id + ",'" + state_name + "'," + Status + "," + userid + "";

                    status = Utility.MultipleTransactions(sqlquery);
                    if (status == "Successfull")
                    {
                        message = "State added.";
                    }
                    else
                    {
                        message = "State not added.";
                    }

                }

            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec Sp_State '" + Statement + "'," + CompanyId + "," + id + "," + country_id + ",'" + state_name + "'," + Status + "," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "State modify successfully.";
                }
                else
                {
                    message = "State not modify successfully.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }

        [HttpGet]
        public JsonResult ShowState(string searchitem)
        {
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<State> State = new List<State>();
            sqlquery = "exec Sp_State '" + Statement + "', '" + CompanyId + "',@searchitem= '" + searchitem + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                State.Add(new State
                {
                    CompanyID = Convert.ToInt32(dr["companyid"]),
                    ID = Convert.ToInt32(dr["id"]),
                    CID = Convert.ToInt32(dr["country_id"]),
                    state = Convert.ToString(dr["state_name"]),
                    countryname = Convert.ToString(dr["country"]),
                    Status = Convert.ToInt32(dr["status"]),

                });
            }
            return Json(State);
        }
        [HttpPost]
        public JsonResult EditState(int ID)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Statement = "EDIT";
            State State = new State();
            sqlquery = "exec Sp_State '" + Statement + "'," + CompanyId + "," + ID + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                State.CompanyID = Convert.ToInt32(dr["companyid"]);
                State.ID = Convert.ToInt32(dr["id"]);
                //State.CID = Convert.ToInt32(dr["AdminId"]);
                State.state = Convert.ToString(dr["state_name"]);
                State.countryname = Convert.ToString(dr["country_id"]);
                State.Status = Convert.ToInt32(dr["status"]);

            }
            return Json(State);
        }
        [HttpPost]
        public JsonResult DeleteState(int id)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            Statement = "DELETE";
            sqlquery = "exec Sp_State '" + Statement + "','" + CompanyId + "'," + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }
        #endregion

        #region City
        public IActionResult City()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            StateBind();
            return View();
        }



        [HttpPost]
        public IActionResult IUCity(int id, int state_id, string city, int Status)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            if (id == 0)
            {
                DataTable dt = Utility.SelectParticular("tblcity", "*", "city='" + city + "' and state_id=" + state_id + "");
                if (dt.Rows.Count > 0)
                {
                    message = "This City is already exist.";
                }
                else
                {
                    Statement = "INSERT";
                    sqlquery = "exec Sp_City '" + Statement + "'," + CompanyId + "," + id + "," + state_id + ",'" + city + "'," + Status + "," + userid + "";
                    status = Utility.MultipleTransactions(sqlquery);
                    if (status == "Successfull")
                    {
                        message = "City added.";
                    }
                    else
                    {
                        message = "City not added.";
                    }
                }

            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec Sp_City '" + Statement + "'," + CompanyId + "," + id + "," + state_id + ",'" + city + "'," + Status + "," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "City modify successfully.";
                }
                else
                {
                    message = "City not modify successfully.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }

        [HttpGet]
        public JsonResult ShowCity(string searchitem)
        {
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<City> City = new List<City>();
            sqlquery = "exec Sp_City '" + Statement + "', '" + CompanyId + "',@searchitem= '" + searchitem + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                City.Add(new City
                {
                    CompanyID = Convert.ToInt32(dr["companyid"]),
                    ID = Convert.ToInt32(dr["id"]),
                    SID = Convert.ToInt32(dr["id"]),
                    city = Convert.ToString(dr["city"]),
                    state = Convert.ToString(dr["state_name"]),
                    countryname = Convert.ToString(dr["country"]),
                    Status = Convert.ToInt32(dr["status"]),


                });
            }
            return Json(City);
        }

        [HttpPost]
        public JsonResult EditCity(int ID)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Statement = "EDIT";
            City City = new City();
            sqlquery = "exec Sp_City '" + Statement + "'," + CompanyId + "," + ID + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                City.CompanyID = Convert.ToInt32(dr["companyid"]);
                City.ID = Convert.ToInt32(dr["id"]);
                City.SID = Convert.ToInt32(dr["state_id"]);
                City.city = Convert.ToString(dr["city"]);
                City.Status = Convert.ToInt32(dr["status"]);

            }
            return Json(City);
        }

        [HttpPost]
        public JsonResult DeleteCity(int id)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            Statement = "DELETE";
            sqlquery = "exec Sp_City '" + Statement + "','" + CompanyId + "'," + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }
        #endregion

        #region Location Master
        public IActionResult Location()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        [HttpPost]
        public JsonResult IULocation(int ID, string LocationName, int PinCode, int Status)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            if (ID == 0)
            {
                DataTable dt = Utility.SelectParticular("tblLocation", "*", "LocationName='" + LocationName + "' and PinCode=" + PinCode + "");
                if (dt.Rows.Count > 0)
                {
                    message = "Your  Location is already exit on this pincode '" + LocationName + "'," + PinCode + "";

                }
                else
                {
                    Statement = "INSERT";
                    sqlquery = "exec SP_IU_LocationMaster '" + Statement + "'," + CompanyId + "," + ID + ",'" + LocationName + "'," + PinCode + "," + Status + "," + userid + "";
                    status = Utility.MultipleTransactions(sqlquery);
                    if (status == "Successfull")
                    {
                        message = "Location added.";
                    }
                    else
                    {
                        message = "Location not added.";
                    }
                }

            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec SP_IU_LocationMaster '" + Statement + "'," + CompanyId + "," + ID + ",'" + LocationName + "'," + PinCode + "," + Status + "," + userid + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Location modify successfully.";
                }
                else
                {
                    message = "Location not modify successfully.";
                }
            }
            var Data = new { message = message, ID = ID };
            return Json(Data);
        }

        [HttpGet]
        public JsonResult ShowLocation(string searchitem)
        {
            Statement = "SELECT";
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Location> Location = new List<Location>();
            sqlquery = "exec SP_IU_LocationMaster '" + Statement + "', '" + CompanyId + "',@searchitem= '" + searchitem + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Location.Add(new Location
                {
                    CompanyID = Convert.ToInt32(dr["companyid"]),
                    ID = Convert.ToInt32(dr["ID"]),
                    LocationName = Convert.ToString(dr["locationname"]),
                    PinCode = Convert.ToInt32(dr["pincode"]),
                    Status = Convert.ToInt32(dr["status"]),


                });
            }
            return Json(Location);
        }

        [HttpPost]
        public JsonResult EditLocation(int id)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Statement = "EDIT";
            Location Location = new Location();
            sqlquery = "exec SP_IU_LocationMaster '" + Statement + "'," + CompanyId + "," + id + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Location.CompanyID = Convert.ToInt32(dr["companyid"]);
                Location.ID = Convert.ToInt32(dr["ID"]);
                Location.LocationName = Convert.ToString(dr["LocationName"]);
                Location.PinCode = Convert.ToInt32(dr["PinCode"]);
                Location.Status = Convert.ToInt32(dr["Status"]);

            }
            return Json(Location);
        }
        [HttpPost]
        public JsonResult DeleteLocation(int id)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            Statement = "DELETE";
            sqlquery = "exec SP_IU_LocationMaster '" + Statement + "','" + CompanyId + "'," + id + "";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }
        #endregion

        #region ApproveItem
        public IActionResult Approve(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            BindDuplicateProduct();
            BindMainItemCategory();
            ItemTypeBind();
            BrandBind();
            BindHSN();
            Statement = "EDIT";
            AddNewProduct product = new AddNewProduct();
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            sqlquery = "exec SP_itemApprove '" + Statement + "'," + CompanyId + "," + id + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                product.ID = Convert.ToInt32(dr["ID"]);
                product.ItemName = dr["ItemName"].ToString();
                product.image = dr["image"].ToString();
                product.image1 = dr["image1"].ToString();
                product.image2 = dr["image2"].ToString();
                product.image3 = dr["image3"].ToString();
                product.URLName = dr["URLName"].ToString();
                product.Main_cat_name = dr["Main_cat_name"].ToString();
                product.brand_name = dr["brand_name"].ToString();
                product.category_name = dr["category_name"].ToString();
                product.cat_name = dr["cat_name"].ToString();
                product.productTag = dr["productTag"].ToString();
                product.HSNName = dr["HSNName"].ToString();
                product.SKUCode = dr["SKUCode"].ToString();
                product.CostPrice = Convert.ToInt32(dr["CostPrice"]);
                product.MRP = Convert.ToInt32(dr["MRP"]);
                product.StockStatus = Convert.ToInt32(dr["StockStatus"]);
                product.Dimension = dr["Dimension"].ToString();
                product.ShipCharge = dr["ShipCharge"].ToString();
                product.Description = dr["Description"].ToString();
                product.additional = dr["additional"].ToString();
                product.ingredients = dr["ingredients"].ToString();
            }
            return View("Approve", product);
        }
        public IActionResult SaveApprove()
        {

            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            IFormFile file = Request.Form.Files[0];
            IFormFile backfiles = Request.Form.Files[1];
            IFormFile leftfiles = Request.Form.Files[2];
            IFormFile rightfiles = Request.Form.Files[3];
            string id = Request.Form["ID"].ToString();
            string ItemName = Request.Form["ItemName"].ToString();
            string status = Request.Form["status"].ToString();
            string flag = Request.Form["Flag"].ToString();
            string URLName = Request.Form["URLName"].ToString();
            string GroupID = Request.Form["GroupID"].ToString();
            string CategoryID = Request.Form["CategoryID"].ToString();
            string SubGroupID = Request.Form["SubGroupID"].ToString();
            string BrandID = Request.Form["BrandID"].ToString();
            string HSNCode = Request.Form["HSNCode"].ToString();
            string SKUCode = Request.Form["SKUCode"].ToString();
            string RegularPrice = Request.Form["RegularPrice"].ToString();
            string SalePrice = Request.Form["SalePrice"].ToString();
            string Stockstatus = Request.Form["Stockstatus"].ToString();
            string Weight = Request.Form["Weight"].ToString();
            string weightval = Request.Form["weightval"].ToString();
            string D_Length = Request.Form["D_Length"].ToString();
            string D_Width = Request.Form["D_Width"].ToString();
            string D_height = Request.Form["txtDimension3"].ToString();
            string ShipCharges = Request.Form["ShipCharges"].ToString();
            string ProductDesc = Request.Form["ProductDesc"].ToString();
            string AddInformation = Request.Form["AddInformation"].ToString();
            string Ingredients = Request.Form["Ingredients"].ToString();
            string hdnbalance = Request.Form["balance"].ToString();
            string hdnStock = Request.Form["hdnStock"].ToString();
            string folderName = "wwwroot/Images/Productimage/BSD INFOTECH/";

            string extension = Path.GetExtension(file.FileName);
            string filename = Path.GetFileNameWithoutExtension(file.FileName);
            string webRootPath = filename + DateTime.Now.ToString("yymmssfff") + extension;
            string newPath = Path.Combine(folderName, webRootPath);
            using (var fileStream = new FileStream(newPath, FileMode.Create))
            {
                file.CopyTo(fileStream);

            }

            string extension1 = Path.GetExtension(backfiles.FileName);
            string backfilename = Path.GetFileNameWithoutExtension(backfiles.FileName);
            string backwebRootPath = backfilename + DateTime.Now.ToString("yymmssfff") + extension1;
            string newPathb = Path.Combine(folderName, backwebRootPath);
            using (var fileStream = new FileStream(newPathb, FileMode.Create))
            {
                backfiles.CopyTo(fileStream);
            }


            string extension2 = Path.GetExtension(leftfiles.FileName);
            string leftfilename = Path.GetFileNameWithoutExtension(leftfiles.FileName);
            string leftwebRootPath = leftfilename + DateTime.Now.ToString("yymmssfff") + extension2;
            string newPatht = Path.Combine(folderName, leftwebRootPath);
            using (var fileStream = new FileStream(newPatht, FileMode.Create))
            {
                leftfiles.CopyTo(fileStream);
            }


            string extension3 = Path.GetExtension(rightfiles.FileName);
            string rightfilename = Path.GetFileNameWithoutExtension(rightfiles.FileName);
            string rightwebRootPath = rightfilename + DateTime.Now.ToString("yymmssfff") + extension3;
            string newPath1 = Path.Combine(folderName, rightwebRootPath);
            using (var fileStream = new FileStream(newPath1, FileMode.Create))
            {
                rightfiles.CopyTo(fileStream);
            }

            if (file.Length > 0)
            {

                if (id == "0")
                {

                    Statement = "INSERT";
                    sqlquery = "exec SP_Up_tblItemStore  '" + Statement + "'," + CompanyId + "," + id + ",'" + ItemName + "','" + URLName + "','" + GroupID + "','" + CategoryID + "'," + SubGroupID + ",'" + BrandID + "','" + HSNCode + "','" + webRootPath + "','" + backwebRootPath + "','" + leftwebRootPath + "','" + rightwebRootPath + "','" + SKUCode + "','" + Stockstatus + "','" + Weight + "','" + RegularPrice + "','" + SalePrice + "'," + ShipCharges + "," + D_Length + ",'" + ProductDesc + "'," + AddInformation + "," + Ingredients + ",'" + status + "'," + flag + "";
                    status = Utility.MultipleTransactions(sqlquery);
                    if (status == "Successfull")
                    {
                        message = "Item Approve added";
                    }
                    else
                    {
                        message = "Item Approve not added.";
                    }
                }
                else
                {
                    Statement = "UPDATE";
                    sqlquery = "exec SP_Up_tblItemStore  '" + Statement + "'," + CompanyId + "," + id + ",'" + ItemName + "','" + URLName + "','" + GroupID + "','" + CategoryID + "'," + SubGroupID + "," + BrandID + "'," + HSNCode + ",'" + webRootPath + "','" + backwebRootPath + "','" + leftwebRootPath + "','" + rightwebRootPath + "','" + SKUCode + "'," + Stockstatus + ",'" + Weight + "'," + RegularPrice + "," + SalePrice + ",'" + ShipCharges + "'," + D_Length + ",'" + ProductDesc + "','" + AddInformation + "','" + Ingredients + "'," + status + "," + flag + "";
                    status = Utility.MultipleTransactions(sqlquery);
                    if (status == "Successfull")
                    {
                        message = "Item Approve added";
                    }
                    else
                    {
                        message = "Item Approve not added.";
                    }
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }
        #endregion

        #region ApproveItemList
        public IActionResult ApproveItemList()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        [HttpPost]
        public JsonResult ShowItemApproveList()
        {
            List<AddNewProduct> product = new List<AddNewProduct>();
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Statement = "SELECT";
            sqlquery = "exec SP_itemApprove'" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                product.Add(new AddNewProduct
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    ItemName = Convert.ToString(dr["ItemName"]),
                    StockStatus = Convert.ToInt32(dr["StockStatus"]),
                    MRP = Convert.ToInt32(dr["MRP"]),
                    Status = Convert.ToBoolean(dr["Status"]),
                    Flag = Convert.ToBoolean(dr["Flag"]),
                });
            }
            return Json(product);
        }
        [HttpPost]
        public JsonResult UpdateApprove(int id)
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            string Statement = "UPDATE";
            int status = 1;
            sqlquery = "exec SP_itemApprove '" + Statement + "','" + CompanyId + "'," + id + "," + status + ",'" + userid + "'";
            string st = Utility.MultipleTransactions(sqlquery);
            if (st == "Successfull")
            {
                message = "Item Approve Successfully.";
            }
            else
            {
                message = "Item not Approve.";
            }
            var Data = new { message = message };
            return Json(Data);
        }
        [HttpPost]
        public IActionResult UpdateApproveStatus(int id)
        {

            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            // string status = Request.Form["status"].ToString();
            int statusvalue = 0;
            Statement = "UPDATE";
            if (id != 0)
            {
                statusvalue = 1;
                sqlquery = "exec SP_tblItemStore  '" + Statement + "'," + CompanyId + "," + id + "," + statusvalue + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Approve added";
                }
                else
                {
                    message = "Approve not added.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }
        [HttpPost]
        public JsonResult NotApproveStatus(int id)
        {

            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            string Statement = "UPDATE";
            int status = 0;
            sqlquery = "exec SP_itemApprove '" + Statement + "','" + CompanyId + "'," + id + "," + status + ",'" + userid + "'";
            string st = Utility.MultipleTransactions(sqlquery);
            if (st == "Successfull")
            {
                message = "Item not Approve.";
            }
            else
            {
                message = "some thing error.";
            }
            var Data = new { message = message };
            return Json(Data);
        }
        [HttpPost]
        public IActionResult Update(AddNewProduct product)
        {

            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            string sqlquery;
            int Flag;
            int status_value;
            try
            {
                ViewBag.SubmitValue = "Update";
                if (product.Flag == true)
                {
                    Flag = 1;
                }
                else
                {
                    Flag = 0;
                }
                if (product.Status == true)
                {
                    status_value = 1;
                }
                else
                {
                    status_value = 0;
                }
                Statement = "INSERT";
                sqlquery = "exec SP_Up_tblItemStore  '" + Statement + "'," + CompanyId + "," + product.ID + ",'" + product.Flag + "," + product.Status + "'," + userid + "";
                string status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    ViewData["msg"] = "Flag modify successFully.";
                }
                else
                {
                    ViewData["msg"] = "Flag not modify.";
                }

            }
            catch (Exception ex)
            {

                throw;
            }

            return View("product");
        }
        #endregion

        #region MasterReport
        #region CustomerList
        public IActionResult CustomerList()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        [HttpPost]
        public JsonResult ShowCustomerList(string searchitem)
        {
            List<Models.Customer> customer = new List<Models.Customer>();
            Statement = "SELECT";
            sqlquery = "exec SP_Show_Customer '" + Statement + "',@searchitem= '" + searchitem + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            String Pass = Utility.decryption(ds.Tables[0].Rows[0]["password"].ToString());
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                customer.Add(new Models.Customer
                {
                    reg_id = Convert.ToInt32(dr["reg_id"]),
                    first_name = Convert.ToString(dr["first_name"]),
                    MobileNo = Convert.ToString(dr["MobileNo"]),
                    email = Convert.ToString(dr["email"]),
                    password = Utility.decryption(dr["password"].ToString()),
                    CityID = Convert.ToString(dr["CityID"]),
                    StateID = Convert.ToString(dr["StateID"]),
                    dob = Convert.ToString(dr["dob"]),
                    status = Convert.ToBoolean(dr["status"]),
                });
            }
            return Json(customer);
        }
        #endregion
        #region BrandReport
        public IActionResult BrandReport()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        [HttpPost]
        public JsonResult ShowBrandReport()
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<order> invoice = new List<order>();
            Statement = "SELECT";
            sqlquery = "exec SP_Search_ItemBrand '" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                invoice.Add(new order
                {
                    order_id = Convert.ToInt32(dr["order_id"]),
                    first_name = Convert.ToString(dr["first_name"]),
                    mobile = Convert.ToString(dr["mobile"]),
                    address1 = Convert.ToString(dr["address1"]),
                    ItemName = Convert.ToString(dr["ItemName"]),
                    brand_name = Convert.ToString(dr["brand_name"]),
                });
            }
            return Json(invoice);
        }
        [HttpPost]
        public JsonResult SearchBrandReport(int order_id, string first_name, string mobile)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<order> invoice = new List<order>();
            Statement = "SEARCH";
            sqlquery = "exec SP_Search_ItemBrand '" + Statement + "'," + CompanyId + "," + order_id + ",'" + first_name + "','" + mobile + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                invoice.Add(new order
                {
                    order_id = Convert.ToInt32(dr["order_id"]),
                    first_name = Convert.ToString(dr["first_name"]),
                    mobile = Convert.ToString(dr["mobile"]),
                    address1 = Convert.ToString(dr["address1"]),
                    ItemName = Convert.ToString(dr["ItemName"]),
                    brand_name = Convert.ToString(dr["brand_name"]),
                });
            }
            return Json(invoice);
        }
        #endregion
        #region VendorReport
        public IActionResult VendorReport()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        #endregion
        #region MainCategoryReport
        public IActionResult MainCategoryReport()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        [HttpPost]
        public JsonResult ShowMainCategoryReport()
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Models.MainCategory> MainCategory = new List<Models.MainCategory>();
            Statement = "SELECT";
            sqlquery = "exec SP_Serch_MainCat '" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                MainCategory.Add(new Models.MainCategory
                {
                    Main_cat_id = Convert.ToInt32(dr["Main_cat_id"]),
                    Main_cat_name = Convert.ToString(dr["Main_cat_name"]),
                    Main_cat_status = Convert.ToBoolean(dr["Main_cat_status"]),
                    entry_date = Convert.ToString(dr["entry_date"]),
                });
            }
            return Json(MainCategory);
        }
        [HttpPost]
        public JsonResult SearchMainCategoryReport(string main_cat_name)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Models.MainCategory> MainCategory = new List<Models.MainCategory>();
            Statement = "SEARCH";
            sqlquery = "exec SP_Serch_MainCat '" + Statement + "'," + CompanyId + ",'" + main_cat_name + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                MainCategory.Add(new Models.MainCategory
                {
                    Main_cat_id = Convert.ToInt32(dr["Main_cat_id"]),
                    Main_cat_name = Convert.ToString(dr["Main_cat_name"]),
                    Main_cat_status = Convert.ToBoolean(dr["Main_cat_status"]),
                    entry_date = Convert.ToString(dr["entry_date"]),
                });
            }
            return Json(MainCategory);
        }
        #endregion
        #region SubCategoryReport
        public IActionResult SubCategoryReport()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            BindMainItemCategory();
            return View();
        }
        [HttpPost]
        public JsonResult ShowSubCategoryReport()
        {
            List<Models.SubCategory> SubCategory = new List<Models.SubCategory>();
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Statement = "SUBCATREPORT";
            sqlquery = "exec Sp_Sel_Del_IU_ItemCategory '" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SubCategory.Add(new Models.SubCategory
                {
                    cat_id = Convert.ToInt32(dr["cat_id"]),
                    cat_name = Convert.ToString(dr["cat_name"]),
                    cat_status = Convert.ToBoolean(dr["cat_status"]),
                    Main_cat_id = Convert.ToInt32(dr["Main_cat_id"]),
                    main_cat_name = Convert.ToString(dr["Main_cat_name"]),
                    category_name = Convert.ToString(dr["category_name"]),
                    category_id = Convert.ToInt32(dr["category_id"]),
                    entry_date = Convert.ToString(dr["entry_date"]),

                });
            }
            return Json(SubCategory);
        }
        [HttpPost]
        public JsonResult SearchSubCategoryReport(int cat_id, string cat_name, string Main_cat_id, string category_id)
        {
            List<Models.SubCategory> SubCategory = new List<Models.SubCategory>();
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Statement = "SEARCH";
            sqlquery = "exec Sp_Sel_Del_IU_ItemCategory '" + Statement + "'," + CompanyId + "," + cat_id + ",'" + cat_name + "','" + Main_cat_id + "''" + category_id + "'";
            //sqlquery = "select Main_cat_id,Main_cat_name,Main_cat_status from tblitem_Main_category where Main_cat_name like 'fashion%' order by main_cat_name";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SubCategory.Add(new Models.SubCategory
                {
                    cat_id = Convert.ToInt32(dr["cat_id"]),
                    cat_name = Convert.ToString(dr["cat_name"]),
                    cat_status = Convert.ToBoolean(dr["cat_status"]),
                    Main_cat_id = Convert.ToInt32(dr["Main_cat_id"]),
                    main_cat_name = Convert.ToString(dr["Main_cat_name"]),
                    category_name = Convert.ToString(dr["category_name"]),
                    category_id = Convert.ToInt32(dr["category_id"]),
                    entry_date = Convert.ToString(dr["entry_date"]),

                });
            }
            return Json(SubCategory);
        }
        #endregion
        #region CategoryReport
        public IActionResult CategoryReport()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            BindMainItemCategory();
            return View();
        }
        [HttpPost]
        public JsonResult ShowCategoryReport()
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            List<Models.Category> Category = new List<Models.Category>();
            Statement = "SELECT";
            sqlquery = "exec Sp_Sel_Del_tblcategory '" + Statement + "'," + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Category.Add(new Models.Category
                {
                    category_id = Convert.ToInt32(dr["category_id"]),
                    category_name = Convert.ToString(dr["category_name"]),
                    main_cat_name = Convert.ToString(dr["Main_cat_name"]),
                    cat_status = Convert.ToBoolean(dr["cat_status"]),
                    entry_date = Convert.ToString(dr["entry_date"]),
                });
            }
            return Json(Category);
        }
        [HttpPost]
        public JsonResult SearchCategoryReport(int cat_id, string category_name, string Main_cat_id)
        {
            List<Models.Category> Category = new List<Models.Category>();
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Statement = "SEARCH";
            sqlquery = "exec Sp_Sel_Del_tblcategory '" + Statement + "'," + CompanyId + "," + cat_id + ",'" + category_name + "','" + Main_cat_id + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Category.Add(new Models.Category
                {
                    category_id = Convert.ToInt32(dr["category_id"]),
                    category_name = Convert.ToString(dr["category_name"]),
                    main_cat_name = Convert.ToString(dr["Main_cat_name"]),
                    cat_status = Convert.ToBoolean(dr["cat_status"]),
                    entry_date = Convert.ToString(dr["entry_date"]),

                });
            }
            return Json(Category);
        }
        #endregion 
        #endregion

        #region Stock
        public IActionResult strockUpdate()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }
            BindMainItemCategory();
            return View();
        }
        [HttpPost]
        public JsonResult ShowItemstrock(int SubCategory)
        {
            List<StockMaster> stocks = new List<StockMaster>();
            Statement = "SELECT";
            sqlquery = "exec SP_Stock '" + Statement + "','" + SubCategory + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                stocks.Add(new StockMaster
                {
                    itemdetailsid = Convert.ToInt32(dr["itemdetailsid"]),
                    //id = Convert.ToInt32(dr["id"]),
                    //cat_id = Convert.ToInt32(dr["cat_id"]),
                    ItemName = Convert.ToString(dr["ItemName"]),
                    //cat_name = Convert.ToString(dr["cat_name"]),
                    //Main_cat_name = Convert.ToString(dr["Main_cat_name"]),
                    stockqty = Convert.ToString(dr["stockqty"]),
                    Unit_Qty = Convert.ToString(dr["stockqty"]),
                });
            }
            return Json(stocks);
        }
        [HttpPost]
        public JsonResult Updatestockqty(string jsonInput)
        {
            DataTable dt = Utility.JsonStringToDataTable(jsonInput);
            int row = dt.Rows.Count;
            int col = dt.Columns.Count;
            int i = 0;
            //string messagge = string.Empty;
            try
            {
                for (i = 0; i < row; i++)
                {
                    Statement = "UPDATE";
                    sqlquery += "exec SP_Stock '" + Statement + "',@id='" + dt.Rows[i]["itemdetailsid"].ToString() + "',@stockqty='" + dt.Rows[i]["stockqty"] + "'";
                }
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "stockqty modify successfully.";
                }
                else
                {
                    message = "stockqty not modify successfully.";
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;

            }
            var Data = new { message = message };
            return Json(Data);
        }


        #endregion

        #region Blog

        public IActionResult Blog()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return RedirectToAction("AdminLogin");
            }

            return View();
        }

        [HttpPost]
        public JsonResult IUBlog()
        {
            string userid = HttpContext.Session.GetString("AdminId");
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            string id = Request.Form["id"].ToString();
            string Title = Request.Form["Title"].ToString();
            string ShortDesc = Request.Form["ShortDesc"].ToString();
            string LongDesc = Request.Form["LongDesc"].ToString();
            string Status = Request.Form["Status"].ToString();
            string flg = Request.Form["flg"].ToString();
            string nefilname = Request.Form["filenmes"].ToString();
            string folderName = "wwwroot/images/Blog/" + CompanyId + "/";
            string webRootPath = "";
            string filename = "";
            if (flg == "okg")
            {
                nefilname = "";
                IFormFile file = Request.Form.Files[0];
                string extension = Path.GetExtension(file.FileName);
                filename = Path.GetFileNameWithoutExtension(file.FileName);
                //nefilname = filename;
                webRootPath = filename + DateTime.Now.ToString("yymmssfff") + extension;
                nefilname = webRootPath;
                string newPath = Path.Combine(folderName, webRootPath);
                using (var fileStream = new FileStream(newPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            if (id == "0")
            {
                dt = Utility.SelectParticular("tblblog", "*", "Title='" + Title + "'");
                if (dt.Rows.Count > 0)
                {
                    message = "This Title is already exit";
                }
                else
                {
                    Statement = "INSERT";
                    sqlquery = "exec Sp_Blog '" + Statement + "'," + id + "," + CompanyId + ",'" + webRootPath + "','" + Title + "','" + ShortDesc + "','" + LongDesc + "'," + Status + "";
                    status = Utility.MultipleTransactions(sqlquery);
                    if (status == "Successfull")
                    {
                        message = "Blog added.";
                    }
                    else
                    {
                        message = "Blog not added.";
                    }
                }
            }
            else
            {
                Statement = "UPDATE";
                sqlquery = "exec Sp_Blog '" + Statement + "'," + id + "," + CompanyId + ",'" + nefilname + "','" + Title + "','" + ShortDesc + "','" + LongDesc + "'," + Status + "";
                status = Utility.MultipleTransactions(sqlquery);
                if (status == "Successfull")
                {
                    message = "Blog added.";
                }
                else
                {
                    message = "Blog not added.";
                }
            }
            var Data = new { message = message, id = id };
            return Json(Data);
        }
        [HttpGet]
        public JsonResult ShowBlog()
        {
            Statement = "SELECT";
            string companyid = HttpContext.Session.GetString("Company_Id");
            List<Models.blog> bg = new List<Models.blog>();
            sqlquery = "exec Sp_Blog '" + Statement + "'," + 0 + "," + companyid + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                bg.Add(new Models.blog
                {
                    ID = Convert.ToInt32(dr["id"]),
                    Title = Convert.ToString(dr["Title"]),
                    BlogImage = Convert.ToString(dr["BlogImage"]),
                    ShortDesc = Convert.ToString(dr["ShortDesc"]),
                    LongDesc = Convert.ToString(dr["LongDesc"]),
                    Status = Convert.ToInt32(dr["Status"]),
                });
            }
            return Json(bg);
        }

        [HttpPost]
        public JsonResult DeleteBlog(int id)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");

            Statement = "DELETE";
            sqlquery = "exec Sp_Blog '" + Statement + "'," + id + ",'" + CompanyId + "'";
            status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            var Data = new { msg = message };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult EditBlog(int id)
        {
            string CompanyId = HttpContext.Session.GetString("Company_Id");
            Statement = "EDIT";
            Models.blog bg = new Models.blog();
            sqlquery = "exec Sp_Blog '" + Statement + "'," + id + "," + CompanyId + "";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                bg.ID = Convert.ToInt32(dr["id"]);
                bg.Title = Convert.ToString(dr["Title"]);
                bg.BlogImage = Convert.ToString(dr["BlogImage"]);
                bg.ShortDesc = Convert.ToString(dr["ShortDesc"]);
                bg.LongDesc = Convert.ToString(dr["LongDesc"]);
                bg.Status = Convert.ToInt32(dr["Status"]);

            }
            return Json(bg);
        }



        #endregion


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("AdminLogin");
        }
    }
}
