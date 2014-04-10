using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LayeredBusinessModel.WebUI
{
    public partial class usercontroltestpage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            dvdInfoUserControl.title = "hallo";
            dvdInfoUserControl.buy_price = 10.7f;
            dvdInfoUserControl.rent_price = 3.5f;

        }
    }
}