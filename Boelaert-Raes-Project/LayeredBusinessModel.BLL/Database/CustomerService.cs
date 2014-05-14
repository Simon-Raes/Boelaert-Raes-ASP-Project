using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.DAO;
using LayeredBusinessModel.BLL.Database;

namespace LayeredBusinessModel.BLL
{
    /*
     *  All business methods for Customer
     */
    public class CustomerService
    {
        private CustomerDAO customerDAO;
        private Validation validation;

        /*
         * Returns a list off Customers
         */ 
        public List<Customer> getAll()
        {
            return new CustomerDAO().getAll();              //Throws NoRecordException || DALException
        }

        /*
         * Returns a Customer based on an ID
         */
        public Customer getByID(String id)
        {
            return new CustomerDAO().getByID(id);                 //Throws NoRecordException || DALException       
        }

        /*
         * Returns a Customer based on an Email
         */
        public Customer getByEmail(String email)
        {
            return new CustomerDAO().getByEmail(email);           //Throws NoRecordException || DALException
        }

        /*
         * Updates a Customer
         */ 
        public Boolean updateCustomer(Customer customer)
        {
            return new CustomerDAO().update(customer);            //Throws DALException
        }

        /*
         * Adds a Customer
         */ 
        public Boolean addCustomer(Customer customer)
        {
            return new CustomerDAO().add(customer);               //Throws DALException
        }

        /*
         * Verrifies a Customer
         */  
        public Boolean verrifyCustomer(Customer customer, Token strToken)
        {
            if (customer.isVerified)
            {
                return false;
            }
            else
            {                
                if (new CustomerDAO().verrify(customer))        //Throws DALException
                {
                    new TokenService().deleteToken(strToken);
                    customer.isVerified = true;
                    return true;
                }
                return false;
            }            
        }
    }
    
}
