using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Data;
//using QRCoder;
using System.Drawing;
using System.Data.OleDb;
using Microsoft.Extensions.Configuration;
//using OfficeOpenXml;
using System.Linq;
//using System.Web.Razor;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
//using System.Web.UI;
//using System.Web.UI.WebControls;

/// <summary>
/// Summary description for ClsUtility
/// </sumary>
public class ClsUtility
{

   

    SqlConnection sqcon;

    SqlCommand sqcmd;
    SqlDataAdapter SqlDa;
    Boolean msg = false;
    //private IHttpContextAccessor Accessor;
    public static int flagall { get; set; }
    public static string id { get; set; }
    
    public static string path = "";
    public static string Filepath = "";
    public static string apikey = "";

    //public string cs = @"Data Source=DESKTOP-HR90T67\SQLEXPRESS;Initial Catalog=CoreMVCApplication;Integrated Security=True; User ID=sa;Password=bsdinfotech";
    //public string cs = @"Data Source = 144.168.39.28,1533; Persist Security Info = True; User ID = e_vet; Password = klqusjntmpyrvgzf0iad";

    //  public string cs = @"Data Source = 180.179.213.214,1533; Initial Catalog=shop; Persist Security Info = True;  User ID = shop; Password = ['
    //  7']";
    public string cs = @"Data Source = 180.179.213.214,1533; Initial Catalog=shop; Persist Security Info = True;  User ID = shop; Password = yb8I&9u39";
    //public string cs = @"Data Source = 1stopkhoj.com,1633; Initial Catalog=wellnesstillulast; Persist Security Info = True;  User ID = wellnesstillulast; Password = ll2A8i&68";
    public ClsUtility()
    {
        //sqcon = new SqlConnection(@"Data Source=hp-pc\sqlexpress;Initial Catalog=Mac;Persist Security Info=True;User ID=sa;Password=bsdinfotech");
        //sqcon = new SqlConnection(@"Data Source=DESKTOP-HR90T67\SQLEXPRESS;Initial Catalog=CoreMVCApplication;Integrated Security=True; User ID=sa;Password=bsdinfotech");
        sqcon = new SqlConnection(@"Data Source = 180.179.213.214,1533; Initial Catalog=shop; Persist Security Info = True; User ID = shop; Password = yb8I&9u39");
        //sqcon = new SqlConnection(@"Data Source = 1stopkhoj.com,1633; Initial Catalog=wellnesstillulast; Persist Security Info = True; User ID = wellnesstillulast; Password = ll2A8i&68");
        //sqcon = new SqlConnection(@"Data Source=144.168.39.28,1533;Initial Catalog=time_tracker;Persist Security Info=True;User ID=time_tracker;Password=vtjy8loecsgbnhuiqpxw");
    }
    public string cryption(string text)
    {
        char[] pwd;
        string passwd = "";
        if (text == "")
        {

        }
        else
        {
            text = FixQuotes(text);
            pwd = text.ToCharArray();
            try
            {
                for (int I = 0; I < pwd.Length; I++)
                {
                    int k = (int)pwd[I];
                    k += 128;
                    passwd += (char)k;
                }
            }
            catch (Exception exce)
            {
                throw exce;
            }
        }
        return passwd;
    }

    public DataTable SelectParticular(string tables, string ColName, string sqcondition)
    {
        DataTable ResSet = new DataTable();
        try
        {
            string query = "select " + ColName + " from " + tables + " where " + sqcondition;
            SqlDa = new SqlDataAdapter(query, sqcon);
            SqlDa.Fill(ResSet);
        }
        catch (Exception exce)
        {
        }
        return ResSet;
    }
    public String generatesessid()
    {
        return DateTime.Now.ToString("yyyyMMddHHmmssFFF").ToString();
    }
    public string MultipleTransactions(string query, string page, string method)
    {
        SqlTransaction SqlTran = null;
        try
        {
            sqcon.Open();
            SqlTran = sqcon.BeginTransaction();
            sqcmd = new SqlCommand(query, sqcon, SqlTran);
            sqcmd.ExecuteNonQuery();
            SqlTran.Commit();
            query = "Successfull";
        }
        catch (Exception exce)
        {
            SqlTran.Rollback();
            query = "Transaction Rolledback. Due to " + exce.Message;
        }
        finally
        {
            sqcon.Close();
        }
        return query;
    }
    public SqlConnection CON
    {
        get
        {
            return sqcon;
        }
    }

