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
    public partial class AccountSettings : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Customer user = (Customer)Session["user"];
            if (user != null)
            {
                inputName.Value = user.name;
                inputEmail.Value = user.email;
            }
        }

        protected void valCustEmail_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomerService customerService = new CustomerService();
            Customer cust = customerService.getCustomerWithEmail(inputEmail.Value);
            if (cust == null)
            {
                //email still available
                args.IsValid = true;
            }
            else
            {
                if (cust.email.Equals(((Customer)Session["user"]).email))
                {
                    //user didn't change his email, OK
                    args.IsValid = true;
                }
                else
                {
                    //email already taken
                    args.IsValid = false;
                }
            }
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
                    //only update if the user entered the correct old password
                    if (inputOldPassword.Value.Equals(CryptographyModel.decryptPassword(user.password)))
                    {
                        //create customer object based on logged-in-user info and info from textfields
                        Customer customer = new Customer
                        {
                            customer_id = user.customer_id, //keep existing customer_id
                            email = inputEmail.Value,
                            login = user.login, //keep existing login
                            name = inputName.Value,
                            numberOfVisits = user.numberOfVisits, //keep existing numberOfVisits
                            password = CryptographyModel.encryptPassword(inputPassword.Value)
                        };

                        //update user's database data
                        CustomerService customerService = new CustomerService();
                        customerService.updateCustomer(customer);

                        //also update his info in the session
                        Session["user"] = customer;

                        lblStatus.Text = "Your info was succesfully updated.";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = "Incorrect old password!";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    string script = "alert(\"You have been logged out due to inactivity. Please log in to change your details.\");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                    //redirect to login
                    Response.Redirect("Index.aspx");
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