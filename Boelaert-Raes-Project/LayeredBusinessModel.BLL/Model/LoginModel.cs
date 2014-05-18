using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using CustomException;

namespace LayeredBusinessModel.BLL
{
    public class LoginModel
    {
        public Customer signIn(String loginName, String password)
        {
            Customer customer = null;
            try
            {
                CustomerService customerService = new CustomerService();
                customer = customerService.getByEmail(loginName);          //Throws NoRecordException || DALException

                if (CryptographyModel.decryptPassword(customer.password).Equals(password))
                {
                    if (customer.isVerified)
                    {
                        //update user's number_of_visits
                        customer.numberOfVisits++;
                        customerService.updateCustomer(customer);
                    }
                    else
                    {
                        //user has not verified his email yet
                        customer = null;
                    }
                }
                else
                {
                    //incorrect password
                    customer = null;
                }
            }
            catch (NoRecordException)
            {
                customer = null;
            }
            return customer;            
        }   
     
        public LoginStatusCode getLoginStatus(String loginName, String password)
        {            
            try 
            {
                Customer customer = new CustomerService().getByEmail(loginName);          //Throws NoRecordException || DALException
                if (CryptographyModel.decryptPassword(customer.password).Equals(password))
                {
                    if (customer.isVerified)
                    {
                       return LoginStatusCode.SUCCESFUL;
                    }
                    else
                    {
                        return LoginStatusCode.NOTVERIFIED;
                    }
                }
                else
                {
                    //incorrect password
                    return LoginStatusCode.WRONGPASSWORD;
                }
            } 
            catch(NoRecordException)
            {
                //No user was found
                return LoginStatusCode.WRONGLOGIN;
            }
       }
    }
}
