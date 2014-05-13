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

        public DvdCopyType getTypeForID(String id)
        {
            typeDAO = new DvdCopyTypeDAO();
            return typeDAO.getTypeForID(id);
        }

        public DvdCopyType getTypeByName(String name)
        {
            typeDAO = new DvdCopyTypeDAO();
            return typeDAO.getTypeByName(name);
        }

    }
}
