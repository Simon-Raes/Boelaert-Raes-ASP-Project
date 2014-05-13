using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using LayeredBusinessModel.Domain;
using System.Configuration;
using System.Data;
using CustomException;

namespace LayeredBusinessModel.DAO
{
    /*
     * All DAO-methods for Customer
     */
    public class CustomerDAO : DAO
    {

        /*
         *  Returns a list with all the customers 
         */
        public List<Customer> getAll()
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM Customers", cnn);            
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<Customer> customerList = new List<Customer>();
                        while (reader.Read())
                        {
                            customerList.Add(createCustomer(reader));
                        }
                        return customerList;
                    }
                }
                catch (Exception ex)
                {
                    throw new MyBaseException("CustomerDAO getAll()", ex);
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
         * Returns a Customer based on an ID
         */
        public Customer getByID(String id)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM Customers WHERE customer_id = @id", cnn);
                command.Parameters.Add(new SqlParameter("@id", id));
                
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return createCustomer(reader);                     
                    }
                }
                catch (Exception ex)
                {
                    throw new MyBaseException("CustomerDAO getCustomerByID", ex);
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
         *  Returns a customer based on an emailaddress
         */
        public Customer getByEmail(String email)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM Customers WHERE email = @email", cnn);
                command.Parameters.Add(new SqlParameter("@email", email));
                               
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();                        
                        return createCustomer(reader);
                    }
                }
                catch (Exception ex)
                {
                    throw new MyBaseException("CustomerDAO getCustomerByEmail()", ex);
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
         *      Updates a customer.
         *      Returns true if succeeded, false if not
         */
        public Boolean update(Customer customer)
        {
            SqlCommand command = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("UPDATE Customers SET name = @name, email = @email, password = @password, number_of_visits = @number_of_visits, street = @street, zip = @zip, municipality=@municipality,isVerrified=@verrified WHERE customer_id = @id", cnn);
                command.Parameters.Add(new SqlParameter("@name", customer.name));
                command.Parameters.Add(new SqlParameter("@email", customer.email));
                command.Parameters.Add(new SqlParameter("@password", customer.password));
                command.Parameters.Add(new SqlParameter("@number_of_visits", customer.numberOfVisits));
                command.Parameters.Add(new SqlParameter("@street", customer.street));
                command.Parameters.Add(new SqlParameter("@zip", customer.zip));
                command.Parameters.Add(new SqlParameter("@municipality", customer.municipality));
                command.Parameters.Add(new SqlParameter("@verrified", customer.isVerified));
                command.Parameters.Add(new SqlParameter("@id", customer.customer_id));

                try
                {
                    cnn.Open();
                    command.ExecuteNonQuery(); //rowsaffected gebruiken om te kijken of de update gelukt is?
                    return true;
                }
                catch (Exception ex)
                {
                    throw new MyBaseException("CustomerDAO updateCustomer", ex);
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
         *      Adds a new customer.
         *      Returns true if succeeded, false if not
         */
        public Boolean add(Customer customer)
        {
            SqlCommand command = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("INSERT INTO Customers (name,email,password,number_of_visits,street,zip,municipality,isVerrified)" +
                "VALUES(@name,@email,@password,@visits,@street,@zip,@municipality,@verrified)", cnn);
                command.Parameters.Add(new SqlParameter("@name", customer.name));
                command.Parameters.Add(new SqlParameter("@email", customer.email));
                command.Parameters.Add(new SqlParameter("@password", customer.password));
                command.Parameters.Add(new SqlParameter("@visits", customer.numberOfVisits));
                command.Parameters.Add(new SqlParameter("@zip", customer.zip));
                command.Parameters.Add(new SqlParameter("@street", customer.street));
                command.Parameters.Add(new SqlParameter("@municipality", customer.municipality));
                command.Parameters.Add(new SqlParameter("@verrified", customer.isVerified));

                try
                {
                    cnn.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new MyBaseException("CustomerDAO addCustomer()", ex);
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

        public Boolean verrify(Customer customer)
        {
            SqlCommand command = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("UPDATE Customers SET isVerrified=@verrified WHERE customer_id = @id", cnn);
                command.Parameters.Add(new SqlParameter("@verrified", customer.isVerified));
                command.Parameters.Add(new SqlParameter("@id", customer.customer_id));

                try
                {
                    cnn.Open();
                    command.ExecuteNonQuery();                    
                    return true;
                }
                catch (Exception ex)
                {
                    throw new MyBaseException("CustomerDAO verrifyCustomer()", ex);
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
         *      Returns a customer based on a sqlReader
         */
        private Customer createCustomer(SqlDataReader reader)
        {
            return new Customer
            {
                customer_id = Convert.ToInt32(reader["customer_id"]),
                name = Convert.ToString(reader["name"]),
                email = Convert.ToString(reader["email"]),
                password = Convert.ToString(reader["password"]),
                numberOfVisits = Convert.ToInt32(reader["number_of_visits"]),
                street = Convert.ToString(reader["street"]),
                municipality = Convert.ToString(reader["municipality"]),
                zip = Convert.ToString(reader["zip"]),
                isVerified = Convert.ToBoolean(reader["isVerrified"])
            };
        }

    }
}
