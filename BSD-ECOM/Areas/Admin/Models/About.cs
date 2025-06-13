using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSD_ECOM.Areas.Admin.Models
{
   public class About
    {
        public int ID { set; get; }

        public string about { set; get; }

        public int CompanyID { set; get; }
    }
   public class Home
    {
        public int ID { set; get; }

        public string home { set; get; }

        public int CompanyID { set; get; }
    }
   public class DeliveryInformation
    {
        public int ID { set; get; }

        public string Delivery_Info { set; get; }

        public int CompanyID { set; get; }



    }
   public class Payment
    {
        public int ID { set; get; }

        public string payment { set; get; }

        public int CompanyID { set; get; }



    }
   public class FAQ
    {
        public int ID { set; get; }

        public string Faq { set; get; }

        public int CompanyID { set; get; }
    }


    public class Terms
    {
        public int ID { set; get; }

        public string terms { set; get; }

        public int CompanyID { set; get; }
    }


    public class PrivacyPolish
    {
        public int ID { set; get; }

        public string Privacypolish { set; get; }

        public int CompanyID { set; get; }



    }


    public class Shipping
    {
        public int ID { set; get; }

        public string shipping { set; get; }

        public int CompanyID { set; get; }
    }

    public class Createuser
    {
        public int ID { set; get; }

        public string Name { set; get; }

        public string Email { set; get; }

        public string Password { set; get; }

        public string MobileNo { set; get; }

        public int Type { set; get; }

        public int Status { set; get; }

        public int CompanyID { set; get; }
    }
    public class Country
    {
        public int ID { set; get; }

        public string country { set; get; }

        public int CompanyID { set; get; }
    }
    public class State
    {
        public int ID { set; get; }

        public int CID { set; get; }

        public int Status { set; get; }

        public string countryname { get; set; }
        public string state { set; get; }
        public int CompanyID { set; get; }

    }
    public class City
    {
        public int ID { set; get; }

        public int CID { set; get; }

        public int SID { set; get; }

        public int Status { set; get; }
        public string city { get; set; }

        public string countryname { get; set; }
        public string state { set; get; }
        public int CompanyID { set; get; }

    }
    public class Location
    {
        public int ID { set; get; }

        public string LocationName { set; get; }

        public int PinCode { set; get; }

        public int Status { set; get; }

        public int CompanyID { set; get; }
    }

}
