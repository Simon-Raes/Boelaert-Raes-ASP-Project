﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;
using System.Web.UI.HtmlControls;


namespace LayeredBusinessModel.WebUI
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ////Menu opvullen met alle categoriën en genres
            //List<Category> categories = new CategoryService().getAll();
            //foreach (Category c in categories)
            //{
            //    HtmlGenericControl titlediv = new HtmlGenericControl("div");
            //    titlediv.Attributes["class"]="block-title-cat";
            //    HtmlAnchor a = new HtmlAnchor();
            //    a.HRef = "catalog.aspx?cat=" + c.category_id;
            //    titlediv.Controls.Add(a);
            //    HtmlGenericControl strong = new HtmlGenericControl("strong");
            //    a.Controls.Add(strong);
            //    HtmlGenericControl span = new HtmlGenericControl("span");
            //    a.Controls.Add(span);
            //    a.InnerText = c.name;
            //    dvMenu.Controls.Add(titlediv);

            //    List<Genre> genres = new GenreService().getGenresForCategory(c.category_id);
            //    foreach (Genre g in genres)
            //    {
            //        HtmlGenericControl contentdiv = new HtmlGenericControl("div");
            //        contentdiv.Attributes["class"] = "block-content-cat";

            //        HtmlGenericControl ul = new HtmlGenericControl("ul");
            //        ul.Attributes["class"] = "sf-menu";
            //        contentdiv.Controls.Add(ul);

            //        HtmlGenericControl li = new HtmlGenericControl("li");
            //        ul.Controls.Add(li);

            //        HtmlAnchor a2 = new HtmlAnchor();
            //        a2.HRef = "catalog.aspx?genre=" + g.genre_id;
            //        li.Controls.Add(a2);

            //        HtmlGenericControl strong2 = new HtmlGenericControl("strong");
            //        a2.Controls.Add(strong2);
            //        HtmlGenericControl span2 = new HtmlGenericControl("span");
            //        strong2.Controls.Add(span2);
            //        span2.InnerText = g.name;


            //        dvMenu.Controls.Add(contentdiv);

            //    }
            //}





            //Menu opvullen met alle categoriën en genres
            List<Category> categories = new CategoryService().getAll();
            foreach (Category c in categories)
            {       
                //category title
                HtmlGenericControl headerDiv = new HtmlGenericControl("div");
                headerDiv.Attributes["class"] = "item";
                headerDiv.InnerHtml = c.name;

                HtmlGenericControl divMenu = new HtmlGenericControl("div");
                divMenu.Attributes["class"] = "menu";

                headerDiv.Controls.Add(divMenu);
                divSideBar.Controls.Add(headerDiv);


                List<Genre> genres = new GenreService().getGenresForCategory(c.category_id);
                foreach (Genre g in genres)
                {                   

                    HtmlAnchor itemGenre = new HtmlAnchor();
                    itemGenre.Attributes["class"] = "item";
                    itemGenre.InnerHtml = g.name;
                    itemGenre.HRef = "catalog.aspx?genre=" + g.genre_id;
                    divMenu.Controls.Add(itemGenre);

                    divMenu.Controls.Add(itemGenre);
                }
            }

            //set login/user-button text
            if (Session["user"] == null)
            {
                btnLogin.Text = "Login";
                //todo: cart button dynamisch toevoegen ipv visibile/invisible setting te gebruiken
                btnCart.Visible = false;

            }
            else
            {
                //set buttons
                Customer user = (Customer)Session["user"];
                btnLogin.Text = user.name;
                btnCart.Visible = true;
                ShoppingCartService shoppingCartService = new ShoppingCartService();
                List<ShoppingcartItem> cartContent = shoppingCartService.getCartContentForCustomer(user.customer_id);
                btnCart.Text = "Cart: " + cartContent.Count;

                //don't change banner on every postback
                if (!Page.IsPostBack)
                {
                    //set personalised banners
                    if (user.numberOfVisits > 5)
                    {
                        //todo: 
                        //favoriete genre van user opzoeken (op basis van ordergeschiedenis), enkel banners van dat genre weergeven
                        arBanner.KeywordFilter = "scifi"; //tijdelijk vastgezet op scifi
                    }
                }

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

        protected void btnCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Cart.aspx");
        }

        protected void btnDev_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/dev.aspx");
        }

        /**
         * This method is fired when a user presses the Search button at the top of the page.
         * It will search on the title, author, barcode, description and categorie. 
        **/
        protected void btnMainSearch_Click(object sender, EventArgs e)
        {
            String searchText = txtSearch.Text;
        }
    }
}