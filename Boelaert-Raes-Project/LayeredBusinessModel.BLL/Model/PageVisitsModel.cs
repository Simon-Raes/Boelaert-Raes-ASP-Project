using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL.Database;
using LayeredBusinessModel.DAO;

namespace LayeredBusinessModel.BLL.Model
{
    public class PageVisitsModel
    {
        /**Increases the number of pagevisits for this user by one. Creates a new record if none currently exists.*/
        public void incrementPageVisits(Customer customer, DvdInfo dvdInfo)
        {
            PageVisitsService pageVisitsService = new PageVisitsService();
            PageVisits pageVisits = pageVisitsService.getPageVisits(customer, dvdInfo);
            if(pageVisits==null)
            {
                pageVisits = new PageVisits();
                pageVisits.customer_id = customer.customer_id;
                pageVisits.dvd_info_id = dvdInfo.dvd_info_id;
                pageVisits.number_of_visits = 1;
                pageVisitsService.addPageVisits(pageVisits);
            }
            else
            {
                pageVisits.number_of_visits = pageVisits.number_of_visits+1;
                pageVisitsService.updatePageVisits(pageVisits);
            }
        }
    }
}
