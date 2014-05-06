using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;

namespace LayeredBusinessModel.WebUI
{
    public partial class Account : System.Web.UI.Page
    {
        private List<Order> customerOrders;

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        public void btnLogOut_Click(object sender, EventArgs e)
        {
            Session["user"] = null;
            Response.Redirect("~/Index.aspx");
        }
    }
}