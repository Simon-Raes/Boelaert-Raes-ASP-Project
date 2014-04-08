using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using System.Text;
using System.Security.Cryptography;

namespace LayeredBusinessModel.WebUI
{
    public partial class Login : System.Web.UI.Page
    {
        CustomerService customerService;
        const string passphrase = "Password@123";  //consant string Pass key

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            customerService = new CustomerService();
            Customer customer = customerService.getCustomerWithLogin(txtEmail.Text);
            
            //todo: validators

            //een null customer object geeft hier nog altijd true, daarom controle op password veld
            if (customer.password != null)
            {
                if (decryptPassword(customer.password).Equals(txtPassword.Text))
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
                    lblStatus.Text = "Incorrect login/password combination";
                }
            }
            else
            {
                //no such user 
                lblStatus.Text = "Unknown user.";

            }

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            //go to register page
            Response.Redirect("~/Register.aspx");

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


    }
}