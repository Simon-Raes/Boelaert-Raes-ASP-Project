using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;

namespace LayeredBusinessModel.BLL
{
    public class UserRecommendations
    {
        /**Method that gets recommendations based on the user's order history.
        1) Gets all past orders and determines the user's 3 favourite genres.
        2) Gets a list of DVDs that have the most matches with the user's favourites. 
           A dvd that contains all 3 favourite genres will rank higher than a dvd that only matches 1.
        3) Removes dvd's that the user has already rented or bought before.
        4) If no dvd's are left (or if the user has no orderlines), a ????????(to be decided, most popular for now) list of DVDs is shown.
         */
        public static List<DvdInfo> getRecommendations(int customer_id, int amount)
        {
            List<DvdInfo> dvdList = new List<DvdInfo>();
            List<Genre> genres = new List<Genre>();

            OrderLineService orderLineService = new OrderLineService();
            List<OrderLine> orderLines = orderLineService.getOrderLinesForCustomer(customer_id);
            List<int> orderLinesDvdIds = new List<int>(); //list that contains the DVDids of the orderlines

            GenreService genreService = new GenreService();
            DvdInfoService dvdInfoService = new DvdInfoService();

            //customer has at least 1 order, base recommendations on orderLines
            if (orderLines.Count > 0)
            {
                foreach (OrderLine line in orderLines)
                {
                    int dvdID = line.dvd_info_id;
                    genres.AddRange(genreService.getGenresForDvd(dvdID));

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
                List<int> dvdIds = dvdInfoService.getRecommendations(maxGenres, 16); //get max 16 dvds. Index will always show maximum 4, but catalog can show up to 16

                List<int> dvdTempIds = new List<int>();

                foreach (int id in dvdIds)
                {
                    if (!orderLinesDvdIds.Contains(id))
                    {
                        dvdTempIds.Add(id);
                    }
                }

                if (dvdTempIds.Count > 0)
                {
                    foreach (int id in dvdTempIds)
                    {
                        dvdList.Add(dvdInfoService.getDvdInfoWithID(id.ToString()));
                    }
                }
                else
                {
                    //customer has already bought or rented every suggestion, just recommend some popular dvds (TODO: find something different to recommend)
                    //this list can ALSO contain dvd's the user has already rented!! Definitely needs something else to suggest
                    dvdList = getSomethingSomething();
                }
            }
            else
            {
                //customer has no orderlines, just recommend something else (TODO: find something else to recommend)
                dvdList = getSomethingSomething();
            }


            //remove elements until the list size is 4 (index rows only show 4 elements)
            //loop deletes a random item until the size is 4 so the user gets different recommendations on every visit/refresh
            Random rnd = new Random();
            while (dvdList.Count > amount)
            {
                dvdList.RemoveAt(rnd.Next(dvdList.Count));
            }

            return dvdList;
        }


        //todo: find something to suggest to users that have no orders or have already bought every possible recommendation
        private static List<DvdInfo> getSomethingSomething()
        {
            DvdInfoService service = new DvdInfoService();
            return service.getMostPopularDvds(4);
        }
    }
}
