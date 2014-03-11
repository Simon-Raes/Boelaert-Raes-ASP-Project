using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.DAO;

namespace LayeredBusinessModel.BLL
{
    public class DvdCopyService
    {
        private DvdCopyDAO dvdCopyDAO;

        public List<DvdCopy> getAllCopiesForDvdInfo(String info_id)
        {
            dvdCopyDAO = new DvdCopyDAO();
            return dvdCopyDAO.getAllCopiesForDvdInfo(info_id);
        }

        public List<DvdCopy> getAllInStockRentCopiesForDvdInfo(String info_id)
        {
            dvdCopyDAO = new DvdCopyDAO();
            return dvdCopyDAO.getAllInStockRentCopiesForDvdInfo(info_id);
        }

        public List<DvdCopy> getAllInStockBuyCopiesForDvdInfo(String info_id)
        {
            dvdCopyDAO = new DvdCopyDAO();
            return dvdCopyDAO.getAllInStockBuyCopiesForDvdInfo(info_id);
        }

        public void updateCopy(DvdCopy copy)
        {
            dvdCopyDAO = new DvdCopyDAO();
            dvdCopyDAO.updateDvdCopy(copy);
        }

        
    }
}
