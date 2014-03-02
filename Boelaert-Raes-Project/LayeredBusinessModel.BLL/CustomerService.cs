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

        public List<Customer> GetAll()
        {
            List<Customer> customerList = new List<Customer>();
            customerDAO = new CustomerDAO();
            customerList = customerDAO.getAllCustomers();
            return customerList;
        }

       
    }
    
}
