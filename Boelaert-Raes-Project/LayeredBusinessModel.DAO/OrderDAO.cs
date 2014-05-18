using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using LayeredBusinessModel.Domain;
using System.Configuration;
using CustomException;

namespace LayeredBusinessModel.DAO
{
    public class OrderDAO : DAO
    {
        /*
         * Returns an order based on an ID
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public Order getByID(String id)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT order_id, customer_id, Orders.orderstatus_id, date, Orderstatus.name FROM Orders " +
                    "Join Orderstatus " +
                    "ON Orderstatus.orderstatus_id = Orders.orderstatus_id " +
                    "WHERE order_id = @order_id", cnn);
                command.Parameters.Add(new SqlParameter("@order_id", id));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return createOrder(reader);         //Throws NoRecordException || DALException  
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get an order based on an ID", ex);                    
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
                throw new NoRecordException("No records were found - OrderDAO getOrderWithId()");
            }
        }

        /*
         * Returns a list with orders for a customer
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<Order> getByCustomer(Customer customer)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT order_id, customer_id, Orders.orderstatus_id, date, Orderstatus.name FROM Orders " +
                    "Join Orderstatus " +
                    "ON Orderstatus.orderstatus_id = Orders.orderstatus_id " +
                    "WHERE customer_id = @customer_id", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id", customer.customer_id));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<Order> orderList = new List<Order>();
                        while (reader.Read())
                        {
                            orderList.Add(createOrder(reader));         //Throws NoRecordException || DALException  
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
                    throw new DALException("Failed to get the order for a customer", ex);
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
                throw new NoRecordException("No records were found - OrderDAO getOrdersForCustomer()");
            }
        }

        /*
         * Updates an order
         * Returns true if the order was updated, false if no order was updated
         * Throws DALException if something else went wrong
         */
        public Boolean update(Order order)
        {
            SqlCommand command = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("UPDATE Orders " +
                "SET customer_id = @customer_id, " +
                "orderstatus_id = @orderstatus_id, " +
                "date = @date " +
                "WHERE order_id=@order_id", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id", order.customer.customer_id));
                command.Parameters.Add(new SqlParameter("@orderstatus_id", order.orderstatus.id));
                command.Parameters.Add(new SqlParameter("@date", order.date));
                command.Parameters.Add(new SqlParameter("@order_id", order.order_id));

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
                    throw new DALException("Failed to update order", ex);
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
         * Inserts an order for a customer
         * Returns the ID from the newly added order
         * Throws DALException if something else went wrong
         */
        public int add(Customer customer)
        {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("INSERT INTO Orders" +
                "(orderstatus_id, customer_id, date) " +
                "OUTPUT INSERTED.order_id " +                
                "VALUES(@orderstatus_id, @customer_id, @date)", cnn);
                command.Parameters.Add(new SqlParameter("@orderstatus_id", 1));
                command.Parameters.Add(new SqlParameter("@customer_id", customer.customer_id));
                command.Parameters.Add(new SqlParameter("@date", DateTime.Today));

                try
                {
                    cnn.Open();
                    return (int)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to insert order for customer", ex);
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

        /*Removes the order from the database, will only work if it has no orderLines left*/
        public Boolean remove(Order order)
        {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("DELETE FROM Orders WHERE order_id = @order_id", cnn);
                command.Parameters.Add(new SqlParameter("@order_id", order.order_id))
                    ;
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
                    throw new DALException("Failed to delete the order", ex);
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
         * Deletes all the orders
         * Returns true if orders where deleted, false if no orders were deleted
         * Throws DALException if something else went wrong
         */
        public Boolean deleteAll()
        {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("DELETE FROM Orders", cnn);
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
                    throw new DALException("Failed to delete all orders", ex);                    
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
         * Creates an Order-object
         */ 
        private Order createOrder(SqlDataReader reader)
        {
            return new Order
            {
                order_id = Convert.ToInt32(reader["order_id"]),
                orderstatus = new OrderStatusDAO().getByID(reader["orderstatus_id"].ToString()),            //Throws NoRecordException
                customer = new CustomerDAO().getByID(reader["customer_id"].ToString()),         //Throws NoRecordException || DALException         
                date = Convert.ToDateTime(reader["date"])
            };
        }
        
        //public List<Order> getAll()
        //{
        //    cnn = new SqlConnection(sDatabaseLocatie);
        //    List<Order> orderList = new List<Order>();

        //    SqlCommand command = new SqlCommand("SELECT * FROM Orders", cnn);
        //    try
        //    {
        //        cnn.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            orderList.Add(createOrder(reader));
        //        }

        //        reader.Close();

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        cnn.Close();
        //    }
        //    return orderList;
        //}
    }
}
