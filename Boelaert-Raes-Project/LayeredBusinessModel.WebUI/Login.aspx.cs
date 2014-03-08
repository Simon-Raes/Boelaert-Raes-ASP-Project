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
    public partial class Login : System.Web.UI.Page
    {
        CustomerService customerService;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            customerService = new CustomerService();
            Customer customer = customerService.getCustomerWithLogin(txtEmail.Text);
            
            //todo: validators

            //een null customer object geeft hier nog altijd true, daarom controle op password veld
            if (customer.password != null)
            {
                if (customer.password.Equals(txtPassword.Text))
                {
                    //update user's number_of_visits
                    customer.numberOfVisits++;
                    customerService.updateCustomer(customer);

                    //put user in session and redirect to index (better would be to redirect to last active page)
                    Session["user"] = customer;                    
                    Response.Redirect("~/Home.aspx");
                }
                else
                {
                    //incorrect password
                    lblStatus.Text = "Incorrect login/password combination";
                }
            }
            else
            {
                //no such user 
                lblStatus.Text = "Incorrect login name.";

            }

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            //go to register page
            Response.Redirect("~/Register.aspx");

        }
    }
}