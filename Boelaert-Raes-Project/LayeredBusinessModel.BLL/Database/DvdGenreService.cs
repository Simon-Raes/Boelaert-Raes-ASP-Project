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
        public Boolean addDvdGenre(Genre genre, DvdInfo dvdInfo)
        {
            return new DvdGenreDAO().addForDvd(genre, dvdInfo);          //Throws NoRecordException            
        }
    }
}
