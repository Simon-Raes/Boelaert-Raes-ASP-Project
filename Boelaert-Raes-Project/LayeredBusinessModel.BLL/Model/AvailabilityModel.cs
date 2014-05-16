using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.DAO;
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

        /**Returns a dictionary with dvd_copy_id, List of dates where the copy is available.*/
        public Dictionary<int, List<DateTime>> getAllAvailableDaysPerCopyForDvdInfo(DvdInfo dvd, DateTime startdate)
        {
            Dictionary<int, List<DateTime>> dicCopyUnavailableDates = new Dictionary<int, List<DateTime>>();
            Dictionary<int, List<DateTime>> result = new Dictionary<int, List<DateTime>>();

            dicCopyUnavailableDates = getAllUnavailableDaysPerCopyForDvdInfo(dvd, startdate);           //Throws NoRecordException

            foreach (int i in dicCopyUnavailableDates.Keys)
            {
                result.Add(i, null);
                List<DateTime> bezet = dicCopyUnavailableDates[i];

                List<DateTime> freeDates = new List<DateTime>();

                for (int j = 0; j < 14; j++)
                {
                    DateTime d = DateTime.Now.Date.AddDays(j);

                    if (!bezet.Contains(d))
                    {
                        freeDates.Add(d);
                    }
                }
                result[i] = freeDates;
            }
            return result;
        }


        /**Returns a dictionary with dvd_copy_id, List of dates where the copy is NOT available.*/
        public Dictionary<int, List<DateTime>> getAllUnavailableDaysPerCopyForDvdInfo(DvdInfo dvd, DateTime startdate)
        {
            Dictionary<int, List<DateTime>> dicCopyUnavailableDates = new Dictionary<int, List<DateTime>>();
                       
            try
            {
                List<OrderLine> orders = new OrderLineDAO().getAllByDvdAndStartdate(dvd, startdate);           //Throws NoRecordException
                               
                foreach (OrderLine order in orders)
                {
                    //add the copy to the list if needed
                    if (!dicCopyUnavailableDates.ContainsKey(order.dvdCopy.dvd_copy_id))
                    {                        
                        List<DateTime> bezettemomenten = new List<DateTime>();
                        dicCopyUnavailableDates.Add(order.dvdCopy.dvd_copy_id, bezettemomenten);
                    }

                    if (dicCopyUnavailableDates.ContainsKey(order.dvdCopy.dvd_copy_id))
                    {
                        for (int i = 0; i < 14; i++)
                        {
                            DateTime tempDate = DateTime.Now.Date.AddDays(i);
                            if (tempDate >= order.startdate && tempDate <= order.enddate)
                            {
                                if (!dicCopyUnavailableDates[order.dvdCopy.dvd_copy_id].Contains(tempDate))
                                {
                                    dicCopyUnavailableDates[order.dvdCopy.dvd_copy_id].Add(tempDate);
                                }
                            }
                        }
                    }
                }
            }
            catch(NoRecordException ex)
            {
                
            }
            
            return dicCopyUnavailableDates;
        }

        /**Returns a list of all days where at least 1 copy is available.*/
        public List<DateTime> getAvailabilities(DvdInfo dvd, DateTime startDate)
        {
            List<DateTime> dates = new List<DateTime>();

            if (fullCopiesAvailable(dvd, startDate))
            {
                //at least 1 copy is fully available for the next 2 weeks, send back a full dates list
                for (int j = 0; j < 14; j++)
                {
                    DateTime d = DateTime.Now.Date.AddDays(j);
                    dates.Add(d);
                }
            }
            else
            {
                //no copies are available for the full 2 weeks, get detailed information about all copies that have some availability in the next 2 weeks:
                Dictionary<int, List<DateTime>> result = getAllAvailableDaysPerCopyForDvdInfo(dvd, startDate);          //Throws NoRecordException

                foreach (List<DateTime> list in result.Values)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (!dates.Contains(list[i]))
                        {
                            dates.Add(list[i]);
                        }
                    }
                }
            }
            return dates;
        }

        /**Returns the number of consecutive days the dvd_info is available, starting from the supplied date*/
        public int getDaysAvailableFromDate(DvdInfo dvd, DateTime startDate)
        {
            int days = -1;

            if (fullCopiesAvailable(dvd, startDate))
            {
                //there is at least 1 copy that is available for the full 14 days, no more checks are needed and the max number of days can be returned
                days = 14;
            }
            else
            {
                //all copies have some rent or reservations in the next 2 weeks, check their availability in detail

                Dictionary<int, DateTime> unavailableDatesMap = new Dictionary<int, DateTime>();

                //get all orderlines for copies that have some orderlines in the next 2 weeks
                List<OrderLine> orders = new OrderLineDAO().getAllByDvdAndStartdate(dvd, startDate);           //Throws NoRecordException

                foreach (OrderLine order in orders)
                {
                    if (!unavailableDatesMap.ContainsKey(order.dvdCopy.dvd_copy_id))
                    {
                        //set the default availability at 2 weeks from now 
                        unavailableDatesMap.Add(order.dvdCopy.dvd_copy_id, order.startdate);
                    }
                    if (unavailableDatesMap.ContainsKey(order.dvdCopy.dvd_copy_id))
                    {
                        //if the order for the copy is unavailable sooner, add that one to the dictionary
                        if (order.startdate < unavailableDatesMap[order.dvdCopy.dvd_copy_id] && order.startdate > DateTime.Today)
                        {
                            unavailableDatesMap[order.dvdCopy.dvd_copy_id] = order.startdate;
                        }
                    }
                }

                //we now have a dictionary with the copies and the first date on which they'll be UNavailable again
                foreach (DateTime date in unavailableDatesMap.Values)
                {
                    //only allow orderLines that start after the supplied date
                    if (date > startDate)
                    {
                        if ((date - startDate).Days > days)
                        {
                            days = (date - startDate).Days;
                        }
                    }
                }
            }
            return days;
        }

        /**Returns true if at least one copy is available for the full 14 days (= no orders in the next 14 days)*/
        private Boolean fullCopiesAvailable(DvdInfo dvd, DateTime startDate)
        {
            try
            {
                //here: the result will contain duplicates (1 copy_id can return multiple records), but this does not affect the result of this code
                new DvdCopyService().getAllFullyAvailableCopies(dvd, startDate);
                return true;
            }
            catch (NoRecordException)
            {
                return false;
            }
        }
    }
}
