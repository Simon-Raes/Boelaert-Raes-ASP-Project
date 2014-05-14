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
using CustomException;

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
            double totalCost = 0;

            shoppingCartService = new ShoppingCartService();
            List<ShoppingcartItem> cartContent = shoppingCartService.getCartContentForCustomer(((Customer)Session["user"]));
            if (cartContent.Count > 0)
            {
                divCartContent.Visible = true;
                divCartEmpty.Visible = false;

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
                cartTable.Columns.Add("Price");

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
                    cartRow[3] = item.dvdInfo.buy_price;

                    if (item.dvdCopyType.id == 1)
                    {
                        double cost = item.dvdInfo.rent_price * (item.enddate - item.startdate).Days;
                        totalCost += cost;
                        cartRow[3] = Math.Round(cost, 2);
                        cartRow[4] = item.startdate.ToString("dd/MM/yyyy");
                        cartRow[5] = item.enddate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        double cost = item.dvdInfo.buy_price;
                        totalCost += cost;
                        cartRow[3] = Math.Round(cost, 2);
                    }
                    cartTable.Rows.Add(cartRow);
                }

                lblTotalCost.Text = Math.Round(totalCost, 2).ToString() + " (todo: euro/dollar)";

                gvCart.DataSource = cartTable;
                gvCart.DataBind();
            }
            else
            {
                divCartContent.Visible = false;
                divCartEmpty.Visible = true;
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

            try
            {
                //set copy as in_stock
                DvdCopyService dvdCopyService = new DvdCopyService();

                DvdCopy dvdCopy = dvdCopyService.getByID(gvCart.Rows[index].Cells[3].Text);             //Throws NoRecordException || DALException
                dvdCopyService.updateStockStatus(dvdCopy, true);            //Throws NoRecordException || DALException
            }
            catch (NoRecordException)
            {
                //No dvd was found
            }

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
            Customer user = (Customer)Session["user"];
            if (user != null)
            {
                //get all items currently in cart
                ShoppingCartService shoppingCartService = new ShoppingCartService();
                List<ShoppingcartItem> cartContent = shoppingCartService.getCartContentForCustomer(user);

                //only do a checkout if the cart actually contains items
                if (cartContent.Count > 0)
                {
                    try
                    {
                        //create new order for this user
                        //problem here: an order will still be created if all cart items are out of stock (=not added to order)
                        int orderID = new OrderService().addOrderForCustomer(user);           //Throws NoRecordException


                        OrderLineService orderLineService = new OrderLineService();
                        dvdCopyService = new DvdCopyService();

                        //add cart items to newly created order
                        foreach (ShoppingcartItem item in cartContent)
                        {

                            OrderLine orderline = new OrderLine
                            {
                                order = new OrderService().getByID(orderID.ToString()),            //Throws NoRecordException
                                dvdInfo = new DvdInfoService().getByID(item.dvdInfo.dvd_info_id.ToString()),           //Throws NoRecordException
                                //dvd_copy_id = copy.dvd_copy_id, //don't add a copy_id yet, only do this when a user has paid (id will be set in admin module)                       
                                startdate = item.startdate,
                                enddate = item.enddate,
                                //order_line_type_id is verhuur/verkoop? dan kunnen we dat eigenlijk via join ophalen via dvd_copy tabel
                                orderLineType = new OrderLineTypeService().getByID(item.dvdCopyType.id.ToString())          //Throws NoRecordException
                            };

                            if (orderLineService.add(orderline))
                            {
                                //succes
                            }


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
                    catch (NoRecordException)
                    {

                    }

                }
            }
        }
    }
}