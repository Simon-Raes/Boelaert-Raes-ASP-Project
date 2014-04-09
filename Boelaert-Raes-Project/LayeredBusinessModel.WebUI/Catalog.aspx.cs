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
                //als er een searchrequest van het grote zoekveld binnenkomt
                if (Request.QueryString["search"] != null)
                {
                    doSearch(Request.QueryString["search"]);
                }
                else
                {


                    String genre = Request.QueryString["genre"];
                    String cat = Request.QueryString["cat"];
                    dvdInfoService = new DvdInfoService();

                    if (genre != null)
                    {
                        gvDvdInfo.DataSource = dvdInfoService.searchDvdWithTextAndGenre("", genre);
                        genreService = new GenreService();
                        lblHeader.Text = genreService.getGenre(Convert.ToInt32(genre)).name + " DVDs";
                    }
                    else if (cat != null)
                    {
                        gvDvdInfo.DataSource = dvdInfoService.searchDvdWithTextAndCategory("", cat);
                        categoryService = new CategoryService();
                        lblHeader.Text = categoryService.getCategory(Convert.ToInt32(cat)).name + " DVDs";
                    }
                    else
                    {
                        gvDvdInfo.DataSource = dvdInfoService.getAll();
                    }
                    gvDvdInfo.DataBind();


                    //set column invisible here (value can still be accessed)
                    gvDvdInfo.Columns[0].Visible = false;

                }

            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            dvdInfoService = new DvdInfoService();

            String searchtext = txtSearch.Text;
            String genre = Request.QueryString["genre"];
            String cat = Request.QueryString["cat"];

            if (genre != null)
            {
                gvDvdInfo.DataSource = dvdInfoService.searchDvdWithTextAndGenre(searchtext, genre);
                genreService = new GenreService();
                lblHeader.Text = genreService.getGenre(Convert.ToInt32(genre)).name + " DVDs matching '" + searchtext + "'";

            }
            else if (cat != null)
            {
                gvDvdInfo.DataSource = dvdInfoService.searchDvdWithTextAndCategory(searchtext, cat);
                categoryService = new CategoryService();
                lblHeader.Text = categoryService.getCategory(Convert.ToInt32(cat)).name + " DVDs matching '" + searchtext + "'";

            }
            else
            {
                gvDvdInfo.DataSource = dvdInfoService.searchDvdWithText(searchtext);
                lblHeader.Text = "DVDs matching '" + searchtext + "'";

            }
            gvDvdInfo.DataBind();
        }

        private void doSearch(String searchText)
        {
            lblHeader.Text = "Results for '" + searchText + "'";



        }


        ///**Handles click-event for the buy button in the gridview.*/
        //protected void gvDvdInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    List<DvdCopy> availabeCopies = null;

        //    int index = Convert.ToInt32(e.CommandArgument.ToString());

        //    //get all available copies of this movie + type (buy/rent)
        //    DvdCopyService dvdCopyService = new DvdCopyService();
        //    if (e.CommandName == "Buy")
        //    {
        //        availabeCopies = dvdCopyService.getAllInStockBuyCopiesForDvdInfo(gvDvdInfo.Rows[index].Cells[0].Text);

        //        //only allow purchase if at least one copy is available
        //        //a user can still add 100 copies to his cart as long as 1 is in stock, not sure if there's a better solution for this
        //        if (availabeCopies.Count > 0)
        //        {
        //            ShoppingCartService shoppingCartService = new ShoppingCartService();
        //            shoppingCartService.addItemToCart(((Customer)Session["user"]).customer_id, Convert.ToInt32(gvDvdInfo.Rows[index].Cells[0].Text));
        //        }
        //        else
        //        {
        //            //tijdelijke messagebox in afwachting van een cleanere oplossing (zoals verbergen van buy/rent knop, greyed out knop, "out of stock" bericht...)
        //            //todo: show date when the dvd will be back in stock + option to reserve
        //            string script = "alert(\"Item niet meer in stock!\");";
        //            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
        //        }
        //    }
        //}



    }
}