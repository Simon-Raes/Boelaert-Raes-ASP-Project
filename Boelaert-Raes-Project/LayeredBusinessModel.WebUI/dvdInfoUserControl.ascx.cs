using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL.Model;
using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;

namespace LayeredBusinessModel.WebUI
{
    public partial class dvdInfoUserControl : System.Web.UI.UserControl
    {

        public delegate void delChoiceComplete(object sender, CustomEvents e);
        public event delChoiceComplete ChoiceComplete;

        public class CustomEvents : EventArgs
        {
            public int dvd_info_id { get; set; }
        }

        public int id { get; set; }
        public String imageUrl { get; set; }
        public String title { get; set; }
        public float buy_price { get; set; }
        public float rent_price { get; set; }        


        protected void Page_Load(object sender, EventArgs e)
        {
            String currency = "€";
            wsCurrencyWebService.CurrencyWebService currencyWebService = new wsCurrencyWebService.CurrencyWebService();

            dvdInfoLink.NavigateUrl = "~/detail.aspx?id=" + id;
            dvdInfoLink2.NavigateUrl = dvdInfoLink.NavigateUrl;
            imgDvdCover.ImageUrl = imageUrl;
            if(title.Length<=40)
            {
                lblTitle.Text = title;
            }
            else
            {
                lblTitle.Text = title.Substring(0, 28) + "...";
            }


            if (Request.QueryString["currency"] == null)
            {
                if (CookieUtil.CookieExists("currency"))
                {
                    switch (CookieUtil.GetCookieValue("currency"))
                    {
                        case "usd":
                            currency = "$";
                            buy_price = currencyWebService.convert(buy_price,"usd");
                            rent_price = currencyWebService.convert(rent_price, "usd");
                            break;
                    }
                }
            }
            else
            {
                switch (Request.QueryString["currency"])
                {
                    case "usd":
                        currency="$";
                        buy_price = currencyWebService.convert(buy_price,"usd");
                        rent_price = currencyWebService.convert(rent_price, "usd");
                        break;
                }
            }
            
            //set buy button color and text
            if (AvailabilityModel.isAvailableForBuying(Convert.ToString(id)))
            {
                btnBuyB.Attributes.Add("Class", "btn btn-success price-box");
            }
            else
            {
                btnBuyB.Attributes.Add("Class", "btn btn-warning price-box");
            }
            btnBuyB.InnerText = "Buy " + currency + " " + buy_price;
            //set rent button text
            btnRent.Text = "Rent " + currency + " " + rent_price;

        }

        protected void btnBuy_Click(object sender, EventArgs e)
        {
            CustomEvents ce = new CustomEvents();
            ce.dvd_info_id = id;
            ChoiceComplete(this, ce);

            

        }

        protected void btnRent_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/detail.aspx?id=" + id + "#rent");
        }
    }
}