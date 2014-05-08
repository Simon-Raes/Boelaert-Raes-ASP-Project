using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;
using System.Data;

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
                        if (order.customer_id == user.customer_id)
                        {
                            //all good
                            lblStatus.Text = orderID;
                            OrderModel helper = new OrderModel();
                            lblCost.Text = "Total cost: " + helper.getOrderCost(orderID);
                            OrderLineService orderLineService = new OrderLineService();
                            List<OrderLine> orderLines = orderLineService.getOrderLinesForOrder(Convert.ToInt32(orderID));

                            Boolean hasRentItems = false;

                            foreach (OrderLine item in orderLines)
                            {
                                if (item.order_line_type_id == 1)
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
                                orderRow[1] = item.dvd_info_name;
                                orderRow[2] = item.order_line_type_name;
                                if(item.order_line_type_id == 1)
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
            //get the order
            String orderID = lblStatus.Text;

            //pay for the order
            OrderModel helper = new OrderModel();
            helper.payOrder(orderID);

            //redirect away from the payment page - todo: go to thank you/info page
            Response.Redirect("~/Index.aspx");
        }


    }
}