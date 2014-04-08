using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using LayeredBusinessModel.Domain;
using System.Configuration;

namespace LayeredBusinessModel.DAO
{
    public class CategoryDAO : DAO
    {        
        public List<Category> getAll()
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<Category> categoryList = new List<Category>();

            SqlCommand command = new SqlCommand("SELECT * FROM categories", cnn);
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    categoryList.Add(createCategory(reader));
                }

                reader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                cnn.Close();
            }
            return categoryList;
        }

        public Category getCategory(int categoryID)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            Category category = null;

            SqlCommand command = new SqlCommand("SELECT * FROM Categories WHERE category_id = " + categoryID, cnn);
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                reader.Read();
                category = createCategory(reader);
                reader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                cnn.Close();
            }
            return category;
        }

        private Category createCategory(SqlDataReader reader)
        {
            Category category = new Category
            {
                category_id = Convert.ToInt32(reader["category_id"]),
                name = Convert.ToString(reader["name"])
            };
            return category;
        }
    }
}
