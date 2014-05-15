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
    /*
     * All methods that rely on the methods defined in this class are surrounded with try catch blocks
     */ 
    public class CategoryDAO : DAO
    {
        /*
         * Returns a category based on an ID
         * Throws a NoRecordException if no records were found
         * Throws a DALException if something else went wrong
         */
        public Category getByID(String id)
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
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to get a category based on an ID", ex);
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
                throw new NoRecordException("No records were found - CategorieDAO getByID()");
            }
        }

        /*
         * Returns a list with all the categories in it
         * Throws a NoRecordException if no records were found
         * Throws a DALException if something else went wrong
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
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to get all the categories", ex);
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
                throw new NoRecordException("No records were found - CategorieDAO getAll()");
            }            
        }

        /*
         * Returns a list with all the categories in it
         * Throws a NoRecordException if no records were found
         * Throws a DALException if something else went wrong
         */
        public List<Category> getAll_StoredProcedure()
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("getAllCategories", cnn);
                command.CommandType = System.Data.CommandType.StoredProcedure;
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
                    throw new DALException("Failed to get all the categories", ex);
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
                throw new NoRecordException("No records were found - CategorieDAO getAll()");
            }
        }

        /*
         * Creates a Category-Object
         */ 
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
