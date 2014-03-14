using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;

namespace LayeredBusinessModel.WebUI
{
    public partial class tempMovie : System.Web.UI.Page
    {
        private DvdCopyService dvdCopyService;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //only show rent/reserve options if a user is logged in
                if (Session["user"] == null)
                {
                    pnlActions.Visible = false;
                }
                else
                {
                    pnlActions.Visible = true;
                }
            }
        }

        /*Disables invalid days in the Rent calendar*/
        protected void calRentStartDate_DayRender(object sender, DayRenderEventArgs e)
        {
            //don't let the user select days in the past    
            if (e.Day.Date < DateTime.Today)
            {
                e.Day.IsSelectable = false;
                e.Cell.BackColor = System.Drawing.Color.LightGray;
            } else
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }
        }

        /*Disables invalid days in the Reservations calendar*/
        protected void calReservationStartDate_DayRender(object sender, DayRenderEventArgs e)
        {
            //movie can be reserved between today and 14 days from now   
            if (e.Day.Date < DateTime.Today || e.Day.Date > DateTime.Today.AddDays(14))
            {
                e.Day.IsSelectable = false;
                e.Cell.BackColor = System.Drawing.Color.LightGray;
            } else
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }
        }

        /*Adds rent copy to shopping cart*/
        protected void btnRent_Click(object sender, EventArgs e)
        {
            //only execute for valid dates
            //todo: check for max date (14 days in advance?)
            if (calRentStartDate.SelectedDate >= DateTime.Today)
            {
                //add rent item to cart            
                DateTime startdate = calRentStartDate.SelectedDate;
                DateTime enddate = startdate.AddDays(Convert.ToInt32(ddlRentDuration.SelectedValue));


                //HERE: hardcoded to shawshank redemption, must get dvdInfoID from generated page
                dvdCopyService = new DvdCopyService();
                List<DvdCopy> availabeCopies = dvdCopyService.getAllInStockRentCopiesForDvdInfo("1");

                //only allow purchase if a copy is available
                if (availabeCopies.Count > 0)
                {
                    //pick the first available copy and assign it to this user
                    DvdCopy chosenCopy = availabeCopies[0];
                    ShoppingCartService shoppingCartService = new ShoppingCartService();
                    shoppingCartService.addItemToCart(((Customer)Session["user"]).customer_id, chosenCopy.dvd_copy_id, startdate, enddate);

                    //mark copy as NOT in_stock
                    chosenCopy.in_stock = false;
                    dvdCopyService.updateCopy(chosenCopy);
                }
                else
                {
                    //tijdelijke messagebox in afwachting van een cleanere oplossing (zoals greyed out knop, "out of stock" tekst...)
                    //todo: show date when the dvd will be back in stock + option to reserve

                    string script = "alert(\"Item niet meer in stock! (temp)\");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                }
            }
            else
            {
                string script = "alert(\"Select a startdate first. (temp)\");";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
            }
        }

        protected void btnReserve_Click(object sender, EventArgs e)
        {
            //todo: add reservation (to cart or straight to order(line)?)
            //wat is een reservatie? wat gebeurt er op die dag? hetzelfde als verhuur op voorhand aanvragen?

            //NYI
            string script = "alert(\"Not yet implemented.\");";
            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
        }





    }
}