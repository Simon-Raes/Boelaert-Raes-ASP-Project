using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.DAO;

namespace LayeredBusinessModel.BLL
{
    public class DvdInfoService
    {
        private DvdInfoDAO dvdInfoDAO;

        public List<DvdInfo> getAll()
        {
            dvdInfoDAO = new DvdInfoDAO();
            return dvdInfoDAO.getAllDvdInfos();
        }

        public List<DvdInfo> searchDvdWithText(String title)
        {
            dvdInfoDAO = new DvdInfoDAO();
            return dvdInfoDAO.searchDvdWithText(title);
        }

        public List<DvdInfo> searchDvdWithTextAndCategory(String title, String categoryID)
        {
            dvdInfoDAO = new DvdInfoDAO();
            return dvdInfoDAO.searchDvdWithTextAndCategory(title, categoryID);
        }

        public List<DvdInfo> searchDvdWithTextAndGenre(String title, String genreID)
        {
            dvdInfoDAO = new DvdInfoDAO();
            return dvdInfoDAO.searchDvdWithTextAndGenre(title, genreID);
        }
    }
}
