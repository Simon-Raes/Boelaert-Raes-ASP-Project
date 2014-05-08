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
                lblStatus.Text = "We've sent you an email to reset your password (not really, not yet implemented).";
                lblStatus.ForeColor = System.Drawing.Color.Green;
                //todo: send email asking the user if he requested the reset.
                //email should contain a link to the website that resets the password when used

                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    //todo: put credentials in web.config
                    Credentials = new NetworkCredential("taboelaertraesa@gmail.com", "KathoVives"),
                    EnableSsl = true
                };
                client.Send("info@TaboelaertRaesa.com", "user-email@gmail.com", "Password reset", "Dear username, your password has been reset to: xdf24Te. Please change "+
                    "this password next time you log in. security etc.");
                Console.WriteLine("Sent");
                Console.ReadLine();

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