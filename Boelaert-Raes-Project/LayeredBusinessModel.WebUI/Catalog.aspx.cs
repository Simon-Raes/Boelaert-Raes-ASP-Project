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
        private CategoryService categoryService;
        private GenreService genreService;

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
            
            String labelText="";
            String genre_id = Request.QueryString["genre"];
            String category_id = Request.QueryString["cat"];
            String type = Request.QueryString["type"];
            String year = Request.QueryString["year"];
            String director = Request.QueryString["director"];
            String actor = Request.QueryString["actor"];
            String related = Request.QueryString["related"];

            if(type!=null)
            {
                if (type.Equals("popular"))
                {
                    dvdContent = new DvdInfoService().getMostPopularDvds(16);
                    labelText = "Most popular DVDs";
                }
                else if (type.Equals("recommended"))
                {
                    if (Session["user"] != null)
                    {
                        RecommendationsModel recModel = new RecommendationsModel();
                        dvdContent = recModel.getRecommendations(((Customer)Session["user"]), 16);
                        labelText = "Recommended for you";
                    }
                }
                else if (type.Equals("recent"))
                {
                    dvdContent = new DvdInfoService().getLatestDvds(16);
                    labelText = "Recent releases";
                }
            }            
            else if (genre_id != null)
            {
                dvdContent = dvdInfoService.searchDvdWithTextAndGenre(searchtext, genre_id);
                genreService = new GenreService();
                labelText = genreService.getGenre(genre_id).name + " DVDs";

            }
            else if (category_id != null)
            {
                dvdContent = dvdInfoService.searchDvdWithTextAndCategory(searchtext, category_id);
                categoryService = new CategoryService();
                labelText = categoryService.getCategoryByID(category_id).name + " DVDs";
            }
            else if (year != null)
            {
                dvdContent = dvdInfoService.searchDvdFromYear(year);
                labelText = "Dvd's from " + year;
            }
            else if (director != null)
            {
                dvdContent = dvdInfoService.searchDvdFromDirector(director);
                labelText = "Dvd's from " + director;
            }
            else if (actor != null)
            {
                dvdContent = dvdInfoService.searchDvdWithActor(actor);
                labelText = "Dvd's with " + actor;
            }
            else if (related != null)
            {
                dvdContent = dvdInfoService.getRelatedDvds(related,16);
                labelText = "Related dvds for " + dvdInfoService.getDvdInfoWithID(related).name;

            }
            else
            {
                dvdContent = dvdInfoService.searchDvdWithText(searchtext);
                labelText = "DVDs";
            }


            //set header text            
            if(!searchtext.Equals(""))
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

            if(dvdContent.Count<1)
            {
                lblStatus.Text = "Could not find any results matching your criteria.";
            }
        }
        

        /*DOESN'T WORK!*/
        protected void btnSearch_Click2(object sender, EventArgs e)
        {
            setDvdTiles();
        }



        /*DOESN'T WORK?!*/
        protected void Button1_Click1(object sender, EventArgs e)
        {
            setDvdTiles();
        }

    }
}