﻿using System;
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
        public List<DvdCopy> getAllCopiesForDvdInfo(DvdInfo dvdInfo)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                List<DvdCopy> dvdCopies = new List<DvdCopy>();

                SqlCommand command = new SqlCommand("SELECT * FROM DvdCopy WHERE dvd_info_id = @dvd_info_id", cnn);
                command.Parameters.Add(new SqlParameter("@dvd_info_id", dvdInfo.dvd_info_id));
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
        }

        public DvdCopy getCopyWithId(String copy_id)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                DvdCopy dvdCopy = new DvdCopy();

                SqlCommand command = new SqlCommand("SELECT * FROM DvdCopy WHERE dvd_copy_id = @dvd_copy_id", cnn);
                command.Parameters.Add(new SqlParameter("@dvd_copy_id", copy_id));

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
        }

        public Boolean addCopiesForDvd(DvdInfo dvdInfo)
        {
            Boolean success = true;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                Random rnd = new Random();

                for (int i = 0; i < 12; i++) //add 12 copies
                {
                    SqlCommand command = new SqlCommand("INSERT INTO DvdCopy (dvd_info_id,copy_type_id,serialnumber,note,in_stock)" +
                    "VALUES(@dvd_info_id,@copy_type_id,@serialnumber,@note,@in_stock)", cnn);
                    command.Parameters.Add(new SqlParameter("@dvd_info_id", dvdInfo.dvd_info_id));
                    command.Parameters.Add(new SqlParameter("@copy_type_id", i < 10 ? 2 : 1)); //insert 10 buy copies and 2 rent copies
                    command.Parameters.Add(new SqlParameter("@serialnumber", rnd.Next(99999999))); //insert random number as serial number
                    command.Parameters.Add(new SqlParameter("@note", ""));
                    command.Parameters.Add(new SqlParameter("@in_stock", true));
                    try
                    {
                        cnn.Open();
                        command.ExecuteNonQuery();


                    }
                    catch (Exception ex)
                    {
                        success = false;
                    }
                    finally
                    {
                        cnn.Close();
                    }
                }

                return success;
            }
        }

        public List<DvdCopy> getAllInStockRentCopiesForDvdInfo(DvdInfo dvdInfo)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                List<DvdCopy> dvdCopies = new List<DvdCopy>();

                //is hier nog met hardcoded type_id
                SqlCommand command = new SqlCommand("SELECT * FROM DvdCopy WHERE dvd_info_id = @dvd_info_id AND copy_type_id = 1 AND in_stock = 1;", cnn);
                command.Parameters.Add(new SqlParameter("@dvd_info_id", dvdInfo.dvd_info_id));
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
        }

        public List<DvdCopy> getAllInStockBuyCopiesForDvdInfo(DvdInfo dvdInfo)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                List<DvdCopy> dvdCopies = new List<DvdCopy>();

                //is hier nog met hardcoded type_id
                SqlCommand command = new SqlCommand("SELECT * FROM DvdCopy WHERE dvd_info_id = @dvd_info_id AND copy_type_id = 2 AND in_stock = 1;", cnn);
                command.Parameters.Add(new SqlParameter("@dvd_info_id", dvdInfo.dvd_info_id));

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
        }

        public void updateDvdCopy(DvdCopy copy)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {

                //todo: alternatief om deze zielige code mee te vervangen

                SqlCommand command = new SqlCommand("UPDATE DvdCopy " +
                "SET dvd_info_id = @dvd_info_id, copy_type_id = @copy_type_id, serialnumber = @serialnumber, note = @note, in_stock = @in_stock "+
                "WHERE dvd_copy_id = @dvd_copy_id", cnn);
                command.Parameters.Add(new SqlParameter("@dvd_info_id", copy.dvdinfo.dvd_info_id));
                command.Parameters.Add(new SqlParameter("@copy_type_id", copy.type.id));
                command.Parameters.Add(new SqlParameter("@serialnumber", copy.serialnumber));
                command.Parameters.Add(new SqlParameter("@note", copy.note));
                command.Parameters.Add(new SqlParameter("@in_stock", copy.in_stock));
                command.Parameters.Add(new SqlParameter("@dvd_copy_id", copy.dvd_copy_id));

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

        //update methode voor wanneer je geen volledig object kan gebruiken
        public void updateDvdCopyInStockStatus(DvdCopy dvdCopy, Boolean in_stock)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                SqlCommand command = new SqlCommand("UPDATE DvdCopy SET in_stock = @in_stock WHERE dvd_copy_id = @dvd_copy_id", cnn);
                command.Parameters.Add(new SqlParameter("@in_stock", in_stock));
                command.Parameters.Add(new SqlParameter("@dvd_copy_id", dvdCopy.dvd_copy_id));
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

        /**Returns a list of all dvd copies that are available for the full 14 day period, starting today*/
        public List<DvdCopy> getAllFullyAvailableCopies(DvdInfo dvd, DateTime startdate)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                List<DvdCopy> orderList = new List<DvdCopy>();
                SqlCommand command = new SqlCommand("SELECT * from DvdCopy " +
                "WHERE DvdCopy.dvd_info_id = @dvd_info_id AND " +
                "DvdCopy.copy_type_id = 1 AND " +
                "DvdCopy.dvd_copy_id NOT IN " +
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
        }

        /*Sets all copies back to in_stock = true*/
        public void resetAllCopies()
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
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
        }

        private DvdCopy createDvdCopy(SqlDataReader reader)
        {
            DvdCopy dvdCopy = new DvdCopy
            {
                dvd_copy_id = Convert.ToInt32(reader["dvd_copy_id"]),
                dvdinfo = new DvdInfoDAO().getDvdInfoWithId(reader["dvd_info_id"].ToString()),
                type = new DvdCopyTypeDAO().getTypeForID(Convert.ToInt32(reader["copy_type_id"])),
                serialnumber = Convert.ToString(reader["serialnumber"]),
                note = Convert.ToString(reader["note"]),
                in_stock = Convert.ToBoolean(reader["in_stock"])
            };

            return dvdCopy;
        }
    }
}
