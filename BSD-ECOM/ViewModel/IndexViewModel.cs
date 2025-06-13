using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BSD_ECOM.Models;

namespace BSD_ECOM.ViewModel
{
    public class IndexViewModel
    {
        public List<MainCategory> mainCategories { get; set; }
        public List<Category> categories { get; set; }
        public List<SubCategory> subcategories { get; set; }
        public List<Locality_service> locality { get; set; }
        public List<Itemdetails> ItemdetailsS { get; set; }
        public List<FeatureBanner> featureBanners { get; set; }
        public List<MainBanner> mainBanners { get; set; }
        public List<footerBanner> footerBanners { get; set; }
        public List<ItemStroe>itemStroes { get; set; }
        public List<ItemStroerelated> ItemStroerelateds { get; set; }
        public List<About> abouts { get; set; }
        public List<DeliveryInformation> deliveryInfo { get; set; }
        public List<paymentInfo> paymentInfos { get; set; }
        public List<FAQ> fAQs { get; set; }
        public List<terms> term { get; set; }
        public List<shippingreturns> shippingreturns { get; set; }
        public List<privacy_policy> privacy_Policies { get; set; }
        public List<Cart> carts { get; set; }
        public List<welfare> Welfares { get; set; }
        public List<blog> blogs { get; set; }
        public List<CustomerReview> Reviews { get; set; }
        public List<FooterInformation> footerInformations { get; set; }
        public List<companyInformation> companyInformations { get; set; }
        public List<socaildata> socaildatas { get; set; }

        public List<customer> customers { get; set; }
        public List<customerOrder> customerOrders { get; set; }
    }
}
