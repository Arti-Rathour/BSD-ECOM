using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSD_ECOM.Models
{
    public class InvoiceModel
    {
        public string companyname { get; set; }
        public string companyaddress { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyCityName { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyGstNo { get; set; }
        public string invoicenumber { get; set; }
        public string Gstin { get; set; }
        public string orderid { get; set; }
        public string orderdate { get; set; }
        public string invoicedate { get; set; }
        public string pan { get; set; }
        public string cin { get; set; }
        public string bname { get; set; }
        public string blastname { get; set; }
        public string baddress1 { get; set; }
        public string baddress2 { get; set; }
        public string bcountry { get; set; }
        public string bcity { get; set; }
        public string bstate { get; set; }
        public string bphone { get; set; }
        public string bPincode { get; set; }
        public string bemail { get; set; }
        public string sname { get; set; }
        public string slastname { get; set; }
        public string saddress1 { get; set; }
        public string saddress2 { get; set; }
        public string sCountry { get; set; }
        public string scity { get; set; }
        public string sstate { get; set; }
        public string sphone { get; set; }
        public string sPincode { get; set; }
        public string semail { get; set; }
        public string productname { get; set; }
        public string titlename { get; set; }
        public string quantity { get; set; }

        public double Walletamtdetect { get; set; }
        public double Grossamount { get; set; }
        public double discount { get; set; }
        public double taxablevalue { get; set; }
        public double igist { get; set; }
        public double total { get; set; }
        public double grandtotal { get; set; }
        public string amountinword { get; set; }
        public string paymentmode { get; set; }
        public string Subscription { get; set; }
        public string SubRs { get; set; }
        public double subtotal { get; set; }
        public double amount { get; set; }
        public string reference { get; set; }
        public int itemid { get; set; }
        public string itemname { get; set; }
        public int Qty { get; set; }
        public string DispatchDate { get; set; }
        public int status { get; set; }
        public decimal Taxable_Value { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal itemTotalPrice { get; set; }
    }
}
