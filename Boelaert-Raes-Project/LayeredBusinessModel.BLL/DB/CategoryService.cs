using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.DAO;

namespace LayeredBusinessModel.BLL
{
    public class CategoryService
    {
        private CategoryDAO categoryDAO;

        public List<Category> getAll()
        {
            categoryDAO = new CategoryDAO();
            return categoryDAO.getAll();
        }

        public Category getCategory(int categoryID)
        {
            categoryDAO = new CategoryDAO();
            return categoryDAO.getCategory(categoryID);
        }
    }
}
