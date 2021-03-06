﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.DAO;
using CustomException;

namespace LayeredBusinessModel.BLL
{
    /*
     * Business methods for Category
     * All methods that rely on the methods defined in this class are allready surrounded with a try catch block
     */ 
    public class CategoryService
    {
        /*
         * Returns a list with all the Categories
         */
        public List<Category> getAll()
        {
            return new CategoryDAO().getAll_StoredProcedure();                    //Throws NoRecordException || DALException   
        }

        /*
         * Return a Category based on an ID
         */
        public Category getByID(String categoryID)
        {
            return new CategoryDAO().getByID(categoryID);         //Throws NoRecordException || DALException
        }
    }
}
