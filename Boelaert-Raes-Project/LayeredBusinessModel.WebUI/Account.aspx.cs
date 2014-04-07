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
    public partial class Account : System.Web.UI.Page
    {
        private List<Order> customerOrders;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mvAccount.ActiveViewIndex = 0;

                //todo: only update(create?) textfields when user accesses the settings page
                Customer user = (Customer)Session["user"];
                if (user != null)
                {
                    //settings page
                    txtName.Text = user.name;
                    txtEmail.Text = user.email;
                    //textfield wil geen tekst tonen als ik dit instel via properties, op deze manier werkt het wel
                    txtPassword.Attributes["type"] = "password";
                    txtPasswordAgain.Attributes["type"] = "password";
                    txtPassword.Text = user.password;
                    txtPasswordAgain.Text = user.password;

                    //orders page
                    OrderService orderService = new OrderService();
                    customerOrders = orderService.getOrdersForCustomer(user.customer_id);
                    gvOrders.DataSource = customerOrders;
                    gvOrders.DataBind();
                }
            }
        }

        protected void menuAccount_MenuItemClick(object sender, MenuEventArgs e)
        {
            int selection = Convert.ToInt16(menuAccount.SelectedValue);
            mvAccount.SetActiveView(mvAccount.Views[selection]);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //only update if validators were passed
            if (Page.IsValid)
            {
                Customer user = (Customer)Session["user"];

                //only update if user is currently logged in
                if (user != null)
                {
                    //create customer object based on logged-in-user info and info from textfields
                    Customer customer = new Customer
                    {
                        customer_id = user.customer_id,
                        email = txtEmail.Text,
                        login = user.login,
                        name = txtName.Text,
                        numberOfVisits = user.numberOfVisits,
                        password = txtPassword.Text,

                    };

                    //update user's database data
                    CustomerService customerService = new CustomerService();
                    customerService.updateCustomer(customer);

                    //also update his info in the session
                    Session["user"] = customer;
                }
                else
                {
                    string script = "alert(\"You have been logged out due to inactivity. Please log in to change your details.\");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                    //redirect to login
                }

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
                    btnPay.Visible = false;
                }
                else
                {
                    btnPay.Visible = true;
                }

                //get all articles in the order and display them
                OrderLineService orderLineService = new OrderLineService();
                List<OrderLine> orderLines = orderLineService.getOrderLinesForOrder(Convert.ToInt32(orderID));
                gvOrderDetails.DataSource = orderLines;
                gvOrderDetails.DataBind();
            }
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            //get the order
            String orderID = lblOrderID.Text;
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

                    //mark the found copy as NOT in_stock
                    copy.in_stock = false;
                    dvdCopyService.updateCopy(copy);
                }
                else
                {
                    //not in stock, will not be assigned a copy!!
                    //todo: handle this error some way, display error for this item or let the user know this item is currently out of stock
                }
            }
        }
    }
}