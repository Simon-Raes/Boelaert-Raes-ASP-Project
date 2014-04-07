﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using LayeredBusinessModel.Domain;
using System.Configuration;

namespace LayeredBusinessModel.DAO
{
    public class CustomerDAO : DAO
    {
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

        public Customer getCustomerWithEmail(string email)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            Customer customer = null;

            SqlCommand command = new SqlCommand("SELECT * FROM Customers WHERE email = @email", cnn);
            command.Parameters.Add(new SqlParameter("@email", email));

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

        public void updateCustomer(Customer customer)
        {
            cnn = new SqlConnection(sDatabaseLocatie);

            SqlCommand command = new SqlCommand("UPDATE Customers " +
            "SET name=@name, " +
            "email=@email, " +
            "password=@password, " +
            "number_of_visits=@number_of_visits " +
            "WHERE login=@login" + customer.login + "';", cnn);

            command.Parameters.Add(new SqlParameter("@name", customer.name));
            command.Parameters.Add(new SqlParameter("@email", customer.email));
            command.Parameters.Add(new SqlParameter("@password", customer.password));
            command.Parameters.Add(new SqlParameter("@number_of_visits", customer.numberOfVisits));
            command.Parameters.Add(new SqlParameter("@login", customer.login));

            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                reader.Close();

            }
            catch (Exception ex)
            {
            }
            finally
            {
                cnn.Close();
            }
        }

        public Boolean addCustomer(Customer customer)
        {
            Boolean status = false;
            cnn = new SqlConnection(sDatabaseLocatie);

            //todo: paramaters (of ander beter systeem) gebruiken!

            SqlCommand command = new SqlCommand("INSERT INTO Customers" +
            "(name,email,login,password,number_of_visits)" +
            "VALUES('" + customer.name + "','" + customer.email + "','" + customer.login + "','" + customer.password + "'," + "0)", cnn);
            try
            {
                cnn.Open();
                command.ExecuteNonQuery();
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
            }
            finally
            {
                cnn.Close();
            }
            return status;
        }

        private Customer createCustomer(SqlDataReader reader)
        {
            //gebruik van object initializer ipv constructor
            Customer customer = new Customer
            {
                customer_id = Convert.ToInt32(reader["customer_id"]),
                name = Convert.ToString(reader["name"]),
                email = Convert.ToString(reader["email"]),
                login = Convert.ToString(reader["login"]),
                password = Convert.ToString(reader["password"]),
                numberOfVisits = Convert.ToInt32(reader["number_of_visits"])
            };
            return customer;
        }




    }


}
