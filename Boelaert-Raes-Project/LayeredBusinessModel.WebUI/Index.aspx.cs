using CustomException;
using LayeredBusinessModel.BLL;
using LayeredBusinessModel.BLL.Model;
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
    public partial class Home : System.Web.UI.Page
    {
        Customer user;

        protected void Page_Load(object sender, EventArgs e)
        {
            //disabled for now so tiles reload after clicking the buy button on a movie tile

            //if (!Page.IsPostBack)
            //{
            user = (Customer)Session["user"];

            //only show recommendations for a logged in user
            if (user != null)
            {
                setupRecommendations();
            }
            setupSpotlight();
            setupNewReleases();
            setupMostPopular();
            //}
        }

        private void setupSpotlight()
        {
            try
            {
                List<DvdInfo> dvdsWithBanner = new DvdInfoService().getAllWithBanner();               //Throws NoRecordException

                //selects a random dvd with a banner image to display as spotlight, could be set using an admin module
                if (dvdsWithBanner.Count > 0)
                {
                    Random rnd = new Random();
                    DvdInfo spotlightDvd = dvdsWithBanner[rnd.Next(dvdsWithBanner.Count)];

                    anchorSpotlight.HRef = "Detail.aspx?id=" + spotlightDvd.dvd_info_id;
                    foreach (KeyValuePair<int, String> k in spotlightDvd.media)
                    {
                        if (k.Key == 4)
                        {
                            imgSpotlight.Src = k.Value;
                        }
                    }
                }
            }
            catch (NoRecordException)
            {

            }
        }

        private void setupRecommendations()
        {
            try
            {
                divRecommended.Visible = true;
                List<DvdInfo> dvdList = new RecommendationsModel().getRecommendations(user, 4);           //Throws NoRecordException()
                addTilesToRow(dvdList, recommened);
            }
            catch (NoRecordException)
            {
                divRecommended.Visible = false;
            }
        }

        private void setupNewReleases()
        {
            try
            {
                List<DvdInfo> dvdList = new DvdInfoService().getLatestDvds(4);              //Throws NoRecordException()
                addTilesToRow(dvdList, newReleases);
            }
            catch (NoRecordException)
            {
                
            }
        }

        private void setupMostPopular()
        {
            try
            {
                List<DvdInfo> dvdList = new DvdInfoService().getMostPopularDvds(4);         //Throws NoRecordException
                addTilesToRow(dvdList, mostPopular);
            }
            catch (NoRecordException)
            {

            }
        }


        private void addTilesToRow(List<DvdInfo> dvds, HtmlGenericControl row)
        {
            foreach (DvdInfo d in dvds)
            {
                dvdInfoUserControl dvdInfo = (dvdInfoUserControl)Page.LoadControl("dvdInfoUserControl.ascx");
                dvdInfo.ChoiceComplete += new dvdInfoUserControl.delChoiceComplete(eventComplete);

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

                row.Controls.Add(dvdInfo);
            }
        }

        public void eventComplete(object sender, dvdInfoUserControl.CustomEvents e)
        {
            //add item to cart
            int dvd_info_id = e.dvd_info_id;

            Customer user = (Customer)Session["user"];

            if (user != null)
            {
                try
                {
                    DvdInfo thisDvd = new DvdInfoService().getByID(dvd_info_id.ToString());          //Throws NoRecordException
                    new ShoppingCartService().addItemToCart(user, thisDvd);
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


    }
}