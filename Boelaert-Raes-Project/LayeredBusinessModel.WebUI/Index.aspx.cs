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
    public partial class Home : System.Web.UI.Page
    {
        Customer user;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                user = (Customer)Session["user"];

                //only show recommendations for a logged in user
                if (user != null)
                {
                    divRecommended.Visible = true;
                    setupRecommendations();
                }
                else
                {
                    divRecommended.Visible = false;
                }

                setupSpotlight();
                setupNewReleases();
                setupMostPopular();
            }
        }

        private void setupSpotlight()
        {
            DvdInfoService dvdInfoService = new DvdInfoService();
            List<DvdInfo> dvdsWithBanner = dvdInfoService.getAllDvdInfosWithBanner();

            //selects a random dvd with a banner image to display as spotlight, could be set using an admin module
            Random rnd = new Random();
            DvdInfo spotlightDvd = dvdsWithBanner[rnd.Next(dvdsWithBanner.Count)];

            anchorSpotlight.HRef = "Detail.aspx?id=" + spotlightDvd.dvd_info_id;
            foreach (KeyValuePair<int, String> k in spotlightDvd.media)
            {
                if(k.Key == 4)
                {
                    imgSpotlight.Src = k.Value;
                }
            }  
        }

        private void setupRecommendations()
        {
            List<DvdInfo> dvdList = UserRecommendations.getRecommendations(user.customer_id, 4);
            addTilesToRow(dvdList, recommened);            
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
    }
}