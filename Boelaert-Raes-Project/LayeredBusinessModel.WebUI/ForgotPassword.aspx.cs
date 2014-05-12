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
                divDefault.Visible = false;
                divResetComplete.Visible = true;

                PasswordResetModel passwordResetModel = new PasswordResetModel();
                if(passwordResetModel.checkResetRequestConfirmation(Request.QueryString["resetToken"]))
                {
                    lblHeader.Text = "Password reset complete";
                    lblStatusComplete.Text = "We've sent you an email with your new password.";
                }
                else
                {
                    lblHeader.Text = "An error occurred";
                    lblStatusComplete.Text = "Something went wrong when trying to reset your password. Please contact support if this problem persists.";
                }
            }
            else
            {
                divDefault.Visible = true;
                divResetComplete.Visible = false;
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