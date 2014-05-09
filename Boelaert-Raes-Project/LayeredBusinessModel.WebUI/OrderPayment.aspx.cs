using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;
using System.Data;
using LayeredBusinessModel.BLL.Model;

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
                    Order order = orderService.getOrder(orderID);
                    if (order != null)
                    {
                        if (order.customer.customer_id == user.customer_id)
                        {
                            //all good
                            lblStatus.Text = orderID;
                            OrderModel helper = new OrderModel();
                            lblCost.Text = "Total cost: " + helper.getOrderCost(orderID);
                            OrderLineService orderLineService = new OrderLineService();
                            List<OrderLine> orderLines = orderLineService.getOrderLinesForOrder(order);

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
                            if(hasRentItems)
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
                                if(item.orderLineType.id == 1)
                                {
                                    orderRow[3] = item.startdate.ToString("dd/MM/yyyy");
                                    orderRow[4] = item.enddate.ToString("dd/MM/yyyy"); 
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
                    else
                    {
                        //order does not exist, error
                        lblStatus.Text = "404 - Page not found";
                    }

                }
                else
                {
                    //not logged in, access denied
                    lblStatus.Text = "Please log in to access this page";
                }

            }
            //check query string, get order

            //check if the order belongs to the logged in user! Only grant access if the user and order.user match
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            Customer user = (Customer)Session["user"];
            if(user!=null)
            {
                //get the order
                String orderID = lblStatus.Text;
                OrderService orderService = new OrderService();
                Order order = orderService.getOrder(orderID);

                //pay for the order
                OrderModel helper = new OrderModel();
                helper.payOrder(order);

                //get the orderLines
                OrderLineService orderLineService = new OrderLineService();
                List<OrderLine> orderLines = orderLineService.getOrderLinesForOrder(order);
                //check if all orderLines can be given a dvdCopy
                Boolean allInStock = hasAllInStock(orderLines);
                //send the user an order confirmation
                EmailModel emailModel = new EmailModel();
                emailModel.sendOrderConfirmationEmail(user, order, orderLines, allInStock);

                //redirect away from the payment page - todo: go to thank you/info page
                Response.Redirect("~/Index.aspx");
            }
            else
            {
                //todo: error about not being logged in
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