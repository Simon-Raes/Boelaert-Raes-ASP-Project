using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;

namespace LayeredBusinessModel.BLL.Model
{
    public class LoginModel
    {
        public Customer signIn(String loginName, String password)
        {
            CustomerService customerService = new CustomerService();
            Customer customer = customerService.getCustomerWithEmail(loginName);

            if (customer != null)
            {
                //een null customer object geeft hier soms nog altijd true, daarom controle op password veld
                if (customer.password != null)
                {
                    if (CryptographyModel.decryptPassword(customer.password).Equals(password))
                    {
                        //update user's number_of_visits
                        customer.numberOfVisits++;
                        customerService.updateCustomer(customer);
                        
                    }
                    else
                    {
                        //incorrect password
                        customer = null;
                    }
                }
                else
                {
                    //no such user 
                    customer = null;
                }
            }
            return customer;            
        }        
    }
}
