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
                            lblCost.Text = "Total cost: " + getOrderInfo(orderID);
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
            OrderService orderService = new OrderService();
            Order selectedOrder = orderService.getOrder(orderID);

            //set the order status to PAID
            selectedOrder.orderstatus_id = 2;
            orderService.updateOrder(selectedOrder);

            //assign copies to the orderlines
            OrderLineService orderLineService = new OrderLineService();
            List<OrderLine> orderLines = orderLineService.getOrderLinesForOrder(Convert.ToInt32(orderID));




            DvdCopyService dvdCopyService = new DvdCopyService();
            List<DvdCopy> availableCopies = null;
            DvdCopy copy = null;

            Boolean allInStock = true;

            foreach (OrderLine orderLine in orderLines)
            {
                //get all available copies
                //get this list again for every item so you can add a new, available copy to the order (todo: do this without going to the database for every item)
                if (orderLine.order_line_type_id == 1) //rent copy
                {
                    availableCopies = dvdCopyService.getAllInStockRentCopiesForDvdInfo(Convert.ToString(orderLine.dvd_info_id));
                }
                else if (orderLine.order_line_type_id == 2) //sale copy
                {
                    availableCopies = dvdCopyService.getAllInStockBuyCopiesForDvdInfo(Convert.ToString(orderLine.dvd_info_id));
                }

                //check if there is a copy available
                if (availableCopies.Count > 0)
                {
                    //get the first available copy
                    copy = availableCopies[0];

                    //set the found copy as the copy for this orderline
                    orderLine.dvd_copy_id = copy.dvd_copy_id;
                    orderLineService.updateOrderLine(orderLine);

                    //update the amount_sold field of the dvdInfo record
                    DvdInfoService dvdInfoService = new DvdInfoService();
                    DvdInfo dvdInfo = dvdInfoService.getDvdInfoWithID(copy.dvd_info_id.ToString());
                    dvdInfo.amount_sold = dvdInfo.amount_sold + 1;
                    dvdInfoService.updateDvdInfo(dvdInfo);

                    //mark the found copy as NOT in_stock
                    copy.in_stock = false;
                    dvdCopyService.updateCopy(copy);
                }
                else
                {
                    allInStock = false;
                    //not in stock, will not be assigned a copy!!
                    //todo: handle this error some way, display error for this item or let the user know this item is currently out of stock
                }
            }
        }

        private String getOrderInfo(String orderID)
        {
            double totalCost = 0;

            OrderLineService orderLineService = new OrderLineService();
            DvdInfoService dvdInfoService = new DvdInfoService();

            List<OrderLine> orderLines = orderLineService.getOrderLinesForOrder(Convert.ToInt32(orderID));

            
            //set gridview
            gvOrderDetails.DataSource = orderLines;
            gvOrderDetails.DataBind();

            //set order total cost
            foreach(OrderLine orderLine in orderLines)
            {
                if(orderLine.order_line_type_id==1) //rent
                {
                    TimeSpan duration = orderLine.enddate - orderLine.startdate;
                    int durationDays = duration.Days;

                    totalCost += dvdInfoService.getDvdInfoWithID(orderLine.dvd_info_id.ToString()).rent_price * durationDays;
                } 
                else if(orderLine.order_line_type_id == 2) //buy
                {
                    totalCost += dvdInfoService.getDvdInfoWithID(orderLine.dvd_info_id.ToString()).buy_price;
                }
            }

            return totalCost.ToString();
        }
    }
}