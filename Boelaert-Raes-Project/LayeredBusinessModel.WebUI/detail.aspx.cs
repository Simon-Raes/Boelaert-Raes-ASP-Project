using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace LayeredBusinessModel.WebUI
{
    public partial class detail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int id;
                    if (int.TryParse(Request.QueryString["id"], out id))
                    {
                        setupDvdInfo(id);
                    }
                }
            }
        }

        private void setupDvdInfo(int id)
        {
            DvdInfoService dvdbll = new DvdInfoService();
            DvdInfo dvdInfo = dvdbll.getDvdInfoWithID(id.ToString());


            lblTitle.Text = dvdInfo.name + " ";
            linkYear.Text = "(" + dvdInfo.year + ")";
            linkYear.NavigateUrl = "~/Catalog.aspx?year=" + dvdInfo.year;


            linkDirector.Text = dvdInfo.author;
            linkDirector.NavigateUrl = "~/Catalog.aspx?director=" + dvdInfo.author;


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
            lblDuration.Text = dvdInfo.duration + " min";


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
            genreLinks.Controls.RemoveAt(j - 1);

            lblPlot.Text = dvdInfo.descripion;
            btnBuy.Text = "Buy € " + dvdInfo.buy_price.ToString();
            btnRent1.Text = "Rent 1 day € " + dvdInfo.rent_price.ToString();
            btnRent3.Text = "Rent 3 days € " + (dvdInfo.rent_price * 3).ToString();
            btnRent7.Text = "Rent 7 days € " + (dvdInfo.rent_price * 7).ToString();

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

        protected void btnBuy_Click(object sender, EventArgs e)
        {
            List<DvdCopy> availabeCopies = null;


            //get all available copies of this movie + type (buy/rent)
            DvdCopyService dvdCopyService = new DvdCopyService();

            availabeCopies = dvdCopyService.getAllInStockBuyCopiesForDvdInfo("1"); //1 = hardcoded op shawshank 

            //only allow purchase if at least one copy is available
            //a user can still add 100 copies to his cart as long as 1 is in stock, not sure if there's a better solution for this
            //if (availabeCopies.Count > 0)
            //{
            ShoppingCartService shoppingCartService = new ShoppingCartService();
            shoppingCartService.addItemToCart(((Customer)Session["user"]).customer_id, Convert.ToInt32("1"));
            //}
            //else
            //{
            //    //tijdelijke messagebox in afwachting van een cleanere oplossing (zoals verbergen van buy/rent knop, greyed out knop, "out of stock" bericht...)
            //    //todo: show date when the dvd will be back in stock + option to reserve
            //    string script = "alert(\"Item niet meer in stock!\");";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
            //}
        }

    }
}