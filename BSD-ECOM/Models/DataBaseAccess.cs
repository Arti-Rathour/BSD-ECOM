using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BSD_ECOM.ViewModel;
using Microsoft.AspNetCore.Http;
using BSD_ECOM.ViewModel;
using System.Data.SqlClient;
using System.Text;
using System.Net;
using System.ComponentModel.Design;
using System.Net.NetworkInformation;

namespace BSD_ECOM.Models
{
    public class DataBaseAccess
    {
        ClsUtility util = new ClsUtility();
        string sqlquery = "";
        DataTable dt = new DataTable();
        SqlCommand sqcmd;
        SqlDataAdapter SqlDa;
        string status, message;
        private static string connectionString;

        public DataSet GetMainCategory(string companyid)
        {
            DataSet dtMain = new DataSet();
            //List<IndexViewModel> mainCat = new List<IndexViewModel>();
            IndexViewModel mainCat = new IndexViewModel();
            sqlquery = "exec sp_Main '" + companyid + "'";
            dtMain = util.TableBind(sqlquery);
           
            return dtMain;
        }

        public DataSet Getrightpannel(string companyid, string Id, string type, string Main_cat_id)
        {
            DataSet dtMain = new DataSet();
            /// List<IndexViewModel> mainCat = new List<IndexViewModel>();
            IndexViewModel mainCat = new IndexViewModel();
            sqlquery = "exec sp_Main_rightpanel '" + companyid + "',@Id='" + Main_cat_id + "',@type='" + type + "'";
            dtMain = util.TableBind(sqlquery);

            return dtMain;
        }
        public List<Category> GetCategories(string companyid)
        {
            
            DataSet dscat = new DataSet();
            List<Category> Categories = new List<Category>();
            sqlquery = "select * from tblcategory where cat_status = 1 and company_id='"+companyid+"'";
            dscat = util.TableBind(sqlquery);
            if (dscat.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dscat.Tables[0].Rows)
                {
                    Categories.Add(new Category
                    {
                        category_id= Convert.ToInt32(dr["category_id"]),
                        Main_cat_id = Convert.ToInt32(dr["Main_cat_id"]),
                        category_name = Convert.ToString(dr["category_name"]),
                        cat_status = Convert.ToBoolean(dr["cat_status"]),
                        company_id = Convert.ToInt32(dr["company_id"])
                    });
                }
            }
            return Categories;
        }

