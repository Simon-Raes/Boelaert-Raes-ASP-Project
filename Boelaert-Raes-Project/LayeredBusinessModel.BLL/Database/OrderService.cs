using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.DAO;
using LayeredBusinessModel.Domain;

namespace LayeredBusinessModel.BLL
{
    public class OrderService
    {
        //public List<Order> getAll()
        //{
        //    orderDAO = new OrderDAO();
        //    return orderDAO.getAll();
        //}

        public Order getByID(String id)
        {
            return new OrderDAO().getByID(id);            //Throws NoRecordException
        }

        public void updateOrder(Order order)
        {
            new OrderDAO().update(order);           //Throws NoRecordException
        }

        public List<Order> getOrdersForCustomer(Customer customer)
        {
            return new OrderDAO().getByCustomer(customer);          //Throws NoRecordException
        }

        public int addOrderForCustomer(Customer customer)
        {
            return new OrderDAO().addForCustomer(customer);         //Throws NoRecordException
        }

        public void DeleteAll()
        {
            new OrderDAO().deleteAll();         //Throws NoRecordException
        }
    }
}
