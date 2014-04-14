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
                    pnlLogin.Visible = true;
                    pnlActions.Visible = false;
                    pnlReserve.Visible = false;
                }
                else
                {
                    pnlLogin.Visible = false;
                    pnlActions.Visible = true;
                    pnlReserve.Visible = false;

                }

                dvdCopyService = new DvdCopyService();
                //set availability label for BUY copies
                List<DvdCopy> availabeCopies = dvdCopyService.getAllInStockBuyCopiesForDvdInfo("1"); //1 = hardcoded op shawshank                 
                if(availabeCopies.Count>5)
                {
                    lblBuyAvailability.Text = "High";
                    lblBuyAvailability.ForeColor = System.Drawing.Color.Green;
                } 
                else if(availabeCopies.Count>0)
                {
                    lblBuyAvailability.Text = "Low";
                    lblBuyAvailability.ForeColor = System.Drawing.Color.Orange;
                } 
                else
                {
                    lblBuyAvailability.Text = "Out of stock";
                    lblBuyAvailability.ForeColor = System.Drawing.Color.Red;
                }

                //set availability label for RENT copies
                List<DvdCopy> availabeRentCopies = dvdCopyService.getAllInStockRentCopiesForDvdInfo("1"); //1 = hardcoded op shawshank                 
                if (availabeRentCopies.Count > 5)
                {
                    lblRentAvailability.Text = "High";
                    lblRentAvailability.ForeColor = System.Drawing.Color.Green;
                }
                else if (availabeRentCopies.Count > 0)
                {
                    lblRentAvailability.Text = "Low";
                    lblRentAvailability.ForeColor = System.Drawing.Color.Orange;
                }
                else
                {
                    lblRentAvailability.Text = "Out of stock";
                    pnlReserve.Visible = true;
                    lblRentAvailability.ForeColor = System.Drawing.Color.Red;
                }

            }
        }

        /**Disables invalid days in the Rent calendar*/
        protected void calRentStartDate_DayRender(object sender, DayRenderEventArgs e)
        {
            //don't let the user select days in the past    
            if (e.Day.Date < DateTime.Today || e.Day.Date > DateTime.Today)
            {
                e.Day.IsSelectable = false;
                e.Cell.BackColor = System.Drawing.Color.LightGray;
            }
            else
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }
        }

        /**Disables invalid days in the Reservations calendar*/
        protected void calReservationStartDate_DayRender(object sender, DayRenderEventArgs e)
        {
            //movie can be reserved between today and 14 days from now   
            if (e.Day.Date < DateTime.Today || e.Day.Date > DateTime.Today.AddDays(14))
            {
                e.Day.IsSelectable = false;
                e.Cell.BackColor = System.Drawing.Color.LightGray;
            }
            else
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
            }
        }

        /*Adds rent copy to shopping cart*/
        protected void btnRent_Click(object sender, EventArgs e)
        {
            //only excecute if a user is logged in
            if (Session["user"] != null)
            {
                Customer user = ((Customer)Session["user"]);

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

                    //check the number of rent items in the user's cart
                    ShoppingCartService shoppingCartService = new ShoppingCartService();
                    List<ShoppingcartItem> cartContent = shoppingCartService.getCartContentForCustomer(user.customer_id);
                    int numberOfCurrentlyRentedItems = 0;
                    foreach (ShoppingcartItem item in cartContent)
                    {
                        if (item.typeName.Equals("Verhuur"))
                        {
                            numberOfCurrentlyRentedItems++;
                        }
                    }

                    //check the number of items currently being rented by the user
                    OrderLineService orderLineService = new OrderLineService();
                    List<OrderLine> orderLines = orderLineService.getActiveRentOrderLinesForCustomer(user.customer_id);
                    foreach(OrderLine orderLine in orderLines)
                    {
                        numberOfCurrentlyRentedItems++;
                    }

                    //check if the user can still rent additional items
                    if (numberOfCurrentlyRentedItems < 5)
                    {
                        ////only allow rent purchase if a copy is available
                        //if (availabeCopies.Count > 0)
                        //{
                            shoppingCartService.addItemToCart(user.customer_id, 1, startdate, enddate); //1 = hardcode to shawshank redemption for now
                        //}
                        //else
                        //{
                        //    //tijdelijke messagebox in afwachting van een cleanere oplossing (zoals greyed out knop, "out of stock" tekst...)
                        //    //todo: show date when the dvd will be back in stock + option to reserve
                        //    string script = "alert(\"Item niet meer in stock! (todo: overzicht van wanneer er terug een copy beschikbaar is)\");";
                        //    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                        //}
                    }
                    else
                    {
                        string script = "alert(\"You are already renting 5 items. (something something more info here) \");";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                    }
                }
                else
                {
                    string script = "alert(\"Select a startdate first. (temp)\");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                }
            }
            else
            {
                string script = "alert(\"You have been logged out due to inactivity.\");";
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

        protected void btnBuy_Click(object sender, EventArgs e)
        {
            List<DvdCopy> availabeCopies = null;


            //get all available copies of this movie + type (buy/rent)
            DvdCopyService dvdCopyService = new DvdCopyService();

            availabeCopies = dvdCopyService.getAllInStockBuyCopiesForDvdInfo("1"); //1 = hardcoded op shawshank 

            //only allow purchase if at least one copy is available
            //a user can still add 100 copies to his cart as long as 1 is in stock, not sure if there's a better solution for this
            //if (availabeCopies.Count > 0)
            //{
                ShoppingCartService shoppingCartService = new ShoppingCartService();
                shoppingCartService.addItemToCart(((Customer)Session["user"]).customer_id, Convert.ToInt32("1"));
            //}
            //else
            //{
            //    //tijdelijke messagebox in afwachting van een cleanere oplossing (zoals verbergen van buy/rent knop, greyed out knop, "out of stock" bericht...)
            //    //todo: show date when the dvd will be back in stock + option to reserve
            //    string script = "alert(\"Item niet meer in stock!\");";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
            //}

        }





    }
}