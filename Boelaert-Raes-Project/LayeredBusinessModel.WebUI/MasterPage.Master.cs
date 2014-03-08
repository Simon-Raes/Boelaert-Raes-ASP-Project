using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.Domain;

namespace LayeredBusinessModel.WebUI
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //set login/user-button text
            if (Session["user"] == null)
            {
                btnLogin.Text = "Login";
            }
            else
            {
                Customer user = (Customer) Session["user"];
                btnLogin.Text = user.name;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Session["user"] == null)
            {                
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                //redirect to account page (purchase history, account settings,...)
                Response.Redirect("~/Account.aspx");

            }
        }
    }
}