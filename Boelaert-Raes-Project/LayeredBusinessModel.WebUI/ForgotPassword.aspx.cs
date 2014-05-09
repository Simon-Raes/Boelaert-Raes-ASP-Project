using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using System.Net.Mail;
using System.Net;

using LayeredBusinessModel.BLL.Model;

namespace LayeredBusinessModel.WebUI
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            checkQueryString();
        }

        private void checkQueryString()
        {
            if(Request.QueryString["resetToken"]!=null)
            {
                PasswordResetModel passwordResetModel = new PasswordResetModel();
                passwordResetModel.checkResetRequestConfirmation(Request.QueryString["resetToken"]);
                //redirect user away from this page so het can't refresh the querystring URL and reset his password a second time.
                Response.Redirect("Index.aspx");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            CustomerService customerService = new CustomerService();
            Customer user = customerService.getCustomerWithEmail(txtPassword.Value);
            if (user == null)
            {
                user = customerService.getCustomerWithEmail(txtPassword.Value);
            }

            if(user!=null)
            {
                lblStatus.Text = "We've sent you an email to reset your password.";
                lblStatus.ForeColor = System.Drawing.Color.Green;

                PasswordResetModel model = new PasswordResetModel();
                model.sendPasswordResetRequest(user);               

            }
            else
            {
                lblStatus.Text = "Unknown email-address.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
            
        }
    }
}