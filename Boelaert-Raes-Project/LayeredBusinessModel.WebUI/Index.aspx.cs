using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace LayeredBusinessModel.WebUI
{
    public partial class Home : System.Web.UI.Page
    {
        Customer user;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                user = (Customer)Session["user"];

                //only show recommendations for a logged in user
                if (user != null)
                {
                    divRecommended.Visible = true;
                    setupRecommendations();
                }
                else
                {
                    divRecommended.Visible = false;
                }

                setupNewReleases();
                setupMostPopular();
            }

        }

        private void setupRecommendations()
        {
            List<DvdInfo> dvdList;
            List<Genre> genres = new List<Genre>();

            OrderLineService orderLineService = new OrderLineService();
            List<OrderLine> orderLines = orderLineService.getOrderLinesForCustomer(user.customer_id);

            GenreService genreService = new GenreService();
            DvdInfoService dvdInfoService = new DvdInfoService();

            if (orderLines.Count > 0)
            {
                //customer has at least 1 order, base recommendations on orderLines
                foreach (OrderLine line in orderLines)
                {
                    int dvdID = line.dvd_info_id;
                    genres.AddRange(genreService.getGenresForDvd(dvdID));
                }

                //put genres in a dictionary (genreID, numberOfOccurrances)
                Dictionary<int, int> genreCountMap = new Dictionary<int, int>();
                foreach(Genre genre in genres)
                {
                    if(genreCountMap.ContainsKey(genre.genre_id))
                    {
                        genreCountMap[genre.genre_id]++;
                    }
                    else
                    {
                        genreCountMap.Add(genre.genre_id, 0);
                    }
                }

                //loop over dictionary and find the most occuring genre
                int maxGenre = 0;
                int maxCount = 0;
                foreach(KeyValuePair<int, int> pair in genreCountMap)
                {
                    if(maxCount<pair.Value)
                    {
                        maxCount = pair.Value;
                        maxGenre = pair.Key;
                    }
                }

                //we now have the user's favourite genre in maxGenre, use it to get all movies of that genre
                dvdList = dvdInfoService.searchDvdWithTextAndGenre("", maxGenre.ToString());  
            }
            else
            {
                //customer has no orderlines, just recommend popular releases (TODO: find something else to recommend)
                dvdList = new DvdInfoService().getMostPopularDvds(4);
            }

            //remove elements until the list size is 4 (index rows only show 4 elements)
            //loop deletes a random item until the size is 4 so the user gets different recommendations on every visit
            Random rnd = new Random();
            while(dvdList.Count>4)
            {                
                dvdList.RemoveAt(rnd.Next(dvdList.Count));
            }
            
            //add dvd cards to page
            foreach (DvdInfo d in dvdList)
            {
                dvdInfoUserControl dvdInfo = (dvdInfoUserControl)Page.LoadControl("dvdInfoUserControl.ascx");
                dvdInfo.id = d.dvd_info_id;
                dvdInfo.imageUrl = d.image;
                dvdInfo.title = d.name;
                dvdInfo.buy_price = d.buy_price;
                dvdInfo.rent_price = d.rent_price;

                recommened.Controls.Add(dvdInfo);
            }

        }

        private void setupNewReleases()
        {
            List<DvdInfo> dvdList = new DvdInfoService().getLatestDvds(4);

            foreach (DvdInfo d in dvdList)
            {
                dvdInfoUserControl dvdInfo = (dvdInfoUserControl)Page.LoadControl("dvdInfoUserControl.ascx");
                dvdInfo.id = d.dvd_info_id;
                dvdInfo.imageUrl = d.image;
                dvdInfo.title = d.name;
                dvdInfo.buy_price = d.buy_price;
                dvdInfo.rent_price = d.rent_price;

                newReleases.Controls.Add(dvdInfo);
            }
        }

        private void setupMostPopular()
        {
            List<DvdInfo> dvdList = new DvdInfoService().getMostPopularDvds(4);

            foreach (DvdInfo d in dvdList)
            {
                dvdInfoUserControl dvdInfo = (dvdInfoUserControl)Page.LoadControl("dvdInfoUserControl.ascx");
                dvdInfo.id = d.dvd_info_id;
                dvdInfo.imageUrl = d.image;
                dvdInfo.title = d.name;
                dvdInfo.buy_price = d.buy_price;
                dvdInfo.rent_price = d.rent_price;

                mostPopular.Controls.Add(dvdInfo);
            }
        }



    }
}