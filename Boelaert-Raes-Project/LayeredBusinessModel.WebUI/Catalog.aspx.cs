using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;

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
            String genre = Request.QueryString["genre"];
            String cat = Request.QueryString["cat"];
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
                        dvdContent = UserRecommendations.getRecommendations(((Customer)Session["user"]).customer_id, 16);
                        labelText = "Recommended for you";
                    }
                }
                else if (type.Equals("recent"))
                {
                    dvdContent = new DvdInfoService().getLatestDvds(16);
                    labelText = "Recent releases";
                }
            }            
            else if (genre != null)
            {
                dvdContent = dvdInfoService.searchDvdWithTextAndGenre(searchtext, genre);
                genreService = new GenreService();
                labelText = genreService.getGenre(Convert.ToInt32(genre)).name + " DVDs";

            }
            else if (cat != null)
            {
                dvdContent = dvdInfoService.searchDvdWithTextAndCategory(searchtext, cat);
                categoryService = new CategoryService();
                labelText = categoryService.getCategory(Convert.ToInt32(cat)).name + " DVDs";
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
                dvdContent = dvdInfoService.getRelatedDvds(Convert.ToInt32(related));
                labelText = "Related dvds for" + dvdInfoService.getDvdInfoWithID(related).name;

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