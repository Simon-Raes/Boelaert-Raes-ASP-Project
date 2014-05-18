using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using CustomException;

namespace LayeredBusinessModel.WebUI
{
    public partial class dvdInfoUserControl : System.Web.UI.UserControl
    {

        public delegate void delChoiceComplete(object sender, CustomEvents e);
        public event delChoiceComplete ChoiceComplete;

        public class CustomEvents : EventArgs
        {
            public String dvd_info_id { get; set; }
        }

        public String id { get; set; }
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
            if (title.Length <= 40)
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
                            buy_price = (float)currencyWebService.convert(buy_price, "usd");
                            rent_price = (float)currencyWebService.convert(rent_price, "usd");
                            break;
                    }
                }
            }
            else
            {
                switch (Request.QueryString["currency"])
                {
                    case "usd":
                        currency = "$";
                        buy_price = (float)currencyWebService.convert(buy_price, "usd");
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

            //set rent button properties and text                 
            btnRentB.Attributes.Add("CommandArgument", id);            

            DvdInfoService dvdbll = new DvdInfoService();
            try
            {
                DvdInfo thisDVD = dvdbll.getByID(Convert.ToString(id));                //Throws NoRecordExample
                List<DateTime> dates = new AvailabilityModel().getAvailabilities(thisDVD, DateTime.Now);            //Throws NoRecordException


                if (dates.Count >= 14)
                {
                    //fully available, green button
                    btnRentB.Attributes.Add("Class", "btn btn-success price-box");
                }
                else if (dates.Count > 0)
                {
                    //available on some days, orange button
                    btnRentB.Attributes.Add("Class", "btn btn-warning price-box");
                }
                else
                {
                    //not available at all, red button
                    btnRentB.Attributes.Add("Class", "btn btn-danger price-box");
                }
            }
            catch (NoRecordException)
            {

            }

            btnRentB.InnerText = "Rent " + currency + " " + rent_price;

        }
           
        protected void btnBuy_Click(object sender, EventArgs e)
        {     
            //add item to cart
            String dvd_info_id = id;

            Customer user = (Customer)Session["user"];

            if (user != null)
            {
                try
                {
                    DvdInfo thisDvd = new DvdInfoService().getByID(dvd_info_id.ToString());          //Throws NoRecordException
                    if (new ShoppingCartService().addByCustomerAndDvd(user, thisDvd))
                    {
                        //success
                    }
                }
                catch (NoRecordException)
                {

                }
            }
            else
            {
                string script = "alert(\"Please log in to buy this item.\");";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
            }                   
        }

        protected void btnRent_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/detail.aspx?id=" + id + "#rent");
        }
    }
}