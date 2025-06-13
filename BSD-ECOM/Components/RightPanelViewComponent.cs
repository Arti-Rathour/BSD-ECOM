using BSD_ECOM.Models;
using BSD_ECOM.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BSD_ECOM.Components
{
    public class RightPanelViewComponent:ViewComponent
    {
        ClsUtility util = new ClsUtility();
        DataBaseAccess Db = new DataBaseAccess();
        IndexViewModel viewmodel = new IndexViewModel();
        List<Category> cat = new List<Category>();
        List<SubCategory> scat = new List<SubCategory>();
        public IViewComponentResult Invoke()
        {
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            string Id = HttpContext.Session.GetString("Id").ToString();
            string type = HttpContext.Session.GetString("type").ToString();
            string Main_cat_id = HttpContext.Session.GetString("Main_cat_id").ToString();
            DataSet maincat = Db.Getrightpannel(companyid, Id, type, Main_cat_id);
            List<ItemStroe> itemStroess = new List<ItemStroe>();
            if (maincat.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[0].Rows)
                {
                    scat.Add(new SubCategory
                    {
                        cat_id = Convert.ToInt32(dr["category_id"]),
                        Main_cat_id = Convert.ToInt32(dr["Main_cat_id"]),
                        cat_name = Convert.ToString(dr["category_name"]),
                        cat_status = Convert.ToBoolean(dr["cat_status"]),
                        company_id = Convert.ToInt32(dr["Company_id"]),
                        //category_id = Convert.ToInt32(dr["category_id"]),
                        //Image = Convert.ToString(dr["Image"]),
                    });
                }
            }
            viewmodel.subcategories = scat;

            if (maincat.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[1].Rows)
                {
                    itemStroess.Add(new ItemStroe
                    {
                        Id = Convert.ToInt32(dr["id"]),
                        ItemName = Convert.ToString(dr["ItemName"]),
                        Frontimage = Convert.ToString(dr["image"]),
                        Backimage = Convert.ToString(dr["image1"]),
                        Rightimage = Convert.ToString(dr["image2"]),
                        Leftimage = Convert.ToString(dr["image3"]),
                        URLName = Convert.ToString(dr["URLName"]),
                        productTag = Convert.ToString(dr["productTag"]),
                        itemdetailsId = Convert.ToInt32(dr["itemdetid"]),
                        CategoryId = Convert.ToInt32(dr["CategoryId"]),


                    });
                }
            }
            viewmodel.itemStroes = itemStroess;
            return View("RightPanel", viewmodel);
        }
    }
}