        public List<SubCategory> GetSubCategories(string companyid)
        {
            DataSet dsSubcat = new DataSet();
            List<SubCategory> SubCategories = new List<SubCategory>();
            sqlquery = "select * from tblitem_category where cat_status = 1 and Company_id='" + companyid+ "'";
            dsSubcat = util.TableBind(sqlquery);
            if (dsSubcat.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsSubcat.Tables[0].Rows)
                {
                    SubCategories.Add(new SubCategory
                    {
                        cat_id = Convert.ToInt32(dr["cat_id"]),
                        Main_cat_id = Convert.ToInt32(dr["Main_cat_id"]),
                        cat_name = Convert.ToString(dr["cat_name"]),
                        cat_status = Convert.ToBoolean(dr["cat_status"]),
                        company_id = Convert.ToInt32(dr["Company_id"]),
                        category_id= Convert.ToInt32(dr["category_id"]),
                        Image= Convert.ToString(dr["Image"]),
                    });
                }
            }
            return SubCategories;
        }
        public DataSet FrontPage(string companyid)
        {
            DataSet dtfront = new DataSet();
            IndexViewModel mainCat = new IndexViewModel();
            sqlquery = "exec sp_Front_main_new '" + companyid + "'";
            dtfront = util.TableBind(sqlquery);
            return dtfront;
        }

        public DataSet welfaredet(string companyid,int id)
        {
            DataSet dtfront = new DataSet();
            IndexViewModel mainCat = new IndexViewModel();
            sqlquery = "exec [welfaredet] '" + companyid + "'," + id + "";
            dtfront = util.TableBind(sqlquery);
            return dtfront;
        }

        public DataSet blogdet(string companyid)
        {
            DataSet dtfront = new DataSet();
            IndexViewModel mainCat = new IndexViewModel();
            sqlquery = "exec [blogdet] '" + companyid + "'";
            dtfront = util.TableBind(sqlquery);
            return dtfront;
        }


        public DataSet ItemView(int id, string companyid, int itemdetailsid, int CategoryId)
        {
            DataSet dsItemView = new DataSet();
            sqlquery = "exec Sp_ProductDetails " + id + ",'" + companyid + "'," + itemdetailsid + ",@CategoryId=" + CategoryId + "";
            dsItemView = util.TableBind(sqlquery);
            return dsItemView;
        }
        public DataSet Itemquickview(int id, string companyid, int itemdetailsid)
        {
            DataSet dsItemView = new DataSet();
            sqlquery = "exec Sp_ProductDetails " + id + ",'" + companyid + "'," + itemdetailsid + "";
            dsItemView = util.TableBind(sqlquery);
            return dsItemView;
        }
        public DataSet ItemViewcatewise(int id, string companyid, int itemdetailsid, int orderbyy,double minamt, double maxamt,string type)
        {
            DataSet dsItemView = new DataSet();
            sqlquery = "exec Sp_ProductDetailsall " + id + ",'" + companyid + "'," + itemdetailsid + ","+ orderbyy + "," + minamt + "," + maxamt + ",@type='"+type+"'";
            dsItemView = util.TableBind(sqlquery);
            return dsItemView;
        }




        public DataSet ItemViewcatewiseasc(int id, string companyid, int itemdetailsid, int orderbyy, double minamt, double maxamt, int val)
        {
            DataSet dsItemView = new DataSet();

            sqlquery = "exec Sp_ProductShortbyname " + id + ",'" + companyid + "'," + itemdetailsid + "," + val + "," + minamt + "," + maxamt + ",@type='" + orderbyy + "'";
            dsItemView = util.TableBind(sqlquery);
            return dsItemView;
        }




        public DataSet branditemwise(int id, string companyid, int itemdetailsid, int orderbyy, string type,int brandcheckbox)
        {
            DataSet dsItemView = new DataSet();
            sqlquery = "exec Sp_ProductDetailsall " + id + ",'" + companyid + "'," + itemdetailsid + "," + orderbyy + ",@brandid=" + brandcheckbox + ",@type='" + type + "'";
            dsItemView = util.TableBind(sqlquery);
            return dsItemView;
        }
        public DataSet wishlistitem(string id, string companyid)
        {
            DataSet dsItemView = new DataSet();
            if (id == "")
                id = "0";
            sqlquery = "exec [Sp_wishlist] " + id + ",'" + companyid + "'";
            dsItemView = util.TableBind(sqlquery);
            return dsItemView;
        }

        public DataSet ItemViewcatewisesearch(string id, string companyid, int itemdetailsid, int orderbyy, double minamt, double maxamt, string data)
        {
            DataSet dsItemView = new DataSet();
            sqlquery = "exec Sp_ProductDetailsallsearch " + id + ",'" + companyid + "'," + itemdetailsid + "," + orderbyy + "," + minamt + "," + maxamt + ",'" + data + "'";
            dsItemView = util.TableBind(sqlquery);

            return dsItemView;
        }

        public DataSet Itemrate(int id, string companyid, int itemid)
        {
            DataSet dsItemView = new DataSet();
            sqlquery = "select td.id, isnull(td.Unit_Qty,0)as Unit_Qty,isnull(td.Unit_Rate,0)as Unit_Rate, (td.Unit_Rate-(td.discount*td.Unit_Rate)/100)as Disamt,item_id,td.discount  from tblitemdetails td, tblItemStore ts where ts.Id =  td.item_id and ts.display=1 and ts.company_id=" + companyid + " and ts.id=" + itemid + " and td.id=" + id + "";
            dsItemView = util.TableBind(sqlquery);
            return dsItemView;
        }
        public DataSet Footer(string companyid)
        {
            DataSet dsAbout = new DataSet();
            sqlquery = "Sp_Footer '" + companyid + "'";
            dsAbout = util.TableBind(sqlquery);
            return dsAbout;
        }
        public DataSet Footernew(string companyid)
        {
            DataSet dsAbout = new DataSet();
            sqlquery = "Sp_Footer_new '" + companyid + "'";
            dsAbout = util.TableBind(sqlquery);
            return dsAbout;
        }

        public Boolean IU_ProductEnquiry(string Fname, string Lname, string Email, string Mobile, string Comments, string CustomerId, string ItemId, string Companyid)
        {
            Boolean msg = false;
            try
            {
                util.CON.Open();
                sqcmd = new SqlCommand("Sp_IU_ProductEnquiry", util.CON);
                sqcmd.CommandType = CommandType.StoredProcedure;
                sqcmd.Parameters.Add(new SqlParameter("@Fname", SqlDbType.VarChar, 100)).Value = Fname;
                sqcmd.Parameters.Add(new SqlParameter("@Lname", SqlDbType.VarChar, 100)).Value = Lname;
                sqcmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100)).Value = Email;
                sqcmd.Parameters.Add(new SqlParameter("@Mobile", SqlDbType.VarChar, 50)).Value = Mobile;
                sqcmd.Parameters.Add(new SqlParameter("@Comments", SqlDbType.VarChar, 1000)).Value = Comments;
                sqcmd.Parameters.Add(new SqlParameter("@CustomerId", SqlDbType.Int, 0)).Value = CustomerId;
                sqcmd.Parameters.Add(new SqlParameter("@ItemId", SqlDbType.Int, 0)).Value = ItemId;
                sqcmd.Parameters.Add(new SqlParameter("@companyid", SqlDbType.Int, 0)).Value = Companyid;
                sqcmd.ExecuteNonQuery();
                msg = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                util.CON.Close();
            }
            return msg;
        }

        public Boolean IU_Enquiry(string Fname, string Lname, string Email, string Mobile, string Comments)
        {
            Boolean msg = false;
            try
            {
                util.CON.Open();
                sqcmd = new SqlCommand("Sp_IU_Enquiry", util.CON);
                sqcmd.CommandType = CommandType.StoredProcedure;
                sqcmd.Parameters.Add(new SqlParameter("@Fname", SqlDbType.VarChar, 100)).Value = Fname;
                sqcmd.Parameters.Add(new SqlParameter("@Lname", SqlDbType.VarChar, 100)).Value = Lname;
                sqcmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100)).Value = Email;
                sqcmd.Parameters.Add(new SqlParameter("@Mobile", SqlDbType.VarChar, 50)).Value = Mobile;
                sqcmd.Parameters.Add(new SqlParameter("@Comments", SqlDbType.VarChar, 400)).Value = Comments;
                sqcmd.ExecuteNonQuery();
                msg = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                util.CON.Close();
            }
            return msg;
        }


        public DataTable show_Enquiry(string companyid, int itemid, int itemdetid)
        {
            DataTable dt = new DataTable();
            sqlquery = "select * from tblEnquiry where company_id='" + companyid + "'";// and ts.id=" + itemid + "";
            //and td.id = " + itemdetid + "
            dt = util.execQuery(sqlquery);
            return dt;
        }

        public Boolean IU_Enquiry1(string Fullname, string Email,  string Mobile,string state,string city, string Comments,int Itemid,int user_id ,int code, int status,string password)
        {
            Boolean msg = false;
            try
            {
                util.CON.Open();
                sqcmd = new SqlCommand("Sp_IU_Enquiry2", util.CON); 
                sqcmd.CommandType = CommandType.StoredProcedure;
                sqcmd.Parameters.Add(new SqlParameter("@Fullname", SqlDbType.VarChar, 100)).Value = Fullname;
               // sqcmd.Parameters.Add(new SqlParameter("@Lname", SqlDbType.VarChar, 100)).Value = Lname;
                
                sqcmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100)).Value = Email;
                sqcmd.Parameters.Add(new SqlParameter("@Mobile", SqlDbType.VarChar, 50)).Value = Mobile;
                sqcmd.Parameters.Add(new SqlParameter("@state", SqlDbType.VarChar, 50)).Value = state;
                sqcmd.Parameters.Add(new SqlParameter("@city", SqlDbType.VarChar, 50)).Value = city;
                sqcmd.Parameters.Add(new SqlParameter("@Comments", SqlDbType.VarChar, 400)).Value = Comments;
                sqcmd.Parameters.Add(new SqlParameter("@Itemid", SqlDbType.Int)).Value = Itemid;
                sqcmd.Parameters.Add(new SqlParameter("@user_id", SqlDbType.Int)).Value = user_id;
                sqcmd.Parameters.Add(new SqlParameter("@code", SqlDbType.Int)).Value = code;
                sqcmd.Parameters.Add(new SqlParameter("@status", SqlDbType.Int)).Value = status;
                sqcmd.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar, 100)).Value = password;
                sqcmd.ExecuteNonQuery();
                msg = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                util.CON.Close();
            }
            return msg;
        }


        public Boolean IU_Enquiry2(string Fullname, string Email, string Mobile, string state, string city, string Comments, int Itemid, int user_id, int code)
        {
            Boolean msg = false;
            try
            {
                util.CON.Open();
                sqcmd = new SqlCommand("insert into tblEnquiry(VerificationCode,createdate)values('" + code + "',getdate())", util.CON);
                //sqcmd.CommandType = CommandType.StoredProcedure;
                //sqcmd.Parameters.Add(new SqlParameter("@Fullname", SqlDbType.VarChar, 100)).Value = Fullname;
                //// sqcmd.Parameters.Add(new SqlParameter("@Lname", SqlDbType.VarChar, 100)).Value = Lname;
                //sqcmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100)).Value = Email;
                //sqcmd.Parameters.Add(new SqlParameter("@Mobile", SqlDbType.VarChar, 50)).Value = Mobile;
                //sqcmd.Parameters.Add(new SqlParameter("@state", SqlDbType.VarChar, 50)).Value = state;
                //sqcmd.Parameters.Add(new SqlParameter("@city", SqlDbType.VarChar, 50)).Value = city;
                //sqcmd.Parameters.Add(new SqlParameter("@Comments", SqlDbType.VarChar, 400)).Value = Comments;
                //sqcmd.Parameters.Add(new SqlParameter("@Itemid", SqlDbType.Int)).Value = Itemid;
                //sqcmd.Parameters.Add(new SqlParameter("@user_id", SqlDbType.Int)).Value = user_id;
                //sqcmd.Parameters.Add(new SqlParameter("@code", SqlDbType.Int)).Value = code;
                sqcmd.ExecuteNonQuery();
                msg = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                util.CON.Close();
            }
            return msg;
        }
        public void sendAdminmail(string Fname, string Lname, string Email, string Mobile, string Comments)
        {

            StringBuilder str = new StringBuilder();
            str.Append("<div style='max-width:600px;min-height:100%;margin:10px auto;padding:0;background-color:#ffffff;font-family:Arial,Tahoma,Verdana,sans-serif;font-weight:299px;font-size:13px;text-align:center' bgcolor='#ffffff'>".ToString());

            str.Append("<table width='600' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6'><tbody><tr><td>".ToString());
            str.Append("<table width='100%' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("<tr>".ToString());
            str.Append("<td width='10' bgcolor='#06384f' style='width:10px;'> </td>".ToString());
            // str.Append("<td valign='middle' align='center' height='90' bgcolor='#06384f' style='padding:0;margin:0'> <a style='text-decoration:none;outline:none;color:#ffffff;font-size:13px' href='' target='_blank'><img border='0' height='70' src='" + Application["MainPath"] + "img/RetailMallwala_LOGO.png' alt='RetailMallwala.com' style='border:none' class='CToWUd'> </a> </td> ".ToString());
            str.Append("<td width='10' bgcolor='#06384f' style='width:10px;'> </td> ".ToString());
            str.Append("</tr> ".ToString());
            str.Append("</tbody>".ToString());
            str.Append("</table> ".ToString());

            str.Append("<table width='100%' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6' bgcolor='#005387'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("<tr> ".ToString());
            str.Append("<td valign='top' align='center' width='450'> ".ToString());
            str.Append("	<table border='0' cellspacing='0' cellpadding='0' width='100%' bgcolor='#005387'>".ToString());
            str.Append("<tbody>".ToString());
            str.Append("	<tr> ".ToString());
            str.Append("<td valign='middle' align='right' width='20%' height='35' style='border-bottom:solid 1px #003a5e;padding:0;color:#ffffff'>&nbsp;  </td>".ToString());
            str.Append(" <td valign='middle' width='30%' align='left' height='35' style='border-bottom:solid 1px #003a5e;border-right:solid 1px #1a6592;padding:0 0 0 5px'>&nbsp;</td> ".ToString());
            str.Append("</tr> ".ToString());
            str.Append("  </tbody>".ToString());
            str.Append(" </table>".ToString());
            str.Append("</td> ".ToString());

            str.Append(" <td valign='top' align='center' width='150'> ".ToString());
            str.Append("<table border='0' cellspacing='0' cellpadding='0' width='100%' bgcolor='#005387'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("<tr> ".ToString());
            // str.Append(" <td valign='middle' align='right' width='20%' height='35' style='border-bottom:solid 1px #003a5e;border-left:solid 1px #003a5e;padding:0;color:#ffffff'> <a style='text-decoration:none;outline:none;display:block' href='" + Application["MainPath"] + "contactus.aspx' target='_blank'> <img border='0' src='" + Application["MainPath"] + "img/customersupport.jpg' alt='' class='CToWUd'> </a> </td> ".ToString());
            str.Append("<td valign='middle' align='left' width='30%' height='35' style='border-bottom:solid 1px #003a5e;padding:0 0 0 5px'> <a style='text-decoration:none;outline:none;display:block' href='' target='_blank'> <span style='font-size:11px;color:#99bacf;line-height:14px'>CUSTOMER</span><br> <span style='font-size:11px;color:#99bacf;line-height:14px'>SUPPORT</span> </a> </td> ".ToString());
            str.Append(" </tr> ".ToString());
            str.Append("</tbody>".ToString());
            str.Append("</table> ".ToString());
            str.Append(" </td> ".ToString());
            str.Append("</tr> ".ToString());
            str.Append("</tbody>".ToString());
            str.Append("</table>".ToString());

            str.Append("<table width='100%' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6;color:#2c2c2c;line-height:20px;font-weight:300;margin:0;border-bottom:1px solid #e6e6e6;background-color:#f9f9f9;padding:20px'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("	<tr>".ToString());
            str.Append("	<td align='left' valign='top' bgcolor='#F9F9F9' width='200'>".ToString());
            //   str.Append("	<img src='" + Application["MainPath"] + "img/BJ-girl.png' height='240' alt='' />".ToString());
            str.Append(" </td>".ToString());
            str.Append("<td align='left' valign='top' bgcolor='#F9F9F9'> ".ToString());

            str.Append(" <p style='padding:0;margin:0;color:#565656;line-height:22px;font-size:13px'> Hi, <br />New Enquiry From</p>".ToString());
            str.Append(" <p>Name: " + Fname.Trim() + " " + Lname.Trim() + "</p>".ToString());
            str.Append(" <p>Email: " + Email.Trim() + "</p>".ToString());
            str.Append(" <p>Mobile: " + Mobile.Trim() + "</p>".ToString());
            str.Append(" <p>Comments: " + Comments.Trim() + "</p>".ToString());


            str.Append("</td> ".ToString());
            str.Append(" </tr> ".ToString());
            str.Append("  </tbody>".ToString());
            str.Append("</table>  ".ToString());

            str.Append("<table width='100%' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6;border-bottom:1px solid #e6e6e6; margin:20px 0;'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("<tr> ".ToString());
            str.Append("<td valign='top' align='center' width='300' style='background-color:#f9f9f9'>".ToString());
            str.Append(" <p style='padding:0;margin:0;color:#fff;font-size:18px; background:#ffd600; padding:5px 10px; display:inline-block; border:4px solid #fff; box-shadow:0 0 0 3px #ffd600; margin-bottom:10px;'> <strong>How it works</strong></p><br />".ToString());
            // str.Append(" <img width='598' src='" + Application["MainPath"] + "img/how-it-works.jpg' alt='' />".ToString());
            str.Append("</td>".ToString());
            str.Append(" </tr>".ToString());
            str.Append(" </tbody>".ToString());
            str.Append("</table>  ".ToString());

            str.Append(" <table style='max-width:600px;border:solid 1px #e6e6e6;border-top:none' cellpadding='0' cellspacing='0' width='100%'> ".ToString());
            str.Append("<tbody> ".ToString());
            str.Append("<tr> ".ToString());
            str.Append("<td style='text-align:center;background-color:#f9f9f9;display:block;margin:0 auto;clear:both;padding:15px 40px' align='center' bgcolor='#F9F9F9' valign='top'> <p style='padding:0;margin:0 0 7px 0'> <a title='RetailMallwala.com' style='text-decoration:none;color:#565656'><span style='color:#565656;font-size:13px'><strong>RetailMallwala.com</strong></span></a> </p>  <p style='padding:10px 0 0 0;margin:0;border-top:solid 1px #cccccc;font-size:11px;color:#565656'>Flexible Payment Options | Refer &amp; Earn | Easy Customization of orders</p><span class='HOEnZb'><font color='#888888'>  </font></span>".ToString());
            str.Append("</td>".ToString());
            str.Append("</tr>".ToString());
            str.Append(" </tbody>".ToString());
            str.Append(" </table>".ToString());


            str.Append(" </td> ".ToString());
            str.Append("  </tr> ".ToString());
            str.Append(" </tbody> ".ToString());
            str.Append("</table>  ".ToString());
            str.Append("</td></tr></tbody></table>".ToString());

            str.Append("</div>".ToString());


            string mailString = str.ToString();

            // util.SendMail("mail.RetailMallwala.com", 25, "info@RetailMallwala.com", "info654321", "Enquiry From " + txtFname.Text.Trim() + "", "info@RetailMallwala.com", "Enquiry From " + txtFname.Text.Trim() + "", "parimal@firststepprojects.com", " Enquiry From " + txtFname.Text.Trim() + "", mailString, false, "info@RetailMallwala.com", "priyanka@bsdinfotech.com");
            //   util.SendMail("mail.RetailMallwala.com", 25, "info@RetailMallwala.com", "info654321", "Enquiry From " + txtFname.Text.Trim() + "", "info@RetailMallwala.com", "Enquiry From " + txtFname.Text.Trim() + "", "shalini@RetailMallwala.com", " Enquiry From " + txtFname.Text.Trim() + "", mailString, false, "info@RetailMallwala.com", "priyanka@bsdinfotech.com");
        }


        public void sendAdminmail1(string Fullname, string Email, string Mobile, string state, string city, string Comments,int Itemid,int code)
        {

            StringBuilder str = new StringBuilder();
            str.Append("<div style='max-width:600px;min-height:100%;margin:10px auto;padding:0;background-color:#ffffff;font-family:Arial,Tahoma,Verdana,sans-serif;font-weight:299px;font-size:13px;text-align:center' bgcolor='#ffffff'>".ToString());

            str.Append("<table width='600' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6'><tbody><tr><td>".ToString());
            str.Append("<table width='100%' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("<tr>".ToString());
            str.Append("<td width='10' bgcolor='#06384f' style='width:10px;'> </td>".ToString());
            // str.Append("<td valign='middle' align='center' height='90' bgcolor='#06384f' style='padding:0;margin:0'> <a style='text-decoration:none;outline:none;color:#ffffff;font-size:13px' href='' target='_blank'><img border='0' height='70' src='" + Application["MainPath"] + "img/RetailMallwala_LOGO.png' alt='RetailMallwala.com' style='border:none' class='CToWUd'> </a> </td> ".ToString());
            str.Append("<td width='10' bgcolor='#06384f' style='width:10px;'> </td> ".ToString());
            str.Append("</tr> ".ToString());
            str.Append("</tbody>".ToString());
            str.Append("</table> ".ToString());

            str.Append("<table width='100%' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6' bgcolor='#005387'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("<tr> ".ToString());
            str.Append("<td valign='top' align='center' width='450'> ".ToString());
            str.Append("	<table border='0' cellspacing='0' cellpadding='0' width='100%' bgcolor='#005387'>".ToString());
            str.Append("<tbody>".ToString());
            str.Append("	<tr> ".ToString());
            str.Append("<td valign='middle' align='right' width='20%' height='35' style='border-bottom:solid 1px #003a5e;padding:0;color:#ffffff'>&nbsp;  </td>".ToString());
            str.Append(" <td valign='middle' width='30%' align='left' height='35' style='border-bottom:solid 1px #003a5e;border-right:solid 1px #1a6592;padding:0 0 0 5px'>&nbsp;</td> ".ToString());
            str.Append("</tr> ".ToString());
            str.Append("  </tbody>".ToString());
            str.Append(" </table>".ToString());
            str.Append("</td> ".ToString());

            str.Append(" <td valign='top' align='center' width='150'> ".ToString());
            str.Append("<table border='0' cellspacing='0' cellpadding='0' width='100%' bgcolor='#005387'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("<tr> ".ToString());
            // str.Append(" <td valign='middle' align='right' width='20%' height='35' style='border-bottom:solid 1px #003a5e;border-left:solid 1px #003a5e;padding:0;color:#ffffff'> <a style='text-decoration:none;outline:none;display:block' href='" + Application["MainPath"] + "contactus.aspx' target='_blank'> <img border='0' src='" + Application["MainPath"] + "img/customersupport.jpg' alt='' class='CToWUd'> </a> </td> ".ToString());
            str.Append("<td valign='middle' align='left' width='30%' height='35' style='border-bottom:solid 1px #003a5e;padding:0 0 0 5px'> <a style='text-decoration:none;outline:none;display:block' href='' target='_blank'> <span style='font-size:11px;color:#99bacf;line-height:14px'>CUSTOMER</span><br> <span style='font-size:11px;color:#99bacf;line-height:14px'>SUPPORT</span> </a> </td> ".ToString());
            str.Append(" </tr> ".ToString());
            str.Append("</tbody>".ToString());
            str.Append("</table> ".ToString());
            str.Append(" </td> ".ToString());
            str.Append("</tr> ".ToString());
            str.Append("</tbody>".ToString());
            str.Append("</table>".ToString());

            str.Append("<table width='100%' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6;color:#2c2c2c;line-height:20px;font-weight:300;margin:0;border-bottom:1px solid #e6e6e6;background-color:#f9f9f9;padding:20px'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("	<tr>".ToString());
            str.Append("	<td align='left' valign='top' bgcolor='#F9F9F9' width='200'>".ToString());
            //   str.Append("	<img src='" + Application["MainPath"] + "img/BJ-girl.png' height='240' alt='' />".ToString());
            str.Append(" </td>".ToString());
            str.Append("<td align='left' valign='top' bgcolor='#F9F9F9'> ".ToString());

            str.Append(" <p style='padding:0;margin:0;color:#565656;line-height:22px;font-size:13px'> Hi, <br />New Enquiry From</p>".ToString());
            str.Append(" <p>Name: " + Fullname.Trim()  + "</p>".ToString());
            str.Append(" <p>Email: " + Email.Trim() + "</p>".ToString());
            str.Append(" <p>Mobile: " + Mobile.Trim() + "</p>".ToString());
            str.Append(" <p>State: " + state.Trim() + "</p>".ToString());
            str.Append(" <p>City: " + city.Trim() + "</p>".ToString());
            str.Append(" <p>Comments: " + Comments.Trim() + "</p>".ToString());
            str.Append(" <p>Itemid: " + Itemid + "</p>".ToString());


            str.Append("</td> ".ToString());
            str.Append(" </tr> ".ToString());
            str.Append("  </tbody>".ToString());
            str.Append("</table>  ".ToString());

            str.Append("<table width='100%' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6;border-bottom:1px solid #e6e6e6; margin:20px 0;'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("<tr> ".ToString());
            str.Append("<td valign='top' align='center' width='300' style='background-color:#f9f9f9'>".ToString());
            str.Append(" <p style='padding:0;margin:0;color:#fff;font-size:18px; background:#ffd600; padding:5px 10px; display:inline-block; border:4px solid #fff; box-shadow:0 0 0 3px #ffd600; margin-bottom:10px;'> <strong>How it works</strong></p><br />".ToString());
            // str.Append(" <img width='598' src='" + Application["MainPath"] + "img/how-it-works.jpg' alt='' />".ToString());
            str.Append("</td>".ToString());
            str.Append(" </tr>".ToString());
            str.Append(" </tbody>".ToString());
            str.Append("</table>  ".ToString());

            str.Append(" <table style='max-width:600px;border:solid 1px #e6e6e6;border-top:none' cellpadding='0' cellspacing='0' width='100%'> ".ToString());
            str.Append("<tbody> ".ToString());
            str.Append("<tr> ".ToString());
            str.Append("<td style='text-align:center;background-color:#f9f9f9;display:block;margin:0 auto;clear:both;padding:15px 40px' align='center' bgcolor='#F9F9F9' valign='top'> <p style='padding:0;margin:0 0 7px 0'> <a title='RetailMallwala.com' style='text-decoration:none;color:#565656'><span style='color:#565656;font-size:13px'><strong>RetailMallwala.com</strong></span></a> </p>  <p style='padding:10px 0 0 0;margin:0;border-top:solid 1px #cccccc;font-size:11px;color:#565656'>Flexible Payment Options | Refer &amp; Earn | Easy Customization of orders</p><span class='HOEnZb'><font color='#888888'>  </font></span>".ToString());
            str.Append("</td>".ToString());
            str.Append("</tr>".ToString());
            str.Append(" </tbody>".ToString());
            str.Append(" </table>".ToString());


            str.Append(" </td> ".ToString());
            str.Append("  </tr> ".ToString());
            str.Append(" </tbody> ".ToString());
            str.Append("</table>  ".ToString());
            str.Append("</td></tr></tbody></table>".ToString());

            str.Append("</div>".ToString());


            string mailString = str.ToString();

            // util.SendMail("mail.RetailMallwala.com", 25, "info@RetailMallwala.com", "info654321", "Enquiry From " + txtFname.Text.Trim() + "", "info@RetailMallwala.com", "Enquiry From " + txtFname.Text.Trim() + "", "parimal@firststepprojects.com", " Enquiry From " + txtFname.Text.Trim() + "", mailString, false, "info@RetailMallwala.com", "priyanka@bsdinfotech.com");
            //   util.SendMail("mail.RetailMallwala.com", 25, "info@RetailMallwala.com", "info654321", "Enquiry From " + txtFname.Text.Trim() + "", "info@RetailMallwala.com", "Enquiry From " + txtFname.Text.Trim() + "", "shalini@RetailMallwala.com", " Enquiry From " + txtFname.Text.Trim() + "", mailString, false, "info@RetailMallwala.com", "priyanka@bsdinfotech.com");
        }

        public void sendAdminmail2(string Fullname, string Email, string Mobile, string state, string city, string Comments, int Itemid, int code)
        {

            StringBuilder str = new StringBuilder();
            str.Append("<div style='max-width:600px;min-height:100%;margin:10px auto;padding:0;background-color:#ffffff;font-family:Arial,Tahoma,Verdana,sans-serif;font-weight:299px;font-size:13px;text-align:center' bgcolor='#ffffff'>".ToString());

            str.Append("<table width='600' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6'><tbody><tr><td>".ToString());
            str.Append("<table width='100%' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("<tr>".ToString());
            str.Append("<td width='10' bgcolor='#06384f' style='width:10px;'> </td>".ToString());
            // str.Append("<td valign='middle' align='center' height='90' bgcolor='#06384f' style='padding:0;margin:0'> <a style='text-decoration:none;outline:none;color:#ffffff;font-size:13px' href='' target='_blank'><img border='0' height='70' src='" + Application["MainPath"] + "img/RetailMallwala_LOGO.png' alt='RetailMallwala.com' style='border:none' class='CToWUd'> </a> </td> ".ToString());
            str.Append("<td width='10' bgcolor='#06384f' style='width:10px;'> </td> ".ToString());
            str.Append("</tr> ".ToString());
            str.Append("</tbody>".ToString());
            str.Append("</table> ".ToString());

            str.Append("<table width='100%' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6' bgcolor='#005387'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("<tr> ".ToString());
            str.Append("<td valign='top' align='center' width='450'> ".ToString());
            str.Append("	<table border='0' cellspacing='0' cellpadding='0' width='100%' bgcolor='#005387'>".ToString());
            str.Append("<tbody>".ToString());
            str.Append("	<tr> ".ToString());
            str.Append("<td valign='middle' align='right' width='20%' height='35' style='border-bottom:solid 1px #003a5e;padding:0;color:#ffffff'>&nbsp;  </td>".ToString());
            str.Append(" <td valign='middle' width='30%' align='left' height='35' style='border-bottom:solid 1px #003a5e;border-right:solid 1px #1a6592;padding:0 0 0 5px'>&nbsp;</td> ".ToString());
            str.Append("</tr> ".ToString());
            str.Append("  </tbody>".ToString());
            str.Append(" </table>".ToString());
            str.Append("</td> ".ToString());

            str.Append(" <td valign='top' align='center' width='150'> ".ToString());
            str.Append("<table border='0' cellspacing='0' cellpadding='0' width='100%' bgcolor='#005387'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("<tr> ".ToString());
            // str.Append(" <td valign='middle' align='right' width='20%' height='35' style='border-bottom:solid 1px #003a5e;border-left:solid 1px #003a5e;padding:0;color:#ffffff'> <a style='text-decoration:none;outline:none;display:block' href='" + Application["MainPath"] + "contactus.aspx' target='_blank'> <img border='0' src='" + Application["MainPath"] + "img/customersupport.jpg' alt='' class='CToWUd'> </a> </td> ".ToString());
            str.Append("<td valign='middle' align='left' width='30%' height='35' style='border-bottom:solid 1px #003a5e;padding:0 0 0 5px'> <a style='text-decoration:none;outline:none;display:block' href='' target='_blank'> <span style='font-size:11px;color:#99bacf;line-height:14px'>CUSTOMER</span><br> <span style='font-size:11px;color:#99bacf;line-height:14px'>SUPPORT</span> </a> </td> ".ToString());
            str.Append(" </tr> ".ToString());
            str.Append("</tbody>".ToString());
            str.Append("</table> ".ToString());
            str.Append(" </td> ".ToString());
            str.Append("</tr> ".ToString());
            str.Append("</tbody>".ToString());
            str.Append("</table>".ToString());

            str.Append("<table width='100%' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6;color:#2c2c2c;line-height:20px;font-weight:300;margin:0;border-bottom:1px solid #e6e6e6;background-color:#f9f9f9;padding:20px'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("	<tr>".ToString());
            str.Append("	<td align='left' valign='top' bgcolor='#F9F9F9' width='200'>".ToString());
            //   str.Append("	<img src='" + Application["MainPath"] + "img/BJ-girl.png' height='240' alt='' />".ToString());
            str.Append(" </td>".ToString());
            str.Append("<td align='left' valign='top' bgcolor='#F9F9F9'> ".ToString());

            str.Append(" <p style='padding:0;margin:0;color:#565656;line-height:22px;font-size:13px'> Hi, <br />New Enquiry From</p>".ToString());
            str.Append(" <p>Name: " + Fullname.Trim() + "</p>".ToString());
            str.Append(" <p>Email: " + Email.Trim() + "</p>".ToString());
            str.Append(" <p>Mobile: " + Mobile.Trim() + "</p>".ToString());
            str.Append(" <p>State: " + state.Trim() + "</p>".ToString());
            str.Append(" <p>City: " + city.Trim() + "</p>".ToString());
            str.Append(" <p>Comments: " + Comments.Trim() + "</p>".ToString());
            str.Append(" <p>Itemid: " + Itemid + "</p>".ToString());


            str.Append("</td> ".ToString());
            str.Append(" </tr> ".ToString());
            str.Append("  </tbody>".ToString());
            str.Append("</table>  ".ToString());

            str.Append("<table width='100%' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6;border-bottom:1px solid #e6e6e6; margin:20px 0;'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("<tr> ".ToString());
            str.Append("<td valign='top' align='center' width='300' style='background-color:#f9f9f9'>".ToString());
            str.Append(" <p style='padding:0;margin:0;color:#fff;font-size:18px; background:#ffd600; padding:5px 10px; display:inline-block; border:4px solid #fff; box-shadow:0 0 0 3px #ffd600; margin-bottom:10px;'> <strong>How it works</strong></p><br />".ToString());
            // str.Append(" <img width='598' src='" + Application["MainPath"] + "img/how-it-works.jpg' alt='' />".ToString());
            str.Append("</td>".ToString());
            str.Append(" </tr>".ToString());
            str.Append(" </tbody>".ToString());
            str.Append("</table>  ".ToString());

            str.Append(" <table style='max-width:600px;border:solid 1px #e6e6e6;border-top:none' cellpadding='0' cellspacing='0' width='100%'> ".ToString());
            str.Append("<tbody> ".ToString());
            str.Append("<tr> ".ToString());
            str.Append("<td style='text-align:center;background-color:#f9f9f9;display:block;margin:0 auto;clear:both;padding:15px 40px' align='center' bgcolor='#F9F9F9' valign='top'> <p style='padding:0;margin:0 0 7px 0'> <a title='RetailMallwala.com' style='text-decoration:none;color:#565656'><span style='color:#565656;font-size:13px'><strong>RetailMallwala.com</strong></span></a> </p>  <p style='padding:10px 0 0 0;margin:0;border-top:solid 1px #cccccc;font-size:11px;color:#565656'>Flexible Payment Options | Refer &amp; Earn | Easy Customization of orders</p><span class='HOEnZb'><font color='#888888'>  </font></span>".ToString());
            str.Append("</td>".ToString());
            str.Append("</tr>".ToString());
            str.Append(" </tbody>".ToString());
            str.Append(" </table>".ToString());


            str.Append(" </td> ".ToString());
            str.Append("  </tr> ".ToString());
            str.Append(" </tbody> ".ToString());
            str.Append("</table>  ".ToString());
            str.Append("</td></tr></tbody></table>".ToString());

            str.Append("</div>".ToString());


            string mailString = str.ToString();

            // util.SendMail("mail.RetailMallwala.com", 25, "info@RetailMallwala.com", "info654321", "Enquiry From " + txtFname.Text.Trim() + "", "info@RetailMallwala.com", "Enquiry From " + txtFname.Text.Trim() + "", "parimal@firststepprojects.com", " Enquiry From " + txtFname.Text.Trim() + "", mailString, false, "info@RetailMallwala.com", "priyanka@bsdinfotech.com");
            //   util.SendMail("mail.RetailMallwala.com", 25, "info@RetailMallwala.com", "info654321", "Enquiry From " + txtFname.Text.Trim() + "", "info@RetailMallwala.com", "Enquiry From " + txtFname.Text.Trim() + "", "shalini@RetailMallwala.com", " Enquiry From " + txtFname.Text.Trim() + "", mailString, false, "info@RetailMallwala.com", "priyanka@bsdinfotech.com");
        }

        public DataTable addcart(string companyid)
        {
            DataTable dtcart = new DataTable();
            sqlquery = "Select Sum(Quantity) As Quantity,Max(Rate) as Rate,ItemID from tblAddToCart Where IsCancelled=0 and companyid='"+companyid+"' group by ItemID";
            dtcart = util.execQuery(sqlquery);
            return dtcart;
        }
        public DataTable getproduct(string companyid, int itemid, int itemdetid)
        {
            DataTable getproduct = new DataTable();
            sqlquery = "select isnull(ts.Id,0)as Id ,isnull(ts.CategoryID,0)as CategoryId,isnull(ts.ItemName,'')as ItemName,isnull(ts.image,'')as image,isnull(ts.URLName,'')as URLName,isnull(td.Unit_Qty,0)as Unit_Qty,isnull(td.Unit_Rate,0)as Unit_Rate, (td.Unit_Rate-(td.discount*td.Unit_Rate)/100)as Disamt,isnull(td.discount,0)as discount,unit_id,isnull(ship_charge,'0')ship_charge,isnull(stockqty,0)stockqty from tblItemStore ts left outer join tblitemdetails td on td.ITEM_ID =  ts.ID left outer join [tblitem_category] cat on cat.cat_id=ts.subgroupid  where ts.status=1 and ts.company_id='" + companyid + "' and ts.id=" + itemid + "";
            //and td.id = " + itemdetid + "
            getproduct = util.execQuery(sqlquery);
            return getproduct;
        }
        public DataSet ViewCart(string companyid)
        {
            DataSet dsViewCart = new DataSet();
            sqlquery = "exec Sp_ViewCart '" + companyid + "'";
            dsViewCart= util.TableBind(sqlquery);
            return dsViewCart;
        }
        public string deleteProductcart(string companyid,int id , int userid)
        {
            sqlquery = "exec sp_del_viewcart '" + companyid + "'," + id + ","+ userid + "";
            status = util.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                message = "Delete Successfull!!";
            }
            else
            {
                message = "Failed to Delete";
            }
            return message;
        }

        public DataSet LocalityDetails(string companyid,string LocalityId,int pincode)
        {
            DataSet dtLocDetails = new DataSet();
            sqlquery = "exec Sp_Sel_Locality_details '" + companyid + "','"+ LocalityId+"',"+ pincode+"";
            dtLocDetails = util.TableBind(sqlquery);
            return dtLocDetails;
        }

        public Boolean Insert_LocalityServices_enquiry(int LocalityId, int locdetailsId, string Fullname, string phone, string email, string description,string companyid)
        {
            Boolean msg = false;
            sqlquery = "insert into LocalityServices_enquiry(FullName,Phone,EmailAddress,descriptions, companyid,LocalityId, Loc_DetailsId,createdate)values('" + Fullname + "','" + phone + "','" + email + "','" + description + "'," + companyid + "," + LocalityId + "," + locdetailsId + ",getdate())";
            status = util.MultipleTransactions(sqlquery);
            if(status== "Successfull")
            {
                msg = true;
            }
            return msg;
        }




        public Boolean  addwishlist(string itemid, string itemdetid,  string companyid,string memberid)
        {
            Boolean msg = false;
            sqlquery = "insert into [tblWatchlist]([ProductID],[MemberID],[EntryDate],[itemdet_id],companyid)values('" + itemid + "','" + memberid + "',getdate(),'" + itemdetid + "'," + companyid + ")";
            status = util.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                msg = true;
            }
            return msg;
        }
        public Boolean delwishlist(string itemid, string itemdetid, string companyid, string memberid)
        {
            Boolean msg = false;
            sqlquery = "delete from [tblWatchlist]  where MemberID='" + memberid + "' and itemdet_id='" + itemdetid + "' and companyid=" + companyid + " and ProductID='" + itemid + "'";
               
            status = util.MultipleTransactions(sqlquery);
            if (status == "Successfull")
            {
                msg = true;
            }
            return msg;
        }

        public Boolean SaveCustomer_Review(int Item_id, string Customer_review, string Customer_name, string Customer_Email, int rating,int companyid)
        {
            DataTable dtreview = new DataTable();
            Boolean msg = false;
            string statement = "INSERT";
            try
            {
                util.CON.Open();
                sqcmd = new SqlCommand("Sp_IU_Review", util.CON);
                sqcmd.CommandType = CommandType.StoredProcedure;
                sqcmd.Parameters.Add(new SqlParameter("@statement", SqlDbType.VarChar, 20)).Value = statement;
                sqcmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 100)).Value = Customer_name;
                sqcmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 50)).Value = Customer_Email;
                sqcmd.Parameters.Add(new SqlParameter("@review", SqlDbType.VarChar, 2000)).Value = Customer_review;
                sqcmd.Parameters.Add(new SqlParameter("@Rating", SqlDbType.Int)).Value = rating;
                sqcmd.Parameters.Add(new SqlParameter("@Item_id", SqlDbType.Int)).Value = Item_id;
                sqcmd.Parameters.Add(new SqlParameter("@company_id", SqlDbType.Int)).Value = companyid;
                sqcmd.ExecuteNonQuery();
                //SqlDa = new SqlDataAdapter(sqcmd);
                //SqlDa.Fill(dtreview);
                msg = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                util.CON.Close();
            }
            return msg;
        }
        public Boolean Insert_orderdetails(int order_id, int item_id, int quantity, double rate, double amount, int UserID, string companyid,int itemdetid,double shpchags,double taxper_CGST, double taxper_SGST, double taxper_IGST, double CGSTAmt, double SGSTAmt, double IGSTAmt, double Ship_cgst, double Ship_sgst, double Ship_igst)
        {
            double Discount = 0; int IsDispatch = 0; int DeliveryDetail = 0; int IsCancelled = 0; int finalTtlpayable = 0; int Flag = 3; 

            amount = quantity * rate;
            Boolean msg = false;
            string statement = "INSERT";
            try
            {
                util.CON.Open();
                sqcmd = new SqlCommand("Sp_InsertOrderDetail_new", util.CON);
                sqcmd.CommandType = CommandType.StoredProcedure;
                //sqcmd.Parameters.Add(new SqlParameter("@statement", SqlDbType.VarChar, 100)).Value = statement;
                //sqcmd.Parameters.Add(new SqlParameter("@order_no", SqlDbType.Int, 100)).Value = "";
                sqcmd.Parameters.Add(new SqlParameter("@order_id", SqlDbType.Int, 100)).Value = order_id;
                sqcmd.Parameters.Add(new SqlParameter("@item_id", SqlDbType.Int, 100)).Value = item_id;
                sqcmd.Parameters.Add(new SqlParameter("@quantity", SqlDbType.VarChar, 100)).Value = quantity;
                sqcmd.Parameters.Add(new SqlParameter("@rate", SqlDbType.Decimal, 100)).Value = rate;
                sqcmd.Parameters.Add(new SqlParameter("@amount", SqlDbType.Decimal)).Value = amount;
                sqcmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 100)).Value = UserID;
                sqcmd.Parameters.Add(new SqlParameter("@Discount", SqlDbType.Decimal, 50)).Value = Discount;
                sqcmd.Parameters.Add(new SqlParameter("@IsDispatch", SqlDbType.Int, 400)).Value = IsDispatch;
                sqcmd.Parameters.Add(new SqlParameter("@DeliveryDetail", SqlDbType.Int, 50)).Value = DeliveryDetail;
                sqcmd.Parameters.Add(new SqlParameter("@IsCancelled", SqlDbType.Int, 50)).Value = IsCancelled;
                sqcmd.Parameters.Add(new SqlParameter("@finalTtlpayable", SqlDbType.Int, 50)).Value = finalTtlpayable;
                sqcmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.Int, 100)).Value = Flag;
                sqcmd.Parameters.Add(new SqlParameter("@shpchags", SqlDbType.Decimal, 100)).Value = shpchags;
                sqcmd.Parameters.Add(new SqlParameter("@companyid", SqlDbType.Int, 100)).Value = companyid;
                sqcmd.Parameters.Add(new SqlParameter("@itemdetid", SqlDbType.Int, 100)).Value = itemdetid;
                sqcmd.Parameters.Add(new SqlParameter("@CGSTPer", SqlDbType.Decimal)).Value = Convert.ToDecimal(taxper_CGST.ToString("0.00"));
                sqcmd.Parameters.Add(new SqlParameter("@SGSTPer", SqlDbType.Decimal)).Value = Convert.ToDecimal(taxper_SGST.ToString("0.00"));
                sqcmd.Parameters.Add(new SqlParameter("@IGSTPer", SqlDbType.Decimal)).Value = Convert.ToDecimal(taxper_IGST.ToString("0.00"));
                sqcmd.Parameters.Add(new SqlParameter("@CGSTAmt", SqlDbType.Decimal)).Value = Convert.ToDecimal(CGSTAmt.ToString("0.00"));
                sqcmd.Parameters.Add(new SqlParameter("@SGSTAmt", SqlDbType.Decimal)).Value = Convert.ToDecimal(SGSTAmt.ToString("0.00"));
                sqcmd.Parameters.Add(new SqlParameter("@IGSTAmt", SqlDbType.Decimal)).Value = Convert.ToDecimal(IGSTAmt.ToString("0.00"));
                //@ship_cgst numeric(18,2)=0,     @ numeric(18,2)=0,	 @ numeric(18,2)=0
                sqcmd.Parameters.Add(new SqlParameter("@ship_cgst", SqlDbType.Decimal)).Value = Convert.ToDecimal(Ship_cgst.ToString("0.00"));
                sqcmd.Parameters.Add(new SqlParameter("@ship_sgst", SqlDbType.Decimal)).Value = Convert.ToDecimal(Ship_sgst.ToString("0.00"));
                sqcmd.Parameters.Add(new SqlParameter("@ship_igst", SqlDbType.Decimal)).Value = Convert.ToDecimal(Ship_igst.ToString("0.00"));
                sqcmd.ExecuteNonQuery();
                msg = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                util.CON.Close();
            }
            return msg;
        }
        public string sendAdminmail(string Fname, string Lname, string Email, string Mobile, string Comments, string pagename, string companyid, string domainname)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<div style='max-width:600px;min-height:100%;margin:10px auto;padding:0;background-color:#ffffff;font-family:Arial,Tahoma,Verdana,sans-serif;font-weight:299px;font-size:13px;text-align:center' bgcolor='#ffffff'>".ToString());
            str.Append("<table width='600' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6'><tbody><tr><td>".ToString());
            str.Append("<table width='100%' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("<tr>".ToString());
            str.Append("<td width='10' bgcolor='#06384f' style='width:10px;'> </td>".ToString());
            // str.Append("<td valign='middle' align='center' height='90' bgcolor='#06384f' style='padding:0;margin:0'> <a style='text-decoration:none;outline:none;color:#ffffff;font-size:13px' href='' target='_blank'><img border='0' height='70' src='" + Application["MainPath"] + "img/RetailMallwala_LOGO.png' alt='RetailMallwala.com' style='border:none' class='CToWUd'> </a> </td> ".ToString());
            str.Append("<td width='10' bgcolor='#06384f' style='width:10px;'> </td> ".ToString());
            str.Append("</tr> ".ToString());
            str.Append("</tbody>".ToString());
            str.Append("</table> ".ToString());
            str.Append("<table width='100%' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6' bgcolor='#005387'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("<tr> ".ToString());
            str.Append("<td valign='top' align='center' width='450'> ".ToString());
            str.Append("	<table border='0' cellspacing='0' cellpadding='0' width='100%' bgcolor='#005387'>".ToString());
            str.Append("<tbody>".ToString());
            str.Append("	<tr> ".ToString());
            str.Append("<td valign='middle' align='right' width='20%' height='35' style='border-bottom:solid 1px #003a5e;padding:0;color:#ffffff'>&nbsp;  </td>".ToString());
            str.Append(" <td valign='middle' width='30%' align='left' height='35' style='border-bottom:solid 1px #003a5e;border-right:solid 1px #1a6592;padding:0 0 0 5px'>&nbsp;</td> ".ToString());
            str.Append("</tr> ".ToString());
            str.Append("  </tbody>".ToString());
            str.Append(" </table>".ToString());
            str.Append("</td> ".ToString());
            str.Append(" <td valign='top' align='center' width='150'> ".ToString());
            str.Append("<table border='0' cellspacing='0' cellpadding='0' width='100%' bgcolor='#005387'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("<tr> ".ToString());
            // str.Append("<td valign='middle' align='right' width='20%' height='35' style='border-bottom:solid 1px #003a5e;border-left:solid 1px #003a5e;padding:0;color:#ffffff'> <a style='text-decoration:none;outline:none;display:block' href='" + Application["MainPath"] + "contactus.aspx' target='_blank'><img border='0' src='" + Application["MainPath"] + "img/customersupport.jpg' alt='' class='CToWUd'></a> </td> ".ToString());
            str.Append("<td valign='middle' align='left' width='30%' height='35' style='border-bottom:solid 1px #003a5e;padding:0 0 0 5px'> <a style='text-decoration:none;outline:none;display:block' href='' target='_blank'> <span style='font-size:11px;color:#99bacf;line-height:14px'>CUSTOMER</span><br> <span style='font-size:11px;color:#99bacf;line-height:14px'>SUPPORT</span> </a> </td> ".ToString());
            str.Append(" </tr> ".ToString());
            str.Append("</tbody>".ToString());
            str.Append("</table> ".ToString());
            str.Append(" </td> ".ToString());
            str.Append("</tr> ".ToString());
            str.Append("</tbody>".ToString());
            str.Append("</table>".ToString());
            str.Append("<table width='100%' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6;color:#2c2c2c;line-height:20px;font-weight:300;margin:0;border-bottom:1px solid #e6e6e6;background-color:#f9f9f9;padding:20px'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("	<tr>".ToString());
            str.Append("	<td align='left' valign='top' bgcolor='#F9F9F9' width='200'>".ToString());
            //   str.Append("	<img src='" + Application["MainPath"] + "img/BJ-girl.png' height='240' alt='' />".ToString());
            str.Append(" </td>".ToString());
            str.Append("<td align='left' valign='top' bgcolor='#F9F9F9'> ".ToString());
            str.Append(" <p style='padding:0;margin:0;color:#565656;line-height:22px;font-size:13px'> Hi, <br />New Enquiry From</p>".ToString());
            str.Append(" <p>Name: " + Fname.Trim() + " " + Lname.Trim() + "</p>".ToString());
            str.Append(" <p>Email: " + Email.Trim() + "</p>".ToString());
            str.Append(" <p>Mobile: " + Mobile.Trim() + "</p>".ToString());
            str.Append(" <p>Comments: " + Comments.Trim() + "</p>".ToString());
            str.Append("</td> ".ToString());
            str.Append(" </tr> ".ToString());
            str.Append("  </tbody>".ToString());
            str.Append("</table>  ".ToString());
            str.Append("<table width='100%' cellspacing='0' cellpadding='0' style='max-width:600px;border-left:solid 1px #e6e6e6;border-right:solid 1px #e6e6e6;border-bottom:1px solid #e6e6e6; margin:20px 0;'> ".ToString());
            str.Append("<tbody>".ToString());
            str.Append("<tr> ".ToString());
            // str.Append("<td valign='top' align='center' width='300' style='background-color:#f9f9f9'>".ToString());
            //str.Append(" <p style='padding:0;margin:0;color:#fff;font-size:18px; background:#ffd600; padding:5px 10px; display:inline-block; border:4px solid #fff; box-shadow:0 0 0 3px #ffd600; margin-bottom:10px;'> <strong>How it works</strong></p><br />".ToString());
            // str.Append(" <img width='598' src='" + Application["MainPath"] + "img/how-it-works.jpg' alt='' />".ToString());
            // str.Append("</td>".ToString());
            str.Append(" </tr>".ToString());
            str.Append(" </tbody>".ToString());
            str.Append("</table>  ".ToString());
            str.Append(" <table style='max-width:600px;border:solid 1px #e6e6e6;border-top:none' cellpadding='0' cellspacing='0' width='100%'> ".ToString());
            str.Append("<tbody> ".ToString());
            str.Append("<tr> ".ToString());
            str.Append("<td style='text-align:center;background-color:#f9f9f9;display:block;margin:0 auto;clear:both;padding:15px 40px' align='center' bgcolor='#F9F9F9' valign='top'> <p style='padding:0;margin:0 0 7px 0'> <a title=" + domainname + "' style='text-decoration:none;color:#565656'><span style='color:#565656;font-size:13px'><strong>'" + domainname + "'</strong></span></a> </p>  <p style='padding:10px 0 0 0;margin:0;border-top:solid 1px #cccccc;font-size:11px;color:#565656'>Flexible Payment Options | Refer &amp; Earn | Easy Customization of orders</p><span class='HOEnZb'><font color='#888888'>  </font></span>".ToString());
            str.Append("</td>".ToString());
            str.Append("</tr>".ToString());
            str.Append(" </tbody>".ToString());
            str.Append(" </table>".ToString());
            str.Append(" </td> ".ToString());
            str.Append("  </tr> ".ToString());
            str.Append(" </tbody> ".ToString());
            str.Append("</table>  ".ToString());
            str.Append("</td></tr></tbody></table>".ToString());
            str.Append("</div>".ToString());
            string mailString = str.ToString();
            string companyemail = GetCompanyEmail(companyid);
            string AdminEmail = util.SendMailViaIIS_html("akash@bsdinfotech.com", companyemail, "", "none", "'" + domainname + "' :" + pagename + "", "", mailString, null, "");
            string useremail = "";
            if (AdminEmail == "Sent")
            {
                util.SendMailViaIIS_html(companyemail, Email, "", "none", "'" + domainname + "':" + pagename + "", "", "Than You For visting on my website.", null, "");
            }
            else
            {
                AdminEmail = "Invalid Eamil.";
            }
            return AdminEmail;
        }
        public string GetCompanyEmail(string companyid)
        {
            string email = "";
            DataTable dt = util.execQuery("select * from tblcompanyinfo where comp_id='" + companyid + "'");
            if (dt.Rows.Count > 0)
            {
                email = dt.Rows[0]["EmailID"].ToString();
            }
            return email;
        }
        public DataTable CheckorderbyPincode(string pincode, string companyid)
        {
            DataTable dt = new DataTable();
            dt = util.execQuery("select * from tbllocation where Pincode='" + pincode + "'and companyid='" + companyid + "'");
            return dt;
        }

        public DataTable OTPSEND(string emailmobile, string companyid)
        {
            string Message = string.Empty;
            string genNum = string.Empty;
            DataTable dt = new DataTable();
            sqlquery = "select * from tblregistration where (email='" + emailmobile + "' or MobileNo='" + emailmobile + "')  AND COMPANYID =" + companyid + "";

            dt = util.execQuery(sqlquery);
            
            return dt;
        }
        public void SMSAPInewwithmsg(string msg)
        {

           // WriteLogFile(msg, "", "", "", "", "", "");

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(msg);


            //   HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://149.20.191.19/VSServices/SendSms.ashx?login=AjayKumar&pass=AjayKumar854D&text="+msg+"&from=ACKAFO&to=91"+mobno+"");

            HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
            System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
            string responseString = respStreamReader.ReadToEnd();
            respStreamReader.Close();
            myResp.Close();
        }
        public DataSet PintInvoice(int orderid,int companyid)
        {
            DataSet ds = new DataSet();
            sqlquery = "exec sp_printInvoice " + orderid + ","+ companyid + "";
            ds = util.TableBind(sqlquery);
            return ds;
        }
        public DataSet customerOrdershow(string customerid, string companyid, int orderid)
        {
            //string statement = "OrderView";
            DataSet ds = new DataSet();
            sqlquery = "exec sp_CustomerShow_Order '" + customerid + "','" + companyid + "','" + orderid + "' ";
            ds = util.TableBind(sqlquery);
            return ds;
        }
        public DataSet customerOrdershowdet(string customerid, string companyid, int orderid)
        {
            //string statement = "OrderView";
            DataSet ds = new DataSet();
            sqlquery = "exec sp_CustomerShow_Orderdet '" + customerid + "','" + companyid + "','" + orderid + "' ";
            ds = util.TableBind(sqlquery);
            return ds;
        }
        public DataSet showCustomeraddress(string customerid)
        {
            string statement = "Selectaddress";
            int id = 0;
            DataSet dsaddress = new DataSet();
            sqlquery = "exec sp_IUTblregistration '" + statement + "','" + Convert.ToInt32(customerid) + "'";
            dsaddress = util.TableBind(sqlquery);
            return dsaddress;
        }
        public DataSet showCustomeraddress(string customerid,string companyid)
        {
            string statement = "Selectaddress";
            int id = 0;
            DataSet dsaddress = new DataSet();
            sqlquery = "exec sp_IUTblregistration_NEW '" + statement + "','" + Convert.ToInt32(customerid) + "',"+companyid+"";
            dsaddress = util.TableBind(sqlquery);
            return dsaddress;
        }
        public DataTable CustomerLogin(string email, string mobile, string password,string COMID)
        {
            DataTable dtlogin = new DataTable();
            //sqlquery = "select * from  tblregistration where (email='" + email + "' and  password='" + util.cryption(password.Trim()) + "')or (MobileNo='" + mobile + "' and password='" + util.cryption(password.Trim()) + "')";
            //sqlquery = "select * from tblregistration where (email='" + email + "' or MobileNo='" + email + "') and password='" + util.cryption(password.Trim()) + "' AND COMPANYID ="+ COMID + "";

            sqlquery = "select reg_id, first_name,last_name,email,password,StateID,state_name,CityID ,city ,MobileNo from tblregistration a left outer join [tblstate] b on a.StateID=b.id   left outer join [dbo].[tblcity] c on a.CityID=c.id  where (email='" + email + "' or MobileNo='" + email + "') and password='" + util.cryption(password.Trim()) + "' AND a.COMPANYID =" + COMID + "";
            dtlogin = util.execQuery(sqlquery);
            return dtlogin;
        }
        public DataTable GetDomain(string domainName)
        {
            DataTable dt = new DataTable();

            string sqlquery = "select  Comp_id, CompanyName, Comp_Address,domainname ,Logo,MobileNo,EmailID,Comp_id SiteCode,ShortName SiteName from tblcompanyinfo where domainname='" + domainName + "'";
            
            dt = util.execQuery(sqlquery);

            return dt;
        }
        public DataTable CustomerLoginverfy(string email, string COMID)
        {
            DataTable dtlogin = new DataTable();
            //sqlquery = "select * from  tblregistration where (email='" + email + "' and  password='" + util.cryption(password.Trim()) + "')or (MobileNo='" + mobile + "' and password='" + util.cryption(password.Trim()) + "')";
            sqlquery = "select * from tblregistration where (email='" + email + "' or MobileNo='" + email + "') AND COMPANYID =" + COMID + "";
            dtlogin = util.execQuery(sqlquery);
            return dtlogin;
        }
        public Boolean IU_Checkout(int id, string address1, string address2, int city, string pincode,int countryid,int stateid,int mobileno,int scountryid,int s_stateid,int s_cityid,string s_address1,string s_address2,string s_mobileno,string Shipping_name,string Billing_name,string addtional_info, string Semail,string spincode)
        {
            Boolean msg = false;
            string statement = "UPDATE";
            //int countryid = 0,stateid=0;
            try
            {
                util.CON.Open();
                sqcmd = new SqlCommand("sp_IUTblregistration", util.CON);
                sqcmd.CommandType = CommandType.StoredProcedure;
                sqcmd.Parameters.Add(new SqlParameter("@reg_id", SqlDbType.Int, 100)).Value = id;
                sqcmd.Parameters.Add(new SqlParameter("@statement", SqlDbType.VarChar, 100)).Value = statement;
                sqcmd.Parameters.Add(new SqlParameter("@CountryID", SqlDbType.Int, 100)).Value = countryid;
                sqcmd.Parameters.Add(new SqlParameter("@StateID", SqlDbType.Int, 100)).Value = stateid;
                sqcmd.Parameters.Add(new SqlParameter("@CityID", SqlDbType.Int, 100)).Value = city;
                sqcmd.Parameters.Add(new SqlParameter("@PinCode", SqlDbType.VarChar, 50)).Value = pincode;
                sqcmd.Parameters.Add(new SqlParameter("@Address", SqlDbType.VarChar, 400)).Value = address1;
                sqcmd.Parameters.Add(new SqlParameter("@address2", SqlDbType.VarChar, 400)).Value = address2;
                sqcmd.Parameters.Add(new SqlParameter("@Shi_Address", SqlDbType.VarChar, 400)).Value = s_address1;
                sqcmd.Parameters.Add(new SqlParameter("@s_address2", SqlDbType.VarChar, 400)).Value = s_address2;
                sqcmd.Parameters.Add(new SqlParameter("@addtional_info", SqlDbType.VarChar, 400)).Value = addtional_info;
                sqcmd.Parameters.Add(new SqlParameter("@Shi_CountryID", SqlDbType.Int, 100)).Value = scountryid;
                sqcmd.Parameters.Add(new SqlParameter("@Shi_StateID", SqlDbType.Int, 100)).Value = s_stateid;
                sqcmd.Parameters.Add(new SqlParameter("@Shi_CityID", SqlDbType.Int, 100)).Value = s_cityid;
                sqcmd.Parameters.Add(new SqlParameter("@Shi_PinCode", SqlDbType.VarChar, 50)).Value = spincode;
               sqcmd.Parameters.Add(new SqlParameter("@Shi_MobileNO", SqlDbType.VarChar, 50)).Value = s_mobileno;
                //sqcmd.Parameters.Add(new SqlParameter("@semail", SqlDbType.VarChar, 50)).Value = Semail;

                //sqcmd.Parameters.Add(new SqlParameter("@addtional_info", SqlDbType.VarChar, 400)).Value = addtional_info; 
                sqcmd.ExecuteNonQuery();
                msg = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                util.CON.Close();
            }
            return msg;
        }


        //public Boolean IU_Checkout1(int id, string fullname, string email, string mobileno, string paymentmode, string shipping_address, string item_name, int rate, int quantity)
        //{
        //   // Boolean msg = false;
        //  //  string statement = "UPDATE";
        //    //int countryid = 0,stateid=0;
        //    try
        //    {
        //        util.CON.Open();
        //        sqcmd = new SqlCommand("sp_IUTblregistration", util.CON);
        //        sqcmd.CommandType = CommandType.StoredProcedure;
        //        sqcmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 100)).Value = id;
        //        sqcmd.Parameters.Add(new SqlParameter("@fullname", SqlDbType.VarChar, 100)).Value = fullname;
        //        sqcmd.Parameters.Add(new SqlParameter("@email", SqlDbType.Int, 100)).Value = email;
        //        sqcmd.Parameters.Add(new SqlParameter("@mobileno", SqlDbType.Int, 100)).Value = mobileno;
        //        sqcmd.Parameters.Add(new SqlParameter("@paymentmode", SqlDbType.Int, 100)).Value = paymentmode;
        //        sqcmd.Parameters.Add(new SqlParameter("@shipping_address", SqlDbType.VarChar, 50)).Value = shipping_address;
        //        sqcmd.Parameters.Add(new SqlParameter("@item_name", SqlDbType.VarChar, 400)).Value = item_name;
        //        sqcmd.Parameters.Add(new SqlParameter("@rate", SqlDbType.VarChar, 400)).Value = rate;
        //        sqcmd.Parameters.Add(new SqlParameter("@quantity", SqlDbType.VarChar, 400)).Value = quantity;
        //        //sqcmd.Parameters.Add(new SqlParameter("@semail", SqlDbType.VarChar, 50)).Value = Semail;

        //        //sqcmd.Parameters.Add(new SqlParameter("@addtional_info", SqlDbType.VarChar, 400)).Value = addtional_info; 
        //        sqcmd.ExecuteNonQuery();
        //       // msg = true;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        util.CON.Close();
        //    }
        // //   return msg;
        //}
        public Boolean IU_Order(int user_id, double amount,string first_name,string last_name,string address1,string address2,int state,int city,string zipcode,int country,string mobile,string email,string first_nameB,string last_nameB,string address1B,string address2B,int stateB,int cityB,string zipcodeB,int countryB,string mobileB,string emailB,string Dob,string payment_mode,int discount,int walletAmtdetect,int companyid)
        {
            Boolean msg = false;
            string statement = "INSERT";
            try
            {
                util.CON.Open();
                sqcmd = new SqlCommand("sp_IU_tblorder_new", util.CON);
                sqcmd.CommandType = CommandType.StoredProcedure;
                sqcmd.Parameters.Add(new SqlParameter("@statement", SqlDbType.VarChar, 100)).Value = statement;
                //sqcmd.Parameters.Add(new SqlParameter("@order_no", SqlDbType.Int, 100)).Value = @order_no;
                sqcmd.Parameters.Add(new SqlParameter("@user_id", SqlDbType.Int, 100)).Value = @user_id;
                sqcmd.Parameters.Add(new SqlParameter("@amount", SqlDbType.Int, 100)).Value = amount;
                sqcmd.Parameters.Add(new SqlParameter("@first_name", SqlDbType.VarChar, 100)).Value = first_name;
                sqcmd.Parameters.Add(new SqlParameter("@last_name", SqlDbType.VarChar, 100)).Value = last_name;
                sqcmd.Parameters.Add(new SqlParameter("@address1", SqlDbType.VarChar, 400)).Value = address1;
                sqcmd.Parameters.Add(new SqlParameter("@address2", SqlDbType.VarChar, 100)).Value = address2;
                sqcmd.Parameters.Add(new SqlParameter("@state", SqlDbType.Int, 50)).Value = state;
                sqcmd.Parameters.Add(new SqlParameter("@city", SqlDbType.Int, 400)).Value = city;
                sqcmd.Parameters.Add(new SqlParameter("@zipcode", SqlDbType.VarChar, 50)).Value = zipcode;
                sqcmd.Parameters.Add(new SqlParameter("@country", SqlDbType.Int, 400)).Value = country;
                sqcmd.Parameters.Add(new SqlParameter("@mobile", SqlDbType.VarChar, 50)).Value = mobile;
                sqcmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 400)).Value = email;
                sqcmd.Parameters.Add(new SqlParameter("@first_nameB", SqlDbType.VarChar, 100)).Value = first_nameB;
                sqcmd.Parameters.Add(new SqlParameter("@last_nameB", SqlDbType.VarChar, 100)).Value = last_nameB;
                sqcmd.Parameters.Add(new SqlParameter("@address1B", SqlDbType.VarChar, 400)).Value = address1B;
                sqcmd.Parameters.Add(new SqlParameter("@address2B", SqlDbType.VarChar, 500)).Value = address2B;
                sqcmd.Parameters.Add(new SqlParameter("@stateB", SqlDbType.Int, 10)).Value = stateB;
                sqcmd.Parameters.Add(new SqlParameter("@cityB", SqlDbType.Int, 20)).Value = cityB;
                sqcmd.Parameters.Add(new SqlParameter("@zipcodeB", SqlDbType.VarChar, 500)).Value = zipcodeB;
                sqcmd.Parameters.Add(new SqlParameter("@countryB", SqlDbType.Int, 400)).Value = countryB;
                sqcmd.Parameters.Add(new SqlParameter("@mobileB", SqlDbType.VarChar, 100)).Value = mobileB;
                sqcmd.Parameters.Add(new SqlParameter("@emailB", SqlDbType.VarChar, 400)).Value = emailB;
                sqcmd.Parameters.Add(new SqlParameter("@Dob", SqlDbType.VarChar, 100)).Value = Dob;
                sqcmd.Parameters.Add(new SqlParameter("@payment_mode", SqlDbType.VarChar, 500)).Value = payment_mode;
                sqcmd.Parameters.Add(new SqlParameter("@subtotal", SqlDbType.Int, 400)).Value = amount;
                //sqcmd.Parameters.Add(new SqlParameter("@discount", SqlDbType.Int, 100)).Value = discount;
                //sqcmd.Parameters.Add(new SqlParameter("@walletAmtdetect", SqlDbType.Int, 100)).Value = walletAmtdetect;
                sqcmd.Parameters.Add(new SqlParameter("@companyid", SqlDbType.Int, 100)).Value = companyid;
                sqcmd.ExecuteNonQuery();
                msg = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                util.CON.Close();
            }
            return msg;
        }
        public DataSet BindCountry(string companyid)
        {
            DataSet dscountry = new DataSet();
            sqlquery = "select id,country from tblcountry where companyid='"+ companyid + "'";
            dscountry = util.BindDropDown(sqlquery);
            return dscountry;
        }
        public DataSet BindState(int country_id,string companyid)
        {
            DataSet dsstate = new DataSet();
            sqlquery = "select id,state_name from tblstate where companyid='"+ companyid+"' and status=1 and country_id=" + country_id+"";
            dsstate = util.BindDropDown(sqlquery);
            return dsstate;
        }
        public DataSet BindCity(int state_id, string companyid)
        {
            DataSet dsstate = new DataSet();
            sqlquery = "select id,city from tblcity where companyid='"+ companyid + "' and status=1 and state_id="+ state_id + "";
            dsstate = util.BindDropDown(sqlquery);
            return dsstate;
        }
        public Boolean updatecustomerShippingaddress(string SAddress1, string SAddress2, int SCountryid, int Sstateid, int scityid, string sbmobile, string customerid, string spincode)
        {
            Boolean msg = false;
            string statement = "UpdateshippinggAdd";
            try
            {
                util.CON.Open();
                sqcmd = new SqlCommand("sp_IUTblregistration", util.CON);
                sqcmd.CommandType = CommandType.StoredProcedure;
                sqcmd.Parameters.Add(new SqlParameter("@statement", SqlDbType.VarChar, 100)).Value = statement;
                sqcmd.Parameters.Add(new SqlParameter("@reg_id", SqlDbType.Int, 100)).Value = customerid;
                sqcmd.Parameters.Add(new SqlParameter("@Shi_CountryID", SqlDbType.Int, 100)).Value = SCountryid;
                sqcmd.Parameters.Add(new SqlParameter("@Shi_StateID", SqlDbType.Int, 200)).Value = Sstateid;
                sqcmd.Parameters.Add(new SqlParameter("@Shi_CityID", SqlDbType.Int, 200)).Value = scityid;
                sqcmd.Parameters.Add(new SqlParameter("@Shi_PinCode", SqlDbType.VarChar, 50)).Value = spincode;
                sqcmd.Parameters.Add(new SqlParameter("@Shi_Address", SqlDbType.VarChar, 500)).Value = SAddress1;
                sqcmd.Parameters.Add(new SqlParameter("@s_address2", SqlDbType.VarChar, 500)).Value = SAddress2;
                sqcmd.Parameters.Add(new SqlParameter("@Shi_MobileNO", SqlDbType.VarChar, 500)).Value = sbmobile;
                sqcmd.ExecuteNonQuery();
                msg = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                util.CON.Close();
            }
            return msg;
        }

        public Boolean updatecustomerbillingaddress(string BAddress1, string BAddress2, int bCountryid, int bstateid, int bcityid, string bmobile, string customerid, string PinCode)
        {
            Boolean msg = false;
            string statement = "UpdatebillingAdd";
            try
            {
                util.CON.Open();
                sqcmd = new SqlCommand("sp_IUTblregistration", util.CON);
                sqcmd.CommandType = CommandType.StoredProcedure;
                sqcmd.Parameters.Add(new SqlParameter("@statement", SqlDbType.VarChar, 100)).Value = statement;
                sqcmd.Parameters.Add(new SqlParameter("@reg_id", SqlDbType.Int, 100)).Value = customerid;
                sqcmd.Parameters.Add(new SqlParameter("@CountryID", SqlDbType.Int, 200)).Value = bCountryid;
                sqcmd.Parameters.Add(new SqlParameter("@StateID", SqlDbType.Int, 200)).Value = bstateid;
                sqcmd.Parameters.Add(new SqlParameter("@CityID", SqlDbType.Int, 200)).Value = bcityid;
                sqcmd.Parameters.Add(new SqlParameter("@PinCode", SqlDbType.VarChar, 500)).Value = PinCode;
                sqcmd.Parameters.Add(new SqlParameter("@MobileNo", SqlDbType.VarChar, 50)).Value = bmobile;
                sqcmd.Parameters.Add(new SqlParameter("@Address", SqlDbType.VarChar, 500)).Value = BAddress1;
                sqcmd.Parameters.Add(new SqlParameter("@address2", SqlDbType.VarChar, 500)).Value = BAddress2;
                sqcmd.ExecuteNonQuery();
                msg = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                util.CON.Close();
            }
            return msg;
        }

        public Boolean UpdateAccount(string firstname, string lastname, string displayname, string emailaddress, string mobilenumber, string customerid)
        {
            Boolean msg = false;
            string statement = "UpdateAccount";
            try
            {
                util.CON.Open();
                sqcmd = new SqlCommand("sp_IUTblregistration", util.CON);
                sqcmd.CommandType = CommandType.StoredProcedure;
                sqcmd.Parameters.Add(new SqlParameter("@statement", SqlDbType.VarChar, 100)).Value = statement;
                sqcmd.Parameters.Add(new SqlParameter("@reg_id", SqlDbType.Int, 100)).Value = customerid;
                sqcmd.Parameters.Add(new SqlParameter("@first_name", SqlDbType.VarChar, 200)).Value = firstname;
                sqcmd.Parameters.Add(new SqlParameter("@last_name", SqlDbType.VarChar, 200)).Value = lastname;
                sqcmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 200)).Value = emailaddress;
                sqcmd.Parameters.Add(new SqlParameter("@MobileNo", SqlDbType.VarChar, 50)).Value = mobilenumber;
                sqcmd.Parameters.Add(new SqlParameter("@displayname", SqlDbType.VarChar, 500)).Value = displayname;
                sqcmd.ExecuteNonQuery();
                msg = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                util.CON.Close();
            }
            return msg;
        }

        public Boolean CustomerRegistraion(string firstname, string lastname, string email, string password, string Mobile, string companyid)
        {
            
            Boolean msg = false;
            string statement = "INSERT";
            try
            {
                util.CON.Open();
                sqcmd = new SqlCommand("sp_IUTblregistration_NEW", util.CON);
                sqcmd.CommandType = CommandType.StoredProcedure;
                sqcmd.Parameters.Add(new SqlParameter("@statement", SqlDbType.VarChar, 100)).Value = statement;
                sqcmd.Parameters.Add(new SqlParameter("@first_name", SqlDbType.VarChar, 200)).Value = firstname;
                sqcmd.Parameters.Add(new SqlParameter("@last_name", SqlDbType.VarChar, 200)).Value = lastname;
                sqcmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 200)).Value = email;
                sqcmd.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar, 200)).Value = password;
                sqcmd.Parameters.Add(new SqlParameter("@MobileNo", SqlDbType.VarChar, 50)).Value = Mobile;
                sqcmd.Parameters.Add(new SqlParameter("@companyid", SqlDbType.VarChar, 50)).Value = companyid;
                sqcmd.ExecuteNonQuery();
                msg = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                util.CON.Close();
            }
            return msg;
        }


        public Boolean CustomerRegistraion1(string vendor_name, string vendor_email, string vendor_mobile, string vendor_address, string companyname)
        {

            Boolean msg = false;
            string type = "INSERT";
            try
            {
                util.CON.Open();
                sqcmd = new SqlCommand("sp_tbl_vender1", util.CON);
                sqcmd.CommandType = CommandType.StoredProcedure;
                sqcmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar, 100)).Value = type;
                sqcmd.Parameters.Add(new SqlParameter("@vendar_name", SqlDbType.VarChar, 200)).Value = vendor_name;
                sqcmd.Parameters.Add(new SqlParameter("@vendar_emailid", SqlDbType.VarChar, 200)).Value = vendor_email;
                sqcmd.Parameters.Add(new SqlParameter("@vendar_mobileno", SqlDbType.VarChar, 200)).Value = vendor_mobile;
                sqcmd.Parameters.Add(new SqlParameter("@vendar_address", SqlDbType.VarChar, 200)).Value = vendor_address;
                sqcmd.Parameters.Add(new SqlParameter("@company_name", SqlDbType.VarChar, 50)).Value = companyname;
                sqcmd.ExecuteNonQuery();
                msg = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                util.CON.Close();
            }
            return msg;
        }
    }
}
