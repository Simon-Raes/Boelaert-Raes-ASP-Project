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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                fillCartGridView();
            }
        }

        private void fillCartGridView()
        {
            try
            {
                List<ShoppingcartItem> cartContent = new ShoppingCartService().getCartContentForCustomer(((Customer)Session["user"]));            //Throws NoRecordException

                Boolean hasRentItems = false;

                double totalCost = 0;

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

                String currency = "€";
                if (Request.QueryString["currency"] == null)
                {
                    if (CookieUtil.CookieExists("currency"))
                    {
                        if (CookieUtil.GetCookieValue("currency").Equals("usd"))
                        {
                            currency = "$";
                        }
                    }
                }
                else if(Request.QueryString["currency"].Equals("usd"))
                {
                    currency = "$";
                }

                foreach (ShoppingcartItem item in cartContent)
                {
                    double cost = 0;
                    DataRow cartRow = cartTable.NewRow();
                    cartRow[0] = item.shoppingcart_item_id;
                    cartRow[1] = item.dvdInfo.name;
                    cartRow[2] = 1;
                    cartRow[3] = item.dvdInfo.buy_price;

                    if (item.dvdCopyType.id == 1)
                    {
                        cost = item.dvdInfo.rent_price * (item.enddate - item.startdate).Days;
                        cartRow[4] = item.startdate.ToString("dd/MM/yyyy");
                        cartRow[5] = item.enddate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        cost = item.dvdInfo.buy_price;
                    }
                    cost = setPriceInRightCurrency((float) cost, currency);

                    totalCost += cost;

                    cartRow[3] = currency + " " +  Math.Round(cost, 2);

                    cartTable.Rows.Add(cartRow);
                }  
                
                lblTotalCost.Text = currency  + " " +   Math.Round(totalCost, 2).ToString();

                gvCart.DataSource = cartTable;
                gvCart.DataBind();
            }
            catch (NoRecordException)
            {
                divCartContent.Visible = false;
                divCartEmpty.Visible = true;
            }
        }

        private float setPriceInRightCurrency(float price, String currency)
        {
            wsCurrencyWebService.CurrencyWebService currencyWebService = new wsCurrencyWebService.CurrencyWebService();

            if (currency.Equals("€"))
            {
                return price;
            }
            return (float) currencyWebService.convert(price, "usd");
        }
        
        /**Deletes item from cart*/
        protected void gvCart_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);

            //systeem hier is zeer onhandig, Cells[nummer] moet aangepast worden elke keer de layout gridview veranderd wordt

            //remove item from shoppingcart
            if (new ShoppingCartService().deleteByID(gvCart.Rows[index].Cells[1].Text))   //cell 1 = shoppingcart_item_id
            { 
                //succes
            }
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
                try
                {
                    //get all items currently in cart
                    List<ShoppingcartItem> cartContent = new ShoppingCartService().getCartContentForCustomer(user);           //Throws NoRecordException

                    //create new order for this user
                    //problem here: an order will still be created if all cart items are out of stock (=not added to order)
                    int orderID = new OrderService().addOrderForCustomer(user);           //Throws NoRecordException

                    OrderLineService orderLineService = new OrderLineService();

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
                    if (new ShoppingCartService().deleteByCustomer(user))
                    {
                        //success
                    }
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