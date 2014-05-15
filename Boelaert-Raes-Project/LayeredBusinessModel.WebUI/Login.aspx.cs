using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using System.Text;
using System.Security.Cryptography;
using CustomException;

namespace LayeredBusinessModel.WebUI
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                CustomerService customerService = new CustomerService();
                Customer customer = customerService.getByEmail(txtEmail.Text);              //Throws NoRecordException || DALException

                //todo: validators

                if (CryptographyModel.decryptPassword(customer.password).Equals(txtPassword.Text))
                {
                    //update user's number_of_visits
                    customer.numberOfVisits++;
                    customerService.updateCustomer(customer);

                    //put user in session and redirect to index (better would be to redirect to last active page)
                    Session["user"] = customer;
                    Response.Redirect("~/Index.aspx");
                }
                else
                {
                    //incorrect password
                    lblStatus.Text = "Incorrect login/password combination";
                }
            }
            catch(NoRecordException)
            {
                //no such user 
                lblStatus.Text = "Unknown user.";
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            //go to register page
            Response.Redirect("~/Register.aspx");
        }
    }
}