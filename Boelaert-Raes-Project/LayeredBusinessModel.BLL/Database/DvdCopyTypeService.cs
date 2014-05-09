using LayeredBusinessModel.DAO;
using LayeredBusinessModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.BLL.Database
{
    public class DvdCopyTypeService
    {
        private DvdCopyTypeDAO typeDAO; 

        public DvdCopyType getTypeForId(int id)
        {
            typeDAO = new DvdCopyTypeDAO();
            return typeDAO.getTypeForID(id);
        }

    }
}
