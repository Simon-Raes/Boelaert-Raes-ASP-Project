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
                dvdInfoService = new DvdInfoService();
                gvDvdInfo.DataSource = dvdInfoService.getAll();
                gvDvdInfo.DataBind();
                //set column invisible here (value can still be accessed)
                gvDvdInfo.Columns[0].Visible = false;

                //only show buy/rent buttons if a user is logged in
                //todo: only show buy / rent button if item is available (in_stock)
                if (Session["user"] == null)
                {
                    //niet echt handig met die hardcoded column nummers
                    gvDvdInfo.Columns[6].Visible = false;
                }

                //Fill category dropdown list
                categoryService = new CategoryService();
                List<Category> categories = categoryService.getAll();
                ddlCategory.Items.Clear();
                ddlCategory.Items.Add(new ListItem("select category", "-1"));
                foreach (Category c in categories)
                {
                    ddlCategory.Items.Add(new ListItem(c.name, Convert.ToString(c.category_id)));
                }
                ddlCategory.DataBind();

                //Set emtpy genre list message item
                ddlGenre.Items.Clear();
                ddlGenre.Items.Add(new ListItem("Select a category first", "-1"));
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            dvdInfoService = new DvdInfoService();
            String searchtext = txtSearch.Text;
            String categoryID = ddlCategory.SelectedValue;
            String genreID = ddlGenre.SelectedValue;

            if (genreID != "-1") //genre can only belong to one category, so no need to filter on both
            {
                //do search on text + genre
                gvDvdInfo.DataSource = dvdInfoService.searchDvdWithTextAndGenre(txtSearch.Text, genreID);
            }
            else if (categoryID != "-1")
            {
                //do search on text + category
                gvDvdInfo.DataSource = dvdInfoService.searchDvdWithTextAndCategory(txtSearch.Text, categoryID);
            }
            else
            {
                //do search on only text
                gvDvdInfo.DataSource = dvdInfoService.searchDvdWithText(txtSearch.Text);
            }

            gvDvdInfo.DataBind();
        }

        /**Handles click-event for the buy button in the gridview.*/
        protected void gvDvdInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            List<DvdCopy> availabeCopies = null;

            int index = Convert.ToInt32(e.CommandArgument.ToString());

            //get all available copies of this movie + type (buy/rent)
            DvdCopyService dvdCopyService = new DvdCopyService();
            if (e.CommandName == "Buy")
            {
                availabeCopies = dvdCopyService.getAllInStockBuyCopiesForDvdInfo(gvDvdInfo.Rows[index].Cells[0].Text);
                
                //only allow purchase if at least one copy is available
                //a user can still add 100 copies to his cart as long as 1 is in stock, not sure if there's a better solution for this
                if (availabeCopies.Count > 0)
                {
                    ShoppingCartService shoppingCartService = new ShoppingCartService();
                    shoppingCartService.addItemToCart(((Customer)Session["user"]).customer_id, Convert.ToInt32(gvDvdInfo.Rows[index].Cells[0].Text)); //2 = verkoop
                }
                else
                {
                    //tijdelijke messagebox in afwachting van een cleanere oplossing (zoals verbergen van buy/rent knop, greyed out knop, "out of stock" bericht...)
                    //todo: show date when the dvd will be back in stock + option to reserve
                    string script = "alert(\"Item niet meer in stock!\");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                }
            }
        }


        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            genreService = new GenreService();
            List<Genre> genres = genreService.getGenresForCategory(Convert.ToInt32(ddlCategory.SelectedValue));

            ddlGenre.Items.Clear();
            ddlGenre.Items.Add(new ListItem("select genre", "-1"));
            foreach (Genre c in genres)
            {
                ddlGenre.Items.Add(new ListItem(c.name, Convert.ToString(c.genre_id)));
            }
            ddlGenre.DataBind();
        }
    }
}