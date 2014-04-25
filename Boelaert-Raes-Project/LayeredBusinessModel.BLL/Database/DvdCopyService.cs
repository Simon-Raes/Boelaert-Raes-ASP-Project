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

        public DvdCopy getDvdCopyWithId(int copy_id)
        {
            dvdCopyDAO = new DvdCopyDAO();
            return dvdCopyDAO.getCopyWithId(copy_id);
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

        /**Returns a list of all dvd copies that are available for the full 14 day period, starting today*/
        public List<DvdCopy> getAllFullyAvailableCopies(DvdInfo dvd, DateTime startdate)
        {
            DvdCopyDAO dvdCopyDAO = new DvdCopyDAO();
            return dvdCopyDAO.getAllFullyAvailableCopies(dvd, startdate);
        }

        public void updateCopy(DvdCopy copy)
        {
            dvdCopyDAO = new DvdCopyDAO();
            dvdCopyDAO.updateDvdCopy(copy);
        }

        public void updateDvdCopyInStockStatus(String dvdCopyID, bool in_stock)
        {
            dvdCopyDAO = new DvdCopyDAO();
            dvdCopyDAO.updateDvdCopyInStockStatus(dvdCopyID,in_stock);
        }

        public void resetAllCopies()
        {
            dvdCopyDAO = new DvdCopyDAO();
            dvdCopyDAO.resetAllCopies();
        }

    }
}
