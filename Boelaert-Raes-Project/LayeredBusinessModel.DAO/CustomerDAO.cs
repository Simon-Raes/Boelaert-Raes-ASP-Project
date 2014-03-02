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
    public class CustomerDAO
    {
        private string strSQL;
        //ga naar web.config . haal de connectionstring "ProjectConnection" .  
        private string sDatabaseLocatie = ConfigurationManager.ConnectionStrings["ProjectConnection"].ConnectionString;
        private SqlConnection cnn;

        public List<Customer> getAllCustomers()
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<Customer> customerList = new List<Customer>();

            SqlCommand command = new SqlCommand("SELECT * FROM Customers", cnn);
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    customerList.Add(createCustomer(reader));
                }

                reader.Close();
                
            }
            catch (Exception ex)
            {
                List<Customer> customejijirList = new List<Customer>();

            }
            finally
            {
                cnn.Close();
            }
            return customerList;
        }





        private Customer createCustomer(SqlDataReader reader)
        {
            //gebruik van object initializer ipv constructor
            Customer customer = new Customer
            {
                customer_id = Convert.ToInt32(reader["customer_id"]),
                name = Convert.ToString(reader["name"]),
                email = Convert.ToString(reader["email"]),
                password = Convert.ToString(reader["password"]),
                numberOfVisits = Convert.ToInt32(reader["number_of_visits"])
            };
            return customer;
        }
    }


}