    public string MultipleTransactions(string query)
    {
        SqlTransaction SqlTran = null;
        try
        {
            WriteLogFile("Apilog", "input'" + query + "'", "", "", "", "", "", "MultipleTransactions");
            sqcon.Open();
            SqlTran = sqcon.BeginTransaction();
            sqcmd = new SqlCommand(query, sqcon, SqlTran);
            sqcmd.ExecuteNonQuery();
            SqlTran.Commit();
            query = "Successfull";
        }
        catch (Exception ex)
        {
            //SqlTran.Rollback();
            //query = "Transaction Rolledback. Due to " + exce.Message;

            query = "Transaction Rolleutilck. Due to " + ex.Message;
            WriteLogFile("Errorlog", "input'" + query + "---Output--" + ex.Message + "'", "", "", "", "", "", "MultipleTransactions");
        }
        finally
        {
            sqcon.Close();
        }
        return query;
    }

    public string AmountInWords(long Amount)
    {
        string[] m10 = new string[20];
        string[] m100 = new string[10];
        string[] mx = new string[10];
        string mstr = "";
        long ml1, ml2, ml;
        int pos, cond;
        int tl1, tl2, tl3;
        if (Amount == 0) mstr = "Zero";
        else
        {
            m10[0] = ""; m10[1] = " One"; m10[2] = " Two"; m10[3] = " Three"; m10[4] = " Four"; m10[5] = " Five"; m10[6] = " Six"; m10[7] = " Seven"; m10[8] = " Eight"; m10[9] = " Nine"; m10[10] = " Ten"; m10[11] = " Eleven"; m10[12] = " Twelve"; m10[13] = " Thirteen"; m10[14] = " Fourteen"; m10[15] = " Fifteen"; m10[16] = " Sixteen"; m10[17] = " Seventeen"; m10[18] = " Eighteen"; m10[19] = " Nineteen";
            m100[0] = ""; m100[1] = ""; m100[2] = " Twenty"; m100[3] = " Thirty"; m100[4] = " Fourty"; m100[5] = " Fifty"; m100[6] = " Sixty"; m100[7] = " Seventy"; m100[8] = " Eighty"; m100[9] = " Ninety";
            mx[0] = ""; mx[1] = ""; mx[2] = ""; mx[3] = " Hundred"; mx[4] = " Thousand"; mx[5] = " Thousand"; mx[6] = " Lac"; mx[7] = " Lac"; mx[8] = " Crore"; mx[9] = " Crore";
            ml1 = Math.Abs(Amount);
            ml = 100000000;
            pos = 9;
            cond = 0;
            while (cond != 1)
            {
                if (ml1 >= ml) break;
                ml = ml / 10;
                pos = pos - 1;
                while (ml1 > 0)
                {
                    if (pos == 9 || pos == 7 || pos == 5 || pos == 2)
                    {
                        ml = ml / 10;
                        pos = pos - 1;
                    }
                    ml2 = (int)(ml1 / ml);
                    ml1 = ml1 - (ml2 * ml);
                    ml = ml / 10;
                    if (ml2 >= 20)
                    {
                        tl2 = (int)(ml2 / 10);
                        tl3 = tl2 * 10;
                        tl1 = (int)ml2 - tl3;
                        mstr = mstr + m100[tl2];
                        mstr = mstr + m10[tl1];
                    }
                    else mstr = mstr + m10[ml2];
                    if (ml2 > 0) mstr = mstr + mx[pos];
                    pos = pos - 1;
                }
            }
        }
        return mstr + " Only";
    }

    public DataSet Fill(string sql)
    {
        DataSet ds = new DataSet();
        try
        {
            WriteLogFile("Apilog", "input'" + sql + "'", "", "", "", "", "", "Fill");
            sqcmd = new SqlCommand(sql, sqcon);
            SqlDa = new SqlDataAdapter(sqcmd);
            SqlDa.Fill(ds);
        }
        catch (Exception ex)
        {
            sql = "Transaction Rolleutilck. Due to " + ex.Message;
            WriteLogFile("Errorlog", "input'" + sql + "---Output--" + ex.Message + "'", "", "", "", "", "", "Fill");
        }
        return ds;
    }

