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
        public OrderStatus getByID(String id)
        {
            return new OrderStatusDAO().getByID(id);          //Throws NoRecordException
        }
    }
}
