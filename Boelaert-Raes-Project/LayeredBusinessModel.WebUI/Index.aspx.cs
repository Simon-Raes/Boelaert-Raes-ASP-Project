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
            DvdInfoService dvdInfoService = new DvdInfoService();
            List<DvdInfo> dvdsWithBanner = dvdInfoService.getAllDvdInfosWithBanner();

            //selects a random dvd with a banner image to display as spotlight, could be set using an admin module
            if(dvdsWithBanner.Count>0)
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

        private void setupRecommendations()
        {
            RecommendationsModel recModel = new RecommendationsModel();
            List<DvdInfo> dvdList = recModel.getRecommendations(user, 4);
            if (dvdList.Count > 0)
            {
                addTilesToRow(dvdList, recommened);
                divRecommended.Visible = true;
            }
            else
            {
                //user has not visited any pages and has not ordered any items, HIDE the recommendations panel
                divRecommended.Visible = false;
            }
                    
        }

        private void setupNewReleases()
        {
            List<DvdInfo> dvdList = new DvdInfoService().getLatestDvds(4);
            addTilesToRow(dvdList, newReleases);            
        }

        private void setupMostPopular()
        {
            List<DvdInfo> dvdList = new DvdInfoService().getMostPopularDvds(4);
            addTilesToRow(dvdList, mostPopular);            
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
                DvdInfoService dvdInfoService = new DvdInfoService();
                DvdInfo thisDvd = dvdInfoService.getDvdInfoWithID(dvd_info_id.ToString());

                if (thisDvd != null)
                {                    
                    ShoppingCartService shoppingCartService = new ShoppingCartService();
                    shoppingCartService.addItemToCart(user, thisDvd);
                }
            }
            else
            {
                //todo: please sign in!
            }
        }

       
    }
}