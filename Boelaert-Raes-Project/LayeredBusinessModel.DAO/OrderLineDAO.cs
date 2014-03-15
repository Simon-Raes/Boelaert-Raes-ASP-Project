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
    public class OrderLineDAO : DAO
    {
        public List<OrderLine> getOrderLinesForOrder(int order_id)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<OrderLine> orderList = new List<OrderLine>();

            SqlCommand command = new SqlCommand("SELECT * FROM OrderLine WHERE order_id = "+order_id, cnn);
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    orderList.Add(createOrderLine(reader));
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

        public Boolean addOrderLine(OrderLine orderline)
        {
            Boolean status = false;
            cnn = new SqlConnection(sDatabaseLocatie);

            //todo: paramaters (of ander beter systeem) gebruiken!

            SqlCommand command = new SqlCommand("INSERT INTO OrderLine" +
            "(order_id, order_line_type_id, dvd_copy_id, startdate, enddate)" +
            "VALUES('" + orderline.order_id + "','" + orderline.order_line_type_id + "','" + orderline.dvd_copy_id + "', " + "convert(datetime,'" + orderline.startdate + "',103), convert(datetime,'" + orderline.enddate + "',103))", cnn);
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


        /*Delete ALL data from this table*/
        public void clearTable()
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            SqlCommand command = new SqlCommand("DELETE FROM OrderLine", cnn);
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

        private OrderLine createOrderLine(SqlDataReader reader)
        {
            OrderLine order = new OrderLine
            {
                orderline_id = Convert.ToInt32(reader["orderline_id"]),
                order_id = Convert.ToInt32(reader["order_id"]),
                order_line_type_id = Convert.ToInt32(reader["order_line_type_id"]),
                dvd_copy_id = Convert.ToInt32(reader["dvd_copy_id"]),
                startdate = Convert.ToDateTime(reader["startdate"]),
                enddate = Convert.ToDateTime(reader["enddate"])
            };
            return order;
        }
    }
}
