using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL.Database;
using LayeredBusinessModel.BLL.Model;
using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;

namespace LayeredBusinessModel.WebUI
{
    public partial class NotYetVerified : System.Web.UI.Page
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnResendVerification_Click(object sender, EventArgs e)
        {
            String email = Request.QueryString["email"];
            if(email!=null)
            {
                SignUpModel signUpModel = new SignUpModel();
                signUpModel.sendVerificationForEmail(email);
                
            }
        }
    }
}