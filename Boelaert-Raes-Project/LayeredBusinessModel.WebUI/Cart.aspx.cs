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

        /**Deletes item from cart*/
        protected void gvCart_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);

            //systeem hier is zeer onhandig, Cells[nummer] moet aangepast worden elke keer de layout gridview veranderd wordt
            
            //remove copy from all shoppingcarts (can only be in one)
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

        /**Creates a new order and moves the cart content to that order*/
        protected void btnCheckout_Click(object sender, EventArgs e)
        {    
            
            if (Session["user"] != null)
            {
                Customer user = (Customer)Session["user"];

                //get all items currently in cart
                ShoppingCartService shoppingCartService = new ShoppingCartService();
                List<ShoppingcartItem> cartContent = shoppingCartService.getCartContentForCustomer(user.customer_id);

                //create new order for this user
                OrderService orderService = new OrderService();
                int orderID = orderService.addOrderForCustomer(user.customer_id);

                //add cart content to newly created order
                OrderLineService orderLineService = new OrderLineService();
                foreach(ShoppingcartItem item in cartContent)
                {
                    OrderLine orderline = new OrderLine
                    {
                        order_id = orderID,
                        dvd_copy_id = item.dvd_copy_id,
                        startdate = item.startdate,
                        enddate = item.enddate,
                        //order_line_type_id is verhuur/verkoop, kunnen we eigenlijk via join ophalen via dvd_copy_id
                        order_line_type_id = 1
                    };
                    orderLineService.addOrderLine(orderline);
                }

                //clear cart
                shoppingCartService.clearCustomerCart(user.customer_id);

                //clear cart display
                gvCart.DataSource = null;
                gvCart.DataBind();
                
            }
        }
    }
}