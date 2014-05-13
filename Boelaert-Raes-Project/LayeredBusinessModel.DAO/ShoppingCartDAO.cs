﻿using System;
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
    public class ShoppingCartDAO : DAO
    {
        /*
         * Returns a list with shoppingcartitems for a customer
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<ShoppingcartItem> getCartContentForCustomer(Customer customer)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                /*Get all info for shoppinglist items, contains data from a lot of different database tables*/
                command = new SqlCommand("SELECT shoppingcart_item_id, customer_id, ShoppingcartItem.dvd_info_id, ShoppingcartItem.copy_type_id, " +
                "DvdInfo.name, startdate, enddate, DvdCopyType.name as typeName " +
                "FROM ShoppingcartItem " +
                "INNER JOIN DvdInfo " +
                "ON ShoppingcartItem.dvd_info_id = DvdInfo.dvd_info_id " +
                "INNER JOIN DvdCopyType " +
                "ON ShoppingcartItem.copy_type_id = DvdCopyType.copy_type_id " +
                "WHERE customer_id = @customer_id", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id", customer.customer_id));
                
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<ShoppingcartItem> cartItems = new List<ShoppingcartItem>();
                        while (reader.Read())
                        {
                            cartItems.Add(createShoppingcartItem(reader));
                        }
                        return cartItems;
                    }
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to get a shoppingcaritems for customer", ex);
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
                throw new NoRecordException("No records were found - ShoppingCartDAO getCartContentForCustomer()");
            }
        }

        /*
         * Adds a buy item for customer
         * Returns true if records were inserted, false if not
         * Throws DALException if something else went wrong
         */
        public Boolean addItemToCart(Customer customer, DvdInfo dvdInfo)
        {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("INSERT INTO shoppingcartItem" +
                "(customer_id, dvd_info_id, copy_type_id)" +
                "VALUES(@customer_id, @dvd_info_id, 2)", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id", customer.customer_id));
                command.Parameters.Add(new SqlParameter("@dvd_info_id", dvdInfo.dvd_info_id));

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
                    throw new DALException("Failed to add buy item for customer", ex);
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
         * Adds a rent item for customer
         * Returns true if records were inserted, false if not
         * Throws DALException if something else went wrong
         */
        public Boolean addItemToCart(Customer customer, String dvdInfoID, DateTime startdate, DateTime enddate)
        {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("INSERT INTO shoppingcartItem" +
                "(customer_id, dvd_info_id, startdate, enddate, copy_type_id)" +
                "VALUES(@customer_id, @dvd_info_id, @startdate, @enddate, 1)", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id", customer.customer_id));
                command.Parameters.Add(new SqlParameter("@dvd_info_id", dvdInfoID));
                command.Parameters.Add(new SqlParameter("@startdate", startdate));
                command.Parameters.Add(new SqlParameter("@enddate", enddate));

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
                    throw new DALException("Failed to add rent item for customer", ex);
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
         * Removes a shoppingcartitem based on an ID
         * Returns true if records were deleted, false if not
         * Throws DALException if something else went wrong
         */
        public Boolean removeItemFromCart(String cartItemID)
        {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("DELETE FROM ShoppingcartItem WHERE shoppingcart_item_id = @shoppingcart_item_id", cnn);
                command.Parameters.Add(new SqlParameter("@shoppingcart_item_id", cartItemID));

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
                    throw new DALException("Failed to delete shoppingcartitem based on ID", ex);
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
        * Removes all shoppingcartitems for customer
        * Returns true if records were deleted, false if not
        * Throws DALException if something else went wrong
        */
        public Boolean clearCustomerCart(Customer customer)
        {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("DELETE FROM ShoppingcartItem WHERE customer_id = @customer_id", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id", customer.customer_id));

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
                    throw new DALException("Failed to delete all shoppingcartitems for customer", ex);
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
        * Removes all shoppingcartitems
        * Returns true if records were deleted, false if not
        * Throws DALException if something else went wrong
        */
        public Boolean clearTable()
        {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("DELETE FROM ShoppingcartItem", cnn);
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
                    throw new DALException("Failed to delete all shoppingcartitems", ex);
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


        /*todo: COUNT items in cart*/
        public int getCartContentCountForCustomer(String id)
        {
            //stub
            return 0;
        }

        private ShoppingcartItem createShoppingcartItem(SqlDataReader reader)
        {
            //date.minvalue omdat een null niet toegelaten is bij datetime, andere klassen zullen dus op mindate moeten controleren
            DateTime startdate = DateTime.MinValue;
            DateTime enddate = DateTime.MinValue;

            if (reader["startdate"] != DBNull.Value)
            {
                startdate = Convert.ToDateTime(reader["startdate"]);
            }
            if (reader["enddate"] != DBNull.Value)
            {
                enddate = Convert.ToDateTime(reader["enddate"]);
            }

            //hier krijg ik een exception als ik "shoppingcart_item_id" gebruik, columnnummer (0) werkt wel zonder problemen
            return new ShoppingcartItem
            {
                shoppingcart_item_id = Convert.ToInt32(reader[0]),
                customer = new CustomerDAO().getByID(reader[1].ToString()),
                dvdInfo = new DvdInfoDAO().getDvdInfoWithId(reader[2].ToString()),   
                dvdCopyType = new DvdCopyTypeDAO().getTypeForID(reader[3].ToString()),
                startdate = startdate,
                enddate = enddate
                //name = Convert.ToString(reader["name"]),
                //typeName = Convert.ToString(reader["typeName"])
            };
        }

    }
}
