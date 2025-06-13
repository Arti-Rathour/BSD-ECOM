using BSD_ECOM.Areas.Admin.Models;
using BSD_ECOM.Models;
using BSD_ECOM.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSD_ECOM.Controllers
{
    public class Customer : Controller
    {
        ClsUtility util = new ClsUtility();
        DataBaseAccess Db = new DataBaseAccess();
        IndexViewModel viewmodel = new IndexViewModel();
        string message = "";
        int cart = 0;
        string sqlquery, status;
        [HttpGet]
        [Route("Login/{id?}")]
        public IActionResult Login(string id)
        {

            ViewBag.companyid = HttpContext.Session.GetInt32("SiteId").ToString();


            ViewBag.type = id;


            return View();
        }
        [NonAction]
        public void GetSiteNameAndCode()
        {
            string url = Request.Host.ToString();
            //string url = "www.indiastatdistrictagri.com";
            string dfds = Request.PathBase;
            if (url.ToLower().Contains("localhost"))
            {
                HttpContext.Session.SetString("SiteName", "bsdecom");
                HttpContext.Session.SetInt32("SiteId", 6);
                HttpContext.Session.SetString("Logo", "/assets/imgs/theme/logo.svg");
                //HttpContext.Session.SetInt32("SiteId", 0);
            }
            else if (url.ToLower().Contains("wellnesstillulast"))
            {
                HttpContext.Session.SetString("SiteName", "wellnesstillulast");
                HttpContext.Session.SetInt32("SiteId", 6);
                HttpContext.Session.SetString("Logo", "/assets/imgs/theme/logo.svg");
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
                DataTable dtSite = Db.GetDomain(domainName);
                if (dtSite.Rows.Count > 0)
                {
                    HttpContext.Session.SetString("SiteName", dtSite.Rows[0]["SiteName"].ToString());
                    HttpContext.Session.SetInt32("SiteId", Convert.ToInt32(dtSite.Rows[0]["SiteCode"]));
                }
                else
                {
                    HttpContext.Session.SetInt32("SiteType", 6);
                    HttpContext.Session.SetInt32("SiteId", 6);
                }
            }
        }

        [HttpGet]
        [Route("Customer/ForgotPassword/{id}")]
        public IActionResult ForgotPassword( string id )
        {
            ViewBag.id = id;
            return View();
        }


        #region Forgot Password
       
       public JsonResult ForgotPasswordid(string email)
       {
         StringBuilder sBody = new StringBuilder();
         string MailStatus = "";

          
            sqlquery = "select * from tblregistration  where email ='" + email + "'";
           DataSet ds = util.TableBind(sqlquery);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                var id = dt.Rows[0]["email"];

                sBody.Append("<!DOCTYPE html><html><body>");

                sBody.Append("<p>Dear <b>" + dt.Rows[0]["first_name"] + " " + dt.Rows[0]["last_name"] + "</b></p>");
                sBody.Append("<p>Greetings from BSD Online Store!\r\n</p>");
                //sBody.Append("<p>We received a request to reset your password. Click the link below to set a new password:</p>");
                sBody.Append("<p>Thank you for reaching out to retrieve your lost password.\r\n</p>");
                sBody.Append("<p>Here are your access details:\r\n\r\n</p>");
                //sBody.Append("<a href='https://localhost:44345/Customer/ForgotPassword/{id}'>Click here</a>");
                //sBody.Append($"<a href='https://localhost:44345/Customer/ForgotPassword/{id}'>Click here</a>");


                sBody.Append("<p>User Name: <b>" + dt.Rows[0]["email"] + "</b></p>");
                sBody.Append("<p>Password: <b>" + util.Decryption(dt.Rows[0]["password"].ToString()) + "</b></p>");
                sBody.Append("<p>For security reasons, we strongly recommend changing your password immediately upon accessing your account.</p>");
                sBody.Append("<p>If you have any questions, feel free to contact us at support@bsdinfotech.com.\r\n</p>");
                sBody.Append("<p>Best Regards </p>");
                sBody.Append("<p><b>Team BSD</b> <br>");
                sBody.Append("</body></html>");
                MailStatus = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email + "", "", "", "Subject : Password Retrieval Request", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");
              message = "Your password Retrieval successfully";




                StringBuilder sBody1 = new StringBuilder();
                string MailStatus1 = "";

                sBody1.Append("<!DOCTYPE html><html><body>");
                sBody1.Append("<p>Dear Team," + Environment.NewLine);
                sBody1.Append("<p> The following password retrieval request has been processed successfully:\r\n " + Environment.NewLine);
                sBody1.Append("<p>Customer Name: <b>" + dt.Rows[0]["first_name"] + " " + dt.Rows[0]["last_name"] + "</b></p>");
                sBody1.Append("<p>Username: <b>" + dt.Rows[0]["email"] + "</b></p>");
                sBody1.Append("<p>Temporary  Password: <b>" + util.Decryption(dt.Rows[0]["password"].ToString()) + "</b></p>");
                sBody1.Append("<p>" + Environment.NewLine);
                sBody1.Append("<p>Please monitor the account for any potential security issues and ensure the user changes their password upon login." + Environment.NewLine);
                sBody1.Append("<p>For any concerns, contact support@bsdinfotech.com." + Environment.NewLine);
                sBody1.Append("<p>Best Regards</p>");
                sBody1.Append("<p><b>Team BSD</b></p>");
                sBody1.Append("</body></html>");
                string email1 = "bsddemos@gmail.com";
                MailStatus1 = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject :Password Retrieval Notification", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");


            }
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    DataTable  dt = ds.Tables[0];

            //     var id = dt.Rows[0]["email"];

            //     sBody.Append("<!DOCTYPE html><html><body>");

            //       sBody.Append("<p>Dear <b>" + dt.Rows[0]["first_name"]  + " "  + dt.Rows[0]["last_name"] + "</b></p>");
            //       sBody.Append("<p>We received a request to reset your password.</p>");
            //       //sBody.Append("<p>We received a request to reset your password. Click the link below to set a new password:</p>");
            //        sBody.Append("<p>Click the link below to set a new password.</p>");
            //     //sBody.Append("<a href='https://localhost:44345/Customer/ForgotPassword/{id}'>Click here</a>");
            //     sBody.Append($"<a href='https://localhost:44345/Customer/ForgotPassword/{id}'>Click here</a>");


            //     //sBody.Append("<p>Password: <b>" + util.Decryption(dt.Rows[0]["password"].ToString()) + "</b></p>");
            //     sBody.Append("<p>If you did not request a password reset, please ignore this email or contact support.</p>");
            //       sBody.Append("<p>Thank you, </p>");
            //       sBody.Append("<p><b>The BSD Infotech Online Store Team</b> <br>");
            //       sBody.Append("</body></html>");
            //      MailStatus = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email + "", "", "", "Subject : Forgot Password", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");

            //     message = "Mail send sucussfully for change the password";

            // }
            else
            {
              message = "Sorry ! This email is not registered";
            }

            var data = new { message = message };
          return Json(data);
       }
       #endregion

        [HttpPost]
        public JsonResult IChangePassworddd(string email, string NewPassword, string ConfirmPassword)
        {
           
                if (NewPassword == ConfirmPassword)
                {
               // message = ("update tblregistration set password='" + util.cryption(NewPassword) + "' where reg_id='" + customerid + "'");
                sqlquery = "update tblregistration set password='" + util.cryption(ConfirmPassword) + "' where email='" + email + "'";
                  string  Status = util.MultipleTransactions(sqlquery);
                    if (Status == "Successfull")
                    {
                        message = "Password Changed Successfully";
                    }
                }
                else
                {
                    message = "New Password nad Confirm Password do not match";
                }
            
            var Data = new { message = message };
            return Json(Data);
        }

        [HttpPost]
        public IActionResult CustomerLogin(string email, string mobile, string password)
        {

            GetSiteNameAndCode();
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataTable dtlogin = Db.CustomerLogin(email, mobile, password, companyid);            
            if (dtlogin.Rows.Count > 0)
            {
                //HttpContext.Session.SetString("customerid", dtlogin.Rows[0]["reg_id"].ToString());
                //HttpContext.Session.SetString("customerfirstname", dtlogin.Rows[0]["first_name"].ToString());
                //HttpContext.Session.SetString("customerlastname", dtlogin.Rows[0]["last_name"].ToString());
                //HttpContext.Session.SetString("customeremail", dtlogin.Rows[0]["email"].ToString());
                //HttpContext.Session.SetString("customerMobileNo", dtlogin.Rows[0]["MobileNo"].ToString());


                HttpContext.Session.SetString("customerid", dtlogin.Rows[0]["reg_id"].ToString());
                HttpContext.Session.SetString("customerfirstname", dtlogin.Rows[0]["first_name"].ToString());
                HttpContext.Session.SetString("customerlastname", dtlogin.Rows[0]["last_name"].ToString());
                HttpContext.Session.SetString("customeremail", dtlogin.Rows[0]["email"].ToString());
                HttpContext.Session.SetString("StateID", dtlogin.Rows[0]["StateID"].ToString());
                HttpContext.Session.SetString("State", dtlogin.Rows[0]["state_name"].ToString());
                HttpContext.Session.SetString("Cityid", dtlogin.Rows[0]["CityID"].ToString());
                HttpContext.Session.SetString("CityName", dtlogin.Rows[0]["City"].ToString());
                HttpContext.Session.SetString("customerMobileNo", dtlogin.Rows[0]["MobileNo"].ToString());

                message = "Success Login.";

            }
            else
            {
                message = "Invalid User.";
            }
            var Data = new { message = message };
            return Json(Data);
        }
        public JsonResult otp(string emailmobile)
        {
            GetSiteNameAndCode();
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataTable dt = new DataTable();
            string Message = string.Empty;
            string genNum = string.Empty;
            dt = Db.OTPSEND(emailmobile, companyid);
            if (dt.Rows.Count > 0)
            {
                Random _rdm = new Random();
                genNum = _rdm.Next(1000, 9999).ToString();
                Message = "Success..!";
                HttpContext.Session.SetString("otpno", genNum);

                long number1 = 0;
                bool canConvert = long.TryParse(emailmobile, out number1);
                if (canConvert == true)
                {
                    HttpContext.Session.SetString("mnumber", emailmobile);
                    Db.SMSAPInewwithmsg("http://103.16.101.52:8080/sendsms/bulksms?username=bsd-bsdinf&password=bsd123&type=0&dlr=1&destination=918587068357&source=BSDINF&message=Your Registration OTP is :" + genNum + " BSD Infotech&entityid=1001834919579515999&tempid=1107160396362855775");
                    message = "OTP";

                  //  Db.SMSAPInewwithmsg("http://103.16.101.52:8080/bulksms/bulksms?username=ic12-maclifesty&password=Mac854uj&type=0&dlr=1&destination=91" + emailmobile + "&source=MACOTP&message=Dear customer, your OTP to access Mac portal is " + genNum + ". Please use this number to validate your login.&entityid=1201162670017427842&tempid=1207162744618053320");
                }
                else
                {
                    string domainname = HttpContext.Session.GetString("SiteName");
                    util.SendMailViaIIS_html(emailmobile, emailmobile, "", "none", "'" + domainname + "':", "", "Dear customer, your OTP to access "+ domainname + " portal is " + genNum + ". Please use this number to validate your login", null, "");
                    //Console.WriteLine("numString is not a valid long");
                }
            }
            else
            {
                message = "This email or phone is not register with us ";
            }
            var Data = new { message = message,opt= genNum };
            return Json(Data);
        }
        [HttpPost]
        public IActionResult otpverfiy(string otp)
        {
            GetSiteNameAndCode();
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            string mobiemail = HttpContext.Session.GetString("mnumber");
            DataTable dtlogin = Db.CustomerLoginverfy(mobiemail, companyid);
            if (dtlogin.Rows.Count > 0)
            {
                HttpContext.Session.SetString("customerid", dtlogin.Rows[0]["reg_id"].ToString());
                HttpContext.Session.SetString("customerfirstname", dtlogin.Rows[0]["first_name"].ToString());
                HttpContext.Session.SetString("customerlastname", dtlogin.Rows[0]["last_name"].ToString());
                HttpContext.Session.SetString("customeremail", dtlogin.Rows[0]["email"].ToString());
                HttpContext.Session.SetString("customerMobileNo", dtlogin.Rows[0]["MobileNo"].ToString());
                message = "Success Login.";
            }
            else
            {
                message = "Invalid User.";
            }
            var Data = new { message = message };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult CustomerRegistration(int id, string firstname, string lastname, string email, string password, string Mobile, string companyid )
        {
            string message1 = "";
            DataTable dt = util.execQuery("select * from tblregistration where email='"+email+ "' or MobileNo='"+Mobile+"'");
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["MobileNo"].ToString()==Mobile)
                {
                    message1 = "This Mobile number already exit.";
                }
                else if(dt.Rows[0]["email"].ToString()==email)
                {
                    message1 = "This Email Id already exit.";
                }
                else
                {
                    message1 = "This Email id and Mobile number already exit.";
                }
            }
            else
            {
                if (id == 0)
                {
                    if (Db.CustomerRegistraion(util.FixQuotes(firstname.Trim()), util.FixQuotes(lastname.Trim()), util.FixQuotes(email.Trim()), util.cryption(password.Trim()), util.FixQuotes(Mobile.Trim()), util.FixQuotes(companyid.Trim())))
                    {
                        message = "Registration Success.";
                    }
                    if (message == "Registration Success.")
                    {
                        StringBuilder sBody = new StringBuilder();
                        string MailStatus = "";
                        var DateTime = new DateTime();
                        DateTime = DateTime.Now;
                        sBody.Append("<!DOCTYPE html><html><body>");
                        sBody.Append("<p>Dear ," + HttpContext.Session.GetString("customerfirstname") + HttpContext.Session.GetString("customerlastname") + Environment.NewLine);
                        sBody.Append("<p>Thank you for registering with BSD Online Store! We’re thrilled to have you on board" + Environment.NewLine);
                        sBody.Append("<p>You can now explore our products, services, and exclusive offers designed to meet your needs. Here are your registration details:" + Environment.NewLine);
                        sBody.Append("<p>User Name: " + email + "" + Environment.NewLine);
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
                        MailStatus = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email + "", "", "", "Subject : Welcome to BSD INFOTECH – Registration Successful!", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");

                    }
                    if (message == "Registration Success.")
                    {
                        StringBuilder sBody1 = new StringBuilder();
                        string MailStatus1 = "";
                        var DateTime = new DateTime();
                        DateTime = DateTime.Now;
                        sBody1.Append("<!DOCTYPE html><html><body>");
                        sBody1.Append("<p>Dear Team," + Environment.NewLine);
                        sBody1.Append("<p>A new user has successfully registered on our platform. Below are the details: " + Environment.NewLine);
                        sBody1.Append("<p>User Name: " + email + "" + Environment.NewLine);
                        sBody1.Append("<p>Password: " + password + ", " + Environment.NewLine);
                        sBody1.Append("<p>Date and Time: " + DateTime + "" + Environment.NewLine);
                        sBody1.Append("<p>Contact Info: " + Mobile + "" + Environment.NewLine);
                        sBody1.Append("<p>" + Environment.NewLine);
                        sBody1.Append("<p>Please ensure their account setup is complete and ready for use. If there are any pending verifications or actions required, kindly address them promptly." + Environment.NewLine);
                        sBody1.Append("<p>Let’s provide them with the best experience!</p>");
                        sBody1.Append("<p>Best Regards,</p>");
                        sBody1.Append("<p><b>BSD Team</b></p>");
                        sBody1.Append("</body></html>");
                        string email1 = "bsddemos@gmail.com";
                        MailStatus1 = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject : New User Registration Notification", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");

                    }
                }
                else
                {
                    message = "Some thing wrong.";
                }
            }
            var Data = new { message = message,email= email ,password= password , message1 = message1 };
            return Json(Data);         
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
        [HttpGet]
        [Route("CustomerAccount")]
        public IActionResult CustomerAccount()
        {
            PopulateResion();
            customer cs = new customer();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("customerid")))
            {
                return RedirectToAction("/Login");
            }
            else
            {
                string customerid = HttpContext.Session.GetString("customerid");
                //string customerid = "5";
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
                        ViewBag.cuntid = Convert.ToString(dr["CountryID"]);
                        ViewBag.StateID = Convert.ToString(dr["StateID"]);
                        ViewBag.citid = Convert.ToString(dr["CityID"]);

                        ViewBag.cuntid1 = Convert.ToString(dr["Shi_CountryID"]);
                        ViewBag.StateID1 = Convert.ToString(dr["Shi_StateID"]);
                        ViewBag.citid1 = Convert.ToString(dr["Shi_CityID"]);



                    }
                }
                    //cs.id = HttpContext.Session.GetString("customerid");
                    //cs.firstname = HttpContext.Session.GetString("customerfirstname");
                    //cs.lastname = HttpContext.Session.GetString("customerlastname");
                    //cs.email = HttpContext.Session.GetString("customeremail");
                    //cs.MobileNo = HttpContext.Session.GetString("customerMobileNo");
                    //cs.displayName = cs.firstname + " " + cs.lastname;

                    //ViewBag.cuntid = Convert.ToString(dr["CountryID"]);
                    //ViewBag.StateID = Convert.ToString(dr["StateID"]);
                    //ViewBag.citid = Convert.ToString(dr["CityID"]);

                    Country();
                BindState();
                BindCity();
            }
            ViewBag.name  = HttpContext.Session.GetString("customerfirstname")+' '+ HttpContext.Session.GetString("customerlastname"); ;
             
            return View(cs);
        }
        [HttpPost]
        public JsonResult UpdateAccount(string firstname, string lastname, string displayname, string emailaddress, string mobilenumber)
        {
            string customerid = HttpContext.Session.GetString("customerid");
            if (Db.UpdateAccount(util.FixQuotes(firstname.Trim()), util.FixQuotes(lastname.Trim()), util.FixQuotes(displayname.Trim()), util.FixQuotes(emailaddress.Trim()), util.FixQuotes(mobilenumber.Trim()), util.FixQuotes(customerid.Trim())))
            {
                message = "Account update succesfully";
            }
            var Data = new { message = message };
            return Json(Data);
        }
        [HttpPost]
        public JsonResult ChangePassword(string currentpassword, string newpassword, string confirmpassword)
        {
            string customerid = HttpContext.Session.GetString("customerid");
            string message = "";
            DataTable dt = util.execQuery("select count(*) total from tblregistration where password='" + util.cryption(currentpassword.Replace(" ", "")) + "' and reg_id='" + customerid + "' ");
            DataTable dt1 = util.execQuery("select *  from tblregistration where password='" + util.cryption(currentpassword.Replace(" ", "")) + "' and reg_id='" + customerid + "' ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["total"]) > 0)
                {
                    string NewPassword = newpassword.Replace(" ", "");
                    string ConfirmPassword = confirmpassword.Replace(" ", "");
                    if (NewPassword == ConfirmPassword)
                    {
                        message = ("update tblregistration set password='" + util.cryption(NewPassword) + "' where reg_id='" + customerid + "'");
                        message = util.MultipleTransactions(message);
                        if (message == "Successfull")
                        {
                            ModelState.Clear();
                         


                            StringBuilder sBody = new StringBuilder();
                            string MailStatus = "";

                            sBody.Append("<!DOCTYPE html><html><body>");

                            sBody.Append("<p>Dear <b>" + dt1.Rows[0]["first_name"] + " " + dt1.Rows[0]["last_name"] + "</b></p>");
                            sBody.Append("<p>We would like to confirm that your password has been successfully updated for your account registered with BSD Online Store.</p>");
                            //sBody.Append("<p>We received a request to reset your password. Click the link below to set a new password:</p>");
                            sBody.Append("<p>Here are your updated login details:</p>");
                            //sBody.Append("<p>Here are your access details:\r\n\r\n</p>");
                            //sBody.Append("<a href='https://localhost:44345/Customer/ForgotPassword/{id}'>Click here</a>");
                            //sBody.Append($"<a href='https://localhost:44345/Customer/ForgotPassword/{id}'>Click here</a>");


                            sBody.Append("<p>User Name: <b>" + dt1.Rows[0]["email"] + "</b></p>");
                            sBody.Append("<p>New Password: <b>" + newpassword  + "</b></p>");
                            sBody.Append("<p>If you did not initiate this change or suspect unauthorized access, please contact us immediately at support@bsdinfotech.com or call [Contact Number].</p>");
                            sBody.Append("<p>For security reasons, we recommend keeping your password confidential and changing it periodically.</p>");
                            sBody.Append("<p>Thank you for choosing BSD Online Store!</p>");
                            sBody.Append("<p>Best Regards </p>");
                            sBody.Append("<p><b>Team BSD</b> <br>");
                            sBody.Append("</body></html>");
                            string email = dt1.Rows[0]["email"].ToString();
                            //string email = "artibsd2024@gmail.com";


                            //MailStatus = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email + "", "", "", "Subject :Password Changed Successfully\r\n", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");
                            MailStatus = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", email, "", "", "Subject: Password Changed Successfully", sBody.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");

                            message = "Password Update succesfully";






                            StringBuilder sBody1 = new StringBuilder();
                            string MailStatus1 = "";

                            sBody1.Append("<!DOCTYPE html><html><body>");
                            sBody1.Append("<p>Dear Team," + Environment.NewLine);
                            sBody1.Append("<p> The password for the following user account has been updated: " + Environment.NewLine);
                            sBody1.Append("<p>User Name: <b>" + dt1.Rows[0]["first_name"] + " " + dt1.Rows[0]["last_name"] + "</b></p>");
                            sBody1.Append("<p>Email/Username: <b>" + dt1.Rows[0]["email"] + "</b></p>");
                            sBody1.Append("<p>New Password: <b>" + newpassword + "</b></p>");
                            sBody1.Append("<p>Change Time: <b>" + " " + "</b></p>");
                            sBody1.Append("<p>" + Environment.NewLine);
                            sBody1.Append("<p>Please monitor the account for any unusual activity. If the user reports unauthorized changes, investigate the issue immediately.\r\n" + Environment.NewLine);
                            //sBody1.Append("<p>For any concerns, contact support@bsdinfotech.com." + Environment.NewLine);
                            sBody1.Append("<p>Best Regards</p>");
                            sBody1.Append("<p><b>Team BSD</b></p>");
                            sBody1.Append("</body></html>");
                            string email1 = "bsddemos@gmail.com";
                            MailStatus1 = util.SendMailViaIIS_htmls("rajesh@bsdinfotech.com", "" + email1 + "", "", "", "Subject :User Password Changed Successfully", sBody1.ToString(), "Rajesh@2023", "mail.bsdinfotech.com", "");













                        }
                    }
                    else
                    {
                        message = "Password and Confirm Password do not match!";
                    }
                }
                else
                {
                    message = "Old Password doest not exist!";
                }
            }
            else
            {
                message = "Old Password doest not exist!";
            }
            var Data = new { message = message };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult Updatebillingaddress(string BAddress1, string BAddress2, int bCountryid, int bstateid, int bcityid, string bmobile, string bpincode)
        {
            string customerid = HttpContext.Session.GetString("customerid");
            if (Db.updatecustomerbillingaddress(BAddress1.Trim(), BAddress2.Trim(), bCountryid, bstateid, bcityid, bmobile, customerid, bpincode))
            {
                message = "update Billing address";
            }
            else
            {
                message = "not update billing address";
            }
            var Data = new { message = message };
            return Json(Data);
        }

        [HttpPost]
        public JsonResult UpdateShippingaddress(string SAddress1, string SAddress2, int SCountryid, int Sstateid, int scityid, string sbmobile, string spincode)
        {
            string customerid = HttpContext.Session.GetString("customerid");
            if (Db.updatecustomerShippingaddress(SAddress1, SAddress2, SCountryid, Sstateid, scityid, sbmobile, customerid, spincode))
            {
                message = "update shipping address";
            }
            else
            {
                message = "Shipping address not update";
            }
            var Data = new { message = message };
            return Json(Data);
        }
        [HttpGet]
        public JsonResult showaddress()
        {
            GetSiteNameAndCode();
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            string customerid = HttpContext.Session.GetString("customerid");
            DataSet ds = Db.showCustomeraddress(customerid, companyid);
            List<customer> customers = new List<customer>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    customers.Add(new customer
                    {
                        PinCode= Convert.ToString(dr["PinCode"]),
                        Address= Convert.ToString(dr["Address"]),
                        address2= Convert.ToString(dr["address2"]),
                        MobileNo= Convert.ToString(dr["MobileNo"]),
                        Shi_PinCode= Convert.ToString(dr["Shi_PinCode"]),
                        Shi_Address= Convert.ToString(dr["Shi_Address"]),
                        s_address2 = Convert.ToString(dr["s_address2"]),
                        Shi_MobileNO= Convert.ToString(dr["Shi_MobileNO"]),
                        bil_country= Convert.ToString(dr["bil_country"]),
                        ship_country= Convert.ToString(dr["ship_country"]),
                        bill_city= Convert.ToString(dr["bill_city"]),
                        ship_city= Convert.ToString(dr["ship_city"]),
                        bill_state= Convert.ToString(dr["bill_state"]),
                        ship_state= Convert.ToString(dr["ship_state"]),
                    });
                }
            }
            return Json(customers);
        }
        [HttpGet]
        public JsonResult showorder()
        {
            string customerid = HttpContext.Session.GetString("customerid");
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            int orderid = 0;
            DataSet ds = Db.customerOrdershow(customerid,companyid, orderid);
            List<customerOrder> customerOrders = new List<customerOrder>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    customerOrders.Add(new customerOrder
                    {
                       id=Convert.ToInt32(dr["order_id"]),
                       order_no=Convert.ToString(dr["order_no"]),
                       orderdate= Convert.ToString(dr["order_date"]),
                       Amount=Convert.ToDouble(dr["amount"]),
                        payment_mode = Convert.ToString(dr["payment_mode"]),
                        status = Convert.ToString(dr["status"]),
                        status_flg= Convert.ToString(dr["status_flg"]),
                        orderday = Convert.ToInt32(dr["ODAYS"]),

                        //itemname =Convert.ToString(dr["itemName"]),
                        //itemid = Convert.ToInt32(dr["item_id"]),
                    });
                }
            }

            return Json(customerOrders);
        }

        public JsonResult showorderdetails(int id)
        {
            string customerid = HttpContext.Session.GetString("customerid");
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet ds = Db.customerOrdershowdet(customerid, companyid,id);
            IndexViewModel viewmodel = new IndexViewModel();
            List<customerOrder> customerOrders = new List<customerOrder>();
            List<customer> customers = new List<customer>();
           // customerOrder co = new customerOrder();
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    customerOrders.Add(new customerOrder
                    {
                        id = Convert.ToInt32(dr["order_id"]),
                        itemname = Convert.ToString(dr["itemName"]),
                        itemid = Convert.ToInt32(dr["item_id"]),
                        orderdate = Convert.ToString(dr["order_date"]),
                        itemimage=Convert.ToString(dr["itemimage"]),
                        subtotal= Convert.ToString(dr["unit_rate"]),
                        Taxable_Value=Convert.ToDecimal(dr["Taxable_Value"]),
                        CGSTAmt=Convert.ToDecimal(dr["CGSTAmt"]),
                        SGSTAmt=Convert.ToDecimal(dr["SGSTAmt"]),
                        IGSTAmt=Convert.ToDecimal(dr["IGSTAmt"])
                    });
                }
            }
            viewmodel.customerOrders = customerOrders;
            if (ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    customers.Add(new customer
                    {
                        Shi_Address = Convert.ToString(dr["Shipaddress1"]),
                        s_address2 = Convert.ToString(dr["Shipaddress2"]),
                        ship_city = Convert.ToString(dr["ship_city"]),
                        ship_state = Convert.ToString(dr["Ship_state_name"]),
                        ship_country = Convert.ToString(dr["ship_country"]),
                        Shi_PinCode = Convert.ToString(dr["sZipcode"]),
                        Shi_MobileNO = Convert.ToString(dr["smobile"]),
                        Address = Convert.ToString(dr["address1B"]),
                        address2 = Convert.ToString(dr["address2B"]),
                        bill_city = Convert.ToString(dr["bill_city"]),
                        bill_state = Convert.ToString(dr["bill_state_name"]),
                        bil_country = Convert.ToString(dr["bill_country"]),
                        PinCode = Convert.ToString(dr["zipcodeB"]),
                        bill_MobileNO= Convert.ToString(dr["mobileB"]),
                    });
                }
            }
            viewmodel.customers = customers;
            HttpContext.Session.SetInt32("orderid", id);
            string orderid= HttpContext.Session.GetInt32("orderid").ToString();
            //ViewBag.orderId = id;
            return Json(viewmodel);
        }

        
        #region Dropdownbind

        public IActionResult PopulateResion()
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet dscity = new DataSet();
            dscity = util.BindDropDown("select ID,Reasion from tblResionofCancellation where Status=1");
            List<SelectListItem> Resion = new List<SelectListItem>();
            foreach (DataRow dr in dscity.Tables[0].Rows)
            {
                Resion.Add(new SelectListItem { Text = dr["Reasion"].ToString(), Value = dr["ID"].ToString() });
            }
            ViewBag.Resion = Resion;
            return View();
        }
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
        #endregion

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login","Customer");
        }

        [HttpGet]
        public JsonResult walletbalance()
        {
            string customerid = HttpContext.Session.GetString("customerid");
            DataSet ds = util.Fill("select isnull(sum(amount),0) Credit,isnull((select sum(amount) from tbluserwallet where user_id=" + customerid+" and type='Dr'),0) Debit,( sum(amount)-isnull((select sum(amount) from tbluserwallet where user_id=" + customerid + " and type='Dr'),0)) Total from tbluserwallet where user_id=" + customerid + " and type='Cr'  and flag in (1,4)");
            List<wallet> wallets = new List<wallet>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    wallets.Add(new wallet
                    {
                        credit = Convert.ToString(dr["Credit"]),
                        debit = Convert.ToString(dr["Debit"]),
                        totalbalance = Convert.ToString(dr["Total"]),
                    });
                }
            }
