using LayeredBusinessModel.DAO;
using LayeredBusinessModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.BLL
{
    public class OrderLineTypeService
    {
        public OrderLineType getByID(String id)
        {
            return new OrderLineTypeDAO().getByID(id);          //Throws NoRecordException
        }
    }
}
