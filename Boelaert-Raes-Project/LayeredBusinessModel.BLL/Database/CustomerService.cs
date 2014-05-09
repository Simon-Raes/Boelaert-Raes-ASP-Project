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
     *  All servicemethods for Customer
     */
    public class CustomerService
    {
        private CustomerDAO customerDAO;

        /*
         *  Returns a list with all the customers
         */
        public List<Customer> getAll()
        {
            List<Customer> customerList = new List<Customer>();
            customerDAO = new CustomerDAO();
            customerList = customerDAO.getAllCustomers();
            return customerList;  
        }

        public Customer getCustomerByID(int id)
        {
            customerDAO = new CustomerDAO();
            return customerDAO.getCustomerByID(id);
        }

        /*
         *  Returns a customer based on an emailadress
         */
        public Customer getCustomerWithEmail(String email)
        {
            customerDAO = new CustomerDAO();
            return customerDAO.getCustomerByEmail(email);
        }

        /*
         *  Updates a customer
         *  Returns true if succeedes, false is not
         */
        public Boolean updateCustomer(Customer customer)
        {
            customerDAO = new CustomerDAO();
            return customerDAO.updateCustomer(customer);
        }

        /*
         *  Adds a new customer
         *  Returns true if succeedes, false is not
         */ 
        public Boolean addCustomer(Customer customer)
        {
            customerDAO = new CustomerDAO();
            return customerDAO.addCustomer(customer);
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
                if (customerDAO.verrifyCustomer(customer))
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
