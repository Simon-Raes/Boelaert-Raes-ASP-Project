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
    public class PageVisitsDAO : DAO
    {

        public PageVisits getPageVisits(Customer customer, DvdInfo dvdInfo)
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
                        return createPageVisits(reader);
                    }                    
                }
                catch (Exception ex)
                {

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
                            pageVisits.Add(createPageVisits(reader));
                        }
                        return pageVisits;
                    }
                }
                catch (Exception ex)
                {

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

        public Boolean addPageVisits(PageVisits pageVisits)
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
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (cnn != null)
                    {
                        cnn.Close();
                    }
                }
                return false;
            }
        }

        public Boolean updatePageVisits(PageVisits pageVisits)
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
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (cnn != null)
                    {
                        cnn.Close();
                    }
                }
                return false;
            }
        }        

        private PageVisits createPageVisits(SqlDataReader reader)
        {
            return new PageVisits
            {
                customer = new CustomerDAO().getByID(reader["customer_id"].ToString()),
                dvdInfo = new DvdInfoDAO().getDvdInfoWithId(reader["dvd_info_id"].ToString()),
                number_of_visits = Convert.ToInt32(reader["number_of_visits"])
            };
        }
    }
}
