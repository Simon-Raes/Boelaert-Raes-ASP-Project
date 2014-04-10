using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LayeredBusinessModel.WebUI
{
    public partial class dvdInfoUserControl : System.Web.UI.UserControl
    {
        public int id { get; set; }
        public String imageUrl { get; set; }
        public String title { get; set; }
        public float buy_price { get; set; }
        public float rent_price { get; set; }        


        protected void Page_Load(object sender, EventArgs e)
        {
            dvdInfoLink.NavigateUrl = "~/detail.aspx?id=" + id;
            dvdInfoLink2.NavigateUrl = dvdInfoLink.NavigateUrl;
            imgDvdCover.ImageUrl = imageUrl;
            lblTitle.Text = title;
            btnBuy.Text = "Buy € " + buy_price;
            btnRent.Text = "Rent € " + rent_price;
        }
    }
}