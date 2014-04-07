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

        public List<OrderLine> getOrderLinesForOrder(int order_id)
        {
            orderLineDAO = new OrderLineDAO();
            return orderLineDAO.getOrderLinesForOrder(order_id);
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
