using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;
using System.Net.Mail;
using CustomException;


namespace LayeredBusinessModel.WebUI
{








    //NO LONGER USED!!










    public partial class Register : System.Web.UI.Page
    {
        CustomerService customerService;

        protected void Page_Load(object sender, EventArgs e)
        {
            setupEID();
        }

        private void setupEID()
        {
            //Loading personal data will force an extra popup for the user asking if        
            //he/she allows the java engine to read the EID card.
            //Personal information contains the picture, address, nationnumber. Only the
            //certificates upon the EID card do not require an
            //extra popup to read.
            EID_Read1.LoadEIDPersonalData = true;
            EID_Read1.Visible = false;
            //This event is called when
            //EID card is read.
            EID_Read1.EIDRead += new Arcabase.EID.SDK.Web.dlgEIDRead(EID_Reader_onEIDRead);
            //This event is called when the user pressed no when the applet asked if
            //he/she allows the java engine to read the EID card.
            //The event is only usefull when "LoadEIDPersonalData" is set to TRUE.
            EID_Read1.ReadEIDDeniedByUser += new
            Arcabase.EID.SDK.Web.dlgReadEIDDeniedByUser(EID_Reader_onReadEIDDeniedByUser);
        }

        void EID_Reader_onReadEIDDeniedByUser()
        {
            //Actie te ondernemen na afsluiten door bezoeker
        }

        void EID_Reader_onEIDRead(Arcabase.EID.SDK.Data.EidInfo curInfo)
        {
            //Actie te ondernemen na geldig uitlezen van de kaar
            //in curInfo worden alle gegevens van de kaart teruggevonden

            txtName.Text = curInfo.FirstName;
            txtStreet.Text = curInfo.Street;
            
            EID_Read1.Visible = false;
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            //check if validators were properly executed
            Page.Validate();
            if (Page.IsValid)
            {
                //todo: remove login field, make the email address the login name
                Customer customer = new Customer
                {
                    name = txtName.Text,
                    email = inputEmail.Value,
                   // login = inputLogin.Value,
                    password = CryptographyModel.encryptPassword(inputPassword.Value)
                };
                customerService = new CustomerService();
                customerService.addCustomer(customer);

                sendRegisterMail(inputEmail.Value);

                //put user in session and redirect to index - todo: redirect to page telling user to click the confirmation link in the email, then handle that link
                Session["user"] = customer;
                Response.Redirect("~/Index.aspx");
            }
        }

        //werkt (nog) niet
        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtName.Text = null;
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
            try
            {
                CustomerService customerService = new CustomerService();
                Customer cust = customerService.getByEmail(inputLogin.Value);           //Throws NoRecordException || DALException
                //If there was a customer found
                args.IsValid = false;
            }
            catch (NoRecordException)
            {
                //When no records were found, this means that the loginname is still open
                args.IsValid = true;
            }
        }

        protected void valCustEmail_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                CustomerService customerService = new CustomerService();
                Customer cust = customerService.getByEmail(inputEmail.Value);               //Throws NoRecordException || DALException
                args.IsValid = false;
            }
            catch (NoRecordException)
            {
                //When no records were found, this means that the email is still open
                args.IsValid = true;
            }
        }


        protected void btnEID_Click(object sender, EventArgs e)
        {
            mpeEid.Show();
            EID_Read1.Visible = true;
        }

       
       
    }
}