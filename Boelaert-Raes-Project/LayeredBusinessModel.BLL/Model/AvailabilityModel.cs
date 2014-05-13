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
            DvdInfoService dvdInfoService = new DvdInfoService();
            DvdInfo dvdInfo = dvdInfoService.getDvdInfoWithID(movieID);

            DvdCopyService dvdCopyService = new DvdCopyService();
            List<DvdCopy> availabeCopies = dvdCopyService.getAllInStockBuyCopiesForDvdInfo(dvdInfo);   
            if(availabeCopies!= null && availabeCopies.Count>0)
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