//            ViewBag.walletbalance = wallets;
            return Json(wallets);
        }

        [HttpPost]
        public JsonResult walletpayemnt(double rechargeamount)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<table cellpadding='0' cellspacing='0' border='1' width='100%' class='table table-striped table-condensed'  >".ToString());
            str.Append("<tr>".ToString());
            str.Append("<th width='40%'>Name</th>".ToString());
            str.Append("<td> " + HttpContext.Session.GetString("customerfirstname") + "</td> ".ToString());
            str.Append(" </tr>".ToString());
            str.Append("<tr>".ToString());
            str.Append("<th width='15%'>Email</th>".ToString());
            str.Append("<td> " + HttpContext.Session.GetString("customeremail") + "</td> ".ToString());
            str.Append(" </tr>".ToString());
            str.Append("<tr>".ToString());
            str.Append("<th width='15%'>Mobile No</th>".ToString());
            str.Append("<td> " + HttpContext.Session.GetString("customerMobileNo") + "</td> ".ToString());
            str.Append(" </tr>".ToString());
            str.Append("<tr>".ToString());
            str.Append("<th width='15%'>Recharge Amount</th>".ToString());
            str.Append("<td><label id='lblamt'>" + rechargeamount + "</label> </td> ".ToString());
            //str.Append(" </td>".ToString());
            str.Append(" </tr>".ToString());
            //str.Append("<tr style='border-color:black;border-width:1px'>".ToString());
            //str.Append("<th width='15%'> Address</th>".ToString());
            //str.Append("<td> " + Session["Address"].ToString() + "</td> ".ToString());
            //// str.Append(" </td>".ToString());
            //str.Append(" </tr>".ToString());
            str.Append("</table>".ToString());
            str.Append("<div style ='text - align:center'>".ToString());
            str.Append("<input type='button' id='btnpaynow' class='btn btn-md btn-golden' onclick='PayNow()' value='Pay Now'/>".ToString());
            str.Append("</div>".ToString());
            return Json(str.ToString());
        }
        [HttpPost]
        public JsonResult rechargewallet(double amt)
        {
            string customerid = HttpContext.Session.GetString("customerid");
            if (amt != 0)
            {
                sqlquery = "Update tbluserwallet set amount=(select isnull(sum(amount),0) Credit from tbluserwallet where User_id='"+customerid +"')+"+ amt+", type ='Cr' where User_id='" + customerid+ "'";
                status = util.MultipleTransactions(sqlquery);
                if(status== "Successfull")
                {
                    message = "Recharge add Successfully.";
                }
                else
                {
                    message = "Recharge not added";
                }
            }
            return Json(message);
        }
    }
}
