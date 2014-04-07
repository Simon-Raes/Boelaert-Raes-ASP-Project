using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using LayeredBusinessModel.Domain;
using System.Configuration;

using System.Data.SqlTypes;

namespace LayeredBusinessModel.DAO
{
    public class OrderLineDAO : DAO
    {
        public List<OrderLine> getOrderLinesForOrder(int order_id)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<OrderLine> orderList = new List<OrderLine>();

            SqlCommand command = new SqlCommand("SELECT * FROM OrderLine WHERE order_id = @order_id", cnn);
            command.Parameters.Add(new SqlParameter("@order_id", order_id));

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

        /**Adds an orderline to the database*/
        public Boolean addOrderLine(OrderLine orderline)
        {
            Boolean status = false;
            cnn = new SqlConnection(sDatabaseLocatie);

            //todo: paramaters (of ander beter systeem) gebruiken!

            SqlCommand command = new SqlCommand("INSERT INTO OrderLine" +
            "(order_id, order_line_type_id, dvd_copy_id, startdate, enddate)" +
            "VALUES(@order_id, @order_line_type_id, @dvd_copy_id, @startdate, @enddate)", cnn);
            
            command.Parameters.Add(new SqlParameter("@order_id", orderline.order_id));
            command.Parameters.Add(new SqlParameter("@order_line_type_id", orderline.order_line_type_id));
            command.Parameters.Add(new SqlParameter("@dvd_copy_id", orderline.dvd_copy_id));

            //TODO: betere oplossing voor dates
            if(orderline.startdate==DateTime.MinValue)
            {
                orderline.startdate = (DateTime)SqlDateTime.Null;
            }
            if (orderline.enddate == DateTime.MinValue)
            {
                orderline.enddate = (DateTime)SqlDateTime.Null;
            }
            command.Parameters.Add(new SqlParameter("@startdate", orderline.startdate));
            command.Parameters.Add(new SqlParameter("@enddate", orderline.enddate));

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
