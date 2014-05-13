using LayeredBusinessModel.DAO;
using LayeredBusinessModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.BLL.Database
{
    public class OrderLineTypeService
    {
        public OrderLineType getOrderLineTypeByID(String id)
        {
            OrderLineTypeDAO orderLineTypeDAO = new OrderLineTypeDAO();
            return orderLineTypeDAO.getOrderLineTypeForID(id);
        }
    }
}
