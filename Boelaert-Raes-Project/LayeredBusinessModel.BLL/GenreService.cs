using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.DAO;

namespace LayeredBusinessModel.BLL
{
    public class GenreService
    {
        private GenreDAO genreDAO;

        public List<Genre> getGenresForCategory(int categoryID)
        {
            genreDAO = new GenreDAO();
            return genreDAO.getGenresForCategory(categoryID);
        }
    }
}
