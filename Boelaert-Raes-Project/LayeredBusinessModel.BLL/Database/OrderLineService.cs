using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.DAO;
using LayeredBusinessModel.Domain;

namespace LayeredBusinessModel.BLL
{
    public class OrderLineService
    {
        private OrderLineDAO orderLineDAO;

        public OrderLine getOrderLine(String orderLineID)
        {
            orderLineDAO = new OrderLineDAO();
            return orderLineDAO.getOrderLineById(orderLineID);
        }

        public Boolean removeOrderLine(OrderLine orderLine)
        {
            orderLineDAO = new OrderLineDAO();
            return orderLineDAO.removeOrderLine(orderLine);
        }

        public List<OrderLine> getOrderLinesForOrder(Order order)
        {
            orderLineDAO = new OrderLineDAO();
            List<OrderLine> orderLines = orderLineDAO.getOrderLinesForOrder(order);            
            return orderLines;
        }

        public List<OrderLine> getOrderLinesForCustomer(Customer customer)
        {
            orderLineDAO = new OrderLineDAO();
            return orderLineDAO.getOrderLinesForCustomer(customer);
        }

        public List<OrderLine> getActiveRentOrderLinesForCustomer(Customer customer)
        {
            orderLineDAO = new OrderLineDAO();
            return orderLineDAO.getActiveRentOrderLinesForCustomer(customer);
        }       

        public void addOrderLine(OrderLine orderLine)
        {
            orderLineDAO = new OrderLineDAO();
            orderLineDAO.addOrderLine(orderLine);
        }

        public void updateOrderLine(OrderLine orderLine)
        {
            orderLineDAO = new OrderLineDAO();
            orderLineDAO.updateOrderLine(orderLine);
        }

        public void clearTable()
        {
            orderLineDAO = new OrderLineDAO();
            orderLineDAO.clearTable();
        }
    }
}
