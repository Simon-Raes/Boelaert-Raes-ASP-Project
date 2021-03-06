﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;

namespace LayeredBusinessModel.WebUI
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlStep1.Visible = true;
                pnlStep2.Visible = false;
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            String email = txtEmail.Text;
            String subject = txtSubject.Text;
            String message = txtMessage.Text;

            new EmailModel().sendContactMail(email, subject, message);
            pnlStep1.Visible = false;
            pnlStep2.Visible = true;
        }
    }
}