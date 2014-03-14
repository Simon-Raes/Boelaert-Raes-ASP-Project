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
    public class OrderDAO : DAO
    {
        public List<Order> getAll()
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<Order> orderList = new List<Order>();

            SqlCommand command = new SqlCommand("SELECT * FROM Order", cnn);
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

            SqlCommand command = new SqlCommand("SELECT * FROM Order WHERE customer_id = "+customer_id, cnn);
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


        public Boolean addOrderForCustomer(int customer_id)
        {
            Boolean status = false;
            cnn = new SqlConnection(sDatabaseLocatie);

            //todo: paramaters (of ander beter systeem) gebruiken!

            SqlCommand command = new SqlCommand("INSERT INTO Order" +
            "(orderstatus_id, customer_id)" +
            //add new order with status 0 (= new?)
            "VALUES('" + 0 + "','" + customer_id + "')", cnn);
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

        private Order createOrder(SqlDataReader reader)
        {
            Order order = new Order
            {
                order_id = Convert.ToInt32(reader["order_id"]),
                orderstatus_id = Convert.ToInt32(reader["orderstatus_id"]),
                customer_id = Convert.ToInt32(reader["customer_id"])
            };
            return order;
        }
    }
}
