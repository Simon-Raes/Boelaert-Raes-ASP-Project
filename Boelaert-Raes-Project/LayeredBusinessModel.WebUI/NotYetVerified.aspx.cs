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
using CustomException;

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
            if(email != null)
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
    }
}