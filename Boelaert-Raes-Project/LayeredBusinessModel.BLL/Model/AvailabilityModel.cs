using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;

namespace LayeredBusinessModel.BLL.Model
{
    public class AvailabilityModel
    {
        public static Boolean isAvailableForBuying(String movieID)
        {
            DvdCopyService dvdCopyService = new DvdCopyService();
            List<DvdCopy> availabeCopies = dvdCopyService.getAllInStockBuyCopiesForDvdInfo(movieID);   
            if(availabeCopies.Count>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
