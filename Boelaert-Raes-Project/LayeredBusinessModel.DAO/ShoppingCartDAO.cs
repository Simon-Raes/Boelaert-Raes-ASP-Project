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
    public class ShoppingCartDAO
    {
        private string strSQL;
        private string sDatabaseLocatie = ConfigurationManager.ConnectionStrings["ProjectConnection"].ConnectionString;
        private SqlConnection cnn;

        public List<DvdCopy> getCartContentForCustomer(int id)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<DvdCopy> dvdCopiesInCart = new List<DvdCopy>();


            //SqlCommand command = new SqlCommand("SELECT * FROM ShoppingcartItem WHERE customer_id = " + id + ";", cnn);

            //todo: extra join op dvd_info voor titel, etc
            SqlCommand command = new SqlCommand("SELECT ShoppingcartItem.dvd_copy_id, dvd_info_id, copy_type_id, serialnumber, note, in_stock, startdate, enddate " +
            "FROM ShoppingcartItem " +
            "INNER JOIN DvdCopy " +
            "ON ShoppingcartItem.dvd_copy_id = DvdCopy.dvd_copy_id " +
            "WHERE customer_id = " + id, cnn);

            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dvdCopiesInCart.Add(createDvdCopy(reader));
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                int test = 0;
            }
            finally
            {
                cnn.Close();
            }
            return dvdCopiesInCart;
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

        private DvdCopy createDvdCopy(SqlDataReader reader)
        {
            DvdCopy dvdCopy = new DvdCopy
            {
                dvd_copy_id = Convert.ToInt32(reader["dvd_copy_id"]),
                dvd_info_id = Convert.ToInt32(reader["dvd_info_id"]),
                copy_type_id = Convert.ToInt32(reader["copy_type_id"]),
                serialnumber = Convert.ToString(reader["serialnumber"]),
                note = Convert.ToString(reader["note"]),
                in_stock = Convert.ToBoolean(reader["in_stock"])
            };

            return dvdCopy;
        }

    }
}
