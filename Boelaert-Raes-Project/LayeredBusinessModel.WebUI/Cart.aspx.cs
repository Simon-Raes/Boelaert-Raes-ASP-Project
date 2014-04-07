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
        private DvdCopyService dvdCopyService;


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

            //remove item from shoppingcart
            ShoppingCartService shoppingCartService = new ShoppingCartService();
            shoppingCartService.removeItemFromCart(gvCart.Rows[index].Cells[1].Text); //cell 1 = shoppingcart_item_id

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
                //problem here: an order will still be created if all cart items are out of stock (=not added to order)
                OrderService orderService = new OrderService();
                int orderID = orderService.addOrderForCustomer(user.customer_id);


                OrderLineService orderLineService = new OrderLineService();
                dvdCopyService = new DvdCopyService();

                //add cart items to newly created order
                foreach (ShoppingcartItem item in cartContent)
                {

                    OrderLine orderline = new OrderLine
                    {
                        order_id = orderID,
                        dvd_info_id = item.dvd_info_id,
                        //dvd_copy_id = copy.dvd_copy_id, //don't add a copy_id yet, only do this when a user has paid (id will be set in admin module)                       
                        startdate = item.startdate,
                        enddate = item.enddate,
                        //order_line_type_id is verhuur/verkoop? dan kunnen we dat eigenlijk via join ophalen via dvd_copy tabel
                        order_line_type_id = item.copy_type_id
                    };

                    orderLineService.addOrderLine(orderline);


                }

                //clear cart
                shoppingCartService.clearCustomerCart(user.customer_id);
                //this also clears items that were not added to the order!

                //clear cart display
                gvCart.DataSource = null;
                gvCart.DataBind();

            }
        }
    }
}