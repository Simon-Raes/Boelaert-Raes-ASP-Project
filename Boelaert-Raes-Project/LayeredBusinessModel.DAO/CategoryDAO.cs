using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using LayeredBusinessModel.Domain;
using CustomException;
using System.Configuration;

namespace LayeredBusinessModel.DAO
{
    public class CategoryDAO : DAO
    {
        private SqlCommand command;
        private SqlDataReader reader;

        /*
         * Returns a list with all the categories in it
         */
        public List<Category> getAll() 
        {  
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM categories", cnn);
                try
                {
                    List<Category> categoryList = new List<Category>();
                    cnn.Open();
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            categoryList.Add(createCategory(reader));
                        }
                        return categoryList;
                    }
                }
                catch (Exception ex)
                {
                    throw new MyBaseException("CategorieDAO getAll()", ex);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (cnn != null)
                    {
                        cnn.Close();
                    }
                }
                return null;
            }            
        }    

        /*
         * Returns a category based on an ID
         */
        public Category getCategoryByID(int id)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM Categories WHERE category_id = @category_id", cnn);
                command.Parameters.Add(new SqlParameter("@category_id", id));
                try
                {
                    Category category = null;
                    cnn.Open();
                    reader = command.ExecuteReader();

                    while(reader.Read()){
                        category = createCategory(reader);
                    }  
                    return category;
                }
                catch (Exception ex)
                {
                    throw new MyBaseException("CategorieDAO getCategoryByID() " + ex.Message, ex);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (cnn != null)
                    {
                        cnn.Close();
                    }
                }
            }
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
