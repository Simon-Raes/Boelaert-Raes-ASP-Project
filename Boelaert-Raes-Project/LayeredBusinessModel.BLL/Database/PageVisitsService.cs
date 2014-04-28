using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.DAO;

namespace LayeredBusinessModel.BLL.Database
{
    public class PageVisitsService
    {
        private PageVisitsDAO pageVisitsDAO;

        public PageVisits getPageVisits(Customer customer, DvdInfo dvdInfo)
        {
            pageVisitsDAO = new PageVisitsDAO();
            return pageVisitsDAO.getPageVisits(customer, dvdInfo);
        }

        public List<PageVisits> getTopPageVisitsForCustomer(Customer customer, int number_of_results)
        {
            pageVisitsDAO = new PageVisitsDAO();
            return pageVisitsDAO.getTopPageVisitsForCustomer(customer, number_of_results);
        }

        public void addPageVisits(PageVisits pageVisits)
        {
            pageVisitsDAO = new PageVisitsDAO();
            pageVisitsDAO.addPageVisits(pageVisits);
        }

        public void updatePageVisits(PageVisits pageVisits)
        {
            pageVisitsDAO = new PageVisitsDAO();
            pageVisitsDAO.updatePageVisits(pageVisits);
        }
    }
}
