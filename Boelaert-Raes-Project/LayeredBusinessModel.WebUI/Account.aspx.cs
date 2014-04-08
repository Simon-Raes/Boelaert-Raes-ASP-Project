﻿using System;
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
                if(selectedOrder.orderstatus_id>1)
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
    }
}