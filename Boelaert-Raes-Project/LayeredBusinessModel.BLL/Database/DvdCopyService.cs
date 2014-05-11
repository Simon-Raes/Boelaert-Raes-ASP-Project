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

        public List<DvdCopy> getAllCopiesForDvdInfo(DvdInfo dvdInfo)
        {
            dvdCopyDAO = new DvdCopyDAO();
            return dvdCopyDAO.getAllCopiesForDvdInfo(dvdInfo);
        }

        public DvdCopy getDvdCopyWithId(String copy_id)
        {
            dvdCopyDAO = new DvdCopyDAO();
            return dvdCopyDAO.getCopyWithId(copy_id);
        }

        public Boolean addCopiesForDvd(DvdInfo dvdInfo)
        {
            DvdCopyDAO dvdCopyDAO = new DvdCopyDAO();
            return dvdCopyDAO.addCopiesForDvd(dvdInfo);
        }

        public List<DvdCopy> getAllInStockRentCopiesForDvdInfo(DvdInfo dvdInfo)
        {
            dvdCopyDAO = new DvdCopyDAO();
            return dvdCopyDAO.getAllInStockRentCopiesForDvdInfo(dvdInfo);
        }

        public List<DvdCopy> getAllInStockBuyCopiesForDvdInfo(DvdInfo dvdInfo)
        {
            dvdCopyDAO = new DvdCopyDAO();
            return dvdCopyDAO.getAllInStockBuyCopiesForDvdInfo(dvdInfo);
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

        public void updateDvdCopyInStockStatus(DvdCopy dvdCopy, bool in_stock)
        {
            dvdCopyDAO = new DvdCopyDAO();
            dvdCopyDAO.updateDvdCopyInStockStatus(dvdCopy, in_stock);
        }

        public void resetAllCopies()
        {
            dvdCopyDAO = new DvdCopyDAO();
            dvdCopyDAO.resetAllCopies();
        }

    }
}
