using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using CustomException;

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

                try
                {
                    if (new PasswordResetModel().checkResetRequestConfirmation(Request.QueryString["resetToken"]))        //Throws NoRecordException
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
                catch (NoRecordException)
                {
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
            try
            {
                Customer user = new CustomerService().getByEmail(txtPassword.Value);          //Throws NoRecordException || DALException

                lblStatus.Text = "We've sent you an email to reset your password.";
                lblStatus.ForeColor = System.Drawing.Color.Green;

                new PasswordResetModel().sendPasswordResetRequest(user);
            }
            catch (NoRecordException)
            {
                //When no customer was found with the given e-mailadress
                lblStatus.Text = "Unknown email-address.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }            
        }
    }
}