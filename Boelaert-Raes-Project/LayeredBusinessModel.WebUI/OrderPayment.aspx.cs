using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;

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
                            OrderHelper helper = new OrderHelper();
                            lblCost.Text = "Total cost: " + helper.getOrderCost(orderID);
                            OrderLineService orderLineService = new OrderLineService();
                            List<OrderLine> orderLines = orderLineService.getOrderLinesForOrder(Convert.ToInt32(orderID));


                            //set gridview
                            gvOrderDetails.DataSource = orderLines;
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
            OrderHelper helper = new OrderHelper();
            helper.payOrder(orderID);

            //redirect away from the payment page - todo: go to thank you/info page
            Response.Redirect("~/Index.aspx");
        }


    }
}