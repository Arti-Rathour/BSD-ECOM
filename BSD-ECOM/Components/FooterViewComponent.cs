using BSD_ECOM.Models;
using BSD_ECOM.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace BSD_ECOM.Components
{
    public class FooterViewComponent:ViewComponent
    {
        ClsUtility util = new ClsUtility();
        DataBaseAccess Db = new DataBaseAccess();
        IndexViewModel viewmodel = new IndexViewModel();
        List<footerBanner> footerBanners = new List<footerBanner>();
        List<FooterInformation> footerInformations = new List<FooterInformation>();
        List<companyInformation> companyInformations = new List<companyInformation>();
        List<socaildata> socaildatas = new List<socaildata>();
        private IMemoryCache _cache;
        // MemoryCache myCache = new MemoryCache(new MemoryCacheOptions());
        public IViewComponentResult Invoke(IMemoryCache cache)
        {
            // _cache = cache;
            string companyid = HttpContext.Session.GetInt32("SiteId").ToString();
            DataSet maincat = new DataSet();
            maincat= (DataSet)TempData["maindata"];
            if (maincat == null)
            {
                maincat = Db.FrontPage(companyid);
            }

            List<MainBanner> mainBanners = new List<MainBanner>();
            if (maincat.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow dr in maincat.Tables[2].Rows)
                {
                    if (dr["typeid"].ToString() == "4")
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
            }
            viewmodel.mainBanners = mainBanners;
            //if (maincat.Tables[5].Rows.Count > 0)
            //{
            //    foreach (DataRow dr in maincat.Tables[5].Rows)
            //    {
            //        footerBanners.Add(new footerBanner
            //        {
            //            id = Convert.ToInt32(dr["ID"]),
            //            Footer_banner = Convert.ToString(dr["Footer_banner"]),
            //            footer_bannerUrl = Convert.ToString(dr["footer_bannerUrl"]),
            //        });
            //    }
            //}
            //viewmodel.footerBanners = footerBanners;
            DataSet dsFooter = Db.Footernew(companyid);
            if (dsFooter.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow dr in dsFooter.Tables[0].Rows)
                {
                    footerInformations.Add(new FooterInformation
                    {
                        id = Convert.ToInt32(dr["id"]),
                        name = Convert.ToString(dr["Name"]),
                        content = Convert.ToString(dr["content"]),
                        image = Convert.ToString(dr["img"]),
                        companyid = Convert.ToInt32(dr["companyId"]),
                    });
                }
            }
            viewmodel.footerInformations = footerInformations;

            if (dsFooter.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in dsFooter.Tables[1].Rows)
                {
                    companyInformations.Add(new companyInformation
                    {
                        id = Convert.ToInt32(dr["comp_id"]),
                        name = Convert.ToString(dr["CompanyName"]),
                        add1 = Convert.ToString(dr["Comp_Address"]),
                       // add2 = Convert.ToString(dr["img"]),
                        phone = Convert.ToString(dr["MobileNo"]),
                        email= Convert.ToString(dr["EmailID"]),
                        logo= Convert.ToString(dr["logo"]),
                    });
                }
            }
            viewmodel.companyInformations = companyInformations;


            if (dsFooter.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow dr in dsFooter.Tables[2].Rows)
                {
                    socaildatas.Add(new socaildata
                    {
                        
                        url = Convert.ToString(dr["url"]),
                        image = Convert.ToString(dr["image"]),
                        // add2 = Convert.ToString(dr["img"]),
                     
                    });
                }
            }
            viewmodel.socaildatas = socaildatas;
            return View("Footer", viewmodel);
        }
    }
}
