using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL.Model;
using CustomException;

namespace LayeredBusinessModel.WebUI
{
    public partial class detail : System.Web.UI.Page
    {
        private List<DateTime> dates;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Hide rent buttons until the user selects a start date
            btnRent1.Visible = false;
            btnRent3.Visible = false;
            btnRent7.Visible = false;

            //disabled to fix images not loading after postback,
            //todo: ajax stuff
            //if (!IsPostBack)
            //{
            String dvd_info_id = Request.QueryString["id"];
            if (dvd_info_id != null)
            {
                setupDvdInfo(dvd_info_id);
                setupRelatedDvds(dvd_info_id);

                Customer user = (Customer)Session["user"];

                if (user != null)
                {
                    try
                    {
                        DvdInfo dvdInfo = new DvdInfoService().getByID(dvd_info_id);         //Throws NoRecordException
                        new PageVisitsModel().incrementPageVisits(user, dvdInfo);           //Throws NoRecordException
                    }
                    catch (NoRecordException)
                    {

                    }
                }
            }
            //}
        }

        private void setupDvdInfo(String id)
        {
            try
            {
                wsCurrencyWebService.CurrencyWebService currencyWebService = new wsCurrencyWebService.CurrencyWebService();
                String currency = "€";

                DvdInfo dvdInfo = new DvdInfoService().getByID(id.ToString());           //Throws NoRecordException
                
                lblTitle.Text = dvdInfo.name + " ";
                linkYear.Text = "(" + dvdInfo.year + ")";
                linkYear.NavigateUrl = "~/Catalog.aspx?year=" + dvdInfo.year;
                
                linkDirector.Text = dvdInfo.author;
                linkDirector.NavigateUrl = "~/Catalog.aspx?director=" + dvdInfo.author;
                
                if (!dvdInfo.actors[0].Equals("")) //even dvd's without actors contain 1 empty string element
                {
                    foreach (String a in dvdInfo.actors)
                    {
                        HyperLink actor = new HyperLink();
                        actor.Text = a;
                        actor.NavigateUrl = "~/Catalog.aspx?actor=" + a;
                        actorLinks.Controls.Add(actor);
                        Label l = new Label();
                        l.Text = ", ";
                        actorLinks.Controls.Add(l);
                    }
                    int i = actorLinks.Controls.Count;
                    actorLinks.Controls.RemoveAt(i - 1);
                    actorLinks.Controls.Add(new LiteralControl("<br />"));
                }
                else
                {
                    lblActors.Visible = false;
                }

                if (!dvdInfo.duration.Equals(""))
                {
                    lblDuration.Text = dvdInfo.duration + " min";
                }
                else
                {
                    spanRuntime.Visible = false;
                }

                foreach (Genre g in dvdInfo.genres)
                {
                    HyperLink genre = new HyperLink();
                    genre.Text = g.name;
                    genre.NavigateUrl = "~/Catalog.aspx?genre=" + g.genre_id;

                    genreLinks.Controls.Add(genre);
                    Label l = new Label();
                    l.Text = ", ";
                    genreLinks.Controls.Add(l);
                }
                int j = genreLinks.Controls.Count;
                if (j > 0)
                {
                    genreLinks.Controls.RemoveAt(j - 1);
                }

                lblPlot.Text = dvdInfo.descripion;
                
                if (Request.QueryString["currency"] == null)
                {
                    if (CookieUtil.CookieExists("currency"))
                    {
                        switch (CookieUtil.GetCookieValue("currency"))
                        {
                            case "usd":
                                currency = "$";
                                dvdInfo.buy_price = (float)currencyWebService.convert(dvdInfo.buy_price, "usd");
                                dvdInfo.rent_price = (float)currencyWebService.convert(dvdInfo.rent_price, "usd");
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
                            dvdInfo.buy_price = (float)currencyWebService.convert(dvdInfo.buy_price, "usd");
                            dvdInfo.rent_price = (float)currencyWebService.convert(dvdInfo.rent_price, "usd");
                            break;
                    }
                }

                if (AvailabilityModel.isAvailableForBuying(Request.QueryString["id"]))
                {
                    lblBuyStatus.Text = "";
                    btnBuyB.Attributes.Add("Class", "btn btn-success");
                }
                else
                {
                    lblBuyStatus.Text = "Item currently out of stock!";
                    btnBuyB.Attributes.Add("Class", "btn btn-warning");
                }

                btnBuyB.InnerText = "Buy " + currency + " " + dvdInfo.buy_price.ToString();
                btnRent1.Text = "Rent 1 day " + currency + " " + dvdInfo.rent_price.ToString();
                btnRent3.Text = "Rent 3 days " + currency + " " + (dvdInfo.rent_price * 3).ToString();
                btnRent7.Text = "Rent 7 days " + currency + " " + (dvdInfo.rent_price * 7).ToString();

                foreach (KeyValuePair<int, String> k in dvdInfo.media)
                {
                    if (k.Key == 1)
                    {
                        imgDvdCoverFocus.ImageUrl = k.Value;
                    }

                    else if (k.Key == 2)
                    {
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Attributes["class"] = "col-xs-3 col-sm-3 col-md-3 col-lg-3 DocumentItem";

                        Image img = new Image();
                        img.ImageUrl = k.Value;
                        div.Controls.Add(img);

                        scrollrow.Controls.Add(div);
                    }
                    else if (k.Key == 3)
                    {
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Attributes["class"] = "col-xs-3 col-sm-3 col-md-3 col-lg-3 DocumentItem";
                        Literal youtube = new Literal();
                        youtube.Text = GetYouTubeScript(k.Value);
                        div.Controls.Add(youtube);
                        scrollrow.Controls.Add(div);
                    }
                }
            }
            catch (NoRecordException)
            {

            }
        }

        private void setupRelatedDvds(String id)
        {
            try
            {
                pnlRelatedDvds.Visible = true;
                List<DvdInfo> list = new DvdInfoService().getRelatedDvds(id, 4);            //Throws NoRecordException

                linkRelated.NavigateUrl = "~/Catalog.aspx?related=" + id;
                foreach (DvdInfo d in list)
                {
                    dvdInfoUserControl dvdInfo = (dvdInfoUserControl)Page.LoadControl("dvdInfoUserControl.ascx");
                    dvdInfo.id = d.dvd_info_id;
                    foreach (KeyValuePair<int, String> k in d.media)
                    {
                        if (k.Key == 1)
                        {
                            dvdInfo.imageUrl = k.Value;
                        }
                    }
                    dvdInfo.title = d.name;
                    dvdInfo.buy_price = d.buy_price;
                    dvdInfo.rent_price = d.rent_price;

                    relatedDvds.Controls.Add(dvdInfo);
                }
            }
            catch (NoRecordException)
            {
                pnlRelatedDvds.Visible = false;
            }
        }

        protected string GetYouTubeScript(string id)
        {
            string scr = @"<object height='186'> ";
            scr = scr + @"<param name='movie' value='http://www.youtube.com/v/" + id + "'></param> ";
            scr = scr + @"<param name='allowFullScreen' value='true'></param> ";
            scr = scr + @"<param name='allowscriptaccess' value='always'></param> ";
            scr = scr + @"<embed src='http://www.youtube.com/v/" + id + "' ";
            scr = scr + @"type='application/x-shockwave-flash' allowscriptaccess='always' ";
            scr = scr + @"allowfullscreen='true' width='auto' height='186px'> ";
            scr = scr + @"</embed></object>";
            return scr;
        }

        /**Adds the buy dvd to the user's shopping cart*/
        protected void btnBuy_Click(object sender, EventArgs e)
        {
            Customer customer = (Customer)Session["user"];
            if (customer != null)
            {
                try
                {
                    DvdInfo dvdInfo = new DvdInfoService().getByID(Request.QueryString["id"]);           //Throws NoRecordException
                    if (new ShoppingCartService().addByCustomerAndDvd(customer, dvdInfo))
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
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<script type = 'text/javascript'>");
                sb.Append("window.onload=function(){");
                sb.Append("alert('");
                sb.Append("To do: alert user that he is not logged in");
                sb.Append("')};");
                sb.Append("</script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());        
            }
        }

        //todo: put rent panel in Ajax element so whole page doesn't need to refresh
        protected void btnRent1_Click(object sender, EventArgs e)
        {
            rentMovie(1);
        }

        protected void btnRent3_Click(object sender, EventArgs e)
        {
            rentMovie(3);
        }

        protected void btnRent7_Click(object sender, EventArgs e)
        {
            rentMovie(7);
        }

        private void rentMovie(int days)
        {
            //todo:delete this popup
            //string scripta = "alert(\"You clicked the rent button (DELETE THIS AGAIN)\");";
            //ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", scripta, true);

            //only excecute if a user is logged in
            Customer user = ((Customer)Session["user"]);

            if (user != null)
            {
                if (Request.QueryString["id"] != null)
                {
                    //add rent item to cart       
                    DateTime startdate = calRent.SelectedDate;
                    DateTime enddate = startdate.AddDays(days - 1);
                    
                    try
                    {
                        //check the number of rent items in the user's cart
                        List<ShoppingcartItem> cartContent = new ShoppingCartService().getCartContentForCustomer(user);           //Throws NoRecordException

                        int numberOfCurrentlyRentedItems = 0;
                        foreach (ShoppingcartItem item in cartContent)
                        {
                            if (item.dvdCopyType.name.Equals("Verhuur"))
                            {
                                numberOfCurrentlyRentedItems++;
                            }
                        }

                        //check the number of items currently being rented by the user
                        List<OrderLine> orderLines = new OrderLineService().getActiveRentOrderLinesByCustomer(user);          //Throws NoRecordException
                        foreach (OrderLine orderLine in orderLines)
                        {
                            numberOfCurrentlyRentedItems++;
                        }

                        //check if the user can still rent additional items
                        if (numberOfCurrentlyRentedItems < 5)
                        {
                            if (new ShoppingCartService().addByCustomerAndStartdateAndEndate(user, Request.QueryString["id"].ToString(), startdate, enddate))
                            {
                                ////all good
                                ////todo:delete this popup
                                //string scriptab = "alert(\"Item succesfully added to cart. (DELETE THIS AGAIN)\");";
                                //ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", scriptab, true);
                            }
                            else
                            {
                                ////something went wrong
                                ////todo:delete this popup
                                //string scriptab = "alert(\"Could not add this item to your cart. (DELETE THIS AGAIN)\");";
                                //ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", scriptab, true);
                            }
                        }
                        else
                        {
                            string script = "alert(\"You are already renting 5 items. (something something more info here) \");";
                            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                        }
                    }
                    catch (NoRecordException)
                    {

                    }
                }
            }
            else
            {
                string script = "alert(\"You have been logged out due to inactivity.\");";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
            }
            ////todo:delete this popup
            //string scriptcz = "alert(\"what\");";
            //ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", scriptcz, true);
        }

        protected void calRent_DayRender(object sender, DayRenderEventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                //only get dates once per calendar build-up
                if (dates == null)
                {
                    try
                    {
                        DvdInfo thisDVD = new DvdInfoService().getByID(Request.QueryString["id"].ToString());                //Throws NoRecordException
                        dates = new RentModel().getAvailabilities(thisDVD, DateTime.Now);           //Throws NoRecordException
                    }
                    catch (NoRecordException)
                    {

                    }
                }

                //movie can be reserved between today and 14 days from now   
                if (dates.Contains(e.Day.Date))
                {
                    e.Day.IsSelectable = true;
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    e.Day.IsSelectable = false;
                    e.Cell.BackColor = System.Drawing.Color.LightGray;
                }
            }
        }


        protected void calRent_SelectionChanged(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                try
                {
                    DvdInfo dvdInfo = new DvdInfoService().getByID(Request.QueryString["id"].ToString());            //Throws NoRecordException

                    //get all dvd copies that are available on that date:
                    int daysAvailable = new RentModel().getDaysAvailableFromDate(dvdInfo, calRent.SelectedDate);    //Throws NoRecordException

                    if (daysAvailable >= 1)
                    {
                        btnRent1.Visible = true;
                    }
                    if (daysAvailable >= 3)
                    {
                        btnRent3.Visible = true;
                    }
                    if (daysAvailable >= 7)
                    {
                        btnRent7.Visible = true;
                    }
                }
                catch (NoRecordException)
                {

                }
            }
        }
    }
}