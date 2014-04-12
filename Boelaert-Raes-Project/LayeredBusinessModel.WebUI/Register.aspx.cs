using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;
using System.Net.Mail;


namespace LayeredBusinessModel.WebUI
{
    public partial class Register : System.Web.UI.Page
    {
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
                    name = inputName.Value,
                    email = inputEmail.Value,
                    login = inputLogin.Value,
                    password = PasswordCrypto.encryptPassword(inputPassword.Value)
                };
                customerService = new CustomerService();
                customerService.addCustomer(customer);

                sendRegisterMail(inputEmail.Value);

                //put user in session and redirect to index - todo: redirect to page telling user to click the confirmation link in the email
                Session["user"] = customer;
                Response.Redirect("~/Index.aspx");
            }
        }

        //werkt (nog) niet
        protected void btnReset_Click(object sender, EventArgs e)
        {
            inputName.Value = "";
            inputEmail.Value = "";
            inputLogin.Value = "";
            inputPassword.Value = "";
            inputPasswordAgain.Value = "";
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
            Customer cust = customerService.getCustomerWithLogin(inputLogin.Value);
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
            Customer cust = customerService.getCustomerWithEmail(inputEmail.Value);
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
       
    }
}