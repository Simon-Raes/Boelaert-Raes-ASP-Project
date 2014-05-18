using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using CustomException;

namespace LayeredBusinessModel.WebUI
{
    public partial class detail : System.Web.UI.Page
    {
        private List<DateTime> availableDates;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Hide rent buttons until the user selects a start date
            lblItemAdded.Text = "";
            btnRent1.Visible = false;
            btnRent3.Visible = false;
            btnRent7.Visible = false;
                        
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
                    dvdInfoUserControl dvdInfoControl = (dvdInfoUserControl)Page.LoadControl("dvdInfoUserControl.ascx");
                    dvdInfoControl.id = d.dvd_info_id;
                    foreach (KeyValuePair<int, String> k in d.media)
                    {
                        if (k.Key == 1)
                        {
                            dvdInfoControl.imageUrl = k.Value;
                        }
                    }
                    dvdInfoControl.title = d.name;
                    dvdInfoControl.buy_price = d.buy_price;
                    dvdInfoControl.rent_price = d.rent_price;

                    relatedDvds.Controls.Add(dvdInfoControl);
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
                        lblBuyStatus.Text = "Item added to cart.";
                        lblBuyStatus.ForeColor = System.Drawing.Color.Green;
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
            //only excecute if a user is logged in
            Customer user = ((Customer)Session["user"]);

            if (user != null)
            {
                if (Request.QueryString["id"] != null)
                {
                    //check if the user can still rent additional items
                    if (new RentModel().getNumberOfActiveRentTotalCopiesForCustomer(user) < 5)
                    {
                        DateTime startdate = calRent.SelectedDate;
                        DateTime enddate = startdate.AddDays(days - 1);

                        //add item to cart
                        if (new ShoppingCartService().addByCustomerAndStartdateAndEnddate(user, Request.QueryString["id"].ToString(), startdate, enddate))
                        {
                            //all good 
                            lblItemAdded.Text = "Item added to cart.";
                            lblItemAdded.ForeColor = System.Drawing.Color.Green;                                                           
                        }
                        else
                        {
                            //something went wrong  
                            lblItemAdded.Text = "An error occurred when adding this item to your cart.";
                            lblItemAdded.ForeColor = System.Drawing.Color.Red;                                                          
                        }
                    }
                    else
                    {
                        string script = "alert(\"You are already renting 5 items. \");";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                    }
                }
            }
            else
            {
                string script = "alert(\"Please log in to buy this item.\");";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
            }
        }

        protected void calRent_DayRender(object sender, DayRenderEventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                //only get dates once per calendar build-up
                if (availableDates == null)
                {
                    try
                    {
                        DvdInfo thisDVD = new DvdInfoService().getByID(Request.QueryString["id"].ToString());                //Throws NoRecordException
                        availableDates = new AvailabilityModel().getAvailabilities(thisDVD, DateTime.Now);           //Throws NoRecordException
                    }
                    catch (NoRecordException)
                    {

                    }
                }

                //movie can be reserved between today and 14 days from now   
                if (availableDates.Contains(e.Day.Date))
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

        /*Shows the availabe rent period buttons.*/
        protected void calRent_SelectionChanged(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                try
                {
                    DvdInfo dvdInfo = new DvdInfoService().getByID(Request.QueryString["id"].ToString());            //Throws NoRecordException

                    //get all dvd copies that are available on that date:
                    int daysAvailable = new AvailabilityModel().getDaysAvailableFromDate(dvdInfo, calRent.SelectedDate);    //Throws NoRecordException

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