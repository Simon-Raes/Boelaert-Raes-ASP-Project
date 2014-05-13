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
        /*
         * Returns a list with all the categories in it
         * Throws a NoRecordException if no records were found
         */
        public List<Category> getAll() 
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

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
                    else
                    {
                        throw new NoRecordException("No records were found - CategorieDAO getAll()");
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
            }            
        }    

        /*
         * Returns a category based on an ID
         */
        public Category getCategoryByID(String id)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM Categories WHERE category_id = @category_id", cnn);
                command.Parameters.Add(new SqlParameter("@category_id", id));
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return createCategory(reader);
                    }
                    else
                    {  
                        throw new NoRecordException("No records were found - CategorieDAO getAll()");
                    }
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
            return new Category
            {
                category_id = Convert.ToInt32(reader["category_id"]),
                name = Convert.ToString(reader["name"])
            };
        }
    }
}
