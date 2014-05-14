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
        public OrderLine getByID(String orderLineID)
        {
            return new OrderLineDAO().getByID(orderLineID);           //Throws NoRecordExeption
        }

        public List<OrderLine> getByOrder(Order order)
        {
            return new OrderLineDAO().getByOrder(order);         //Throws NoRecordException
        }

        public List<OrderLine> getByCustomer(Customer customer)
        {
            return new OrderLineDAO().getByCustomer(customer);          //Throws NoRecordException
        }

        public List<OrderLine> getActiveRentOrderLinesByCustomer(Customer customer)
        {
            return new OrderLineDAO().getActiveRentOrderLinesByCustomer(customer);          //Throws NoRecordException
        }

        public Boolean add(OrderLine orderLine)
        {
            return new OrderLineDAO().add(orderLine);
        }

        public Boolean delete(OrderLine orderLine)
        {
            return new OrderLineDAO().delete(orderLine);            
        }
           
        public Boolean deleteAll()
        {
            return new OrderLineDAO().deleteAll();
        }

        public void updateOrderLine(OrderLine orderLine)
        {
            //Stond geen code in
        }
        /*
        public Boolean update(OrderLine orderLine)
        {
            return new OrderLineDAO().update(orderLine);
        }*/
    }
}
