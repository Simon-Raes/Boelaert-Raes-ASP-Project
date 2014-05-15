using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using LayeredBusinessModel.Domain;
using System.Configuration;

using System.Data.SqlTypes;
using CustomException;

namespace LayeredBusinessModel.DAO
{
    public class OrderLineDAO : DAO
    {
        /*
         * Returns an orderline based on an ID
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public OrderLine getByID(String orderLineId)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM OrderLine WHERE orderline_id = @orderline_id", cnn);
                command.Parameters.Add(new SqlParameter("@orderline_id", orderLineId));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return createOrderLine(reader);                 //Throws NoRecordException || DALException
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get an orderline based on an ID", ex);
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
                throw new NoRecordException("No records were found - OrderLineDAO getOrderLineById()");
            }
        }

        /*
         * Returns a list with orderline from a certain order
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<OrderLine> getByOrder(Order order)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT orderline_id, order_id, OrderLine.order_line_type_id, OrderLine.dvd_info_id, dvd_copy_id, " +
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
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<OrderLine> orderList = new List<OrderLine>();
                        while (reader.Read())
                        {
                            orderList.Add(createOrderLineWithNames(reader));
                        }
                        return orderList;
                    }
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to get the orderlines for a certain order", ex);
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
                throw new NoRecordException("No records were found - OrderLineDAO getOrderLinesForOrder()");
            }
        }

        /*
         * Returns a list with orderlines from a certain customer
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<OrderLine> getByCustomer(Customer customer)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM OrderLine " +
                    "INNER JOIN Orders " +
                    "ON OrderLine.order_id = Orders.order_id " +
                    "WHERE customer_id = @customer_id", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id", customer.customer_id));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<OrderLine> orderList = new List<OrderLine>();
                        while (reader.Read())
                        {
                            orderList.Add(createOrderLine(reader));         //Throws NoRecordException || DALException
                        }
                        return orderList;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get the orderlines for a certain customer", ex);
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
                throw new NoRecordException("No records were found - OrderLineDAO getOrderLinesForCustomer()");
            }
        }

        /*
         * Returns a list with all the orderlines from a certain startdate
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<OrderLine> getAllByDvdAndStartdate(DvdInfo dvd, DateTime startdate)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("select * from OrderLine where dvd_info_id = @dvd_info_id and order_line_type_id = 1 and (	(startdate <= @startdate and (enddate > @startdate and enddate < DATEADD(dd, 14, getdate())))	or (startdate >=  @startdate and enddate < DATEADD(dd,14, GETDATE()))	or (startdate >=  @startdate and startdate < DATEADD(dd,14,getdate()))	) AND dvd_copy_id IS NOT NULL order by dvd_copy_id, startdate, enddate", cnn);
                command.Parameters.Add(new SqlParameter("@dvd_info_id", dvd.dvd_info_id));
                command.Parameters.Add(new SqlParameter("@startdate", startdate));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<OrderLine> orderList = new List<OrderLine>();
                        while (reader.Read())
                        {
                            orderList.Add(createOrderLine(reader));             //Throws NoRecordException || DALException
                        }
                        return orderList;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get orderlines from a certain startdate", ex);
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
                throw new NoRecordException("No records were found - OrderLineDAO getAllOrderlinesForDvdFromStartdate()");
            }
        }
        
        /*
         * Returns a list with active rent orderlines for a customer
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<OrderLine> getActiveRentOrderLinesByCustomer(Customer customer)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM OrderLine " +
                    "JOIN Orders " +
                    "ON Orders.order_id = OrderLine.order_id " +
                    "WHERE customer_id = @customer_id AND " +
                    "OrderLine.order_line_type_id = 1 AND " +
                    "enddate > getdate() "
                    , cnn);
                command.Parameters.Add(new SqlParameter("@customer_id", customer.customer_id));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<OrderLine> orderList = new List<OrderLine>();
                        while (reader.Read())
                        {
                            orderList.Add(createOrderLine(reader));                 //Throws NoRecordException || DALException
                        }
                        return orderList;
                    }                    
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get the active rent orderlines for a customer", ex);
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
                throw new NoRecordException("No records were found - OrderLineDAO getOrderLinesForCustomer()");
            }
        }

        /*
         * Adds an orderline 
         * Returns true if the orderline was inserted, false if no orderline was inserted
         * Throws DALException if something else went wrong
         */
        public Boolean add(OrderLine orderline)
        {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("INSERT INTO OrderLine" +
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
                    if (command.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to add the orderline", ex);
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
         * Removes an ordeline
         * Returns true if orderline was deleted, false if no orderline was deleted
         * Throws DALException if something else went wrong
         */
        public Boolean delete(OrderLine orderLine)
        {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("DELETE FROM OrderLine WHERE orderline_id = @orderline_id", cnn);
                command.Parameters.Add(new SqlParameter("@orderline_id", orderLine.orderline_id));

                try
                {
                    cnn.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to delete orderline", ex);
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
         * Deletes all the orderlines
         * Returns true if ordelines were deleted, false if no orderline were deleted
         * Throws DALException if something else went wrong
         */
        public Boolean deleteAll()
        {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("DELETE FROM OrderLine", cnn);
                try
                {
                    cnn.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to delete orderlines", ex);
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
         * Creates and Ordeline-Object
         */ 
        private OrderLine createOrderLine(SqlDataReader reader)
        {
            if (reader["dvd_copy_id"] == DBNull.Value)
            {
                return new OrderLine
                {
                    orderline_id = Convert.ToInt32(reader["orderline_id"]),
                    order = new OrderDAO().getByID(reader["order_id"].ToString()),                                      //Throws NoRecordException
                    orderLineType = new OrderLineTypeDAO().getByID(reader["order_line_type_id"].ToString()),            //Throws NoRecordException
                    dvdInfo = new DvdInfoDAO().getByID(reader["dvd_info_id"].ToString()),                               //Throws NoRecordException
                    startdate = Convert.ToDateTime(reader["startdate"]),
                    enddate = Convert.ToDateTime(reader["enddate"])
                };
            }
            else
            {
                return new OrderLine
                {
                    orderline_id = Convert.ToInt32(reader["orderline_id"]),
                    order = new OrderDAO().getByID(reader["order_id"].ToString()),          //Throws NoRecordException
                    orderLineType = new OrderLineTypeDAO().getByID(reader["order_line_type_id"].ToString()),            //Throws NoRecordException
                    dvdCopy = new DvdCopyDAO().getByID(reader["dvd_copy_id"].ToString()),               //Throws NoRecordException || DALException
                    dvdInfo = new DvdInfoDAO().getByID(reader["dvd_info_id"].ToString()),               //Throws NoRecordException
                    startdate = Convert.ToDateTime(reader["startdate"]),
                    enddate = Convert.ToDateTime(reader["enddate"])
                };
            }
        }

        /*
         * Creates and Ordeline-Object with names
         */ 
        private OrderLine createOrderLineWithNames(SqlDataReader reader)
        {
            if (reader["dvd_copy_id"] == DBNull.Value)
            {
                return new OrderLine
                {
                    orderline_id = Convert.ToInt32(reader["orderline_id"]),
                    order = new OrderDAO().getByID(reader["order_id"].ToString()),          //Throws NoRecordException
                    orderLineType = new OrderLineTypeDAO().getByID(reader["order_line_type_id"].ToString()),            //Throws NoRecordException
                    dvdInfo = new DvdInfoDAO().getByID(reader["dvd_info_id"].ToString()),               //Throws NoRecordException
                    startdate = Convert.ToDateTime(reader["startdate"]),
                    enddate = Convert.ToDateTime(reader["enddate"])
                };
            }
            else
            {
                return new OrderLine
                {
                    orderline_id = Convert.ToInt32(reader["orderline_id"]),
                    order = new OrderDAO().getByID(reader["order_id"].ToString()),          //Throws NoRecordException
                    orderLineType = new OrderLineTypeDAO().getByID(reader["order_line_type_id"].ToString()),            //Throws NoRecordException
                    dvdCopy = new DvdCopyDAO().getByID(reader["dvd_copy_id"].ToString()),               //Throws NoRecordException || DALException
                    dvdInfo = new DvdInfoDAO().getByID(reader["dvd_info_id"].ToString()),               //Throws NoRecordException
                    startdate = Convert.ToDateTime(reader["startdate"]),
                    enddate = Convert.ToDateTime(reader["enddate"])
                };
            }
        }

        /*
         * Updates an orderline 
         * Returns true if the orderline was updated, false if no orderline was updates
         * Throws DALException if something else went wrong
         */
        /*
        public Boolean update(OrderLine orderline)
        {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("UPDATE OrderLine " +
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
                    if (command.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to update orderline", ex);
                }
                finally
                {
                    if (cnn != null)
                    {
                        cnn.Close();
                    }
                }
            }
        }*/

        //public List<OrderLine> getOrderLinesForOrder(String order_id)
        //{
        //    using (var cnn = new SqlConnection(sDatabaseLocatie))
        //    {
        //        List<OrderLine> orderList = new List<OrderLine>();

        //        //todo: exacte query
        //        SqlCommand command = new SqlCommand("SELECT * FROM OrderLine WHERE order_id = @order_id", cnn);
        //        command.Parameters.Add(new SqlParameter("@order_id", order_id));

        //        try
        //        {
        //            cnn.Open();
        //            SqlDataReader reader = command.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                orderList.Add(createOrderLine(reader));
        //            }

        //            reader.Close();

        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        finally
        //        {
        //            cnn.Close();
        //        }
        //        return orderList;
        //    }
        //}
    }
}
