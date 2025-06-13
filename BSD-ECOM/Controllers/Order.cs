using BSD_ECOM.Models;
using BSD_ECOM.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace BSD_ECOM.Controllers
{
    public class Order : Controller
    {
        ClsUtility util = new ClsUtility();
        DataBaseAccess Db = new DataBaseAccess();
        IndexViewModel viewmodel = new IndexViewModel();
        string message = "";
        string sqlquery = "";
        string status = "";
        [HttpGet]
        [Route("CheckOut")]
        public IActionResult CheckOut()
        {
            customer cs = new customer();
            GetSiteNameAndCode();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("customerid")))
            {
                //TempData["Message"] = "Form submitted successfully!";
                // HttpContext.Session.GetString("customerid");
                // HttpContext.Session.SetString("checkouturl", "Form submitted successfully!");
                return RedirectToAction("Login", "Customer", new { id = "1" });
            }

            else
            {


                //  
                // string customerid = HttpContext.Session.GetString("customerid");
                string customerid = HttpContext.Session.GetString("customerid");
                string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
                DataSet maincat = Db.showCustomeraddress(customerid, companyid);

                if (maincat.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in maincat.Tables[0].Rows)
                    {
                        cs.id = Convert.ToString(dr["id"]);
                        cs.firstname = Convert.ToString(dr["first_name"]);
                        cs.lastname = Convert.ToString(dr["last_name"]);
                        cs.MobileNo = Convert.ToString(dr["MobileNo"]);
                        cs.email = Convert.ToString(dr["email"]);
                        cs.Address = Convert.ToString(dr["Address"]);
                        cs.address2 = Convert.ToString(dr["address2"]);
                        cs.CountryID = Convert.ToString(dr["CountryID"]);
                        cs.StateID = Convert.ToString(dr["StateID"]);
                        cs.CityID = Convert.ToString(dr["CityID"]);
                        cs.PinCode = Convert.ToString(dr["PinCode"]);
                        ViewBag.cuntid = Convert.ToString(dr["CountryID"]);
                        ViewBag.StateID = Convert.ToString(dr["StateID"]);
                        ViewBag.citid = Convert.ToString(dr["CityID"]);
                    }

                }


                //cs.id = HttpContext.Session.GetString("customerid");
                //cs.firstname = HttpContext.Session.GetString("customerfirstname");
                //cs.lastname = HttpContext.Session.GetString("customerlastname");
                //cs.MobileNo = HttpContext.Session.GetString("customerMobileNo");
                //cs.email = HttpContext.Session.GetString("customeremail");


                Country();
                BindState();
                BindCity();
                if (HttpContext.Session.GetInt32("CartItems").ToString() != "")
                {
                    ViewBag.cart = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("CartItems"));


                    ViewBag.count = (HttpContext.Session.GetInt32("count"));
                }
            }
            return View(cs);

        }



        [HttpPost]
        public JsonResult checkout(int id, double amount, string firstname, string lastname, string address1, string address2, int countryId, int stateId, int city, string pincode, string phone, string email, string addtionalinfo, string Sfirstname, string Slastname, string Saddress1, string Saddress2, int ScountryId, int SstateId, int ScityId, string Spincode, string paymentmode, string smobile, string Semail, string shipc, string productjson)
        {
            bool msg = false;
            int customerid = Convert.ToInt32(HttpContext.Session.GetInt32("customerid"));
            int companyid = Convert.ToInt32(HttpContext.Session.GetInt32("SiteId"));
            string companystatecode = Convert.ToString(HttpContext.Session.GetString("statecode"));
            //string coustomerstatecode = Convert.ToString(HttpContext.Session.GetString("cstatecode"));
            string coustomerstatecode = "07";
            DataTable dtproductjson = util.JsonStringToDataTable(productjson);
            string dob = "";
            int discount = 0, walletAmtdetect = 0;
            int row = dtproductjson.Rows.Count;
            int col = dtproductjson.Columns.Count;
            int i = 0;
            if (Db.IU_Checkout(id, address1, address2, city, pincode, countryId, stateId, ScityId, ScountryId, SstateId, ScityId, Saddress1, Saddress2, smobile, Sfirstname, firstname, addtionalinfo, Semail, Spincode))
            {
                if (Db.IU_Order(id, amount, Sfirstname, Slastname, Saddress1, Saddress2, SstateId, ScityId, Spincode, ScountryId, smobile, Semail, firstname, lastname, address1, address2, stateId, city, pincode, countryId, phone, email, dob, paymentmode, discount, walletAmtdetect, companyid))
                {

                    ViewBag.cart = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("CartItems"));

                    foreach (var person in ViewBag.cart)
                    {
                        double amt = 0, FinalAmount = 0, Amount, subtotal = 0, taxper_CGST = 0, taxper_SGST = 0, taxper_IGST = 0, taxper_UTST = 0, CGSTAmt = 0, SGSTAmt = 0, IGSTAmt = 0, UTSTAmt = 0, ShippingCharge = 0, Ship_sgst = 0, Ship_cgst = 0, Ship_igst = 0;


                        DataTable dtTaxper = util.SelectParticular("tblHSN as H,tblItemStore as M,tblRate as R,tbltax as T ", " H.HSNName,H.HSNCode,M.ItemName,R.TaxPer,T.TaxName,T.TaxCode", " H.id=M.HSNCode and R.EffectiveDate<=(convert(datetime,getdate(),103)) and R.HSNID=h.id and t.id=r.taxid and M.id=" + person.productid + " and r.taxid in(1,2,3) order by taxid asc");
                        double GTotal = 0;

                        if (dtTaxper.Rows.Count > 0)
                        {
                            if (companystatecode == coustomerstatecode.ToString())
                            {

                                taxper_CGST = Convert.ToDouble(dtTaxper.Rows[0]["TaxPer"].ToString());
                                taxper_SGST = Convert.ToDouble(dtTaxper.Rows[1]["TaxPer"].ToString());
                                GTotal = Convert.ToDouble(Convert.ToDouble(person.productprice) * Convert.ToDouble(person.quantity));

                                CGSTAmt = (taxper_CGST * GTotal) / (100 + taxper_CGST);
                                SGSTAmt = (taxper_SGST * GTotal) / (100 + taxper_SGST);
                                IGSTAmt = 0;
                                taxper_IGST = 0;

                            }
                            if (companystatecode == coustomerstatecode.ToString())
                            {

                                taxper_CGST = 9; taxper_SGST = 9;

                                Ship_sgst = (Convert.ToDouble(Convert.ToDouble(person.shipcharge) * taxper_SGST) / (100 + taxper_SGST)); ;
                                Ship_cgst = (Convert.ToDouble(Convert.ToDouble(person.shipcharge) * taxper_CGST) / (100 + taxper_CGST)); ;
                                Ship_igst = 0;

                            }
                            if (companystatecode != coustomerstatecode.ToString())
                            {
                                taxper_IGST = Convert.ToDouble(dtTaxper.Rows[2]["TaxPer"].ToString());
                                GTotal = Convert.ToDouble(Convert.ToDouble(person.productprice) * Convert.ToDouble(person.quantity));
                                IGSTAmt = (taxper_IGST * GTotal) / (100 + taxper_IGST);
                                CGSTAmt = 0;
                                SGSTAmt = 0;
                            }
                            if (companystatecode != coustomerstatecode.ToString())
                            {

                                Ship_igst = (Convert.ToDouble(Convert.ToDouble(person.shipcharge) * 18) / (100 + 18)); ;
                                Ship_cgst = 0;
                                Ship_sgst = 0;


                            }
                        }

                      msg =  Db.Insert_orderdetails(0, person.productid, person.quantity, person.productprice, Convert.ToDouble(person.totalprice), customerid, companyid.ToString(), person.itemdetid, person.shipcharge, taxper_CGST, taxper_SGST, taxper_IGST, CGSTAmt, SGSTAmt, IGSTAmt, Ship_cgst, Ship_sgst, Ship_igst);


                        if (msg == true)
                        {

                            DataTable dt1 = util.execQuery("select top(1) a.order_no from tblorder a order by a.order_id desc");

                            StringBuilder sBody = new StringBuilder();
                            string MailStatus = "";
                            var DateTime = new DateTime();
                            DateTime = DateTime.Now;
                            sBody.Append("<!DOCTYPE html><html><body>");
                            sBody.Append("<p>Dear " + HttpContext.Session.GetString("customerfirstname") +" "+ HttpContext.Session.GetString("customerlastname") + Environment.NewLine);
                            sBody.Append("<p>Thank you for placing your order with us. Your temporary order number is " + dt1.Rows[0]["order_no"] + " " + Environment.NewLine);
                            //sBody.Append("<p>User Name: " + email + "" + Environment.NewLine);
                            //sBody.Append("<p>Contact Info: " + Mobile + "" + Environment.NewLine);
                            //sBody.Append("<p>Email: " + email + "" + Environment.NewLine);
                            sBody.Append("<p>" + Environment.NewLine);

                            sBody.Append("<p>As a transparent and ethical company, we are committed to delivering the best to our valuable clients. To ensure complete clarity and satisfaction, a member of our team will contact you via phone or an online meeting to reconfirm your order." + Environment.NewLine);
                            sBody.Append("<p>" + Environment.NewLine);

                            sBody.Append("<p>During this discussion, we will review the product details to ensure you have a thorough understanding before reconfirming your order. Please note that:" + Environment.NewLine);
                            sBody.Append("<p>" + Environment.NewLine);

                            sBody.Append("<p>This order is currently in temporary status, and no payment has been processed yet." + Environment.NewLine);
                            sBody.Append("<p>After reconfirmation, you will receive a final confirmation email with payment details." + Environment.NewLine);
                            sBody.Append("<p>We require 100% advance payment, and refunds are not available, so feel free to ask any questions or clarify any doubts during the process." + Environment.NewLine);

                            sBody.Append("<p>Once payment is received, we will ship your product within 24 hours and provide the courier details, including the AWB number, for tracking purposes." + Environment.NewLine);
                            sBody.Append("<p>If you have any further queries, please feel free to contact us." + Environment.NewLine);
                            // sBody.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
                            sBody.Append("<p>Regards," + Environment.NewLine);
                           // sBody.Append("<p>" + HttpContext.Session.GetString("customerfirstname") + " " + HttpContext.Session.GetString("customerlastname") + Environment.NewLine);
                            sBody.Append("<p>Team BSD" + Environment.NewLine);
                            sBody.Append("</body></html>");
                            MailStatus = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + HttpContext.Session.GetString("customeremail") + "", "", "", "Subject :Temporary Order Confirmation -  " + dt1.Rows[0]["order_no"] + "", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");
                        //}
                        //if (msg == true)
                        //{

                            StringBuilder sBody1 = new StringBuilder();
                            string MailStatus1 = "";
                           //var DateTime = new DateTime();
                            DateTime = DateTime.Now;
                            sBody1.Append("<!DOCTYPE html><html><body>");
                            sBody1.Append("<p>Dear Team," + Environment.NewLine);
                            sBody1.Append("<p>A temporary order has been placed with the following details: " + Environment.NewLine);
                            sBody1.Append("<p>Customer Name : " + HttpContext.Session.GetString("customerfirstname") + " " + HttpContext.Session.GetString("customerlastname") + Environment.NewLine);
                            sBody1.Append("<p>Temporary Order Number : " + dt1.Rows[0]["order_no"] + " " + Environment.NewLine);
                            sBody1.Append("<p>Please note the following steps to proceed: " + Environment.NewLine);
                            sBody1.Append("<p>Contact the customer via phone or an online meeting to reconfirm the order and discuss product details." + Environment.NewLine);
                            sBody1.Append("<p>Ensure the customer fully understands the order details and addresses any questions or concerns." + Environment.NewLine);
                            sBody1.Append("<p>Inform the customer that no payment has been taken at this stage and that the order remains in a temporary status." + Environment.NewLine);
                            sBody1.Append("<p>Once reconfirmed, send the final confirmation email with payment details.Important:" + Environment.NewLine);
                            sBody1.Append("<p>Payment is 100% advance, and refunds are not available." + Environment.NewLine);
                            sBody1.Append("<p>Upon receiving payment, ship the product within 24 hours and provide courier details, including the AWB number, for tracking." + Environment.NewLine);
                            //sBody1.Append("<p> Customer Bank Details " + Environment.NewLine);
                            //sBody1.Append("<p>Bank name: " + return_bank_name + ", " + Environment.NewLine);
                            //sBody1.Append("<p>IFSC name: " + return_IFSC_code + ", " + Environment.NewLine);
                            //sBody1.Append("<p>Account No: " + return_Account_no + ", " + Environment.NewLine);
                            //sBody1.Append("<p>Account Holder name: " + return_Account_holder_name + ", " + Environment.NewLine);
                            sBody1.Append("<p>" + Environment.NewLine);
                            sBody1.Append("<p>Let’s ensure a smooth and transparent process for the customer." + Environment.NewLine);
                            sBody1.Append("<p>Regards</p>");
                            sBody1.Append("<p><b>Team BSD</b></p>");
                            sBody1.Append("</body></html>");
                            string email1 = "bsddemos@gmail.com";
                            MailStatus1 = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject :Temporary Order Confirmation -  " + dt1.Rows[0]["order_no"] + "", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");

                            message = "order place save.";
                        }

                        //else
                        //{
                        //    message = "order not place save.";
                        //}


                    }

                    message = "order place save.";
                }
                
            }

           




            var Data = new { message = message };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult checkOrderplacebypincode(string pincode)
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataTable dt = new DataTable();
            dt = Db.CheckorderbyPincode(pincode, companyid);
            if (dt.Rows.Count > 0)
            {
                message = "This pincode available for delivery.";
            }
            else
            {
                message = "This pincode not available for delivery.";
            }
            var Data = new { message = message };
            return Json(Data);
        }
        [HttpGet]
        [Route("/thankyou")]
        public IActionResult ThankYou()
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            HttpContext.Session.SetString("CartItems", "");
            HttpContext.Session.SetInt32("count", 0);
            DataTable dt = util.execQuery(" SELECT TOP 1 order_no FROM tblorder where companyid = " + companyid + " ORDER BY order_id DESC ");





            if (dt.Rows.Count > 0)
            {
                ViewBag.orderno = dt.Rows[0]["order_no"];
            }

            return View();
        }
        //[HttpPost]
        //public IActionResult proceedcheckout()
        //{
        //    if (string.IsNullOrEmpty(HttpContext.Session.GetString("customerid")))
        //    {
        //        message = "Please Login.";
        //    }
        //    var Data = new { message = message };
        //    return Json(Data);
        //}

        [HttpGet]
        [Route("Invoice/{id}")]
        public IActionResult Invoice()
        {
            //int order_id = Convert.ToInt32(HttpContext.Session.GetInt32("order_id"));
            int companyid = Convert.ToInt32(HttpContext.Session.GetInt32("SiteId"));
            int order_id = Convert.ToInt32(RouteData.Values["id"]);
            DataSet dsinvoice = Db.PintInvoice(order_id, companyid);
            List<InvoiceModel> invoiceModels = new List<InvoiceModel>();
            InvoiceModel im = new InvoiceModel();
            if (dsinvoice.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsinvoice.Tables[0].Rows)
                {
                    im.orderdate = Convert.ToString(dr["order_date"]);
                    im.paymentmode = Convert.ToString(dr["payment_mode"]);
                    //im.Subscription = Convert.ToString(dr["Subscription"]);
                    //im.SubRs = Convert.ToString(dr["SubRs"]);
                    im.subtotal = Convert.ToDouble(dr["subtotal"]);
                    im.discount = Convert.ToDouble(dr["discount"]);
                    im.amount = Convert.ToDouble(dr["amount"]);
                    // im.Walletamtdetect = Convert.ToDouble(dr["Walletamtdetect"]);
                    //im.reference = Convert.ToString(dr["reference"]);
                    im.sname = Convert.ToString(dr["name"]);
                    im.saddress1 = Convert.ToString(dr["address1B"]);
                    im.saddress2 = Convert.ToString(dr["address2B"]);
                    im.sCountry = Convert.ToString(dr["country"]);
                    im.sstate = Convert.ToString(dr["State"]);
                    im.scity = Convert.ToString(dr["City"]);
                    im.sphone = Convert.ToString(dr["mobile"]);
                    im.semail = Convert.ToString(dr["email"]);
                    im.sPincode = Convert.ToString(dr["zipcode"]);
                    im.bname = Convert.ToString(dr["nameB"]);
                    im.baddress1 = Convert.ToString(dr["address1B"]);
                    im.baddress2 = Convert.ToString(dr["address2B"]);
                    im.bcountry = Convert.ToString(dr["country"]);
                    im.bstate = Convert.ToString(dr["StateB"]);
                    im.bcity = Convert.ToString(dr["CityB"]);
                    im.bphone = Convert.ToString(dr["mobileB"]);
                    im.bemail = Convert.ToString(dr["emailB"]);
                    im.bPincode = Convert.ToString(dr["zipcodeB"]);
                    im.companyaddress = Convert.ToString(dr["companyaddress"]);
                    im.companyname = Convert.ToString(dr["CompanyName"]);
                    im.CompanyEmail = Convert.ToString(dr["CompanyEmail"]);
                    im.CompanyPhone = Convert.ToString(dr["CompanyPhone"]);
                    im.CompanyCityName = Convert.ToString(dr["CompanyCityName"]);
                    im.CompanyGstNo = Convert.ToString(dr["CompanyGstNo"]);
                    im.CompanyLogo = Convert.ToString(dr["CompanyLogo"]);
                }
            }
            if (dsinvoice.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in dsinvoice.Tables[1].Rows)
                {
                    invoiceModels.Add(new InvoiceModel
                    {
                        itemid = Convert.ToInt32(dr["ITEMID"]),
                        itemname = Convert.ToString(dr["ITEM"]),
                        Qty = Convert.ToInt32(dr["QTY"]),
                        Taxable_Value = Convert.ToInt16(dr["Taxable_Value"]),
                        SGSTAmt = Convert.ToDecimal(dr["SGSTAmt"]),
                        CGSTAmt = Convert.ToDecimal(dr["CGSTAmt"]),
                        IGSTAmt = Convert.ToDecimal(dr["IGSTAmt"]),
                        discount = Convert.ToInt32(dr["Discount"]),
                        amount = Convert.ToDouble(dr["amount"]),
                    });
                }
            }
            if (dsinvoice.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow dr in dsinvoice.Tables[2].Rows)
                {
                    im.DispatchDate = Convert.ToString(dr["DispatchDate"]);
                    im.status = Convert.ToInt32(dr["status"]);
                }
            }
            ViewBag.invoiceModels = invoiceModels;
            return View(im);
        }

        [HttpPost]
        public JsonResult returnorder(int id, int return_type, string return_Resion, string return_bank_name, int return_IFSC_code, int return_Account_no, string return_Account_holder_name)
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            string customerid = HttpContext.Session.GetString("customerid");
            sqlquery = "update tblorder set status_flg=6,Return_User='" + customerid + "', Resion='" + return_Resion + "',Return_Type=" + return_type + ",return_date=getdate(),Bank_name='" + return_bank_name + "',IFSC_code='" + return_IFSC_code + "',Account_no='" + return_Account_no + "',Account_holder_name='" + return_Account_holder_name + "' where  order_id=" + id + " and  companyid=" + companyid + " ";
            status = util.MultipleTransactions(sqlquery);



          //  DataTable dt = util.execQuery("select * from tblorder where user_id ='" + HttpContext.Session.GetString("customerid") + "' and order_id = '" + id + "'");

            //DataTable dt1 = util.execQuery("select  a.order_no ,a.first_name , a.last_name, a.email, d.Reasion , c.itemname, b.item_id from tblorder a join tblorderdetail b on a.order_id = b.order_id join tblItemStore c on b.item_id = c.id join tblResionofCancellation d on d.id = a.return_type  where a.order_id ='" + id + "'");




            if (status == "Successfull")
            {



                DataTable dt1 = util.execQuery("select c.*, a.ItemName from tblItemStore   a join tblorderdetail b on a.ID=b.item_id  join tblorder c on b.order_id=c.order_id where c.order_id = '" + id + "'");







                //StringBuilder sBody = new StringBuilder();
                //string MailStatus = "";
                //var DateTime = new DateTime();
                //DateTime = DateTime.Now;
                //sBody.Append("<!DOCTYPE html><html><body>");
                //sBody.Append("<p>Dear " + dt1.Rows[0]["first_name"] + " " + dt1.Rows[0]["last_name"] + "," + Environment.NewLine);
                //sBody.Append("<p>" + Environment.NewLine);
                //sBody.Append("<p>We have received your request to return the following order:" + Environment.NewLine);
                //// sBody.Append("<h3>Order Details:</h3>");
                //sBody.Append("<p>Order Number: " + dt1.Rows[0]["order_no"] + "</p>");
                //sBody.Append("<p>Order Return Date:" + dt1.Rows[0]["return_date"] + "</p>");
                //sBody.Append("<p>Product Name: " + dt1.Rows[0]["ItemName"] + "</p>");
                //// sBody.Append("<p>Tracking Number:  " + dt1.Rows[0]["cour_no"] + "</p>");
                ////sBody.Append("<p>User Name: " + email + "" + Environment.NewLine);
                ////sBody.Append("<p>Contact Info: " + Mobile + "" + Environment.NewLine);
                ////sBody.Append("<p>Email: " + email + "" + Environment.NewLine);
                ////sBody.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
                //// sBody.Append("<p>Track your shipment here:" + Environment.NewLine);
                ////  sBody.Append("<p>[Tracking Link]" + Environment.NewLine);
                //sBody.Append("<p>Your return has been successfully processed. If a refund or replacement is applicable, it will be initiated as per our return policy: </p>");
                //sBody.Append("<p>Refund Processing Time: [3-5] business days (if applicable).</ p>");
                //sBody.Append("<p>Replacement Details: Under Process.</ p>");

                //sBody.Append("<p>If you have any further questions or concerns, please feel free to contact us at support@bsdinfotech.com or 9871092024.<p>");
                //sBody.Append("<p>Thank you for choosing BSD Online Store.</p>");
                //sBody.Append("<p>Best Regards</p>");
                //sBody.Append("<p><b>Team BSD</b></p>");
                //sBody.Append("</body></html>");
                //MailStatus = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + dt1.Rows[0]["email"] + "", "", "", "Subject : Order Return Confirmation", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");




                StringBuilder sBody1 = new StringBuilder();
                string MailStatus1 = "";
                // var DateTime = new DateTime();
                //  DateTime = DateTime.Now;
                sBody1.Append("<!DOCTYPE html><html><body>");
                sBody1.Append("<p>Dear Team," + Environment.NewLine);
                sBody1.Append("<p>The following order has been returned by the customer:" + Environment.NewLine);
                sBody1.Append("<p>Customer Name:" + dt1.Rows[0]["first_name"] + " " + dt1.Rows[0]["last_name"] + "," + Environment.NewLine);
                sBody1.Append("<p>Order Number: " + dt1.Rows[0]["order_no"] + "" + Environment.NewLine);
                sBody1.Append("<p>Order Return  Date:" + dt1.Rows[0]["return_date"] + "</p>");
                sBody1.Append("<p>Product Name: " + dt1.Rows[0]["ItemName"] + "</p>");
                sBody1.Append("<p>" + Environment.NewLine);
                sBody1.Append("<p>Please ensure the returned product is inspected, and the next steps are carried out as per our return policy:" + Environment.NewLine);
                sBody1.Append("<p>Refund/Replacement: Initiate the process promptly.</p>");
                sBody1.Append("<p>Customer Notification: Inform the customer once the process is complete.</p>");
                sBody1.Append("<p>For any discrepancies or issues, coordinate with the relevant team and keep the customer informed.</p>");
                sBody1.Append("<p>Best Regards</p>");
                sBody1.Append("<p><b>Team BSD</b></p>");
                sBody1.Append("</body></html>");
                string email1 = "bsddemos@gmail.com";
                MailStatus1 = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject :Order Return Notification", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");









                message = "Your Order Return Successfully.";
            }

            else
            {
                message = "Your Return not Cancel.";
            }
            var Data = new { message = message };
            return Json(Data);
        }
        [HttpPost]
        public JsonResult cancelorder(int id, int cancel_type, string cancel_Resion)
        {

            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            string customerid = HttpContext.Session.GetString("customerid");
            DataTable dt = util.execQuery("select * from tblorder where user_id ='" + HttpContext.Session.GetString("customerid") + "' and order_id = '"+id+"'");

           sqlquery = "update tblorder set status_flg=5,Cancelled_User=" + customerid + ",cal_date=getdate(),cancel_type=" + cancel_type + ",cancel_Resion='" + cancel_Resion + "' where  order_id=" + id + " and  companyid=" + companyid + " ";
            status = util.MultipleTransactions(sqlquery);


            



            if (status == "Successfull")
            {

                DataTable dt1 = util.execQuery("select c.*, a.ItemName from tblItemStore   a join tblorderdetail b on a.ID=b.item_id  join tblorder c on b.order_id=c.order_id where c.order_id = '" + id + "'");







               // StringBuilder sBody = new StringBuilder();
               // string MailStatus = "";
               // var DateTime = new DateTime();
               // DateTime = DateTime.Now;
               // sBody.Append("<!DOCTYPE html><html><body>");
               // sBody.Append("<p>Dear " + dt1.Rows[0]["first_name"] + " " + dt1.Rows[0]["last_name"] + "," + Environment.NewLine);
               // sBody.Append("<p>" + Environment.NewLine);
               // sBody.Append("<p>We regret to inform you that your order with the following details has been cancelled:" + Environment.NewLine);
               //// sBody.Append("<h3>Order Details:</h3>");
               // sBody.Append("<p>Order Number: " + dt1.Rows[0]["order_no"] + "</p>");
               // sBody.Append("<p>Order Cancellation  Date:" + dt1.Rows[0]["cal_date"] + "</p>");
               // sBody.Append("<p>Product Name: " + dt1.Rows[0]["ItemName"] + "</p>");
               //// sBody.Append("<p>Tracking Number:  " + dt1.Rows[0]["cour_no"] + "</p>");
               // //sBody.Append("<p>User Name: " + email + "" + Environment.NewLine);
               // //sBody.Append("<p>Contact Info: " + Mobile + "" + Environment.NewLine);
               // //sBody.Append("<p>Email: " + email + "" + Environment.NewLine);
               // //sBody.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
               // // sBody.Append("<p>Track your shipment here:" + Environment.NewLine);
               // //  sBody.Append("<p>[Tracking Link]" + Environment.NewLine);
               // sBody.Append("<p>If this cancellation was initiated by you, no further action is required. However, if this was unexpected, please contact us immediately at support@bsdinfotech.com or 9871092024. </p>");
               // sBody.Append("<p>If you have already made any payment, please allow 4-5 business days for the refund (if applicable) to be processed.</p>");
               // sBody.Append("<p>We apologize for any inconvenience caused and hope to serve you again in the future.</p>");
               // sBody.Append("<p>Best Regards</p>");
               // sBody.Append("<p><b>Team BSD</b></p>");
               // sBody.Append("</body></html>");
               // MailStatus = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + dt1.Rows[0]["email"] + "", "", "", "Subject : Order Cancellation Confirmation", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");




                StringBuilder sBody1 = new StringBuilder();
                string MailStatus1 = "";
                // var DateTime = new DateTime();
                //  DateTime = DateTime.Now;
                sBody1.Append("<!DOCTYPE html><html><body>");
                sBody1.Append("<p>Dear Team," + Environment.NewLine);
                sBody1.Append("<p>The following order has been cancelled:" + Environment.NewLine);
                sBody1.Append("<p>Customer Name:" + dt1.Rows[0]["first_name"] + " " + dt1.Rows[0]["last_name"] + "," + Environment.NewLine);
                sBody1.Append("<p>Order Number: " + dt1.Rows[0]["order_no"] + "" + Environment.NewLine);
                sBody1.Append("<p>Order Cancellation  Date:" + dt1.Rows[0]["cal_date"] + "</p>");
                sBody1.Append("<p>Product Name: " + dt1.Rows[0]["ItemName"] + "</p>");
                sBody1.Append("<p>" + Environment.NewLine);
                sBody1.Append("<p>Please take the necessary steps to update the order status in our system. If payment was already received, ensure the refund process is initiated (if applicable) and notify the customer accordingly." + Environment.NewLine);
                sBody1.Append("<p>For any further clarification or assistance, contact the customer at " + dt1.Rows[0]["mobile"] + ".</p>");
                sBody1.Append("<p>Best Regards</p>");
                sBody1.Append("<p><b>Team BSD</b></p>");
                sBody1.Append("</body></html>");
                string email1 = "bsddemos@gmail.com";
                MailStatus1 = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : Order Cancellation Notification", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");


                message = "Your Order Cancel Successfully.";
            }

            //else
            //{
            //    message = "Table not modify successfully.";
            //}
            ////else
            //{
            //    message = "Your Order not Cancel.";
            //}
            var Data = new { message = message };
            return Json(Data);
        }
        #region DropDown
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
        public IActionResult BindState()
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet dsstate = util.BindDropDown("select id,state_name from tblstate where companyid='" + companyid + "' and status=1");
            List<SelectListItem> State = new List<SelectListItem>();
            foreach (DataRow dr in dsstate.Tables[0].Rows)
            {
                State.Add(new SelectListItem { Text = dr["state_name"].ToString(), Value = dr["id"].ToString() });
            }
            ViewBag.State = State;
            return View(State);
        }
        public IActionResult BindCity()
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet dscity = new DataSet();
            dscity = util.BindDropDown("select id,city from tblcity where companyid='" + companyid + "' and status=1");
            List<SelectListItem> city = new List<SelectListItem>();
            foreach (DataRow dr in dscity.Tables[0].Rows)
            {
                city.Add(new SelectListItem { Text = dr["city"].ToString(), Value = dr["id"].ToString() });
            }
            ViewBag.city = city;
            return View(city);
        }

        #endregion
        [NonAction]
        public void GetSiteNameAndCode()
        {
            string url = Request.Host.ToString();
            //string url = "www.indiastatdistrictagri.com";
            string dfds = Request.PathBase;
            if (url.ToLower().Contains("localhost"))
            {
                DataTable dtSite = Db.GetDomain("wellnesstillulast");
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
            else if (url.ToLower().Contains("wellnesstillulast"))
            {

                DataTable dtSite = Db.GetDomain("wellnesstillulast");
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
                DataTable dtSite = Db.GetDomain("wellnesstillulast");
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