    public DataTable Fill(string sql, int k)
    {
        DataTable ds = new DataTable();
        try
        {
            sqcmd = new SqlCommand(sql, sqcon);
            SqlDa = new SqlDataAdapter(sqcmd);
            SqlDa.Fill(ds);
        }
        catch (Exception exce)
        {

        }
        return ds;
    }
    public string decryption(string text)
    {
        char[] pwd;
        string passwd = "";
        if (text == "")
        {

        }
        else
        {
            text = FixQuotes(text);
            pwd = text.ToCharArray();
            try
            {
                for (int I = 0; I < pwd.Length; I++)
                {
                    int k = (int)pwd[I];
                    k -= 128;
                    passwd += (char)k;
                }
            }
            catch (Exception exce)
            {
                throw exce;
            }
        }
        return passwd;
    }
    public DataTable execQuery(string sql)
    {
        DataTable dt = new DataTable();
        try
        {
            WriteLogFile("Apilog", "input'" + sql + "'", "", "", "", "", "", "execQuery");
            //Writejson(sql + "" + sqcon);
            //WriteLogFile("Connection", "execQuery", "Check Connection");
            sqcmd = new SqlCommand(sql, sqcon);
            SqlDa = new SqlDataAdapter(sqcmd);
            SqlDa.Fill(dt);
        }
        catch (Exception exce)
        {
            sql = "Transaction Rolleutilck. Due to " + exce.Message;
            WriteLogFile("Errorlog", "input'" + sql + "---Output--" + exce.Message + "'", "", "", "", "", "", "execQuery");
        }
        return dt;
    }

    public string FixQuotes(string strValue)
    {
        string strRestrict = "";
        strRestrict = strValue.Replace("'", "");
        string[] badstuffs = { ";", "--", "xp_", "*", "<", ">", "[", "]", "(", ")", "select", "union", "drop", "insert", "delete", "update" };
        if (strRestrict != "")
        {
            for (int i = 0; i < badstuffs.Length; i++)
            {
                strRestrict = strRestrict.Replace(badstuffs[i], "").Trim();
            }
        }
        else
        {
            strRestrict = "";
        }
        return strRestrict;
    }

    public string Decryption(string text)
    {
        char[] pwd;
        string passwd = "";
        if (text == "")
        {
        }
        else
        {
            text = FixQuotes(text);
            pwd = text.ToCharArray();
            try
            {
                for (int I = 0; I < pwd.Length; I++)
                {
                    int k = (int)pwd[I];
                    k -= 128;
                    passwd += (char)k;
                }
            }
            catch (Exception exce)
            {
                throw exce;
            }
        }
        return passwd;
    }
    public String generatefilename()
    {
        return DateTime.Now.ToString("yyyyMMddHHmmssFFF").ToString();
    }

