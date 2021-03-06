﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using CustomException;

namespace LayeredBusinessModel.WebUI
{
    public partial class AccountSettings : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            checkCheckBoxPassword();

            Customer user = (Customer)Session["user"];
            if (user != null)
            {                
                if(!Page.IsPostBack)
                {
                    inputName.Value = user.name;
                    inputEmail.Value = user.email;
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void valCustEmail_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                CustomerService customerService = new CustomerService();
                Customer cust = customerService.getByEmail(inputEmail.Value);           //Throws NoRecordException || DALException
                if (cust.email.Equals(((Customer)Session["user"]).email))
                {
                    //user didn't change his email, OK
                    args.IsValid = true;
                }
                else
                {
                    //email already taken
                    args.IsValid = false;
                }
            }
            catch (NoRecordException)
            {
                args.IsValid = true;
            }
        }
        
        public void btnLogOut_Click(object sender, EventArgs e)
        {
            Session["user"] = null;
            Response.Redirect("~/Index.aspx");
        }

        protected void cbPassword_CheckedChanged(object sender, EventArgs e)
        {
            checkCheckBoxPassword();            
        }
        
        /*Enable or disable new password fields*/
        private void checkCheckBoxPassword()
        {
            if (cbPassword.Checked)
            {
                divNewPass.Visible = true;
                inputPassword.Visible = true;
                inputPasswordAgain.Visible = true;
                valRequiredNewPassword.Enabled = true;
                valRequiredNewPasswordAgain.Enabled = true;
                valCompareNewPassword.Enabled = true;
            }
            else
            {
                divNewPass.Visible = false;
                inputPassword.Visible = false;
                inputPasswordAgain.Visible = false;
                //disable validators when fields are not being used
                valRequiredNewPassword.Enabled = false;
                valRequiredNewPasswordAgain.Enabled = false;
                valCompareNewPassword.Enabled = false;
            }
        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            //only update if validators were passed
            if (Page.IsValid)
            {
                Customer user = (Customer)Session["user"];

                //only update if user is currently logged in
                if (user != null)
                {
                    //only update if the user entered the correct old password
                    if (inputOldPassword.Value.Equals(CryptographyModel.decryptPassword(user.password)))
                    {
                        //update customer                        
                        Customer customer = user;
                        customer.name = inputName.Value;
                        customer.email = inputEmail.Value;

                        if (cbPassword.Checked)
                        {
                            //user wanted a new password, update it
                            customer.password = CryptographyModel.encryptPassword(inputPassword.Value);
                        }
                        else
                        {
                            //user did not want to change his password, keep the old one
                            customer.password = customer.password;
                        }


                        //update user's database data
                        CustomerService customerService = new CustomerService();
                        customerService.updateCustomer(customer);

                        //also update his info in the session
                        Session["user"] = customer;

                        lblStatus.Text = "Your info was succesfully updated.";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblStatus.Text = "Incorrect old password!";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    string script = "alert(\"You have been logged out due to inactivity. Please log in to change your details.\");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                    //redirect to login
                    Response.Redirect("Index.aspx");
                }
            }            
        }
    }
}