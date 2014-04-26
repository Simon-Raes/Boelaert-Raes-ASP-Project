using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace LayeredBusinessModel.WebUI
{
    /// <summary>
    /// Summary description for CurrencyWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CurrencyWebService : System.Web.Services.WebService
    {

        [WebMethod(Description="Currency web service", EnableSession=true)]
        public decimal convert(float price, String currency)
        {
            switch (currency)
            {
                case "euro":
                    return Math.Round(Convert.ToDecimal(price),2);
                case "usd":
                    return Math.Round(Convert.ToDecimal(price * 1.27F), 2);
            }
            return -1;
        }
    }
}
