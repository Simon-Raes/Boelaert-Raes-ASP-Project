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

        public DvdCopyType getByName(String name)
        {
            return new DvdCopyTypeDAO().getByName(name);         //Throws NoRecordException 
        }

        /*
        public DvdCopyType getByID(String id)
        {
            typeDAO = new DvdCopyTypeDAO();
            return typeDAO.getByID(id);            //Throws NoRecordException 
        }
        */
    }
}
