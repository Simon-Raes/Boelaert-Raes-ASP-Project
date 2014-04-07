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
    public class OrderDAO : DAO
    {
        public List<Order> getAll()
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<Order> orderList = new List<Order>();

            SqlCommand command = new SqlCommand("SELECT * FROM Orders", cnn);
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    orderList.Add(createOrder(reader));
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
            return orderList;
        }

        public List<Order> getOrdersForCustomer(int customer_id)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<Order> orderList = new List<Order>();

            SqlCommand command = new SqlCommand("SELECT * FROM Orders WHERE customer_id = "+customer_id, cnn);
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    orderList.Add(createOrder(reader));
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
            return orderList;
        }

        /*Inserts new order for customer and returns the order_id*/
        public int addOrderForCustomer(int customer_id)
        {
            int orderID = -1;
            cnn = new SqlConnection(sDatabaseLocatie);

            //todo: paramaters (of ander beter systeem) gebruiken!

            SqlCommand command = new SqlCommand("INSERT INTO Orders" +
            "(orderstatus_id, customer_id, date) " +
            "OUTPUT INSERTED.order_id "+
            //add new order with status 1 (= new) and date of today
            "VALUES('" + 1 + "','" + customer_id + "'," + "convert(datetime,'" + DateTime.Today + "',103)"+")", cnn);

             
            try
            {
                cnn.Open();
                orderID = (int)command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //return -1 on error
                orderID = -1;
            }
            finally
            {
                cnn.Close();
            }
            return orderID;
        }

        /*Delete ALL data from this table*/
        public void clearTable()
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            SqlCommand command = new SqlCommand("DELETE FROM Orders", cnn);
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

        private Order createOrder(SqlDataReader reader)
        {
            Order order = new Order
            {
                order_id = Convert.ToInt32(reader["order_id"]),
                orderstatus_id = Convert.ToInt32(reader["orderstatus_id"]),
                customer_id = Convert.ToInt32(reader["customer_id"]),
                date = Convert.ToDateTime(reader["date"])
            };
            return order;
        }
    }
}