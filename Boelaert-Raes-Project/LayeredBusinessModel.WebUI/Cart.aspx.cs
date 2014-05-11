using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using System.Data;
using LayeredBusinessModel.BLL.Database;

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
                
                if (Session["user"] != null)
                {
                    fillCartGridView();
                }
            }
        }

        private void fillCartGridView()
        {
            Boolean hasRentItems = false;

            shoppingCartService = new ShoppingCartService();
            List<ShoppingcartItem> cartContent = shoppingCartService.getCartContentForCustomer(((Customer)Session["user"]));

            foreach (ShoppingcartItem item in cartContent)
            {
                if (item.dvdCopyType.id == 1)
                {
                    hasRentItems = true;
                }
            }

            DataTable cartTable = new DataTable();

            cartTable.Columns.Add("Id");
            cartTable.Columns.Add("Name");
            cartTable.Columns.Add("Amount");

            if (hasRentItems)
            {
                cartTable.Columns.Add("Start date");
                cartTable.Columns.Add("End date");
            }

            foreach (ShoppingcartItem item in cartContent)
            {
                DataRow cartRow = cartTable.NewRow();
                cartRow[0] = item.shoppingcart_item_id;
                cartRow[1] = item.dvdInfo.name;
                cartRow[2] = 1;

                if (item.dvdCopyType.id == 1)
                {
                    cartRow[3] = item.startdate.ToString("dd/MM/yyyy");
                    cartRow[4] = item.enddate.ToString("dd/MM/yyyy");                    
                }
                cartTable.Rows.Add(cartRow);
            }

            gvCart.DataSource = cartTable;
            gvCart.DataBind();
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
                fillCartGridView();
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
                List<ShoppingcartItem> cartContent = shoppingCartService.getCartContentForCustomer(user);

                //only do a checkout if the cart actually contains items
                if (cartContent.Count > 0)
                {
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
                            order = new OrderService().getOrder(orderID.ToString()),
                            dvdInfo = new DvdInfoService().getDvdInfoWithID(item.dvdInfo.dvd_info_id.ToString()),
                            //dvd_copy_id = copy.dvd_copy_id, //don't add a copy_id yet, only do this when a user has paid (id will be set in admin module)                       
                            startdate = item.startdate,
                            enddate = item.enddate,
                            //order_line_type_id is verhuur/verkoop? dan kunnen we dat eigenlijk via join ophalen via dvd_copy tabel
                            orderLineType = new OrderLineTypeService().getOrderLineTypeByID(item.dvdCopyType.id)
                        };

                        orderLineService.addOrderLine(orderline);


                    }

                    //clear cart
                    shoppingCartService.clearCustomerCart(user);
                    //this also clears items that were not added to the order!

                    //clear cart display
                    gvCart.DataSource = null;
                    gvCart.DataBind();

                    //redirect to payment page, with query string to connect to the order
                    Response.Redirect("~/OrderPayment.aspx?order=" + orderID);

                }
            }
        }
    }
}