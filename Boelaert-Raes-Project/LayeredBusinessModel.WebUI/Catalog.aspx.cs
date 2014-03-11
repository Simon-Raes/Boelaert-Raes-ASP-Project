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


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                dvdInfoService = new DvdInfoService();
                gvDvdInfo.DataSource = dvdInfoService.getAll();
                gvDvdInfo.DataBind();
                //set column invisible here so value can still be accessed
                gvDvdInfo.Columns[0].Visible = false;

                //only show buy/rent buttons if a user is logged in
                //todo: only show buy / rent button if item is available (in_stock)
                if(Session["user"] == null)
                {
                    gvDvdInfo.Columns[5].Visible = false;
                    gvDvdInfo.Columns[6].Visible = false;
                }                
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            dvdInfoService = new DvdInfoService();
            gvDvdInfo.DataSource = dvdInfoService.getAllWithTitleSearch(txtSearch.Text);
            gvDvdInfo.DataBind();
        }

        protected void gvDvdInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {                       
            
            //todo: check if buy or rent was clicked

            if (e.CommandName == "Select")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());

                //get all available copies of this movie
                //todo: call buy OR rent method depending on the button that was clicked (now: always checking buy copies)
                DvdCopyService dvdCopyService = new DvdCopyService();
                List<DvdCopy> availabeCopies = dvdCopyService.getAllInStockBuyCopiesForDvdInfo(gvDvdInfo.Rows[index].Cells[0].Text);

                //only allow purchase if a copy is available
                if(availabeCopies.Count>0)
                {
                    //pick the first available copy and assign it to this user
                    DvdCopy chosenCopy = availabeCopies[0];
                    ShoppingCartService shoppingCartService = new ShoppingCartService();
                    shoppingCartService.addItemToCart(((Customer)Session["user"]).customer_id, chosenCopy.dvd_copy_id);

                    //mark copy as NOT in_stock
                    chosenCopy.in_stock = false;
                    dvdCopyService.updateCopy(chosenCopy);
                }
                else
                {
                    //tijdelijke messagebox in afwachting van een cleanere oplossing (zoals verbergen van buy/rent knop, greyed out knop, "out of stock" bericht...)
                    string script = "alert(\"Item niet meer in stock!\");";
                    ScriptManager.RegisterStartupScript(this, GetType(),
                                          "ServerControlScript", script, true);
                }

                
            }
        }

       
    }
}