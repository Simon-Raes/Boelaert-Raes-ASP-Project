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
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            CustomerService customerService = new CustomerService();
            Customer user = customerService.getCustomerWithEmail(txtPassword.Value);
            if (user == null)
            {
                user = customerService.getCustomerWithLogin(txtPassword.Value);
            }

            if(user!=null)
            {
                lblStatus.Text = "We've sent you an email to reset your password (not really, not yet implemented).";
                lblStatus.ForeColor = System.Drawing.Color.Green;
                //todo: send email asking the user if he requested the reset.
                //email should contain a link to the website that resets the password when used

                //after link gets clicked: generate new random password for the user and mail it to them
            }
            else
            {
                lblStatus.Text = "Unknown email-address or login name.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
            
        }
    }
}