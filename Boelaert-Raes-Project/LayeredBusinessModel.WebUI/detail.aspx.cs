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
using LayeredBusinessModel.BLL.Model;

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

            //if (!IsPostBack)
            //{
                if (Request.QueryString["id"] != null)
                {
                    int id;
                    if (int.TryParse(Request.QueryString["id"], out id))
                    {
                        setupDvdInfo(id);
                        setupRelatedDvds(id);
                    }

                    Customer user = (Customer)Session["user"];
                    if (user != null)
                    {
                        PageVisitsModel pageVisitsModel = new PageVisitsModel();
                        DvdInfoService dvdInfoService = new DvdInfoService();
                        DvdInfo dvdInfo = dvdInfoService.getDvdInfoWithID(id.ToString());
                        pageVisitsModel.incrementPageVisits(user, dvdInfo);
                    }
                }                
            //}
        }

        private void setupDvdInfo(int id)
        {
            wsCurrencyWebService.CurrencyWebService currencyWebService = new wsCurrencyWebService.CurrencyWebService();
            String currency = "€";

            DvdInfoService dvdbll = new DvdInfoService();
            DvdInfo dvdInfo = dvdbll.getDvdInfoWithID(id.ToString());
           


            lblTitle.Text = dvdInfo.name + " ";
            linkYear.Text = "(" + dvdInfo.year + ")";
            linkYear.NavigateUrl = "~/Catalog.aspx?year=" + dvdInfo.year;


            linkDirector.Text = dvdInfo.author;
            linkDirector.NavigateUrl = "~/Catalog.aspx?director=" + dvdInfo.author;


            if(!dvdInfo.actors[0].Equals("")) //even dvd's without actors contain 1 empty string element
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
            

            if(!dvdInfo.duration.Equals(""))
            {
                lblDuration.Text = dvdInfo.duration + " min";
            } else
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
            if(j>0)
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
                            dvdInfo.buy_price = currencyWebService.convert(dvdInfo.buy_price,"usd");
                            dvdInfo.rent_price = currencyWebService.convert(dvdInfo.rent_price, "usd");
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
                        dvdInfo.buy_price = currencyWebService.convert(dvdInfo.buy_price,"usd");
                        dvdInfo.rent_price = currencyWebService.convert(dvdInfo.rent_price, "usd");
                        break;
                }
            }





            btnBuy.Text = "Buy " + currency + " " + dvdInfo.buy_price.ToString();
            btnRent1.Text = "Rent 1 day " + currency + " "  + dvdInfo.rent_price.ToString();
            btnRent3.Text = "Rent 3 days " + currency + " "  + (dvdInfo.rent_price * 3).ToString();
            btnRent7.Text = "Rent 7 days " + currency + " "  + (dvdInfo.rent_price * 7).ToString();

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

        private void setupRelatedDvds(int id)
        {
            List<DvdInfo> list = new DvdInfoService().getRelatedDvds(id,4);
            if (list.Count == 0)
            {
                pnlRelatedDvds.Visible = false;
            }
            else
            {
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
            if (Request.QueryString["id"] != null)
            {
                ShoppingCartService shoppingCartService = new ShoppingCartService();
                shoppingCartService.addItemToCart(((Customer)Session["user"]).customer_id, Convert.ToInt32(Request.QueryString["id"]));
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
            //only excecute if a user is logged in
            if (Session["user"] != null)
            {
                Customer user = ((Customer)Session["user"]);

                //add rent item to cart       
                DateTime startdate = calRent.SelectedDate;
                DateTime enddate = startdate.AddDays(days-1);

                DvdCopyService dvdCopyService = new DvdCopyService();
                List<DvdCopy> availabeCopies = dvdCopyService.getAllInStockRentCopiesForDvdInfo(Request.QueryString["id"]);

                //check the number of rent items in the user's cart
                ShoppingCartService shoppingCartService = new ShoppingCartService();
                List<ShoppingcartItem> cartContent = shoppingCartService.getCartContentForCustomer(user.customer_id);
                int numberOfCurrentlyRentedItems = 0;
                foreach (ShoppingcartItem item in cartContent)
                {
                    if (item.typeName.Equals("Verhuur"))
                    {
                        numberOfCurrentlyRentedItems++;
                    }
                }

                //check the number of items currently being rented by the user
                OrderLineService orderLineService = new OrderLineService();
                List<OrderLine> orderLines = orderLineService.getActiveRentOrderLinesForCustomer(user.customer_id);
                foreach (OrderLine orderLine in orderLines)
                {
                    numberOfCurrentlyRentedItems++;
                }

                //check if the user can still rent additional items
                if (numberOfCurrentlyRentedItems < 5)
                {
                    shoppingCartService.addItemToCart(user.customer_id, Convert.ToInt32(Request.QueryString["id"]), startdate, enddate);

                }
                else
                {
                    string script = "alert(\"You are already renting 5 items. (something something more info here) \");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                }

            }
            else
            {
                string script = "alert(\"You have been logged out due to inactivity.\");";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);

            }
        }

        protected void calRent_DayRender(object sender, DayRenderEventArgs e)
        {            
            if (Request.QueryString["id"] != null)
            {
                //only get dates once per calendar build-up
                if(dates == null)
                {
                    RentModel rentService = new RentModel();

                    DvdInfoService dvdbll = new DvdInfoService();
                    DvdInfo thisDVD = dvdbll.getDvdInfoWithID(Request.QueryString["id"].ToString());

                    dates = rentService.getAvailabilities(thisDVD, DateTime.Now);
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
            if(Request.QueryString["id"]!=null)
            {
                DvdInfoService dvdbll = new DvdInfoService();
                DvdInfo dvdInfo = dvdbll.getDvdInfoWithID(Request.QueryString["id"].ToString());

                //get all dvd copies that are available on that date:  
                RentModel rentService = new RentModel();
                int daysAvailable = rentService.getDaysAvailableFromDate(dvdInfo, calRent.SelectedDate);
                
                if(daysAvailable>=1)
                {
                    btnRent1.Visible = true;
                }
                if(daysAvailable>=3)
                {
                    btnRent3.Visible = true;
                }
                if(daysAvailable>=7)
                {
                    btnRent7.Visible = true;
                }
            }
        }
    }
}