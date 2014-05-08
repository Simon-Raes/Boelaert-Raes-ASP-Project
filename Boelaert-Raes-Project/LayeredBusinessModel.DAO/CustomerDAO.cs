using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using LayeredBusinessModel.Domain;
using System.Configuration;
using System.Data;

namespace LayeredBusinessModel.DAO
{
    public class CustomerDAO : DAO
    {
        public List<Customer> getAllCustomers()
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            SqlCommand command = new SqlCommand("SELECT * FROM Customers", cnn);
            List<Customer> customerList = new List<Customer>();
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    customerList.Add(createCustomer(reader));
                }
                reader.Close();
                return customerList;
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                cnn.Close();
            }
            return null;
        }

        /*
         * niet meer nodig normaal
         * 
        public Customer getCustomerWithLogin(string login)
        {
            
            cnn = new SqlConnection(sDatabaseLocatie);
            Customer customer = null;

            SqlCommand command = new SqlCommand("SELECT * FROM Customers WHERE login = @login", cnn);
            command.Parameters.Add(new SqlParameter("@login", login));

            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                customer = createCustomer(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                customer = null;
            }
            finally
            {
                cnn.Close();
            }

            return customer;
        }
         * */

        public Customer getCustomerWithEmail(string email)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            SqlCommand command = new SqlCommand("SELECT * FROM Customers WHERE email = @email", cnn);
            command.Parameters.Add(new SqlParameter("@email",email));

            Customer customer = null;
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    customer = createCustomer(reader);
                }
                reader.Close();
                return customer;
            }
            catch (Exception ex)
            {
               
            }
            finally
            {
                cnn.Close();
            }
            return null;
        }

        public Boolean updateCustomer(Customer customer)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            SqlCommand command = new SqlCommand("UPDATE Customers SET name = @name, email = @email, password = @password, number_of_visits = @number_of_visits, street = @street, zip = @zip, municipality=@municipality,isVerrified=@verrified WHERE customer_id = @id", cnn);

            command.Parameters.Add(new SqlParameter("@name", customer.name));
            command.Parameters.Add(new SqlParameter("@email", customer.email));
            command.Parameters.Add(new SqlParameter("@password", customer.password));
            command.Parameters.Add(new SqlParameter("@number_of_visits", customer.numberOfVisits));
            command.Parameters.Add(new SqlParameter("@street", customer.street));
            command.Parameters.Add(new SqlParameter("@zip", customer.zip));
            command.Parameters.Add(new SqlParameter("@municipality", customer.municipality));
            command.Parameters.Add(new SqlParameter("@verrified", customer.isVerrified));
            command.Parameters.Add(new SqlParameter("@id", customer.customer_id));

            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
                return true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cnn.Close();
            }
            return false;
        }

        public Boolean addCustomer(Customer customer)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            
            SqlCommand command = new SqlCommand("INSERT INTO Customers (name,email,password,number_of_visits,street,zip,municipality,isVerrified)" +
            "VALUES(@name,@email,@password,@visits,@street,@zip,@municipality,@verrified)", cnn);

            /*
             * SqlParameter nameParam = new SqlParameter("@name",SqlDbType.VarChar,50);
            nameParam.Value = customer.name;
            SqlParameter emailParam = new SqlParameter("@email", SqlDbType.VarChar,50);
            emailParam.Value = customer.email;
            SqlParameter passwordParam = new SqlParameter("@password", SqlDbType.VarChar, 50);
            passwordParam.Value = customer.password;
            SqlParameter visitsParam = new SqlParameter("@visits", SqlDbType.Int);
            visitsParam.Value = 0;
            SqlParameter zipParam = new SqlParameter("@zip", SqlDbType.VarChar, 10);
            zipParam.Value = customer.zip;
            SqlParameter streetParam = new SqlParameter("@street", SqlDbType.VarChar, 50);
            streetParam.Value = customer.street;
            SqlParameter municipalityParam = new SqlParameter("@municipality", SqlDbType.VarChar, 50);
            municipalityParam.Value = customer.municipality;
            SqlParameter verrifiedParam = new SqlParameter("@verrified", SqlDbType.Bit);
            verrifiedParam.Value = false;
            */

            command.Parameters.Add(new SqlParameter("@name",customer.name));
            command.Parameters.Add(new SqlParameter("@email",customer.email));
            command.Parameters.Add(new SqlParameter("@password",customer.password));
            command.Parameters.Add(new SqlParameter("@visits",customer.numberOfVisits));
            command.Parameters.Add(new SqlParameter("@zip", customer.zip));
            command.Parameters.Add(new SqlParameter("@street", customer.street));
            command.Parameters.Add(new SqlParameter("@municipality", customer.municipality));
            command.Parameters.Add(new SqlParameter("@verrified", customer.isVerrified));

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
                cnn.Close();
            }
            return false;
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
                numberOfVisits = Convert.ToInt32(reader["number_of_visits"]),
                street = Convert.ToString(reader["street"]),
                municipality = Convert.ToString(reader["municipality"]),
                zip = Convert.ToString(reader["zip"]),
                isVerrified = Convert.ToBoolean(reader["isVerrified"])
            };
            return customer;
        }

    }


}
