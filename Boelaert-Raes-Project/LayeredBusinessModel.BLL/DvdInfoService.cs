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

        public DvdInfo getDvdInfoWithID(String id)
        {
            dvdInfoDAO = new DvdInfoDAO();
            DvdInfo dvd  = dvdInfoDAO.getDvdInfoWithId(id);
            GenreService g = new GenreService();
            dvd.genres = g.getGenresForDvd(Convert.ToInt32(id));

            return dvd;
        }

        public void updateDvdInfo(DvdInfo dvd)
        {
            dvdInfoDAO = new DvdInfoDAO();
            dvdInfoDAO.updateDvdInfo(dvd); 
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

        public List<DvdInfo> searchDvdFromYear(String year)
        {
            dvdInfoDAO = new DvdInfoDAO();
            return dvdInfoDAO.searchDvdFromYear(year);
        }

        public List<DvdInfo> searchDvdFromDirector(String director)
        {

            dvdInfoDAO = new DvdInfoDAO();
            return dvdInfoDAO.searchDvdFromDirector(director);
        }


        public List<DvdInfo> getLatestDvds(int amount)
        {
            dvdInfoDAO = new DvdInfoDAO();
            return dvdInfoDAO.getLatestDvds(amount);
        }

        public List<DvdInfo> getMostPopularDvds(int amount)
        {
            dvdInfoDAO = new DvdInfoDAO();
            return dvdInfoDAO.getMostPopularDvds(amount);
        }
    }
}
