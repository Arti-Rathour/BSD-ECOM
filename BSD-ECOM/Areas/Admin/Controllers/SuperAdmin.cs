using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
//using BSD_ECOM.Areas.SuperAdmin.Models;
using BSD_ECOM.Areas.Admin.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;


namespace BSD_ECOM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class SuperAdmin : Controller
    {
        ClsUtility Utility = new ClsUtility();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sqlquery = "";
        string message = "";
        string status = "";
        string Statement = "";
        string CompanyId = "";
        #region SuperLogin
        public IActionResult SuperLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SuperLogin(SuperLogin super)
        {
            DataTable dt = Utility.execQuery("select * from tblSuperAdminlogin WHERE (S_Email='" + super.S_Email + "' AND passwords='" + super.passwords.Trim() + "') or (S_Mobile_no='" + super.S_Mobile_no + "' AND passwords='" + super.passwords.Trim() + "')");
            if (dt.Rows.Count > 0)
            {
                HttpContext.Session.SetString("S_AdminId", dt.Rows[0]["S_AdminId"].ToString());
                HttpContext.Session.SetString("S_AdminName", dt.Rows[0]["S_Name"].ToString());
                HttpContext.Session.SetString("S_AdminEmail", dt.Rows[0]["S_Email"].ToString());
                HttpContext.Session.SetString("S_AdminMobile_no", dt.Rows[0]["S_Mobile_no"].ToString());
               
                //string CompanyId = HttpContext.Session.GetString("Company_Id");
                //DataTable dtlogo = Utility.execQuery("select * from tblCompanyInfo where Comp_Id='" + CompanyId + "'");
                //HttpContext.Session.SetString("Logo", dtlogo.Rows[0]["Logo"].ToString());
                //HttpContext.Session.SetString("CompanyName", dtlogo.Rows[0]["CompanyName"].ToString());
                return RedirectToAction("CompanyMasters");
            }
            else
            {
                ViewBag.Message = "Email and password Invalid";
            }

            return View();
        }
        #endregion
        #region CompanyMaster
        [HttpGet]
        public IActionResult CompanyMasters()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("S_AdminId")))
            {
                return RedirectToAction("SuperLogin");
            }
            return View();
        }
        [HttpPost]
        public IActionResult SaveCompanyMasters()
        {
            string userid = HttpContext.Session.GetString("S_AdminId");
            IFormFile file = Request.Form.Files[0];
            string Comp_Id = Request.Form["Comp_Id"].ToString();
            string CompanyName = Request.Form["CompanyName"].ToString();
            string ShortName = Request.Form["ShortName"].ToString();
            string EmailID = Request.Form["EmailID"].ToString();
            string PhoneNo = Request.Form["PhoneNo"].ToString();
            string Comp_Address = Request.Form["Comp_Address"].ToString();
            string MobileNo = Request.Form["MobileNo"].ToString();
            string CityName = Request.Form["CityName"].ToString();
            string FaxNo = Request.Form["FaxNo"].ToString();
            string GstNo = Request.Form["GstNo"].ToString();
            string PinCode = Request.Form["PinCode"].ToString();
            string webs = Request.Form["webs"].ToString();
            string TinNo = Request.Form["TinNo"].ToString();
            string Status = Request.Form["Status"].ToString();
            //string PrintingID = Request.Form["PrintingID"].ToString();
            string domainname = Request.Form["domainname"].ToString();
            string folderName = "wwwroot/Images/CompanyLogo";
            string extension = Path.GetExtension(file.FileName);
            string filename = Path.GetFileNameWithoutExtension(file.FileName);
            string webRootPath = filename + DateTime.Now.ToString("yymmssfff") + extension;
            string newPath = Path.Combine(folderName, webRootPath);
            using (var fileStream = new FileStream(newPath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            if (file.Length > 0)
            {

                if (Comp_Id == "0")
                {
                    Statement = "INSERT";
                    sqlquery = "exec SP_tblCompanyInfo  '" + Statement + "'," + Comp_Id + ",'" + CompanyName + "','" + ShortName + "','" + EmailID + "','" + PhoneNo + "','" + Comp_Address + "','" + MobileNo + "','" + CityName + "','" + FaxNo + "','" + GstNo + "','" + PinCode + "','" + webs + "','" + TinNo + "','" + webRootPath + "'," + Status + ",'" + domainname + "'";
                    status = Utility.MultipleTransactions(sqlquery);
                    if (status == "Successfull")
                    {
                        message = "Company Master added";
                    }
                    else
                    {
                        message = "Company Master not added.";
                    }
                }
                else
                {
                    Statement = "UPDATE";
                    sqlquery = "exec SP_tblCompanyInfo  '" + Statement + "'," + Comp_Id + ",'" + CompanyName + "','" + ShortName + "','" + EmailID + "','" + PhoneNo + "','" + Comp_Address + "','" + MobileNo + "','" + CityName + "','" + FaxNo + "','" + GstNo + "','" + PinCode + "','" + webs + "','" + TinNo + "','" + webRootPath + "'," + Status + ",'" + domainname + "'";
                    string status = Utility.MultipleTransactions(sqlquery);
                    if (status == "Successfull")
                    {
                        message = "Company Master added";
                    }
                    else
                    {
                        message = "Company Master not added.";
                    }
                }
            }
            var Data = new { message = message, id = Comp_Id };
            return Json(Data);
        }
        [HttpGet]
        public JsonResult AllCompany()
        {
            List<CompanyMaster> Comp = new List<CompanyMaster>();
            Statement = "SELECT";
            sqlquery = "exec SP_tblCompanyInfo '" + Statement + "'";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Comp.Add(new CompanyMaster
                {
                    Comp_Id = Convert.ToInt32(dr["Comp_Id"]),
                    CompanyName = Convert.ToString(dr["CompanyName"]),
                    ShortName = Convert.ToString(dr["ShortName"]),
                    domainname = Convert.ToString(dr["domainname"]),
                    Logo = Convert.ToString(dr["Logo"]),
                    EmailID = Convert.ToString(dr["EmailID"]),
                    PhoneNo = Convert.ToString(dr["PhoneNo"]),
                    Comp_Address = Convert.ToString(dr["Comp_Address"]),
                    MobileNo = Convert.ToString(dr["MobileNo"]),
                    webs = Convert.ToString(dr["webs"]),
                    comp_status = Convert.ToBoolean(dr["comp_status"]),
                });
            }
            return Json(Comp);
        }
        [HttpPost]
        public JsonResult EditCompany(int comp_Id)
        {
            Statement = "EDIT";
            CompanyMaster comp = new CompanyMaster();
            sqlquery = "exec SP_tblCompanyInfo  '" + Statement + "', " + comp_Id + " ";
            DataSet ds = Utility.TableBind(sqlquery);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                comp.Comp_Id = Convert.ToInt32(dr["Comp_Id"]);
                comp.CompanyName = Convert.ToString(dr["CompanyName"]);
                comp.ShortName = Convert.ToString(dr["ShortName"]);
                comp.domainname = Convert.ToString(dr["domainname"]);
                comp.EmailID = Convert.ToString(dr["EmailID"]);
                comp.PhoneNo = Convert.ToString(dr["PhoneNo"]);
                comp.Comp_Address = Convert.ToString(dr["Comp_Address"]);
                comp.MobileNo = Convert.ToString(dr["MobileNo"]);
                comp.CityName = Convert.ToString(dr["CityName"]);
                comp.FaxNo = Convert.ToString(dr["FaxNo"]);
                comp.GstNo = Convert.ToString(dr["GstNo"]);
                comp.PinCode = Convert.ToString(dr["PinCode"]);
                comp.webs = Convert.ToString(dr["webs"]);
                comp.TinNo = Convert.ToString(dr["TinNo"]);
                comp.printingType = Convert.ToString(dr["PrintingID"]);
                comp.Logo = Convert.ToString(dr["Logo"]);
                comp.comp_status = Convert.ToBoolean(dr["comp_status"]);
            }
            return Json(comp);
        }
        [HttpPost]
        public JsonResult DeleteCompany(int id)
        {
            string messagge;
            Statement = "DELETE";
            sqlquery = "exec SP_tblCompanyInfo  '" + Statement + "', " + id + " ";
            string status = Utility.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                messagge = "Delete Successfull!!";
                ViewBag.SubmitValue = "Save";
            }
            else
            {
                messagge = "Failed to Delete";
            }

            var Data = new { msg = messagge };
            return Json(Data);
        }
        #endregion

    }

}
