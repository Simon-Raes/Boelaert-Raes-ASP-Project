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
        public List<Genre> getAll()
        {
            return new GenreDAO().getAll();           //Throws NoRecordException
        }

        public List<Genre> getCategory(String categoryID)
        {
            return new GenreDAO().getByCategory(categoryID);          //Throws NoRecordException
        }

        public List<Genre> getGenresForDvd(String categoryID)
        {
            return new GenreDAO().getByDvd(categoryID);         //Throws NoRecordException
        }

        public Genre getByID(String id)
        {
            return new GenreDAO().getByID(id);          //Throws NoRecordException
        }
    }
}
