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
                            buy_price = (float) currencyWebService.convert(buy_price,"usd");
                            rent_price = (float) currencyWebService.convert(rent_price, "usd");
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
                        buy_price = (float)currencyWebService.convert(buy_price,"usd");
                        rent_price = (float)currencyWebService.convert(rent_price, "usd");
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
            RentModel rentService = new RentModel();
            DvdInfoService dvdbll = new DvdInfoService();
            DvdInfo thisDVD = dvdbll.getDvdInfoWithID(Convert.ToString(id));
            List<DateTime> dates = rentService.getAvailabilities(thisDVD, DateTime.Now);

            if(dates.Count>=14)
            {
                //fully available, green button
                btnRentB.Attributes.Add("Class", "btn btn-success price-box");
            }
            else if(dates.Count>0)
            {
                //available on some days, orange button
                btnRentB.Attributes.Add("Class", "btn btn-warning price-box");
            }
            else
            {
                //not available at all, red button
                btnRentB.Attributes.Add("Class", "btn btn-danger price-box");
            }

            btnRentB.InnerText = "Rent " + currency + " " + rent_price;

        }

        //todo: fix postback bug
        //site uses ID of movie that will take up the place of the selected movie
        //click on godfather, page refreshes, pulp fiction takes up that place now, pulp fiction gets added to cart
        //^fix that! querystring (addToCart?id=xxx) if no other solution works
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