    public string PostMethod(string str, string url)
    {
        string str1 = string.Empty;
        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = (long)bytes.Length;
            using (Stream requestStream = httpWebRequest.GetRequestStream())
                requestStream.Write(bytes, 0, bytes.Length);
            using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                    str1 = streamReader.ReadToEnd();
            }
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)ex.Response)
                {
                    using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                        str1 = streamReader.ReadToEnd();
                }
            }
        }
        return str1;
    }

    public DataTable JsonStringToDataTable(string jsonString)
    {
        DataTable dt = new DataTable();
        string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
        List<string> ColumnsName = new List<string>();
        foreach (string jSA in jsonStringArray)
        {
            string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
            foreach (string ColumnsNameData in jsonStringData)
            {
                try
                {
                    int idx = ColumnsNameData.IndexOf(":");
                    string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                    if (!ColumnsName.Contains(ColumnsNameString))
                    {
                        ColumnsName.Add(ColumnsNameString);
                    }
                }
                catch (Exception ex)
                {
                    //throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));

                    //  WriteLogFile(string.Format("Error Parsing Column Name : {0}", ColumnsNameData), "", "", "", "", "", "");
                }
            }
            break;
        }
        foreach (string AddColumnName in ColumnsName)
        {
            dt.Columns.Add(AddColumnName);
        }
        foreach (string jSA in jsonStringArray)
        {
            string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
            DataRow nr = dt.NewRow();
            foreach (string rowData in RowData)
            {
                try
                {
                    int idx = rowData.IndexOf(":");
                    string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "");
                    string RowDataString = rowData.Substring(idx + 1).Replace("\"", "");
                    nr[RowColumns] = RowDataString;
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            dt.Rows.Add(nr);
        }
        return dt;
    }

    public DataSet BindDropDown(string Query)
    {
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand com = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
    }
    public DataSet TableBind(string query)
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        try
        {
            WriteLogFile("Apilog", "input'" + query + "'", "", "", "", "", "", "TableBind");
            using (SqlConnection sqlcon = new SqlConnection(cs))
            {
                sqlcon.Open();
                SqlCommand cmd = new SqlCommand(query, sqlcon);
                SqlDataAdapter sqlda = new SqlDataAdapter(cmd);
                sqlda.Fill(ds);
                sqlcon.Close();
               
            }
        }
        catch(Exception ex)
        {

            query = "Transaction Rolleutilck. Due to " + ex.Message;
            WriteLogFile("Errorlog", "input'" + query + "---Output--" + ex.Message + "'", "", "", "", "", "", "TableBind");
        }
        return ds;
    }

    public DataTable JsonStringTodataTable(string jsonstring)
    {
        DataTable dt = new DataTable();
        string[] jsonstringarray = Regex.Split(jsonstring.Replace("[", "").Replace("]", ""), "},{");
        List<string> ColumnsName = new List<string>();
        foreach (string JSA in jsonstringarray)
        {
            string[] jsonStringData = Regex.Split(JSA.Replace("{", "").Replace("}", ""), ",");
            foreach (string coloumnsNameData in jsonStringData)
            {
                try
                {
                    int idx = coloumnsNameData.IndexOf(":");
                    string columnsNameString = idx == -1 ? " " : coloumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                    if (!ColumnsName.Contains(columnsNameString))
                    {
                        ColumnsName.Add(columnsNameString);
                    }
                }
                catch (Exception)
                {

                }
            }
            break;
        }
        foreach (string AddColumnName in ColumnsName)
        {
            dt.Columns.Add(AddColumnName);
        }
        foreach (string JSA in jsonstringarray)
        {
            string[] RowData = Regex.Split(JSA.Replace("{", "").Replace("}", ""), ",");
            DataRow nr = dt.NewRow();
            foreach (string rowData in RowData)
            {
                try
                {
                    int idx = rowData.IndexOf(":");
                    string RowColoumns = idx == -1 ? " " : rowData.Substring(0, idx - 1).Replace("\"", "");
                    string RowDataString = rowData.Substring(idx + 1).Replace("\"", "");
                    nr[RowColoumns] = RowDataString;

                }
                catch (Exception)
                {

                    throw;
                }

            }
            dt.Rows.Add(nr);
        }
        return dt;
    }


    public static DataTable ConvertCSVtoDataTable(string strFilePath)
    {
        DataTable dt = new DataTable();
        using (StreamReader sr = new StreamReader(strFilePath))
        {
            string[] headers = sr.ReadLine().Split(',');
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }

            while (!sr.EndOfStream)
            {
                string[] rows = sr.ReadLine().Split(',');
                if (rows.Length > 1)
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i].Trim();
                    }
                    dt.Rows.Add(dr);
                }
            }

        }


        return dt;
    }

    public  DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
    {
        OleDbConnection oledbConn = new OleDbConnection(connString);
        DataTable dt = new DataTable();
        try
        {
            oledbConn.Open();
            using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn))
            {
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                oleda.SelectCommand = cmd;
                DataSet ds = new DataSet();
                oleda.Fill(ds);

                dt = ds.Tables[0];
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {

            oledbConn.Close();
        }

        return dt;

    }

    public List<SelectListItem> PopulateDropDown(string Query, string constring, string select = "")
    {
        //int max = 0;
        DataTable dt = new DataTable();
        List<SelectListItem> ddl = new List<SelectListItem>();
        try
        {

            using (SqlConnection con = new SqlConnection(constring))
            using (SqlCommand cmd = new SqlCommand(Query, con))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    da.Fill(dt);
            }
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl.Add(new SelectListItem { Text = dt.Rows[i][1].ToString(), Value = dt.Rows[i][0].ToString() });
                }
            }

            //max = Convert.ToInt32(dt.AsEnumerable().Max(row => row["id"]));
            //select = maximum;
            //if (select != "")
            //{
            //    var selddl = ddl.ToList().Where(x => x.Value = select).First();
            //    selddl.Selected = true;
            //}


        }
        catch (Exception ex)
        {
            WriteLogFile("Errorlog", "input'" + Query + "---Output--" + ex.Message + "'", "", "","","","", "Fill");
        }
        return ddl;
    }

    public void WriteLogFile(string Query, string Button, string Page, string IP, string BrowserName, string BrowerVersion, string javascript,string function)
    {
        try
        {

            if (!string.IsNullOrEmpty(Query))
            {
                string path = Path.Combine("LOG/" + System.DateTime.UtcNow.ToString("dd-MM-yyyy") + ".txt");

                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();

                    using (System.IO.FileStream file = new FileStream(path, FileMode.Append, FileAccess.Write))
                    {

                        StreamWriter streamWriter = new StreamWriter(file);

                        streamWriter.WriteLine((((((((System.DateTime.Now + " - ") + Query + " - ") + Button + " - ") + Page + " - ") + IP + " - ") + BrowserName + " - ") + BrowerVersion + " - ") + javascript+function);

                        streamWriter.Close();

                    }
                }
                else
                {
                    using (System.IO.FileStream file = new FileStream(path, FileMode.Append, FileAccess.Write))
                    {

                        StreamWriter streamWriter = new StreamWriter(file);

                        streamWriter.WriteLine((((((((System.DateTime.Now + " - ") + Query + " - ") + Button + " - ") + Page + " - ") + IP + " - ") + BrowserName + " - ") + BrowerVersion + " - ") + javascript+function);

                        streamWriter.Close();

                    }
                }

            }

        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public  DataSet UploadExcel(DataTable dt, int UserId, string UploadTypeId,string Uploadtypename)
    {
        DataSet ds = new DataSet();
        string message = "";
        try
        {
            
                SqlCommand cmd = new SqlCommand("UploadData", sqcon);
                sqcon.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@UploadTypeId", UploadTypeId);
                cmd.Parameters.AddWithValue("@" + Uploadtypename + "", dt);
                //int row = cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
                return ds;
            
        }
        catch (Exception ex)
        {
            message = "Something went wrong " + ex.Message;
        }
        return ds;
    }


    public string SendMailViaIIS_htmls(string from, string to, string cc, string bcc, string subject, string _body, string MAIL_PASSWORD, string Host, string attachPath = "")
    {
        //create the mail message
        string functionReturnValue = null;
        string _from = from, _to = to, _cc = cc, _bcc = bcc, _subject = subject; //MAIL_PASSWORD = "15M7Y1998@$";
        try
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            //set the addresses
            if (_from.Trim().Length == 0)
            {
                _from = "bsddemos@gmail.com";
                //_from = """Support Team"" support@indiastat.com"
            }
            mail.From = new System.Net.Mail.MailAddress(_from);

            if (_to.Trim().Length > 0)
            {
                mail.To.Add(new System.Net.Mail.MailAddress(_to));
            }
            if (_cc.Trim().Length > 0)
            {
                mail.CC.Add(new System.Net.Mail.MailAddress(_cc));
            }
            if (bcc.Trim().Length > 0 & bcc.Trim() != "none")
            {
                mail.Bcc.Add(new System.Net.Mail.MailAddress(_bcc));
            }
            else if (bcc.Trim().Length == 0 & bcc.Trim() != "none")
            {
                //mail.Bcc.Add(New system.net.mail.mailaddress("support@indiastat.com"))
                //mail.Bcc.Add(New system.net.mail.mailaddress("diplnd07@gmail.com"))
            }

            if (!string.IsNullOrEmpty(attachPath))
            {
                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(attachPath);
                //create the attachment
                mail.Attachments.Add(attachment);
                //add the attachment
            }
            mail.Subject = _subject;
            mail.Body = _body;
            mail.IsBodyHtml = true;
            System.Net.Mail.SmtpClient SmtpClient = new System.Net.Mail.SmtpClient();
            //SmtpClient.Host = iConfig.GetSection("ISSMTPSERVER").Value;
            //SmtpClient.Port = Convert.ToInt32(iConfig.GetSection("ISSMTPPORT").Value);
            //   SmtpClient.Host = Host;//"mail.bsdinfotech.com";
            SmtpClient.Host = Host;//"mail.bsdinfotech.com";
            SmtpClient.Credentials = new NetworkCredential(_from, MAIL_PASSWORD);
			///SmtpClient.EnableSsl = true;
			SmtpClient.Port = 587;
            SmtpClient.Send(mail);
            functionReturnValue = "Sent";
            mail.Dispose();
            SmtpClient = null;
        }
        catch (System.FormatException ex)
        {
            functionReturnValue = ex.Message;
        }
        catch (SmtpException ex)
        {
            functionReturnValue = ex.Message;
        }
        catch (System.Exception ex)
        {
            functionReturnValue = ex.Message;
        }
        return functionReturnValue;
    }


    public string SendMailViaIIS_html(string from, string to, string cc, string bcc, string subject, string attach, string _body, IConfiguration iConfig, string attachPath = "")
    {
        //create the mail message
        string functionReturnValue = null;
        string _from = from, _to = to, _cc = cc, _bcc = bcc, _subject = subject;
        try
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            //set the addresses
            if (_from.Trim().Length == 0)
            {
                _from = "ajay@bsdinfotech.com";
                //_from = """Support Team"" support@indiastat.com"
            }
            mail.From = new System.Net.Mail.MailAddress(_from);

            if (_to.Trim().Length > 0)
            {
                mail.To.Add(new System.Net.Mail.MailAddress(_to));
            }
            if (_cc.Trim().Length > 0)
            {
                mail.CC.Add(new System.Net.Mail.MailAddress(_cc));
            }
            if (bcc.Trim().Length > 0 & bcc.Trim() != "none")
            {
                mail.Bcc.Add(new System.Net.Mail.MailAddress(_bcc));
            }
            else if (bcc.Trim().Length == 0 & bcc.Trim() != "none")
            {
                //mail.Bcc.Add(New system.net.mail.mailaddress("support@indiastat.com"))
                //mail.Bcc.Add(New system.net.mail.mailaddress("diplnd07@gmail.com"))
            }

            if (!string.IsNullOrEmpty(attachPath))
            {
                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(attachPath);
                //create the attachment
                mail.Attachments.Add(attachment);
                //add the attachment
            }
            mail.Subject = _subject;
            mail.Body = _body;
            mail.IsBodyHtml = true;
            System.Net.Mail.SmtpClient SmtpClient = new System.Net.Mail.SmtpClient();
            //SmtpClient.Host = iConfig.GetSection("ISSMTPSERVER").Value;
            //SmtpClient.Port = Convert.ToInt32(iConfig.GetSection("ISSMTPPORT").Value);
            SmtpClient.Host = "mail.bsdinfotech.com";
            SmtpClient.Port = 587;
            SmtpClient.Send(mail);
            functionReturnValue = "Sent";
            mail.Dispose();
            SmtpClient = null;
        }
        catch (System.FormatException ex)
        {
            functionReturnValue = ex.Message;
        }
        catch (SmtpException ex)
        {
            functionReturnValue = ex.Message;
        }
        catch (System.Exception ex)
        {
            functionReturnValue = ex.Message;
        }
        return functionReturnValue;
    }


    public string SendMailViaIIS(string from, string to, string cc, string bcc, string subject, string attach, string _body, IConfiguration iConfig, string attachPath = "")
    {
        string functionReturnValue = null;
        string _from = from, _to = to, _cc = cc, _bcc = bcc, _subject = subject;
        try
        {
            //create the mail message
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            //set the addresses
            if (_from.Trim().Length == 0)
            {
                _from = "akash@bsdinfotech.com";
                //_from = "support@indiastat.com";
                //_from = """Support Team"" support@indiastat.com"
            }
            mail.From = new System.Net.Mail.MailAddress(_from);

            if (_to.Trim().Length > 0)
            {
                mail.To.Add(new System.Net.Mail.MailAddress(_to));//                    
            }
            if (_cc.Trim().Length > 0)
            {
                mail.CC.Add(new System.Net.Mail.MailAddress(_cc));
            }
            if (bcc.Trim().Length > 0 & bcc.Trim() != "none")
            {
                mail.Bcc.Add(new System.Net.Mail.MailAddress(_bcc));
            }
            else if (bcc.Trim().Length == 0 & bcc.Trim() != "none")
            {
                //mail.Bcc.Add(New system.net.mail.mailaddress("support@indiastat.com"))
                //mail.Bcc.Add(New system.net.mail.mailaddress("diplnd07@gmail.com"))
            }
            //mail.
            //If mCC <> "" Or mCC <> String.Empty Then
            //    Dim strCC() As String = Split(mCC, ";")
            //    Dim strThisCC As String
            //    For Each strThisCC In strCC
            //        message.CC.Add(Trim(strThisCC))
            //    Next
            //End If

            //===================================================================================
            //How do I change the TO address to a friendly name? 
            //mail.To = """Jane Doe"" <me@mycompany.com>"
            //mail.From = """John Smith"" <you@yourcompany.com>"
            //===================================================================================
            // How do I send an email with attachments?
            if (!string.IsNullOrEmpty(attachPath))
            {
                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(attachPath);
                //create the attachment
                mail.Attachments.Add(attachment);
                //add the attachment
            }
            //===================================================================================
            // How do I specify multiple recipients?
            //To specify multiple recipients for a MailMessage, simply separate each recipient with a semicolon. For example:
            //mail.To = "me@mycompany.com;him";
            //===================================================================================
            //2.7 How do I add the Reply-To header to the MailMessage?
            //To add the Reply-To header to an email, you need to use the MailMessage.Headers property. For example:
            //mail.Headers.Add("Reply-To", "alternate_email@mycompany.com")

            //set the content
            mail.Subject = _subject;
            mail.Body = _body;
            mail.IsBodyHtml = true;
            System.Net.Mail.SmtpClient SmtpClient = new System.Net.Mail.SmtpClient();
            //===================================================================================
            //SmtpClient.Host = "127.0.0.1"
            //SmtpClient.Host = "203.115.122.5"
            SmtpClient.Host = iConfig.GetSection("ISSMTPSERVER").Value;
            //===================================================================================
            //SmtpClient.Host = "mail.datanetindia.com"
            //SmtpClient.Host = 25
            //SmtpClient.Credentials = New system.net.networkcredential("support@indiastat.com", "isptos27")
            //SmtpClient.EnableSsl = True
            //===================================================================================
            SmtpClient.Port = Convert.ToInt32(iConfig.GetSection("ISSMTPPORT").Value);
            SmtpClient.Send(mail)
;

            functionReturnValue = "Sent";
            mail.Dispose();
            SmtpClient = null;

        }
        catch (System.FormatException ex)
        {
            functionReturnValue = ex.Message;
        }
        catch (SmtpException ex)
        {
            functionReturnValue = ex.Message;

        }
        catch (System.Exception ex)
        {
            functionReturnValue = ex.Message;
        }
        return functionReturnValue;

        //objMail.Priority = MailPriority.High
        //objMail.BodyFormat = MailFormat.Html
        //     message.BodyEncoding = System.Text.Encoding.ASCII;
        //message.IsBodyHtml = true;
        //message.Priority = MailPriority.Normal;
    }

    //    public void SendMail(string sHost, int nPort, string sUserName, string sPassword, string sFromName, string sFromEmail,
    //string sToName, string sToEmail, string sHeader, string sMessage, bool fSSL, string sbcc)
    //    {
    //        try
    //        {

    //            if (sToName.Length == 0)
    //                sToName = sToEmail;
    //            if (sFromName.Length == 0)
    //                sFromName = sFromEmail;

    //            System.Web.Mail.MailMessage Mail = new System.Web.Mail.MailMessage();
    //            Mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserver"] = sHost;
    //            Mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 2;

    //            Mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"] = nPort.ToString();
    //            if (fSSL)
    //                Mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"] = "False";

    //            if (sUserName.Length == 0)
    //            {
    //                //Ingen auth 
    //            }
    //            else
    //            {
    //                Mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"] = 1;
    //                Mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendusername"] = sUserName;
    //                Mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"] = sPassword;
    //            }

    //            if (fSSL)
    //                Mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"] = "False";

    //            Mail.To = sToEmail;
    //            Mail.From = sFromEmail;
    //            Mail.Subject = sHeader;
    //            Mail.Body = sMessage;
    //            Mail.Bcc = sbcc + ";Munna@bsdinfotech.com;AJAY@bsdinfotech.com";
    //            Mail.BodyFormat = System.Web.Mail.MailFormat.Html;

    //            System.Web.Mail.SmtpMail.SmtpServer = sHost;
    //            System.Web.Mail.SmtpMail.Send(Mail);

    //        }

    //        catch (Exception ex)
    //        {

    //        }
    //    }

   
  
}