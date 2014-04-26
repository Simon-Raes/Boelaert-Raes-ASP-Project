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
    public class DvdCopyDAO : DAO
    {
        public List<DvdCopy> getAllCopiesForDvdInfo(String info_id)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<DvdCopy> dvdCopies = new List<DvdCopy>();

            SqlCommand command = new SqlCommand("SELECT * FROM DvdCopy WHERE dvd_info_id = "+info_id, cnn);
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dvdCopies.Add(createDvdCopy(reader));
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
            return dvdCopies;
        }

        public DvdCopy getCopyWithId(int copy_id)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            DvdCopy dvdCopy = new DvdCopy();

            SqlCommand command = new SqlCommand("SELECT * FROM DvdCopy WHERE dvd_copy_id = " + copy_id, cnn);
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dvdCopy = createDvdCopy(reader);
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
            return dvdCopy;
        }

        public List<DvdCopy> getAllInStockRentCopiesForDvdInfo(String info_id)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<DvdCopy> dvdCopies = new List<DvdCopy>();

            //is hier nog met hardcoded type_id
            SqlCommand command = new SqlCommand("SELECT * FROM DvdCopy WHERE dvd_info_id = " + info_id + " AND copy_type_id = 1 AND in_stock = 1;", cnn);
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dvdCopies.Add(createDvdCopy(reader));
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
            return dvdCopies;
        }

        public List<DvdCopy> getAllInStockBuyCopiesForDvdInfo(String info_id)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<DvdCopy> dvdCopies = new List<DvdCopy>();

            //is hier nog met hardcoded type_id
            SqlCommand command = new SqlCommand("SELECT * FROM DvdCopy WHERE dvd_info_id = " + info_id + " AND copy_type_id = 2 AND in_stock = 1;", cnn);
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dvdCopies.Add(createDvdCopy(reader));
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
            return dvdCopies;
        }

        public void updateDvdCopy(DvdCopy copy)
        {
            cnn = new SqlConnection(sDatabaseLocatie);

            //todo: alternatief om deze zielige code mee te vervangen

            SqlCommand command = new SqlCommand("UPDATE DvdCopy" +
            " SET dvd_info_id =" + copy.dvd_info_id +
            ", copy_type_id = " + copy.copy_type_id +
            ", serialnumber ='" + copy.serialnumber +
            "', note = '" + copy.note +
            "', in_stock = '" + copy.in_stock +
            "' WHERE dvd_copy_id =" + copy.dvd_copy_id + ";", cnn);
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

        //update methode voor wanneer je geen volledig object kan gebruiken
        public void updateDvdCopyInStockStatus(String dvdCopyID, Boolean in_stock)
        {
            cnn = new SqlConnection(sDatabaseLocatie);


            SqlCommand command = new SqlCommand("UPDATE DvdCopy" +
            " SET in_stock = '" + in_stock +
            "' WHERE dvd_copy_id =" + dvdCopyID + ";", cnn);
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

        /**Returns a list of all dvd copies that are available for the full 14 day period, starting today*/
        public List<DvdCopy> getAllFullyAvailableCopies(DvdInfo dvd, DateTime startdate)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<DvdCopy> orderList = new List<DvdCopy>();
            SqlCommand command = new SqlCommand(
            "select * from DvdCopy  " +
            "where DvdCopy.dvd_info_id = @dvd_info_id and  " +
            "DvdCopy.copy_type_id = 1 and "+
            "DvdCopy.dvd_copy_id not in " +
            "(select dvd_copy_id from OrderLine where dvd_copy_id is not null and (startdate >= @startdate or enddate >= @startdate)) ", cnn);
            command.Parameters.Add(new SqlParameter("@dvd_info_id", dvd.dvd_info_id));
            command.Parameters.Add(new SqlParameter("@startdate", startdate));

            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    orderList.Add(createDvdCopy(reader));
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

        /*Sets all copies back to in_stock = true*/
        public void resetAllCopies()
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            SqlCommand command = new SqlCommand("UPDATE DvdCopy SET in_stock = 1;", cnn);
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
