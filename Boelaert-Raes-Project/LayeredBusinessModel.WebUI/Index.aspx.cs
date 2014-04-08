using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace LayeredBusinessModel.WebUI
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setupNewReleases();
            }
            
        }

        private void setupNewReleases()
        {
            int counter = 0;
            List<DvdInfo> dvdList = new DvdInfoService().getLatestDvds();

            
            foreach (DvdInfo d in dvdList)
            {
                counter++;

                HtmlGenericControl li = new HtmlGenericControl("li");
                li.Attributes["class"] = "item col-3";
                Image img = new Image();
                img.ImageUrl = d.image;
                li.Controls.Add(img);

                HtmlGenericControl div = new HtmlGenericControl("div");
                div.Attributes["class"] = "product-shop";

                li.Controls.Add(div);

                HtmlGenericControl h3 = new HtmlGenericControl("h3");
                h3.Attributes["class"] = "product-name";

                div.Controls.Add(h3);

                HtmlAnchor a = new HtmlAnchor();
                a.HRef = "catalog.aspx?dvdinfo=" + d.dvd_info_id;
                a.InnerText = d.name;
                h3.Controls.Add(a);

                HtmlGenericControl author = new HtmlGenericControl("div");
                author.Attributes["class"] = "product-author";
                author.InnerText = d.author;
                div.Controls.Add(author);


                HtmlGenericControl price = new HtmlGenericControl("div");
                price.Attributes["class"] = "price-box";
                div.Controls.Add(price);


                HtmlGenericControl span1 = new HtmlGenericControl("span");
                span1.Attributes["class"] = "regular-price";
                price.Controls.Add(span1);
                HtmlGenericControl span2 = new HtmlGenericControl("span");
                span2.Attributes["class"] = "price";
                span1.Controls.Add(span2);
                span2.InnerText = "€ " + d.buy_price;

                HtmlGenericControl actions = new HtmlGenericControl("div");
                actions.Attributes["class"] = "actions";

                div.Controls.Add(actions);

                HtmlAnchor cartlink = new HtmlAnchor();
                cartlink.Attributes["class"] = "button btn-cart";
                cartlink.HRef = "#";
                cartlink.InnerText = "Add to Cart";

                actions.Controls.Add(cartlink);

                if(counter < 4) {
                    row_1.Controls.Add(li);
                } else {
                    row_2.Controls.Add(li);
                }
                
            }
        }
    }
}