using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;
using CustomException;

namespace LayeredBusinessModel.BLL
{
    public class RecommendationsModel
    {
        private List<String> orderLinesDvdIds;

        /**Returns a list of recommendations based on the user's order history.
        1) Gets all past orders and determines the user's 3 favourite genres.
        2) Gets a list of DVDs that have the most matches with the user's favourites. 
           A dvd that contains all 3 favourite genres will rank higher than a dvd that only matches 1.
        3) Removes dvds that the user has already rented or bought before.
        4) If no dvd's are left (or if the user has no orderlines), a list of DVDs is generated based on the users page views shown.
         */
        public List<DvdInfo> getRecommendations(Customer customer, int amount)
        {
            List<DvdInfo> dvdList = new List<DvdInfo>();
            List<Genre> genres = new List<Genre>();
            List<OrderLine> orderLines;
            try
            {
                orderLines = new OrderLineService().getByCustomer(customer);          //Throws NoRecordException 
            } catch(Exception error)
            {
                orderLines = new List<OrderLine>();
            }
                     
            orderLinesDvdIds = new List<String>(); //list that contains the DVDids of the orderlines (=movies that the user has rented before)

            GenreService genreService = new GenreService();
            DvdInfoService dvdInfoService = new DvdInfoService();

            //customer has at least 1 order, base recommendations on orderLines
            if (orderLines.Count > 0)
            {
                foreach (OrderLine line in orderLines)
                {
                    String dvdID = line.dvdInfo.dvd_info_id;
                    genres.AddRange(genreService.getGenresForDvd(dvdID));            //Throws NoRecordException

                    //fill orderLinesDvdIds
                    if (!orderLinesDvdIds.Contains(dvdID))
                    {
                        orderLinesDvdIds.Add(dvdID);
                    }
                }

                //put genres in a dictionary (genreID, numberOfOccurrances)
                Dictionary<int, int> genreCountMap = new Dictionary<int, int>();
                foreach (Genre genre in genres)
                {
                    if (genreCountMap.ContainsKey(genre.genre_id))
                    {
                        genreCountMap[genre.genre_id]++;
                    }
                    else
                    {
                        genreCountMap.Add(genre.genre_id, 1);
                    }
                }

                //loop over dictionary and find the 3 most occuring genres                
                int[] maxGenres = { 0, 0, 0 };
                int[] maxCounts = { 0, 0, 0 };

                foreach (KeyValuePair<int, int> pair in genreCountMap)
                {
                    if (maxCounts[0] < pair.Value)
                    {
                        maxCounts[2] = maxCounts[1];
                        maxCounts[1] = maxCounts[0];
                        maxCounts[0] = pair.Value;

                        maxGenres[2] = maxGenres[1];
                        maxGenres[1] = maxGenres[0];
                        maxGenres[0] = pair.Key;
                    }
                    else if (maxCounts[1] < pair.Value)
                    {
                        maxCounts[2] = maxCounts[1];
                        maxCounts[1] = pair.Value;

                        maxGenres[2] = maxGenres[1];
                        maxGenres[1] = pair.Key;
                    }
                    else if (maxCounts[2] < pair.Value)
                    {
                        maxCounts[2] = pair.Value;

                        maxGenres[2] = pair.Key;
                    }
                }

                //we now have the user's 3 favourite genres in maxGenres, use that info to get the dvds that match those 3 genres the most
                //get max 16 dvds. Index will always show maximum 4, but catalog can show up to 16
                List<String> dvdIds = dvdInfoService.getRecommendations(maxGenres, 16);            //Throws NoRecordException

                List<String> dvdTempIds = new List<String>();

                //only accept dvd's that the user had not watched yet
                foreach (String id in dvdIds)
                {
                    if (!orderLinesDvdIds.Contains(id))
                    {
                        dvdTempIds.Add(id);
                    }
                }

                if (dvdTempIds.Count > 0)
                {
                    foreach (String id in dvdTempIds)
                    {
                        dvdList.Add(dvdInfoService.getByID(id.ToString()));            //Throws NoRecordException            
                    }
                }
                else
                {
                    //customer has already bought or rented every suggestion, recommend the movies whose pages the user has viewed most often
                    dvdList = getMostViewedDvdInfos(customer);          //Throws NoRecordException
                }
            }
            else
            {
                //customer has no orderlines, recommend the movies whose pages the user has viewed the most
                dvdList = getMostViewedDvdInfos(customer);              //Throws NoRecordException
            }

            //remove elements until the list size is the requested size (4 for index row, 16 for catalog page)
            //loop deletes a random item until the size is ok so the user gets different recommendations on every visit/refresh
            Random rnd = new Random();
            while (dvdList.Count > amount)
            {
                dvdList.RemoveAt(rnd.Next(dvdList.Count));
            }

            return dvdList;
        }


        /**Returns a list of the most viewed pages.*/
        private List<DvdInfo> getMostViewedDvdInfos(Customer customer)
        {
            List<DvdInfo> dvdInfos = new List<DvdInfo>();
            try
            {
                DvdInfoService dvdInfoService = new DvdInfoService();

                List<PageVisits> pageVisitsList = new PageVisitsService().getTopPageVisitsForCustomer(customer, 16);          //Throws NoRecordException
                
                List<DvdInfo> dvdInfosFinal = new List<DvdInfo>();

                foreach (PageVisits pageVisits in pageVisitsList)
                {
                    dvdInfos.Add(dvdInfoService.getByID(pageVisits.dvdInfo.dvd_info_id.ToString()));           //Throws NoRecordException
                }

                //only return dvd's that the user hasn't bought before
                foreach (DvdInfo dvdInfo in dvdInfos)
                {
                    if (!orderLinesDvdIds.Contains(dvdInfo.dvd_info_id))
                    {
                        dvdInfosFinal.Add(dvdInfo);
                    }
                }               
            }
            catch (DALException ex)
            {
                throw new NoRecordException("failed to retrieve data", ex);
            }
            catch(NoRecordException ex)
            {
                
            }
            return dvdInfos;
        }        
    }
}
