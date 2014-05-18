using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
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
                else if (Request.QueryString["currency"].Equals("usd"))
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
                    cost = setPriceInRightCurrency((float)cost, currency);

                    totalCost += cost;

                    cartRow[3] = currency + " " + Math.Round(cost, 2);

                    cartTable.Rows.Add(cartRow);
                }

                lblTotalCost.Text = currency + " " + Math.Round(totalCost, 2).ToString();

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
            return (float)currencyWebService.convert(price, "usd");
        }

        /**Deletes item from cart*/
        protected void gvCart_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);

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

                //update gridview            
                if (Session["user"] != null)
                {
                    fillCartGridView();
                }
            }
            catch (NoRecordException)
            {
                //No dvd was found
            }
        }

        /**Creates a new order and moves the cart content to that order*/
        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            Customer user = (Customer)Session["user"];
            if (user != null)
            {
                //create the order
                Order order = new OrderModel().createOrder(user);

                //clear cart display
                gvCart.DataSource = null;
                gvCart.DataBind();

                //redirect to payment page, with query string to connect to the order
                Response.Redirect("~/OrderPayment.aspx?order=" + order.order_id);

            }
        }
    }
}