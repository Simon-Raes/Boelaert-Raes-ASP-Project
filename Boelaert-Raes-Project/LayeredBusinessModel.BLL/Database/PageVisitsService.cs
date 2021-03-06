﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.DAO;

namespace LayeredBusinessModel.BLL
{
    public class PageVisitsService
    {
        public PageVisits getByDvdAndCustomer(Customer customer, DvdInfo dvdInfo)
        {
            return new PageVisitsDAO().getByDvdAndCustomer(customer, dvdInfo);            //Throws NoRecordException
        }

        public List<PageVisits> getTopPageVisitsForCustomer(Customer customer, int number_of_results)
        {
            return new PageVisitsDAO().getTopPageVisitsForCustomer(customer, number_of_results);            //Throws NoRecordExaption
        }

        public Boolean addPageVisits(PageVisits pageVisits)
        {
            return new PageVisitsDAO().add(pageVisits);
        }

        public Boolean updatePageVisits(PageVisits pageVisits)
        {
            if (pageVisits != null)
            {
                if(pageVisits.number_of_visits>1)
                {
                    return new PageVisitsDAO().update(pageVisits);
                }
                else
                {
                    return new PageVisitsDAO().add(pageVisits);
                }
                
            }
            return false;
        }
    }
}
