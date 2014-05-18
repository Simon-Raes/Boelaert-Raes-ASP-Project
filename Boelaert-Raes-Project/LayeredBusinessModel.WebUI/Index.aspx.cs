using CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;

namespace LayeredBusinessModel.WebUI
{
    public partial class Home : System.Web.UI.Page
    {
        Customer user;

        protected void Page_Load(object sender, EventArgs e)
        {            
            user = (Customer)Session["user"];

            //only show recommendations for a logged in user
            if (user != null)
            {
                setupRecommendations();
            }
            setupSpotlight();
            setupNewReleases();
            setupMostPopular();
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
                divRecent.Visible = true;
                List<DvdInfo> dvdList = new DvdInfoService().getLatestDvds(4);              //Throws NoRecordException()
                addTilesToRow(dvdList, newReleases);
            }
            catch (NoRecordException)
            {
                divRecent.Visible = false;
            }
        }

        private void setupMostPopular()
        {
            try
            {
                divPopular.Visible = true;
                List<DvdInfo> dvdList = new DvdInfoService().getMostPopularDvds(4);         //Throws NoRecordException
                addTilesToRow(dvdList, mostPopular);
            }
            catch (NoRecordException)
            {
                divPopular.Visible = false;
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
            //not used            
        }
    }
}