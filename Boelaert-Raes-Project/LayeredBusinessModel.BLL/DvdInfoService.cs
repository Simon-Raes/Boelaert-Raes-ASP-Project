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

        public List<DvdInfo> getAllWithTitleSearch(String title)
        {
            dvdInfoDAO = new DvdInfoDAO();
            return dvdInfoDAO.getAllWithTitleSearch(title);
        }
    }
}
