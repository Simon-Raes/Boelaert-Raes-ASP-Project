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
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                PageVisits pageVisits = null;

                SqlCommand command = new SqlCommand("SELECT * FROM PageVisits WHERE customer_id = @customer_id AND dvd_info_id = @dvd_info_id", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id", customer.customer_id));
                command.Parameters.Add(new SqlParameter("@dvd_info_id", dvdInfo.dvd_info_id));

                try
                {
                    cnn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    pageVisits = createPageVisits(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    pageVisits = null;
                }
                finally
                {
                    cnn.Close();
                }

                return pageVisits;
            }
        }

        public List<PageVisits> getTopPageVisitsForCustomer(Customer customer, int number_of_results)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                List<PageVisits> pageVisits = new List<PageVisits>();

                SqlCommand command = new SqlCommand("SELECT top " + number_of_results + " * FROM PageVisits WHERE customer_id = @customer_id ORDER BY number_of_visits DESC", cnn);
                command.Parameters.Add(new SqlParameter("@amount", number_of_results));
                command.Parameters.Add(new SqlParameter("@customer_id", customer.customer_id));


                try
                {
                    cnn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        pageVisits.Add(createPageVisits(reader));
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

                return pageVisits;
            }
        }

        public void addPageVisits(PageVisits pageVisits)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {

                SqlCommand command = new SqlCommand("INSERT INTO PageVisits " +
                "(customer_id, dvd_info_id, number_of_visits) " +
                "VALUES(@customer_id, @dvd_info_id, @number_of_visits)", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id", pageVisits.customer.customer_id));
                command.Parameters.Add(new SqlParameter("@dvd_info_id", pageVisits.dvdInfo.dvd_info_id));
                command.Parameters.Add(new SqlParameter("@number_of_visits", pageVisits.number_of_visits));

                try
                {
                    cnn.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    cnn.Close();
                }
            }
        }

        public void updatePageVisits(PageVisits pageVisits)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {

                SqlCommand command = new SqlCommand("UPDATE PageVisits " +
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
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    cnn.Close();
                }
            }
        }        

        private PageVisits createPageVisits(SqlDataReader reader)
        {
            PageVisits pageVisits = new PageVisits
            {
                customer = new CustomerDAO().getCustomerByID(Convert.ToInt32(reader["customer_id"])),
                dvdInfo = new DvdInfoDAO().getDvdInfoWithId(reader["dvd_info_id"].ToString()),
                number_of_visits = Convert.ToInt32(reader["number_of_visits"])
            };
            return pageVisits;
        }
    }
}
