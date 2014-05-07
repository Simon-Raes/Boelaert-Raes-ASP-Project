using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL.Model;

namespace LayeredBusinessModel.WebUI
{
    public partial class dvdInfoUserControl : System.Web.UI.UserControl
    {
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
            btnBuy.Text = "Buy " + currency + " " + buy_price;
            btnRent.Text = "Rent " + currency + " "+  rent_price;

            if (AvailabilityModel.isAvailableForBuying(Convert.ToString(id)))
            {
                btnBuy.Attributes.Add("Class", "class='btn btn-success form-control'");
            }
            else
            {
                btnBuy.Attributes.Add("Class", "class='btn btn-warning form-control'");
            }
        }
    }
}