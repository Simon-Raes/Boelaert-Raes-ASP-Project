using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using System.Data;
using LayeredBusinessModel.BLL.Model;

namespace LayeredBusinessModel.WebUI
{
    public partial class AccountOrders : System.Web.UI.Page
    {
        private List<Order> customerOrders;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            Customer user = (Customer)Session["user"];
            if (user != null)
            {
                fillOrdersGridView();                        
            }
            else
            {
                //todo: error about not being logged in
            }
        }

        private void fillOrdersGridView()
        {
            OrderService orderService = new OrderService();
            customerOrders = orderService.getOrdersForCustomer(((Customer)Session["user"]));
            if(customerOrders.Count>0)
            {
                DataTable orderTable = new DataTable();
                orderTable.Columns.Add("Ordernumber");
                orderTable.Columns.Add("Status");
                orderTable.Columns.Add("Cost");
                orderTable.Columns.Add("Date");

                foreach (Order item in customerOrders)
                {
                    DataRow orderRow = orderTable.NewRow();
                    orderRow[0] = item.order_id;
                    orderRow[1] = item.orderstatus.name;
                    OrderModel orderModel = new OrderModel();
                    orderRow[2] = Math.Round(orderModel.getOrderCost(item), 2);
                    orderRow[3] = item.date.ToString("dd/MM/yyyy");
                    orderTable.Rows.Add(orderRow);
                }

                gvOrders.DataSource = orderTable;
                gvOrders.DataBind();  
            }
            else
            {
                lblNoOrders.Visible = true;
            }            
        }

        /*Handles rowCommands for the Orders gridview*/
        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {      
            if (e.CommandName == "Details")
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                String orderID = gvOrders.Rows[index].Cells[1].Text;

                displayOrderDetails(orderID);
            }
        }

        /*Displays the details of the selected order.*/
        private void displayOrderDetails(String orderID)
        {
            //show details
            pnlOrderDetails.Visible = true;

            //get the order info
            OrderService orderService = new OrderService();
            Order selectedOrder = orderService.getOrder(orderID);
            lblOrderStatus.Text = "(" + selectedOrder.orderstatus.name + ")"; //1 = new, 2 = paid, 3 = shipped
            lblOrderID.Text = selectedOrder.order_id.ToString();

            //hide pay button if the order has already been paid
            if (selectedOrder.orderstatus.id != 1)
            {
                lblPay.Visible = false;
                btnPay.Visible = false;
            }
            else
            {
                lblPay.Visible = true;
                btnPay.Visible = true;
            }

            //get all articles in the order and display them
            Order order = orderService.getOrder(orderID);
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
                    orderRow[3] = Math.Round(item.dvdInfo.rent_price * (item.enddate - item.startdate).Days, 2);
                    orderRow[4] = item.startdate.ToString("dd/MM/yyyy");
                    orderRow[5] = item.enddate.ToString("dd/MM/yyyy");
                }
                else
                {
                    orderRow[3] = Math.Round(item.dvdInfo.buy_price, 2);
                }

                orderTable.Rows.Add(orderRow);
            }


            gvOrderDetails.DataSource = orderTable;
            gvOrderDetails.DataBind();

            //total cost
            OrderModel orderModel = new OrderModel();
            lblTotalCost.Text = orderModel.getOrderCost(order).ToString();

            //user has already paid, check status of copies in cart
            if (selectedOrder.orderstatus.id > 1)
            {

                Boolean allInStock = hasAllInStock(orderLines);

                updateOrderStatusDetails(allInStock);
            }
            else
            {
                //user hasn't paid yet, check status of copies in store: (todo) (?)
            }
        }


        protected void btnPay_Click(object sender, EventArgs e)
        {
            Customer user = (Customer)Session["user"];
            if(user!=null)
            {
                //get the order
                String orderID = lblOrderID.Text;  
                
                //redirect to payment page, with query string to connect to the order
                Response.Redirect("~/OrderPayment.aspx?order=" + orderID);
            }
            else
            {
                //todo= error about not being logged in
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

        private void updateOrderStatusDetails(Boolean allInStock)
        {
            if (!allInStock)
            {
                //user has paid, but not all orderlines could be assigned an available copy
                lblOrderStatusDetails.Text = "Some items in this order are currently out of stock. Your order will be dispatched as soon as they become available.";
                lblOrderStatusDetails.ForeColor = System.Drawing.Color.Orange;
            }
            else
            {
                lblOrderStatusDetails.Text = "";
            }
        }

        /*Removes an item from the order if allowed.*/
        protected void gvOrderDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("CancelOrderLine"))
            {
                //get the orderline
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                String orderLineID = gvOrderDetails.Rows[index].Cells[1].Text;
                OrderLineService orderLineService = new OrderLineService();
                OrderLine orderLine = orderLineService.getOrderLine(orderLineID);

                if (orderLine.orderLineType.id == 1) //only allow cancelling of rent-lines (type 1)
                {
                    if ((orderLine.startdate - DateTime.Today.Date) >= TimeSpan.FromDays(2)) //only allow cancelling if there is at least 2 days between today and the startdate
                    {
                        if(orderLineService.removeOrderLine(orderLine))
                        {
                            //succesfully removed - refresh the details gridview                            
                            displayOrderDetails(orderLine.order.order_id.ToString());
                        }
                        else
                        {
                            //something went wrong
                            string script = "alert(\"An error occured while trying to removing this item from your order.\");";
                            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                        }
                    }
                    else if ((orderLine.startdate - DateTime.Today.Date) < TimeSpan.FromDays(0))
                    {
                        //can't remove this copy anymore - too late
                        string script = "alert(\"Can't cancel an item that has already shipped!\");";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                    }
                    else
                    {
                        //can't remove this copy anymore - too late
                        string script = "alert(\"This item will ship within 2 days and can no longer be cancelled.\");";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                    }
                }
                else
                {
                    //can't remove a BUY copy
                    string script = "alert(\"Only rent-copies can be cancelled.\");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                }
            }
        }

        public void btnLogOut_Click(object sender, EventArgs e)
        {
            Session["user"] = null;
            Response.Redirect("~/Index.aspx");
        }
    }
}