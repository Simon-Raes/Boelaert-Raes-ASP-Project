using LayeredBusinessModel.BLL;
using LayeredBusinessModel.BLL.Model;
using LayeredBusinessModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LayeredBusinessModel.WebUI
{
    public partial class signup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            checkQueryString();
            setupEID();

            if (!Page.IsPostBack)
            {
                pnlSignup.Visible = true;
                pnlSignupCompleted.Visible = false;                
            }
            else
            {
                
            }
        }

        private void checkQueryString()
        {
            if (Request.QueryString["token"] != null)
            {
                SignUpModel signUpModel = new SignUpModel();
                if(signUpModel.checkSignUpVerification(Request.QueryString["token"]))
                {
                    //user verification succesful, log in the user and redirect
                    
                }
                else
                {
                    //something went wrong (none existing token?)
                    //todo:alert user?
                }
            }
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

            txtName.Text = curInfo.FirstName + " "  + curInfo.LastName;
            txtStreet.Text = curInfo.Street;
            txtPostalcode.Text = curInfo.Zip;
            txtMunicipality.Text = curInfo.Municipality;
           
            

            EID_Read1.Visible = false;
            enableValidation();
        }

        protected void btnSignup_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                Customer customer = new Customer
                {
                    email = txtEmail.Text,
                    password = CryptographyModel.encryptPassword(txtPassword.Text),
                    name = txtName.Text,
                    street = txtStreet.Text,
                    zip = txtPostalcode.Text,
                    municipality = txtMunicipality.Text,
                    isVerified = false
                };
                SignUpModel signUpModel = new SignUpModel();
                if (signUpModel.signUpCustomer(customer))
                {
                    pnlSignup.Visible = false;
                    lblEmailSent.Text = "An email has been sent to "+customer.email+". Please follow the instructions in the email to complete your registration.";
                    pnlSignupCompleted.Visible = true;
                }
                else
                {
                    pnlSignup.Visible = false;
                    lblEmailSent.Text = "An error occured while completing your registration, please try again later. Contact our support if this problem persists.";
                    pnlSignupCompleted.Visible = true;
                }
            }
        }

        protected void btneID_Click(object sender, EventArgs e)
        {            
            ModalPopupExtender1.Show();
            EID_Read1.Visible = true;
            disableValidation();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtEmail.Text = null;
            txtPassword.Text = null;
            txtpassword2.Text = null;
            txtName.Text = null;
            txtStreet.Text = null;
            txtPostalcode.Text = null;
            txtMunicipality.Text = null;
        }

        private void disableValidation()
        {
            rfvEmail.Enabled = false;
            cvEmail.Enabled = false;
            rfvPassword.Enabled = false;
            rfvPassword2.Enabled = false;
            cvPasswords.Enabled = false;

            rfvName.Enabled = false;
            rfvStreet.Enabled = false;
            rfvPostalcode.Enabled = false;
            rfvMunicipality.Enabled = false;

        }

        private void enableValidation()
        {
            rfvEmail.Enabled = true;
            cvEmail.Enabled = true;
            rfvPassword.Enabled = true;
            rfvPassword2.Enabled = true;
            cvPasswords.Enabled = true;

            rfvName.Enabled = true;
            rfvStreet.Enabled = true;
            rfvPostalcode.Enabled = true;
            rfvMunicipality.Enabled = true;

        }

        #region Validation

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

        #endregion

    }
}