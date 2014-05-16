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
            if (!Page.IsPostBack)
            {
                setDvdTiles();
            }
        }

        private void setDvdTiles()
        {

            dvdInfoService = new DvdInfoService();
            List<DvdInfo> dvdContent = null;

            //hier nog wat geknoei omdat er 2 searchboxes zijn
            String searchtext;
            if (Request.QueryString["search"] != null)
            {
                searchtext = Request.QueryString["search"];
            }
            else
            {
                searchtext = txtSearchNew.Value;
            }

            String labelText = "";
            String genre_id = Request.QueryString["genre"];
            String category_id = Request.QueryString["cat"];
            String type = Request.QueryString["type"];
            String year = Request.QueryString["year"];
            String director = Request.QueryString["director"];
            String actor = Request.QueryString["actor"];
            String related = Request.QueryString["related"];

            if (type != null)
            {

                if (type.Equals("popular"))
                {
                    try
                    {
                        dvdContent = new DvdInfoService().getMostPopularDvds(16);           //Throws NoRecordException
                        labelText = "Most popular DVDs";
                    }
                    catch (NoRecordException)
                    {

                    }
                }
                else if (type.Equals("recommended"))
                {
                    if (Session["user"] != null)
                    {
                        try
                        {
                            dvdContent = new RecommendationsModel().getRecommendations(((Customer)Session["user"]), 16);          //Throws NoRecordException
                            labelText = "Recommended for you";
                        }
                        catch (NoRecordException)
                        {

                        }
                    }
                }
                else if (type.Equals("recent"))
                {
                    try
                    {
                        dvdContent = new DvdInfoService().getLatestDvds(16);                //Throws NoRecordException
                        labelText = "Recent releases";
                    }
                    catch (NoRecordException)
                    {

                    }
                }
            }
            else if (genre_id != null)
            {
                try
                {
                    dvdContent = dvdInfoService.searchDvdWithTextAndGenre(searchtext, genre_id);            //Throws NoRecordException
                    labelText = new GenreService().getByID(genre_id).name + " DVDs";          //Throws NorecordException
                }
                catch (NoRecordException)
                {

                }
            }
            else if (category_id != null)
            {
                try
                {
                    dvdContent = dvdInfoService.searchDvdWithTextAndCategory(searchtext, category_id);      //Throws NoRecordException
                    labelText = new CategoryService().getByID(category_id).name + " DVDs";         //Throws NoRecordException || DALException
                }
                catch (NoRecordException)
                {

                }
            }
            else if (year != null)
            {
                try
                {
                    dvdContent = dvdInfoService.searchDvdFromYear(year);            //Throws NoRecordException
                    labelText = "Dvd's from " + year;
                }
                catch (NoRecordException)
                {

                }
            }
            else if (director != null)
            {
                try
                {
                    dvdContent = dvdInfoService.searchDvdFromDirector(director);            //Throws NoRecordException
                    labelText = "Dvd's from " + director;
                }
                catch (NoRecordException)
                {

                }
            }
            else if (actor != null)
            {
                try
                {
                    dvdContent = dvdInfoService.searchDvdWithActor(actor);              //Throws NoRecordException        
                    labelText = "Dvd's with " + actor;
                }
                catch (NoRecordException)
                {

                }
            }
            else if (related != null)
            {
                try
                {
                    dvdContent = dvdInfoService.getRelatedDvds(related, 16);         //Throws NoRecordException
                    labelText = "Related dvds for " + dvdInfoService.getByID(related).name;        //Throws NoRecordException
                }
                catch (NoRecordException)
                {

                }

            }
            else
            {
                try
                {
                    dvdContent = dvdInfoService.searchDvdWithText(searchtext);          //Throws NoRecordException  
                    labelText = "DVDs";
                }
                catch (NoRecordException)
                {

                }
            }


            //set header text            
            if (!searchtext.Equals(""))
            {
                labelText += " matching '" + searchtext + "'";
            }
            lblHeader.Text = labelText;

            foreach (DvdInfo d in dvdContent)
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

                catalogContent.Controls.Add(dvdInfo);
            }

            if (dvdContent.Count < 1)
            {
                lblStatus.Text = "Could not find any results matching your criteria.";
            }
        }


        /*DOESN'T WORK!*/
        protected void btnSearch_Click2(object sender, EventArgs e)
        {
            setDvdTiles();
        }



        /*DOESN'T WORK?*/
        protected void Button1_Click1(object sender, EventArgs e)
        {
            setDvdTiles();
        }

    }
}