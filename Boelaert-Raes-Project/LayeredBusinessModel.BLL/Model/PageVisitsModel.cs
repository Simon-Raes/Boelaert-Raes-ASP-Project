using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;
using LayeredBusinessModel.DAO;
using CustomException;

namespace LayeredBusinessModel.BLL
{
    public class PageVisitsModel
    {
        /*Increases the number of pagevisits for this user and movie by one. Creates a new record if none currently exists.*/
        public void incrementPageVisits(Customer customer, DvdInfo dvdInfo)
        {
            PageVisits pageVisits = null;
            PageVisitsService pageVisitsService = new PageVisitsService();
            try
            {
                pageVisits = pageVisitsService.getByDvdAndCustomer(customer, dvdInfo);           //Throws NoRecordException                
                pageVisits.number_of_visits += 1;
            }
            catch (NoRecordException)
            {
                pageVisits = new PageVisits();
                pageVisits.customer = customer;
                pageVisits.dvdInfo = dvdInfo;
                pageVisits.number_of_visits = 1;
            }
            catch (DALException)
            {

            }
            if (pageVisitsService.updatePageVisits(pageVisits))
            {
                //succes
            }
        }
    }
}
