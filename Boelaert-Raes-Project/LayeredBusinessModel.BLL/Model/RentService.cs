﻿using LayeredBusinessModel.Domain;
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
            Dictionary<int, List<DateTime>> beschikbaarheden = new Dictionary<int, List<DateTime>>();
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
                if (beschikbaarheden.ContainsKey(order.dvd_copy_id))
                {
                    for (int i = 0; i < 14; i++)
                    {
                        DateTime tempDate = DateTime.Now.Date.AddDays(i);
                        if (tempDate >= order.startdate && tempDate <= order.enddate)
                        {
                            if (!beschikbaarheden[order.dvd_copy_id].Contains(tempDate))
                            {
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
            List<DateTime> dates = new List<DateTime>();


            //todo: first get all fully available copies, only send back list of available dates if that first list is 0
            DvdCopyService orderLineService = new DvdCopyService();

            //here: the result will contain duplicates (1 copy_id can return multiple records), but this does not affect the result of this code
            List<DvdCopy> dvdCopies = orderLineService.getAllFullyAvailableCopies(dvd, startdate);
            if (dvdCopies.Count > 0)
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
                Dictionary<int, List<DateTime>> result = getAllOrdersForDVD(dvd, startdate);


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


    }
}
