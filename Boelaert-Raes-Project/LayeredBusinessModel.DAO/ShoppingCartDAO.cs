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
    public class ShoppingCartDAO : DAO
    {
        public List<ShoppingcartItem> getCartContentForCustomer(int id)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                List<ShoppingcartItem> cartItems = new List<ShoppingcartItem>();


                /*Get all info for shoppinglist items, contains data from a lot of different database tables*/
                SqlCommand command = new SqlCommand("SELECT shoppingcart_item_id, customer_id, ShoppingcartItem.dvd_info_id, ShoppingcartItem.copy_type_id, " +
                "DvdInfo.name, startdate, enddate, DvdCopyType.name as typeName " +
                "FROM ShoppingcartItem " +
                "INNER JOIN DvdInfo " +
                "ON ShoppingcartItem.dvd_info_id = DvdInfo.dvd_info_id " +
                "INNER JOIN DvdCopyType " +
                "ON ShoppingcartItem.copy_type_id = DvdCopyType.copy_type_id " +
                "WHERE customer_id = " + id, cnn);

                try
                {
                    cnn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        cartItems.Add(createShoppingcartItem(reader));
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
                return cartItems;
            }
        }

        /**Adds BUY dvd to cart*/
        public Boolean addItemToCart(int customerID, int dvdInfoID)
        {
            Boolean status = false;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {

                //todo: paramaters (of ander beter systeem) gebruiken!

                SqlCommand command = new SqlCommand("INSERT INTO shoppingcartItem" +
                "(customer_id, dvd_info_id, copy_type_id)" +
                "VALUES(@customer_id, @dvd_info_id, 2)", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id", customerID));
                command.Parameters.Add(new SqlParameter("@dvd_info_id", dvdInfoID));

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

        /**Adds RENT dvd to cart*/
        public Boolean addItemToCart(int customerID, int dvdInfoID, DateTime startdate, DateTime enddate)
        {
            Boolean status = false;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {

                SqlCommand command = new SqlCommand("INSERT INTO shoppingcartItem" +
                "(customer_id, dvd_info_id, startdate, enddate, copy_type_id)" +
                "VALUES(@customer_id, @dvd_info_id, convert(datetime,'" + startdate + "',103), convert(datetime,'" + enddate + "',103), 1)", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id", customerID));
                command.Parameters.Add(new SqlParameter("@dvd_info_id", dvdInfoID));
                //command.Parameters.Add(new SqlParameter("@startdate", "convert(datetime,'" + startdate + "',103)"));
                //command.Parameters.Add(new SqlParameter("@enddate", "convert(datetime,'" + enddate + "',103)"));

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


        public Boolean removeItemFromCart(String cartItemID)
        {
            Boolean status = false;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {

                SqlCommand command = new SqlCommand("DELETE FROM ShoppingcartItem WHERE shoppingcart_item_id = @shoppingcart_item_id", cnn);
                command.Parameters.Add(new SqlParameter("@shoppingcart_item_id", cartItemID));


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

        /*Delete all items from this user's shoppingcart*/
        public void clearCustomerCart(int customer_id)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {


                SqlCommand command = new SqlCommand("DELETE FROM ShoppingcartItem WHERE customer_id = @customer_id", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id", customer_id));

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

        /*Delete ALL data from this table*/
        public void clearTable()
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                SqlCommand command = new SqlCommand("DELETE FROM ShoppingcartItem", cnn);
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


        /*todo, COUNT items in cart*/
        public int getCartContentCountForCustomer(int id)
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
            ShoppingcartItem cartItem = new ShoppingcartItem
            {
                shoppingcart_item_id = Convert.ToInt32(reader[0]),
                customer = new CustomerDAO().getCustomerByID(Convert.ToInt32(reader[1])),
                dvdInfo = new DvdInfoDAO().getDvdInfoWithId(reader[2].ToString()),   
                dvdCopyType = new DvdCopyTypeDAO().getTypeForID(Convert.ToInt32(reader[3])),
                startdate = startdate,
                enddate = enddate
                //name = Convert.ToString(reader["name"]),
                //typeName = Convert.ToString(reader["typeName"])
            };


            return cartItem;
        }

    }
}
