using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using CustomException;

namespace LayeredBusinessModel.WebUI
{
    public partial class Customers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    gvBeer.DataSource = new CustomerService().getAll();           //Throws NoRecordException
                    gvBeer.DataBind();
                }
                catch(NoRecordException ex) 
                {
                    int i = 0;
                }
            }
        }
    }
}