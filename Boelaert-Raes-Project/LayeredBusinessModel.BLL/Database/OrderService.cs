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
        OrderDAO orderDAO;

        //public List<Order> getAll()
        //{
        //    orderDAO = new OrderDAO();
        //    return orderDAO.getAll();
        //}

        public Order getOrder(String id)
        {
            orderDAO = new OrderDAO();
            return orderDAO.getOrderWithId(id);
        }

        public void updateOrder(Order order)
        {
            orderDAO = new OrderDAO();
            orderDAO.updateOrder(order);
        }

        public List<Order> getOrdersForCustomer(Customer customer)
        {
            orderDAO = new OrderDAO();
            return orderDAO.getOrdersForCustomer(customer);
        }

        public int addOrderForCustomer(Customer customer)
        {
            orderDAO = new OrderDAO();
            return orderDAO.addOrderForCustomer(customer);
        }

        public void clearTable()
        {
            orderDAO = new OrderDAO();
            orderDAO.clearTable();
        }
    }
}
