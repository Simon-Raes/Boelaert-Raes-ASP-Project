using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;

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
                OrderService orderService = new OrderService();
                customerOrders = orderService.getOrdersForCustomer(user.customer_id);
                gvOrders.DataSource = customerOrders;
                gvOrders.DataBind();
            }
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument.ToString());
            String orderID = gvOrders.Rows[index].Cells[1].Text;

            if (e.CommandName == "Details")
            {
                //show details
                pnlOrderDetails.Visible = true;

                //get the order info
                OrderService orderService = new OrderService();
                Order selectedOrder = orderService.getOrder(orderID);
                lblOrderStatus.Text = selectedOrder.orderstatus_id.ToString(); //1 = new, 2 = paid, 3 = shipped
                lblOrderID.Text = selectedOrder.order_id.ToString();

                //hide pay button if the order has already been paid
                if (selectedOrder.orderstatus_id != 1)
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
                OrderLineService orderLineService = new OrderLineService();
                List<OrderLine> orderLines = orderLineService.getOrderLinesForOrder(Convert.ToInt32(orderID));
                gvOrderDetails.DataSource = orderLines;
                gvOrderDetails.DataBind();


                //check if all items have been assigned a copy
                Boolean allInStock = true;

                //user has already paid, check status of copies in cart
                if (selectedOrder.orderstatus_id > 1)
                {
                    foreach (OrderLine orderLine in orderLines)
                    {
                        if (orderLine.dvd_copy_id <= 0)
                        {
                            allInStock = false;
                        }
                    }
                    updateOrderStatusDetails(allInStock);
                }
                else
                {
                    //user hasn't paid yet, check status of copies in store: (todo)
                }
            }
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            //get the order
            String orderID = lblOrderID.Text;

            //redirect to payment page, with query string to connect to the order
            Response.Redirect("~/OrderPayment.aspx?order=" + orderID);
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

        protected void gvOrderDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("CancelOrderLine"))
            {
                //get the orderline
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                String orderLineID = gvOrderDetails.Rows[index].Cells[1].Text;
                OrderLineService orderLineService = new OrderLineService();
                OrderLine orderLine = orderLineService.getOrderLine(orderLineID);

                if (orderLine.order_line_type_id == 1) //only allow cancelling of rent-lines (type 1)
                {
                    if ((orderLine.startdate - DateTime.Today.Date) >= TimeSpan.FromDays(2)) //only allow cancelling if there is at least 2 days between today and the startdate
                    {
                        if(orderLineService.removeOrderLine(orderLine))
                        {
                            //succesfully removed
                            string script = "alert(\"The item has been removed from your order. (Refresh to view changes (temp))\");";
                            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                            //todo: update gridview (reload content, re-bind?)
                        }
                        else
                        {
                            //something went wrong
                            string script = "alert(\"An error occured while trying to removing this item from your order.\");";
                            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                        }
                    }
                    else
                    {
                        //can't remove this copy anymore - too late
                        string script = "alert(\"This item will ship within 2 days and can no longer be cancelled!\");";
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
    }
}