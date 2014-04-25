using LayeredBusinessModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.BLL.Model
{
    public class RentService
    {
        public Dictionary<int, List<DateTime>> getAllOrdersForDVD(DvdInfo dvd, DateTime startdate)
        {
            Dictionary<int, List<DateTime>> beschikbaarheden= new Dictionary<int, List<DateTime>>();
            Dictionary<int, List<DateTime>> result = new Dictionary<int, List<DateTime>>();

            //alle bezettingen
            List<OrderLine> orders = new LayeredBusinessModel.DAO.OrderLineDAO().getAllOrderlinesForDvdFromStartdate(dvd, startdate);

            //bezettingen overlopen
            foreach (OrderLine order in orders)
            {
                //kijken of copy_id al in de lijst zit
                if (!beschikbaarheden.ContainsKey(order.dvd_copy_id))
                { 
                    //zo niet, in de lijst steken
                    List<DateTime> bezettemomenten = new List<DateTime>();
                    beschikbaarheden.Add(order.dvd_copy_id, bezettemomenten);
                }
                if(beschikbaarheden.ContainsKey(order.dvd_copy_id))
                {                    
                    for (int i = 0; i < 14; i++)
                    {
                        DateTime tempDate = DateTime.Now.Date.AddDays(i);
                        if(tempDate >= order.startdate && tempDate <= order.enddate)
                        {
                            if(!beschikbaarheden[order.dvd_copy_id].Contains(tempDate)) {
                                beschikbaarheden[order.dvd_copy_id].Add(tempDate);
                            }                            
                        }
                    }
                }
            }

            foreach (int i in beschikbaarheden.Keys)
            {
                result.Add(i, null);
                List<DateTime> bezet = beschikbaarheden[i];

                List<DateTime> vrij = new List<DateTime>();

                for (int j = 0; j < 14; j++)
                {
                    DateTime d = DateTime.Now.Date.AddDays(j);

                    if (!bezet.Contains(d))
                    {

                        

                        vrij.Add(d);
                    }
                }
                result[i] = vrij;
            }

            return result;
        }


        public List<DateTime> getAvailabilities(DvdInfo dvd, DateTime startdate)
        {
            Dictionary<int, List<DateTime>> result = getAllOrdersForDVD(dvd, startdate);
            List<DateTime> dates = new List<DateTime>();

            foreach(List<DateTime> list in result.Values)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if(!dates.Contains(list[i]))
                    {
                        dates.Add(list[i]);
                    }
                }
            }


            return dates;

        }


    }
}
