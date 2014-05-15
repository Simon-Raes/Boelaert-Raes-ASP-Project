using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.DAO;
using LayeredBusinessModel.BLL.Database;
using CustomException;

namespace LayeredBusinessModel.BLL
{
    public class DvdCopyService
    { 
        /*
         * Returns a DvdCopy based on an ID
         */
        public DvdCopy getByID(String copy_id)
        {
            return new DvdCopyDAO().getByID(copy_id);                                       //Throws NoRecordException || DALException                        
        }        

        public List<DvdCopy> getAllInStockBuyCopiesForDvdInfo(DvdInfo dvdInfo)
        {
            DvdCopyType type = new DvdCopyTypeService().getByName("Verkoop");               //Throws NoRecordException || DALException
            return new DvdCopyDAO().getAllInStockCopiesForDvdInfo(dvdInfo, type);           //Throws NoRecordException || DALException
        }

        /*Returns a list of all dvd copies that are available for the full 14 day period, starting today*/
        public List<DvdCopy> getAllFullyAvailableCopies(DvdInfo dvd, DateTime startdate)
        {
            DvdCopyDAO dvdCopyDAO = new DvdCopyDAO();
            return dvdCopyDAO.getAllFullyAvailableCopies(dvd, startdate);                   //Throws NoRecordException || DALException          
        }

        public Boolean updateCopy(DvdCopy copy)
        {
            return new DvdCopyDAO().update(copy);                                           //Throws NoRecordException || DALException            
        }

        //Nog veranderen naar boolean? 
        public void updateStockStatus(DvdCopy dvdCopy, bool in_stock)
        {
            new DvdCopyDAO().updateStockStatus(dvdCopy, in_stock);                          //Throws NoRecordException || DALException         
        }

        public Boolean resetAllCopies()
        {
            return new DvdCopyDAO().deleteAll();                                            //Throws NoRecordException || DALException 
        }

        /*
        public Boolean add(DvdInfo dvdInfo)
        {
            DvdCopyDAO dvdCopyDAO = new DvdCopyDAO();
            return dvdCopyDAO.addCopiesForDvd(dvdInfo);
        }
        */

        /*
        public List<DvdCopy> getAllInStockRentCopiesForDvdInfo(DvdInfo dvdInfo)
        {
            dvdCopyDAO = new DvdCopyDAO();
            DvdCopyType type = new DvdCopyTypeService().getTypeByName("Verhuur");
            return dvdCopyDAO.getAllInStockCopiesForDvdInfo(dvdInfo,type);                  //Throws NoRecordException || DALException
        }*/

        /*
        public List<DvdCopy> getAllCopiesForDvdInfo(DvdInfo dvdInfo)            
        {
            return new DvdCopyDAO().getAllCopiesForDvdInfo(dvdInfo);                       //Throws NoRecordException || DALException
        }*/
    }
}
