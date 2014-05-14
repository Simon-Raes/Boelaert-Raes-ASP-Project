using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;
using LayeredBusinessModel.BLL.Model;

using System.Web.UI.HtmlControls;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Specialized;
using CustomException;

namespace LayeredBusinessModel.WebUI
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                fillSideBar();
            }
            catch (NoRecordException ex)
            {
                //bacause this a main function - it fills the sidebar with the genres - this exception will be thrown up so that the global.asax can take care of the exception
                //and redirect the user to the error page

                //for debugging reasons i will place a breakpoint here. I'll work my way throu the WebUI layer until i have handled every exception. After that i'll throws the exceptions 
                //throw;
            }


            setupCurrencyLinks();

            //set login/user-button text
            if (Session["user"] == null)
            {
                liCart.Visible = false;
                liLogin.Visible = true;
                liAccount.Visible = false;
                liSignup.Visible = true;

                //btnLogin.Text = "Login";
                //todo: cart button dynamisch toevoegen ipv visibile/invisible setting te gebruiken
                //btnCart.Visible = false;

            }
            else
            {
                liCart.Visible = true;
                liLogin.Visible = false;
                liAccount.Visible = true;
                liSignup.Visible = false;

                //set buttons
                Customer user = (Customer)Session["user"];
                liAccount.InnerHtml = "<a href='Overview.aspx'>" + user.name + "</a>";
                ShoppingCartService shoppingCartService = new ShoppingCartService();
                List<ShoppingcartItem> cartContent = shoppingCartService.getCartContentForCustomer(user);
                liCart.InnerText = "Cart: " + cartContent.Count;

            }
        }

        private void setupCurrencyLinks()
        {
            //als de querystring leeg is
            if (Request.QueryString.Count == 0)
            {
                //query string aanmaken
                euroLink.HRef = Request.Url.AbsoluteUri + "?currency=euro";
                dollerLink.HRef = Request.Url.AbsoluteUri + "?currency=usd";
            }
            //als de querystring al bestaat
            else
            {
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                string[] separateURL = url.Split('?');
                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(separateURL[1]);
                if (Request.QueryString["currency"] != null)
                {
                    queryString.Remove("currency");
                }
                //als de querystring al bestaat, deze uitbreiden
                euroLink.HRef = separateURL[0] + "?" + queryString.ToString() + "&currency=euro";
                dollerLink.HRef = separateURL[0] + "?" + queryString.ToString() + "&currency=usd";
            }


            //als de querystring het attribut currency bevat (als de gebruiker op één van de twee currencylinks heeft geklikt)
            if (Request.QueryString["currency"] != null)
            {
                //cookie aanmaken/updaten en currencysymbol updaten
                CookieUtil.CreateCookie("currency", Request.QueryString["currency"], 30);
                currencySymbol.Attributes["class"] = "glyphicon glyphicon-" + Request.QueryString["currency"];
            }
            else
            {
                //kijken of de cookie bestaat
                if (CookieUtil.CookieExists("currency"))
                {
                    //currencysymbol aanpassen adhv de waarde in de cookie
                    currencySymbol.Attributes["class"] = "glyphicon glyphicon-" + CookieUtil.GetCookieValue("currency");
                }
            }
        }


        protected void btnLogin_Click(object sender, EventArgs e)
        {


            if (txtEmail.Value != null && txtEmail.Value != null)
            {
                LoginModel loginModel = new LoginModel();
                Customer customer = loginModel.signIn(txtEmail.Value, txtPassword.Value);                       //Throws DALException

                if (customer != null)
                {
                    //put user in session and send user back to his last active page
                    Session["user"] = customer;
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    //user couldn't be logged in, request the status code so the correct error can be displayed to the user
                    LoginStatusCode status = loginModel.getLoginStatus(txtEmail.Value, txtPassword.Value);      //Throws DALException
                    switch (status)
                    {
                        case LoginStatusCode.NOTVERIFIED:
                            Response.Redirect("NotYetVerified.aspx?email=" + txtEmail.Value);
                            break;
                        case LoginStatusCode.WRONGLOGIN:
                            String script = "alert(\"Unknown login name.\");";
                            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                            break;
                        case LoginStatusCode.WRONGPASSWORD:
                            script = "alert(\"Incorrect login/password combination.\");";
                            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                            break;

                    }
                    //todo: find a better way to give feedback (without alert dialog)                    
                }
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
        protected void btnMainSearch_Click1(object sender, EventArgs e)
        {
            //String searchText = txtMainSearch.Text;
            String searchText = txtSearchNew.Value;
            Response.Redirect("Catalog.aspx?search=" + searchText);
        }

        /*
         * Fills the sidebar category and genre menu with database data.
         */
        private void fillSideBar()
        {
            //retrieve list with categories and loop throu it
            List<Category> categories = new CategoryService().getAll();         //Throws NoRecordException || DALException         
            foreach (Category c in categories)
            {
                //create category list
                HtmlGenericControl categoryDiv = new HtmlGenericControl("div");
                categoryDiv.Attributes["class"] = "list-group";

                //create category title item
                HtmlAnchor categoryHeader = new HtmlAnchor();
                categoryHeader.Attributes["class"] = "list-group-item active";
                categoryHeader.HRef = "catalog.aspx?cat=" + c.category_id;
                categoryHeader.InnerHtml = c.name;

                //add title to list
                categoryDiv.Controls.Add(categoryHeader);

                //add list to sidebar
                divSideBar.Controls.Add(categoryDiv);

                //retrieve list of genres in a certain category and loop throu it
                List<Genre> genres = new GenreService().getGenresForCategory(c.category_id.ToString());
                foreach (Genre g in genres)
                {
                    //create sidebar genre item
                    HtmlAnchor genreItem = new HtmlAnchor();
                    genreItem.Attributes["class"] = "list-group-item";
                    genreItem.HRef = "catalog.aspx?genre=" + g.genre_id;
                    genreItem.InnerHtml = g.name;

                    //add it to the category list
                    categoryDiv.Controls.Add(genreItem);
                }
            }
        }
    }
}