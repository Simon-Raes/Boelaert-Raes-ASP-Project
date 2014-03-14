using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using System.Data;

namespace LayeredBusinessModel.WebUI
{
    public partial class Cart : System.Web.UI.Page
    {
        private ShoppingCartService shoppingCartService;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                shoppingCartService = new ShoppingCartService();
                if (Session["user"] != null)
                {
                    gvCart.DataSource = shoppingCartService.getCartContentForCustomer(((Customer)Session["user"]).customer_id);
                    gvCart.DataBind();
                }
            }
        }

        /*Delete item from cart*/
        protected void gvCart_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);

            //systeem hier is zeer onhandig, Cells[nummer] moet aangepast worden elke keer de layout gridview veranderd wordt
            
            //remove copy from all shoppingcarts (can    only be in one)
            ShoppingCartService shoppingCartService = new ShoppingCartService();
            shoppingCartService.removeItemFromCart(gvCart.Rows[index].Cells[3].Text);
            
            //set copy as in_stock
            DvdCopyService dvdCopyService = new DvdCopyService();
            dvdCopyService.updateDvdCopyInStockStatus(gvCart.Rows[index].Cells[3].Text, true);

            //update gridview
            //todo: find a way to remove a gridview row without needing a new query + databind
            if (Session["user"] != null)
            {
                gvCart.DataSource = shoppingCartService.getCartContentForCustomer(((Customer)Session["user"]).customer_id);
                gvCart.DataBind();
            }
            
        }

        /*Move cart-content to a new order*/
        protected void btnCheckout_Click(object sender, EventArgs e)
        {            
            //NYI
            string script = "alert(\"Not yet implemented.\");";
            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);


            if(false) //tijdelijk afgezet tot ik een betere oplossing vind
            {
                if (Session["user"] != null)
                {
                    Customer user = (Customer)Session["user"];

                    //get all items currently in cart
                    ShoppingCartService shoppingCartService = new ShoppingCartService();
                    List<ShoppingcartItem> cartContent = shoppingCartService.getCartContentForCustomer(user.customer_id);

                    //create new order for this user
                    OrderService orderService = new OrderService();
                    orderService.addOrderForCustomer(user.customer_id);

                    //add cart content to newly created order
                    OrderLineService orderLineService = new OrderLineService();
                    //... order Id onbekend, cart bevat niet alle nodige info, etc...
                }   
            }
                      

        }

        
    }
}