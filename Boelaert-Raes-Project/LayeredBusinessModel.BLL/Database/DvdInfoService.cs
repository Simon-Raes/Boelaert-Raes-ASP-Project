﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.DAO;

namespace LayeredBusinessModel.BLL
{
    public class DvdInfoService
    {
        private DvdInfoDAO dvdInfoDAO;

        /*
        public List<DvdInfo> getAll()
        {
            return new DvdInfoDAO().getAll();         //Throws NoRecordException
        }*/

        public List<DvdInfo> getAllWithBanner()
        {
            return new DvdInfoDAO().getAllWithBanner();           //Throws NoRecordException
        }

        public int add(DvdInfo dvdInfo)
        {
            return new DvdInfoDAO().add(dvdInfo);                   //Throws NoRecordException
        }

        public DvdInfo getByID(String id)
        {
            dvdInfoDAO = new DvdInfoDAO();
            DvdInfo dvd = dvdInfoDAO.getByID(id);                  //Throws NoRecordException
            if(dvd!=null)
            {
                GenreService g = new GenreService();
                dvd.genres = g.getGenresForDvd(id);
            }            

            return dvd;
        }

        public void update(DvdInfo dvd)
        {
            new DvdInfoDAO().update(dvd);                                 //Throws NoRecordException
        }

        public List<DvdInfo> searchDvdWithText(String title)
        {
            return new DvdInfoDAO().searchDvdWithText(title);           //Throws NoRecordException
        }

        public List<DvdInfo> searchDvdWithTextAndCategory(String title, String categoryID)
        {
            return new DvdInfoDAO().searchDvdWithTextAndCategory(title, categoryID);          //Throws NoRecordException
        }

        public List<DvdInfo> searchDvdWithTextAndGenre(String title, String genreID)
        {
            return new DvdInfoDAO().searchDvdWithTextAndGenre(title, genreID);            //Throws NoRecordException
        }

        public List<DvdInfo> searchDvdFromYear(String year)
        {
            return new DvdInfoDAO().searchDvdFromYear(year);          //Throws NoRecordException
        }

        public List<DvdInfo> searchDvdFromDirector(String director)
        {
            return new DvdInfoDAO().searchDvdFromDirector(director);          //Throws NoRecordException
        }

        public List<DvdInfo> searchDvdWithActor(String actor)
        {
            return new DvdInfoDAO().searchDvdWithActor(actor);            //Throws NoRecordException
        }

        public List<DvdInfo> getLatestDvds(int amount)
        {
            return new DvdInfoDAO().getLatestDvds(amount);            //Throws NoRecordException
        }

        public List<DvdInfo> getMostPopularDvds(int amount)
        {
            return new DvdInfoDAO().getMostPopularDvds(amount);           //Throws NoRecordException
        }

        public List<DvdInfo> getRelatedDvds(String id, int amount)
        {
            List<DvdInfo> relatedDvds = new List<DvdInfo>();
            DvdInfoService dvdInfoService = new DvdInfoService();
            DvdInfo dvdInfo = dvdInfoService.getByID(id);                                      //Throws NoRecordException

            foreach (int i in new DvdGenreDAO().findRelatedDvdsBasedOnGenre(dvdInfo, amount))           //Throws NoRecordException
            {
                relatedDvds.Add(new DvdInfoDAO().getByID(i.ToString()));                                //Throws NoRecordException
            }

            return relatedDvds;
        }

        public List<int> getRecommendations(int[] values, int amount)
        {
            return new DvdInfoDAO().getRecommendations(values, amount);                                 //Throws NoRecordException       
        }
    }
}
