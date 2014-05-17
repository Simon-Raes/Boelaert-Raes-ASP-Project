using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.BLL.Database;
using LayeredBusinessModel.BLL.Model;
using LayeredBusinessModel.Domain;
using System.Data;

namespace LayeredBusinessModel.WebUI
{
    public partial class Account : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            setStatistics();
        }

        private void setStatistics()
        {
            Customer user = (Customer)Session["user"];
            if (user != null)
            {
                OrderModel orderModel = new OrderModel();
                RentModel rentModel = new RentModel();

                lblOrderTotal.Text = "You have purchased " + orderModel.getNumberOfOrderLinesForCustomer(user) + " items ("
                    + orderModel.getItemsBoughtByCustomer(user).Count + " unique) so far.";

                lblActiveRentCopies.Text = "You are currently renting " + rentModel.getNumberOfActiveRentOrdersCopiesForCustomer(user) + " items:";

                if (rentModel.getNumberOfActiveRentOrdersCopiesForCustomer(user) > 0)
                {
                    List<OrderLine> orderLines = new OrderLineService().getActiveRentOrderLinesByCustomer(user);



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

                    Boolean hasRentItems = false;

                    foreach (OrderLine item in orderLines)
                    {
                        if (item.orderLineType.id == 1)
                        {
                            hasRentItems = true;
                        }
                    }

                    DataTable orderTable = new DataTable();
                    orderTable.Columns.Add("Item number");
                    orderTable.Columns.Add("Name");
                    orderTable.Columns.Add("Type");
                    orderTable.Columns.Add("Price");

                    if (hasRentItems)
                    {
                        orderTable.Columns.Add("Start date");
                        orderTable.Columns.Add("End date");
                    }


                    foreach (OrderLine item in orderLines)
                    {
                        DataRow orderRow = orderTable.NewRow();
                        orderRow[0] = item.orderline_id;
                        orderRow[1] = item.dvdInfo.name;
                        orderRow[2] = item.orderLineType.name;
                        orderRow[3] = currency + " " + setPriceInRightCurrency(item.dvdInfo.buy_price, currency);
                        orderRow[4] = item.startdate.ToString("dd/MM/yyyy");
                        orderRow[5] = item.enddate.ToString("dd/MM/yyyy");

                        orderTable.Rows.Add(orderRow);
                    }

                    gvActiveRent.DataSource = orderTable;
                    gvActiveRent.DataBind();
                }


            }

        }

        public void btnLogOut_Click(object sender, EventArgs e)
        {
            Session["user"] = null;
            Response.Redirect("~/Index.aspx");
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

        protected void gvActiveRent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime dt=DateTime.ParseExact(e.Row.Cells[5].Text, "dd/MM/yyyy", null);
                if ((dt - DateTime.Now).Days < 2)
                {
                    //user has 2 days left, display a warning
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Orange;
                }
                else if ((dt - DateTime.Now).Days < 2)
                {
                    //copy is over time, mark the date in red
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}