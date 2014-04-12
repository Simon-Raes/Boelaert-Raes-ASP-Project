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

        public void addDvdGenre(int genre_id, int dvd_info_id)
        {
            dvdGenreDAO = new DvdGenreDAO();
            dvdGenreDAO.addDvdGenre(genre_id, dvd_info_id);            
        }
    }
}
