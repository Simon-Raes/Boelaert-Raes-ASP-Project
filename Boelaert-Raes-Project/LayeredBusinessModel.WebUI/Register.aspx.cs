using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;
using System.Net.Mail;

using LayeredBusinessModel.BLL;
using System.Text;
using System.Security.Cryptography;


namespace LayeredBusinessModel.WebUI
{
    public partial class Register : System.Web.UI.Page
    {
        const string passphrase = "Password@123";  //todo: op centrale locatie opslaan

        CustomerService customerService;


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            //check if validators were properly executed
            Page.Validate();
            if (Page.IsValid)
            {
                Customer customer = new Customer
                {
                    name = txtName.Text,
                    email = txtEmail.Text,
                    login = txtLogin.Text,
                    password = encryptPassword(txtPassword.Text)
                };
                customerService = new CustomerService();
                customerService.addCustomer(customer);

                sendRegisterMail(txtEmail.Text);

                //put user in session and redirect to index - todo: redirect to page telling user to click the confirmation link in the email
                Session["user"] = customer;
                Response.Redirect("~/Index.aspx");
            }
        }

        private void sendRegisterMail(String address)
        {

            //todo: email sturen wanneer nieuwe klant zich registreert, eventueel met verplichte confirmation link

            //werkt (nog) niet




            //SmtpClient smtpClient = new SmtpClient("smtp.telenet.be", 587);

            //smtpClient.Credentials = new System.Net.NetworkCredential("xxxxxxxxx", "xxxxxxx");
            //smtpClient.UseDefaultCredentials = false;
            //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtpClient.EnableSsl = true;
            //MailMessage mail = new MailMessage();

            ////Setting From, To
            //mail.From = new MailAddress("noreply@dvdshop.be", "DVDShop");
            //mail.To.Add(new MailAddress(address));

            //smtpClient.Send(mail);
        }

        protected void valCustLogin_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomerService customerService = new CustomerService();
            Customer cust = customerService.getCustomerWithLogin(txtLogin.Text);
            if (cust == null)
            {
                //login name still available
                args.IsValid = true;
            } 
            else
            {
                //login name already taken
                args.IsValid = false;
            }            
        }

        protected void valCustEmail_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomerService customerService = new CustomerService();
            Customer cust = customerService.getCustomerWithEmail(txtEmail.Text);
            if (cust == null)
            {
                //email still available
                args.IsValid = true;
            }
            else
            {
                //email already taken
                args.IsValid = false;
            }
        }


        public string encryptPassword(string message)
        {
            byte[] results;
            UTF8Encoding utf8 = new UTF8Encoding();            
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] encryptionKey = md5.ComputeHash(utf8.GetBytes(passphrase)); 
          
            TripleDESCryptoServiceProvider encryptionProvider = new TripleDESCryptoServiceProvider();
            encryptionProvider.Key = encryptionKey;
            encryptionProvider.Mode = CipherMode.ECB;
            encryptionProvider.Padding = PaddingMode.PKCS7;

            byte[] encrypt_data = utf8.GetBytes(message);

            try
            {
                ICryptoTransform encryptor = encryptionProvider.CreateEncryptor();
                results = encryptor.TransformFinalBlock(encrypt_data, 0, encrypt_data.Length);
            }
            finally
            {
                encryptionProvider.Clear();
                md5.Clear();
            }

            //convert array back to a string
            return Convert.ToBase64String(results);
        }
    }
}