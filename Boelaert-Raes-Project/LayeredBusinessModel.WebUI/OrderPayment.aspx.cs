using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;
using CustomException;

namespace LayeredBusinessModel.WebUI
{
    public partial class OrderPayment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Customer user = (Customer)Session["user"];
                OrderService orderService = new OrderService();
                if (user != null)
                {
                    String orderID = Request.QueryString["order"];
                    try
                    {
                        Order order = orderService.getByID(orderID);           //Throws NoRecordException
                        if (order.customer.customer_id == user.customer_id)
                        {

                            String currency = "€";
                            if (Request.QueryString["currency"] == null)
                            {
                                //Check if the user has set the currencypreference
                                if (CookieUtil.CookieExists("currency"))
                                {
                                    if (CookieUtil.GetCookieValue("currency").Equals("usd"))
                                    {
                                        currency = "$";
                                    }
                                }
                            }
                            else
                            {
                                switch (Request.QueryString["currency"])
                                {
                                    case "usd":
                                        currency = "$";
                                        break;
                                }
                            }


                            //all good
                            lblStatus.Text = orderID;
                            OrderModel helper = new OrderModel();
                            lblCost.Text = currency + " " + Math.Round(helper.getOrderCost(order), 2).ToString();
                            List<OrderLine> orderLines = new OrderLineService().getByOrder(order);            //Throws NoRecordException

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
                                if (item.orderLineType.id == 1)
                                {
                                    double cost = item.dvdInfo.rent_price * (item.enddate - item.startdate).Days;
                                    orderRow[3] = currency + " " + Math.Round(cost, 2);
                                    orderRow[4] = item.startdate.ToString("dd/MM/yyyy");
                                    orderRow[5] = item.enddate.ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    double cost = item.dvdInfo.buy_price;
                                    orderRow[3] = currency + " " + Math.Round(cost, 2);
                                }

                                orderTable.Rows.Add(orderRow);
                            }

                            //set gridview
                            gvOrderDetails.DataSource = orderTable;
                            gvOrderDetails.DataBind();
                        }
                        else
                        {
                            //this order does not belong to the logged in user, access denied.
                            lblStatus.Text = "Access denied";
                        }
                    }
                    catch (NoRecordException)
                    {

                    } 
                }
                else
                {
                    //not logged in, access denied
                    lblStatus.Text = "Please log in to access this page";
                }

            }            
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            Customer user = (Customer)Session["user"];
            if(user!=null)
            {
                try
                {
                    //get the order
                    String orderID = lblStatus.Text;
                    Order order = new OrderService().getByID(orderID);           //Throws NoRecordException
                    new OrderModel().payOrder(order);       //Throws NoRecordException || DALException

                    //get the orderLines
                    List<OrderLine> orderLines = new OrderLineService().getByOrder(order);            //Throws NoRecordException           
                    //check if all orderLines can be given a dvdCopy
                    Boolean allInStock = hasAllInStock(orderLines);
                    //send the user an order confirmation

                    String currency = "€";
                    if (CookieUtil.CookieExists("currency"))
                    {
                        if (CookieUtil.GetCookieValue("currency").Equals("usd"))
                        {
                            currency = "$";
                        }
                    }

                    new EmailModel().sendOrderConfirmationEmail(user, order, orderLines, allInStock, currency);

                    Response.Redirect("~/ThankYou.aspx");
                }
                catch (NoRecordException)
                {
                    
                }

                
            }
            else
            {
                lblStatus.Text = "Please log in to access this page";
            }
            

            
        }

        private Boolean hasAllInStock(List<OrderLine> orderLines)
        {
            Boolean allInStock = true;

            foreach (OrderLine orderLine in orderLines)
            {
                if (orderLine.dvdCopy == null)
                {
                    allInStock = false;
                }
            }

            return allInStock;
        }


    }
}