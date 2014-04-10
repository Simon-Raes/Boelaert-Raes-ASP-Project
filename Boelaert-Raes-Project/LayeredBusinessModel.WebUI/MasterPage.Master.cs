﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;
using System.Web.UI.HtmlControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using System.Text;
using System.Security.Cryptography;

namespace LayeredBusinessModel.WebUI
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        const string passphrase = "Password@123";  //consant string Pass key TODO: more secure passphrase (but will break current users login)

        protected void Page_Load(object sender, EventArgs e)
        {
            fillSideBar();

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
                //btnLogin.Text = user.name;
                liAccount.InnerHtml = "<a href='Account.aspx'>"+user.name+"</a>";
                //btnCart.Visible = true;
                
                ShoppingCartService shoppingCartService = new ShoppingCartService();
                List<ShoppingcartItem> cartContent = shoppingCartService.getCartContentForCustomer(user.customer_id);
                //btnCart.Text = "Cart: " + cartContent.Count;
                liCart.InnerText = "Cart: " + cartContent.Count;
                //don't change banner on every postback
                if (!Page.IsPostBack)
                {
                    //set personalised banners
                    if (user.numberOfVisits > 5)
                    {
                        //todo: 
                        //favoriete genre van user opzoeken (op basis van ordergeschiedenis), enkel banners van dat genre weergeven
                    }
                }

            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //if (Session["user"] == null)
            //{
            //    Response.Redirect("~/Login.aspx");
            //}
            //else
            //{
            //    //redirect to account page (purchase history, account settings,...)
            //    Response.Redirect("~/Account.aspx");
            //}


            CustomerService customerService = new CustomerService();
            Customer customer = customerService.getCustomerWithLogin(txtEmail.Value);

            //todo: validators

            if (customer != null)
            {

                //een null customer object geeft hier nog altijd true, daarom controle op password veld
                if (customer.password != null)
                {
                    if (decryptPassword(customer.password).Equals(txtPassword.Value))
                    {
                        //update user's number_of_visits
                        customer.numberOfVisits++;
                        customerService.updateCustomer(customer);

                        //put user in session and redirect to index (better would be to redirect to last active page)
                        Session["user"] = customer;
                        Response.Redirect("~/Index.aspx");
                    }
                    else
                    {
                        //incorrect password
                        //lblStatus.Text = "Incorrect login/password combination";
                    }
                }
                else
                {
                    //no such user 
                    //lblStatus.Text = "Unknown user.";

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


        public string decryptPassword(string message)
        {
            byte[] results;
            UTF8Encoding utf8 = new UTF8Encoding();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] encryptionKey = md5.ComputeHash(utf8.GetBytes(passphrase));

            TripleDESCryptoServiceProvider encryptionProvider = new TripleDESCryptoServiceProvider();
            encryptionProvider.Key = encryptionKey;
            encryptionProvider.Mode = CipherMode.ECB;
            encryptionProvider.Padding = PaddingMode.PKCS7;

            byte[] decrypt_data = Convert.FromBase64String(message);

            try
            {
                ICryptoTransform decryptor = encryptionProvider.CreateDecryptor();
                results = decryptor.TransformFinalBlock(decrypt_data, 0, decrypt_data.Length);
            }
            finally
            {
                encryptionProvider.Clear();
                md5.Clear();
            }

            return utf8.GetString(results);
        }


        private void fillSideBar()
        {
            //Menu opvullen met alle categoriën en genres
            List<Category> categories = new CategoryService().getAll();
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


                List<Genre> genres = new GenreService().getGenresForCategory(c.category_id);
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