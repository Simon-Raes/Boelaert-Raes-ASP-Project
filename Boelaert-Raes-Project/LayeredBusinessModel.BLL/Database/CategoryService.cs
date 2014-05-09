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
    public class CategoryService
    {
        private CategoryDAO categoryDAO;

        public List<Category> getAll()
        {
            try
            {
                categoryDAO = new CategoryDAO();
                return categoryDAO.getAll();
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public Category getCategoryByID(int categoryID)
        {
            categoryDAO = new CategoryDAO();
            return categoryDAO.getCategoryByID(categoryID);
        }
    }
}
