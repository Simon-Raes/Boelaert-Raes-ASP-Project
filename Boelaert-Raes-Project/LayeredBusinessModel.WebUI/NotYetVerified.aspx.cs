using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using CustomException;
using System.Drawing;

namespace LayeredBusinessModel.WebUI
{
    public partial class NotYetVerified : System.Web.UI.Page
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
                String email = Request.QueryString["email"];
                if (email != null)
                {
                    pnlNoEmail.Visible = false;
                    pnlWithEmail.Visible = true;
                }
            else {
                pnlNoEmail.Visible = true;
                pnlWithEmail.Visible = false;
            }
        }

        protected void btnResendVerification_Click(object sender, EventArgs e)
        {
            String email = Request.QueryString["email"];
            if (email != null)
            {                
                try
                {
                    new SignUpModel().sendVerificationForEmail(email);            //Throws NoRecordException || DALException  
                }
                catch (NoRecordException)
                {

                }
            }
            
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            txtEmailError.ForeColor = Color.Red;
            if (txtEmail.Text != null)
            {
                //check if user is allready verrified
                Customer custmer = new CustomerService().getByEmail(txtEmail.Text);
                if (custmer.isVerified)
                {
                    txtEmailError.Text = "This account is allready activated!";
                }
                else
                {
                    new SignUpModel().sendVerificationForEmail(txtEmail.Text);
                    txtEmailError.Text = "We've send you an email with further instructions.";
                    txtEmailError.ForeColor = Color.Black;
                }

            }
            else
            {
                txtEmailError.Text = "Please fill in your emailaddress.";
            }
        }
    }
}