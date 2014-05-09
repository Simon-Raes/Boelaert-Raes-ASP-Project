using LayeredBusinessModel.DAO;
using LayeredBusinessModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.BLL.Database
{
    public class OrderStatusService
    {
        public OrderStatus getOrderstatusByID(int id)
        {
            OrderStatusDAO orderstatusDAO = new OrderStatusDAO();
            return orderstatusDAO.getOrderStatusByID(id);
        }
    }
}
