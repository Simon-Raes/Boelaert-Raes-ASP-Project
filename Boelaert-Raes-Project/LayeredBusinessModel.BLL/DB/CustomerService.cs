using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.DAO;

namespace LayeredBusinessModel.BLL
{

    public class CustomerService
    {
        private CustomerDAO customerDAO;

        public List<Customer> getAll()
        {
            List<Customer> customerList = new List<Customer>();
            customerDAO = new CustomerDAO();
            customerList = customerDAO.getAllCustomers();
            return customerList;  
        }

        public Customer getCustomerWithLogin(String login)
        {
            customerDAO = new CustomerDAO();
            return customerDAO.getCustomerWithLogin(login);
        }

        public Customer getCustomerWithEmail(String email)
        {
            customerDAO = new CustomerDAO();
            return customerDAO.getCustomerWithEmail(email);
        }

        public void updateCustomer(Customer customer)
        {
            customerDAO = new CustomerDAO();
            customerDAO.updateCustomer(customer);
        }

        public Boolean addCustomer(Customer customer)
        {
            customerDAO = new CustomerDAO();
            return customerDAO.addCustomer(customer);
       }
    }
    
}
