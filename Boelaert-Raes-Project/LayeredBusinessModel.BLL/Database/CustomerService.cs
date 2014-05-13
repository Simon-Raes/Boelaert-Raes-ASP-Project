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
            return new CustomerDAO().getAll();
        }

        public Customer getByID(String id)
        {
            customerDAO = new CustomerDAO();
            return customerDAO.getByID(id);           
        }

        
        public Customer getCustomerWithEmail(String email)
        {
            customerDAO = new CustomerDAO();
            return customerDAO.getByEmail(email);
        }

        
        public Boolean updateCustomer(Customer customer)
        {
            customerDAO = new CustomerDAO();
            return customerDAO.update(customer);
        }

        
        public Boolean addCustomer(Customer customer)
        {
            customerDAO = new CustomerDAO();
            return customerDAO.add(customer);
        }

        public Boolean verrifyCustomer(Customer customer, Token strToken)
        {
            if (customer.isVerified)
            {
                return false;
            }
            else
            {
                customer.isVerified = true;
                customerDAO = new CustomerDAO();
                if (customerDAO.verrify(customer))
                {
                    TokenService t = new TokenService();
                    t.deleteToken(strToken);
                    return true;
                }
                return false;
            }            
        }
    }
    
}
