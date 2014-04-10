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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setupNewReleases();
                setupMostPopular();
            }
            
        }

        private void setupNewReleases()
        {
            List<DvdInfo> dvdList = new DvdInfoService().getLatestDvds(4);
            
            foreach (DvdInfo d in dvdList)
            {
                dvdInfoUserControl dvdInfo = (dvdInfoUserControl) Page.LoadControl("dvdInfoUserControl.ascx");
                dvdInfo.id = d.dvd_info_id;
                dvdInfo.imageUrl = d.image;
                dvdInfo.title = d.name;
                dvdInfo.buy_price = d.buy_price;
                dvdInfo.rent_price = d.rent_price;

                newReleases.Controls.Add(dvdInfo);                            
            }            
        }

        private void setupMostPopular()
        {
            List<DvdInfo> dvdList = new DvdInfoService().getMostPopularDvds(4);

            foreach (DvdInfo d in dvdList)
            {
                dvdInfoUserControl dvdInfo = (dvdInfoUserControl)Page.LoadControl("dvdInfoUserControl.ascx");
                dvdInfo.id = d.dvd_info_id;
                dvdInfo.imageUrl = d.image;
                dvdInfo.title = d.name;
                dvdInfo.buy_price = d.buy_price;
                dvdInfo.rent_price = d.rent_price;

                mostPopular.Controls.Add(dvdInfo);  
            }  
        }


       
    }
}