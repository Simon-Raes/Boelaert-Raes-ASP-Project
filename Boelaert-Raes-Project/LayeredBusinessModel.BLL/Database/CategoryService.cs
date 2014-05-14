using System;
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
        private CategoryDAO categoryDAO;

        /*
         * Returns a list with all the Categories
         */
        public List<Category> getAll()
        {
            categoryDAO = new CategoryDAO();
            return categoryDAO.getAll();                    //Throws NoRecordException || DALException   
        }

        /*
         * Return a Category based on an ID
         */
        public Category getByID(String categoryID)
        {
            categoryDAO = new CategoryDAO();
            return categoryDAO.getByID(categoryID);         //Throws NoRecordException || DALException
        }
    }
}
