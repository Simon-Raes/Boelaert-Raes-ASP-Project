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
            if(Request.QueryString["resetId"]!=null)
            {
                //if(databaseContainsThisResetID)
                //{
                    
                //get user from database (using resetID)
                //Customer user = database.getUserfromresetstringthingamagik();
 

                //}

                //generate new password
                String newPassword = "randomTextGeneration!";

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    //todo: put credentials in web.config
                    Credentials = new NetworkCredential("taboelaertraesa@gmail.com", "KathoVives"),
                    EnableSsl = true
                };
                client.Send("info@TaboelaertRaesa.com", "user.email", "Your Taboelaert Raesa password has been reset", "Dear " + "user.name" +
                    ",\n\nyour password has been reset to: '" + newPassword + "'." +
                    "For security reasons, please change this password next time you log in." +
                    "\n\nRegards,\nThe Taboelaert Raesa team.");
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
                
                SmtpClient clienta = new SmtpClient("smtp.gmail.com", 587)
                {
                    //todo: put credentials in web.config
                    Credentials = new NetworkCredential("taboelaertraesa@gmail.com", "KathoVives"),
                    EnableSsl = true
                };
                clienta.Send("info@TaboelaertRaesa.com", user.email, "Password reset request", "Dear " + user.name + ",\n\n. " +
                    "We received a request to reset the password on your Taboelaert Raesa account. Click the following URL to complete the process:\n"+
                    "(URL with querystring here ..../ForgotPassword?resetId=xxxxx (id should already be in database to compare to querystring)\n\n"+
                    "If you did not request this reset, you can ignore this email.\n\nRegards,\nThe Taboelaert Raesa team.");

                //generate new passwordReset record: useremail - resetID
                
               

            }
            else
            {
                lblStatus.Text = "Unknown email-address or login name.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
            
        }
    }
}