using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;

namespace LayeredBusinessModel.WebUI
{
    public partial class Account : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            if(!Page.IsPostBack)
            {
                mvAccount.ActiveViewIndex = 0;

                //todo: only update textfields when user accesses the settings page
                Customer user = (Customer) Session["user"];
                if (user != null)
                {
                    txtName.Text = user.name;
                    txtEmail.Text = user.email;
                    //textfield wil geen tekst tonen als ik dit instel via properties, op deze manier werkt het wel
                    txtPassword.Attributes["type"] = "password";
                    txtPasswordAgain.Attributes["type"] = "password";
                    txtPassword.Text = user.password;
                    txtPasswordAgain.Text = user.password;
                }
            }
        }

        protected void menuAccount_MenuItemClick(object sender, MenuEventArgs e)
        {
            int selection = Convert.ToInt16(menuAccount.SelectedValue);
            mvAccount.SetActiveView(mvAccount.Views[selection]);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //only update if validators were passed
            if(Page.IsValid)
            {
                Customer user = (Customer) Session["user"];

                //only update if user is currently logged in
                if(user!=null)
                {
                    //create customer object based on logged-in-user info and info from textfields
                    Customer customer = new Customer
                    {
                        customer_id = user.customer_id,
                        email = txtEmail.Text,
                        login = user.login,
                        name = txtName.Text,
                        numberOfVisits = user.numberOfVisits,
                        password = txtPassword.Text,

                    };
                    //update user's database data
                    CustomerService customerService = new CustomerService();
                    customerService.updateCustomer(customer);
                    //also update his info in the session
                    Session["user"] = customer;
                }
                else
                {
                    string script = "alert(\"You have been logged out due to inactivity. Please log in to change your information.\");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                }
                
            }
            
        }
    }
}