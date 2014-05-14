using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using LayeredBusinessModel.Domain;
using System.Configuration;
using CustomException;

namespace LayeredBusinessModel.DAO
{
    public class PageVisitsDAO : DAO
    {
        /*
         * Returns a Pagevitit for a customer and dvd
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public PageVisits getByDvdAndCustomer(Customer customer, DvdInfo dvdInfo)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM PageVisits WHERE customer_id = @customer_id AND dvd_info_id = @dvd_info_id", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id", customer.customer_id));
                command.Parameters.Add(new SqlParameter("@dvd_info_id", dvdInfo.dvd_info_id));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return createPageVisits(reader);            //Throws NoRecordException || DALException  
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get pagevisits for customer and dvd", ex);
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
                throw new NoRecordException("No records were found - PageVisitsDAO getPageVisits()");
            }
        }

        /*
         * Returns a list with Pagevisits for a customer 
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<PageVisits> getTopPageVisitsForCustomer(Customer customer, int number_of_results)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT top @amount * FROM PageVisits WHERE customer_id = @customer_id ORDER BY number_of_visits DESC", cnn);
                command.Parameters.Add(new SqlParameter("@amount", number_of_results));
                command.Parameters.Add(new SqlParameter("@customer_id", customer.customer_id));
                
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<PageVisits> pageVisits = new List<PageVisits>();
                        while (reader.Read())
                        {
                            pageVisits.Add(createPageVisits(reader));           //Throws NoRecordException || DALException  
                        }
                        return pageVisits;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get pagevisits for customer", ex);
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
                throw new NoRecordException("No records were found - PageVisitsDAO getTopPageVisitsForCustomer()");
            }
        }

        /*
         * Adds a pagevisit
         * Returns true if the pagevisit was inserted, false if no records were inserted
         * Throws DALException if something else went wrong
         */
        public Boolean add(PageVisits pageVisits)
        {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("INSERT INTO PageVisits " +
                "(customer_id, dvd_info_id, number_of_visits) " +
                "VALUES(@customer_id, @dvd_info_id, @number_of_visits)", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id", pageVisits.customer.customer_id));
                command.Parameters.Add(new SqlParameter("@dvd_info_id", pageVisits.dvdInfo.dvd_info_id));
                command.Parameters.Add(new SqlParameter("@number_of_visits", pageVisits.number_of_visits));

                try
                {
                    cnn.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to insert a pagevisit", ex);
                }
                finally
                {
                    if (cnn != null)
                    {
                        cnn.Close();
                    }
                }
            }
        }

        /*
         * Updates a pagevisit
         * Returns true if the pagevisit was updated, false if no records were updated
         * Throws DALException if something else went wrong
         */
        public Boolean update(PageVisits pageVisits)
        {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("UPDATE PageVisits " +
                "SET number_of_visits = @number_of_visits " +
                "WHERE customer_id = @customer_id " +
                "AND dvd_info_id = @dvd_info_id", cnn);
                command.Parameters.Add(new SqlParameter("@number_of_visits", pageVisits.number_of_visits));
                command.Parameters.Add(new SqlParameter("@customer_id", pageVisits.customer.customer_id));
                command.Parameters.Add(new SqlParameter("@dvd_info_id", pageVisits.dvdInfo.dvd_info_id));

                try
                {
                    cnn.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to insert a pagevisit", ex);
                }
                finally
                {
                    if (cnn != null)
                    {
                        cnn.Close();
                    }
                }
            }
        }        

        /*
         * Creates a PageVisits-Object
         */ 
        private PageVisits createPageVisits(SqlDataReader reader)
        {
            return new PageVisits
            {
                customer = new CustomerDAO().getByID(reader["customer_id"].ToString()),         //Throws NoRecordException || DALException  
                dvdInfo = new DvdInfoDAO().getByID(reader["dvd_info_id"].ToString()),           //Throws NoRecordException
                number_of_visits = Convert.ToInt32(reader["number_of_visits"])
            };
        }
    }
}
