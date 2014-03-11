using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;

namespace LayeredBusinessModel.WebUI
{
    public partial class Customers : System.Web.UI.Page
    {
        private CustomerService customerService;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                customerService = new CustomerService();

                gvBeer.DataSource = customerService.getAll();
                gvBeer.DataBind();
            }
        }
    }
}