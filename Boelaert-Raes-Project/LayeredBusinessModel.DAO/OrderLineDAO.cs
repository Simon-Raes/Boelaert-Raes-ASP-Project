﻿using System;
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
        public OrderLine getOrderLine(String orderLineId)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                OrderLine orderLine = null;
                SqlCommand command = new SqlCommand("SELECT * FROM OrderLine WHERE orderline_id = @orderline_id", cnn);
                command.Parameters.Add(new SqlParameter("@orderline_id", orderLineId));

                try
                {
                    cnn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        orderLine = createOrderLine(reader);
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
                return orderLine;
            }
        }

        public Boolean removeOrderLine(OrderLine orderLine)
        {
            Boolean status = false;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {

                SqlCommand command = new SqlCommand("DELETE FROM OrderLine WHERE orderline_id = @orderline_id", cnn);
                command.Parameters.Add(new SqlParameter("@orderline_id", orderLine.orderline_id));

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
        }

        public List<OrderLine> getOrderLinesForOrder(Order order)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                List<OrderLine> orderList = new List<OrderLine>();

                SqlCommand command = new SqlCommand("SELECT orderline_id, order_id, OrderLine.order_line_type_id, OrderLine.dvd_info_id, dvd_copy_id, " +
                    "startdate, enddate, DvdInfo.name as dvd_info_name, OrderLineType.name as order_line_type_name FROM OrderLine " +
                    "JOIN DvdInfo " +
                    "ON DvdInfo.dvd_info_id = OrderLine.dvd_info_id " +
                    "JOIN OrderLineType " +
                    "ON OrderLineType.order_line_type_id = OrderLine.order_line_type_id " +
                    "WHERE order_id = @order_id", cnn);
                command.Parameters.Add(new SqlParameter("@order_id", order.order_id));

                try
                {
                    cnn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {                        
                        orderList.Add(createOrderLineWithNames(reader));
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
        }

        public List<OrderLine> getOrderLinesForCustomer(Customer customer)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                List<OrderLine> orderList = new List<OrderLine>();

                SqlCommand command = new SqlCommand("SELECT * FROM OrderLine " +
                    "INNER JOIN Orders " +
                    "ON OrderLine.order_id = Orders.order_id " +
                    "WHERE customer_id = @customer_id", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id", customer.customer_id));

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
        }

        public List<OrderLine> getActiveRentOrderLinesForCustomer(int customer_id)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                List<OrderLine> orderList = new List<OrderLine>();


                SqlCommand command = new SqlCommand("SELECT * FROM OrderLine " +

                    "JOIN Orders " +
                    "ON Orders.order_id = OrderLine.order_id " +
                    "WHERE customer_id = @customer_id AND " +
                    "OrderLine.order_line_type_id = 1 AND " +
                    "enddate > getdate() "
                    , cnn);

                command.Parameters.Add(new SqlParameter("@customer_id", customer_id));

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
        }




        /**Gets all rent copies that will be back in stock in the next 2 weeks*/
        public List<OrderLine> getOrderLinesForReservation(String order_id)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                List<OrderLine> orderList = new List<OrderLine>();

                //todo: exacte query
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
        }

        /**Adds a new orderline to the database*/
        public Boolean addOrderLine(OrderLine orderline)
        {
            Boolean didComplete = false;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {

                SqlCommand command = new SqlCommand("INSERT INTO OrderLine" +
                "(order_id, order_line_type_id, dvd_info_id, dvd_copy_id, startdate, enddate)" +
                "VALUES(@order_id, @order_line_type_id, @dvd_info_id, null, @startdate, @enddate)", cnn);

                command.Parameters.Add(new SqlParameter("@order_id", orderline.order.order_id));
                command.Parameters.Add(new SqlParameter("@order_line_type_id", orderline.orderLineType.id));
                command.Parameters.Add(new SqlParameter("@dvd_info_id", orderline.dvdInfo.dvd_info_id));
                //command.Parameters.Add(new SqlParameter("@dvd_copy_id", null)); //a copy is not yet assigned here, will only be added after a customer pays

                //TODO: betere oplossing voor dates
                if (orderline.startdate == DateTime.MinValue)
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
                    didComplete = true;
                }
                catch (Exception ex)
                {
                    didComplete = false;
                }
                finally
                {
                    cnn.Close();
                }
                return didComplete;
            }
        }

        /**Updates an existing orderline*/
        public Boolean updateOrderLine(OrderLine orderline)
        {
            Boolean didComplete = false;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {

                SqlCommand command = new SqlCommand("UPDATE OrderLine " +
                "SET order_line_type_id = @order_line_type_id, " +
                "dvd_info_id = @dvd_info_id, " +
                "dvd_copy_id = @dvd_copy_id, " +
                "startdate = @startdate, " +
                "enddate = @enddate " +
                "WHERE orderline_id = @orderline_id", cnn);

                command.Parameters.Add(new SqlParameter("@order_line_type_id", orderline.orderLineType.id));
                command.Parameters.Add(new SqlParameter("@dvd_info_id", orderline.dvdInfo.dvd_info_id));
                command.Parameters.Add(new SqlParameter("@dvd_copy_id", orderline.dvdCopy.dvd_copy_id));

                //TODO: betere oplossing voor dates
                if (orderline.startdate == DateTime.MinValue)
                {
                    orderline.startdate = (DateTime)SqlDateTime.Null;
                }
                if (orderline.enddate == DateTime.MinValue)
                {
                    orderline.enddate = (DateTime)SqlDateTime.Null;
                }

                command.Parameters.Add(new SqlParameter("@startdate", orderline.startdate));
                command.Parameters.Add(new SqlParameter("@enddate", orderline.enddate));
                command.Parameters.Add(new SqlParameter("@orderline_id", orderline.orderline_id));

                try
                {
                    cnn.Open();
                    command.ExecuteNonQuery();
                    didComplete = true;
                }
                catch (Exception ex)
                {
                    didComplete = false;
                }
                finally
                {
                    cnn.Close();
                }
                return didComplete;
            }
        }

        public List<OrderLine> getAllOrderlinesForDvdFromStartdate(DvdInfo dvd, DateTime startdate)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                List<OrderLine> orderList = new List<OrderLine>();

                SqlCommand command = new SqlCommand("select * from OrderLine where dvd_info_id = @dvd_info_id and order_line_type_id = 1 and (	(startdate <= @startdate and (enddate > @startdate and enddate < DATEADD(dd, 14, getdate())))	or (startdate >=  @startdate and enddate < DATEADD(dd,14, GETDATE()))	or (startdate >=  @startdate and startdate < DATEADD(dd,14,getdate()))	) AND dvd_copy_id IS NOT NULL order by dvd_copy_id, startdate, enddate", cnn);
                command.Parameters.Add(new SqlParameter("@dvd_info_id", dvd.dvd_info_id));
                command.Parameters.Add(new SqlParameter("@startdate", startdate));

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
        }


        


        /*Delete ALL data from this table (for dev use)*/
        public void clearTable()
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
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
        }

        private OrderLine createOrderLine(SqlDataReader reader)
        {
            OrderLine order;

            if (reader["dvd_copy_id"] == DBNull.Value)
            {
                order = new OrderLine
                {
                    orderline_id = Convert.ToInt32(reader["orderline_id"]),
                    order = new OrderDAO().getOrderWithId(reader["order_id"].ToString()),
                    orderLineType = new OrderLineTypeDAO().getOrderLineTypeForID(Convert.ToInt32(reader["order_line_type_id"])),
                    dvdInfo = new DvdInfoDAO().getDvdInfoWithId(reader["dvd_info_id"].ToString()),
                    startdate = Convert.ToDateTime(reader["startdate"]),
                    enddate = Convert.ToDateTime(reader["enddate"])
                };
            }
            else
            {
                order = new OrderLine
                {
                    orderline_id = Convert.ToInt32(reader["orderline_id"]),
                    order = new OrderDAO().getOrderWithId(reader["order_id"].ToString()),
                    orderLineType = new OrderLineTypeDAO().getOrderLineTypeForID(Convert.ToInt32(reader["order_line_type_id"])),
                    dvdCopy = new DvdCopyDAO().getCopyWithId(reader["dvd_copy_id"].ToString()),
                    dvdInfo = new DvdInfoDAO().getDvdInfoWithId(reader["dvd_info_id"].ToString()),
                    startdate = Convert.ToDateTime(reader["startdate"]),
                    enddate = Convert.ToDateTime(reader["enddate"])
                };
            }

            return order;
        }

        private OrderLine createOrderLineWithNames(SqlDataReader reader)
        {
            OrderLine order;

            if (reader["dvd_copy_id"] == DBNull.Value)
            {
                order = new OrderLine
                {
                    orderline_id = Convert.ToInt32(reader["orderline_id"]),
                    order = new OrderDAO().getOrderWithId(reader["order_id"].ToString()),
                    orderLineType = new OrderLineTypeDAO().getOrderLineTypeForID(Convert.ToInt32(reader["order_line_type_id"])),
                    dvdInfo = new DvdInfoDAO().getDvdInfoWithId(reader["dvd_info_id"].ToString()),
                    startdate = Convert.ToDateTime(reader["startdate"]),
                    enddate = Convert.ToDateTime(reader["enddate"])
                };
            }
            else
            {
                order = new OrderLine
                {
                    orderline_id = Convert.ToInt32(reader["orderline_id"]),
                    order = new OrderDAO().getOrderWithId(reader["order_id"].ToString()),
                    orderLineType = new OrderLineTypeDAO().getOrderLineTypeForID(Convert.ToInt32(reader["order_line_type_id"])),
                    dvdCopy = new DvdCopyDAO().getCopyWithId(reader["dvd_copy_id"].ToString()),
                    dvdInfo = new DvdInfoDAO().getDvdInfoWithId(reader["dvd_info_id"].ToString()),
                    startdate = Convert.ToDateTime(reader["startdate"]),
                    enddate = Convert.ToDateTime(reader["enddate"])
                };
            }

            return order;
        }


    }
}
