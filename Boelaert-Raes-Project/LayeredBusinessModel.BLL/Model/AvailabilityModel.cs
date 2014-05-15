using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using CustomException;

namespace LayeredBusinessModel.BLL.Model
{
    public class AvailabilityModel
    {
        public static Boolean isAvailableForBuying(String movieID)
        {
            try
            {
                DvdInfo dvdInfo = new DvdInfoService().getByID(movieID);             //Throws NoRecordException
                List<DvdCopy> availabeCopies = new DvdCopyService().getAllInStockBuyCopiesForDvdInfo(dvdInfo);        //Throws NoRecordException || DALException
                return true;
            }
            catch (NoRecordException)
            {
                return false;
            }
        }
    }
}
