using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;


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
            Customer customer = new Customer
            {                
                name = txtName.Text,
                email = txtEmail.Text,
                login = txtLogin.Text,
                password = txtPassword.Text
            };
            customerService = new CustomerService();
            customerService.addCustomer(customer);
        }
    }
}