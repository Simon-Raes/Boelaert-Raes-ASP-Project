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
            cnn = new SqlConnection(sDatabaseLocatie);
            List<ShoppingcartItem> cartItems = new List<ShoppingcartItem>();



            /*Get all info for shoppinglist items, contains data from a lot of different database tables*/
            SqlCommand command = new SqlCommand("SELECT shoppingcart_item_id, customer_id, ShoppingcartItem.dvd_copy_id, DvdInfo.dvd_info_id, DvdCopy.copy_type_id, "+
            "DvdInfo.name, serialnumber, note, in_stock, startdate, enddate, DvdCopyType.name as typeName " +
            "FROM ShoppingcartItem " +
            "INNER JOIN DvdCopy " +
            "ON ShoppingcartItem.dvd_copy_id = DvdCopy.dvd_copy_id " +
            "INNER JOIN DvdInfo "+
            "ON DvdCopy.dvd_info_id = DvdInfo.dvd_info_id "+
            "INNER JOIN DvdCopyType "+
            "ON DvdCopy.copy_type_id = DvdCopyType.copy_type_id " +
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

        public Boolean addItemToCart(int customerID, int dvdCopyID)
        {
            Boolean status = false;
            cnn = new SqlConnection(sDatabaseLocatie);

            //todo: paramaters (of ander beter systeem) gebruiken!

            SqlCommand command = new SqlCommand("INSERT INTO shoppingcartItem" +
            "(customer_id,dvd_copy_id)" +
            "VALUES(" + customerID + "," + dvdCopyID + ")", cnn);
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

        //overloaded method for adding rent items with dates
        public Boolean addItemToCart(int customerID, int dvdCopyID, DateTime startdate, DateTime enddate)
        {
            Boolean status = false;
            cnn = new SqlConnection(sDatabaseLocatie);

            //todo: paramaters (of ander beter systeem) gebruiken!

            SqlCommand command = new SqlCommand("INSERT INTO shoppingcartItem" +
            "(customer_id,dvd_copy_id, startdate, enddate)" +
            "VALUES(" + customerID + "," + dvdCopyID + ",convert(datetime,'" + startdate + "',103),convert(datetime,'" + enddate + "',103))", cnn);
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


        public Boolean removeItemFromCart(String dvdCopyID)
        {
            Boolean status = false;
            cnn = new SqlConnection(sDatabaseLocatie);

            //todo: paramaters (of ander beter systeem) gebruiken!

            SqlCommand command = new SqlCommand("DELETE FROM ShoppingcartItem WHERE dvd_copy_id = " + dvdCopyID, cnn);
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



        public int getCartContentCountForCustomer(int id)
        {
            //stub
            return 0;
        }

        private ShoppingcartItem createShoppingcartItem(SqlDataReader reader)
        {
            //minvalue omdat een null niet toegelaten is, andere klassen zullen dus op mindate moeten controleren
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
                customer_id = Convert.ToInt32(reader[1]),
                dvd_copy_id = Convert.ToInt32(reader[2]),                
                startdate = startdate,
                enddate = enddate,
                name = Convert.ToString(reader["name"]),
                typeName = Convert.ToString(reader["typeName"])
            };


            return cartItem;
        }

    }
}
