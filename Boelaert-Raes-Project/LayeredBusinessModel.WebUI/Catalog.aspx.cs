using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using CustomException;

namespace LayeredBusinessModel.WebUI
{
    public partial class Catalog : System.Web.UI.Page
    {
        private DvdInfoService dvdInfoService;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setDvdTiles(null);
            }
        }

        private void setDvdTiles(String search)
        {
            String type = Request.QueryString["type"];

            dvdInfoService = new DvdInfoService();
            List<DvdInfo> dvdContent = null;
            
            String searchtext = "";
            if (Request.QueryString["search"] != null)
            {
                searchtext = Request.QueryString["search"];
            }

            String labelText = "";
            String genre_id = Request.QueryString["genre"];
            String category_id = Request.QueryString["cat"];

            String year = Request.QueryString["year"];
            String director = Request.QueryString["director"];
            String actor = Request.QueryString["actor"];
            String related = Request.QueryString["related"];
            
            try
            {

                if (type != null)
                {

                    switch (type)
                    {
                        case "popular":
                            labelText = "Most popular DVDs";
                            dvdContent = new DvdInfoService().getMostPopularDvds(16);           //Throws NoRecordException

                            break;
                        case "recommended":
                            if (Session["user"] != null)
                            {
                                labelText = "Recommended for you";
                                dvdContent = new RecommendationsModel().getRecommendations(((Customer)Session["user"]), 16);          //Throws NoRecordException

                            }
                            break;
                        case "recent":
                            labelText = "Recent releases";
                            dvdContent = new DvdInfoService().getLatestDvds(16);                //Throws NoRecordException

                            break;
                    }
                }
                else if (genre_id != null)
                {
                    labelText = new GenreService().getByID(genre_id).name + " DVDs";                        //Throws NoRecordException
                    dvdContent = dvdInfoService.searchDvdWithTextAndGenre(searchtext, genre_id);            //Throws NoRecordException

                }
                else if (category_id != null)
                {
                    labelText = new CategoryService().getByID(category_id).name + " DVDs";                  //Throws NoRecordException 
                    dvdContent = dvdInfoService.searchDvdWithTextAndCategory(searchtext, category_id);      //Throws NoRecordException

                }
                else if (year != null)
                {
                    labelText = "Dvd's from " + year;
                    dvdContent = dvdInfoService.searchDvdFromYear(year);                                    //Throws NoRecordException

                }
                else if (director != null)
                {
                    labelText = "Dvd's from " + director;
                    dvdContent = dvdInfoService.searchDvdFromDirector(director);                            //Throws NoRecordException

                }
                else if (actor != null)
                {
                    labelText = "Dvd's with " + actor;
                    dvdContent = dvdInfoService.searchDvdWithActor(actor);                                   //Throws NoRecordException    
                }
                else if (related != null)
                {
                    labelText = "Related dvds for " + dvdInfoService.getByID(related).name;                 //Throws NoRecordException
                    dvdContent = dvdInfoService.getRelatedDvds(related, 16);                                //Throws NoRecordException
                }
                else
                {
                    labelText = "Catalog";
                    dvdContent = dvdInfoService.searchDvdWithText(searchtext);                              //Throws NoRecordException  
                }

                //set header text            
                if (!searchtext.Equals(""))
                {
                    labelText += " matching '" + searchtext + "'";
                }


                foreach (DvdInfo d in dvdContent)
                {
                    if (search != null && checkMatches(d, search))
                    {
                        dvdInfoUserControl dvdInfo = (dvdInfoUserControl)Page.LoadControl("dvdInfoUserControl.ascx");
                        dvdInfo.ChoiceComplete += new dvdInfoUserControl.delChoiceComplete(dvdInfo_ChoiceComplete);
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
                        catalogContent.Controls.Add(dvdInfo);
                    }
                    else if (search == null)
                    {
                        dvdInfoUserControl dvdInfo = (dvdInfoUserControl)Page.LoadControl("dvdInfoUserControl.ascx");
                        dvdInfo.ChoiceComplete += new dvdInfoUserControl.delChoiceComplete(dvdInfo_ChoiceComplete);
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
                        catalogContent.Controls.Add(dvdInfo);
                    }
                }
                lblHeader.Text = labelText;
            }
            catch (NoRecordException)
            {
                lblHeader.Text = labelText;
                lblStatus.Text = "Could not find any results matching your criteria.";
            }
        }

        void dvdInfo_ChoiceComplete(object sender, dvdInfoUserControl.CustomEvents e)
        {
            //not used, event is handled by the userControl
        }

        private Boolean checkMatches(DvdInfo d, String text)
        {            
            if (d.name.ToLower().Contains(text.ToLower()))
            {
                return true;
            }
            return false;
        }

        /*Lets a user refine his catalog result with a second search.*/
        protected void btnSearch2_Click(object sender, EventArgs e)
        {
            String searchText = txtSearchNewer.Value;
            setDvdTiles(searchText);
        } 
    }
}