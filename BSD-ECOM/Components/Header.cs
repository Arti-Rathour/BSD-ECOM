using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BSD_ECOM.Models;
using BSD_ECOM.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Data;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;

namespace BSD_ECOM.Components
{
    public class Header:ViewComponent
    {
        ClsUtility util = new ClsUtility();
        DataBaseAccess Db = new DataBaseAccess();
        IndexViewModel viewmodel = new IndexViewModel();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            

            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("CartItems")))
            {
                ViewBag.cart = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("CartItems"));
                ViewBag.ccc = 1;
            }
            else
            {
                ViewBag.ccc = 0;
            }


            DataSet maincat = Db.GetMainCategory(companyid);
            List<MainCategory> mainc = new List<MainCategory>();
            List<Category> cat = new List<Category>();
            List<SubCategory> scat = new List<SubCategory>();
            List<Locality_service> loc = new List<Locality_service>();
            if (maincat.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[0].Rows)
                {
                    mainc.Add(new MainCategory
                    {
                        Main_cat_id = Convert.ToInt32(dr["Main_cat_id"]),
                        Main_cat_name = Convert.ToString(dr["Main_cat_name"]),
                        Main_cat_status = Convert.ToBoolean(dr["Main_cat_status"]),
                        services = Convert.ToInt32(dr["services"]),
                        companyId = Convert.ToInt32(dr["companyId"])
                    });
                }
            }
            viewmodel.mainCategories = mainc;
            if (maincat.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[1].Rows)
                {
                    cat.Add(new Category
                    {
                        category_id = Convert.ToInt32(dr["category_id"]),
                        Main_cat_id = Convert.ToInt32(dr["Main_cat_id"]),
                        category_name = Convert.ToString(dr["category_name"]),
                        cat_status = Convert.ToBoolean(dr["cat_status"]),
                        company_id = Convert.ToInt32(dr["company_id"])
                    });
                }
            }
            viewmodel.categories = cat;
            if (maincat.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[2].Rows)
                {
                    scat.Add(new SubCategory
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

            return View("Default",viewmodel);
        }
    }
 

}
