using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.DAO;

namespace LayeredBusinessModel.BLL
{
    public class DvdGenreService
    {
        private DvdGenreDAO dvdGenreDAO;

        public void addDvdGenre(Genre genre, DvdInfo dvdInfo)
        {
            dvdGenreDAO = new DvdGenreDAO();
            dvdGenreDAO.addGenreForDvd(genre, dvdInfo);            
        }
    }
}
