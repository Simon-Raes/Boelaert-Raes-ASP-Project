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
    public class DvdCopyDAO : DAO
    {
        /*
         *  Returns a dvdcopy based on an ID
         *  Throws a NoRecordException if no records were found
         *  Throws a DALException if something else went wrong
         */
        public DvdCopy getByID(String copy_id)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM DvdCopy WHERE dvd_copy_id = @dvd_copy_id", cnn);
                command.Parameters.Add(new SqlParameter("@dvd_copy_id", copy_id));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        DvdCopy dvdCopy = new DvdCopy();
                        while (reader.Read())
                        {
                            dvdCopy = createDvdCopy(reader);                //Throws NoRecordException
                        }
                        return dvdCopy;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get copy by ID", ex);
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
                throw new NoRecordException("No records were found - DvdCopyDAO getCopyWithId()");
            }
        }
        
        /*
         * Returns a List of dvdCopyes based on a dvdInfo, a type(rent or buy) that are in stock
         * Throws a NoRecordException if no records were found
         * Throws a DALException if something elsd went wrong
         */
        public List<DvdCopy> getAllInStockCopiesForDvdInfo(DvdInfo dvdInfo, DvdCopyType type)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM DvdCopy WHERE dvd_info_id = @dvd_info_id AND copy_type_id = @type_id AND in_stock = 1;", cnn);
                command.Parameters.Add(new SqlParameter("@dvd_info_id", dvdInfo.dvd_info_id));
                command.Parameters.Add(new SqlParameter("@type_id", type.id));
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<DvdCopy> dvdCopies = new List<DvdCopy>();
                        while (reader.Read())
                        {
                            dvdCopies.Add(createDvdCopy(reader));       //Throws NoRecordException
                        }
                        return dvdCopies;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get all in stock copies for dvdinfo", ex);
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
                throw new NoRecordException("No records were found - DvdCopyDAO getAllInStockCopiesForDvdInfo()");
            }
        }

        /*
         * Returns a list of all dvd copies that are available for the full 14 day period, starting today
         * Throws a NoRecordException if no records were found
         * Throws a DALException if something else went wrong
         */
        public List<DvdCopy> getAllFullyAvailableCopies(DvdInfo dvd, DateTime startdate)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * from DvdCopy " +
                "WHERE DvdCopy.dvd_info_id = @dvd_info_id AND " +
                "DvdCopy.copy_type_id = 1 AND " +
                "DvdCopy.dvd_copy_id NOT IN " +
                "(select dvd_copy_id from OrderLine where dvd_copy_id is not null and (startdate >= @startdate or enddate >= @startdate)) ", cnn);
                command.Parameters.Add(new SqlParameter("@dvd_info_id", dvd.dvd_info_id));
                command.Parameters.Add(new SqlParameter("@startdate", startdate));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<DvdCopy> orderList = new List<DvdCopy>();
                        while (reader.Read())
                        {
                            orderList.Add(createDvdCopy(reader));           //Throws NoRecordException
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
                    throw new DALException("Failed to get fully available copies for dvdinfo", ex);
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
                throw new NoRecordException("No records were found - DvdCopyDAO getAllFullyAvailableCopies()");
            }
        }

        /*
         * Updates a dvdCopy
         * Returns true if copy was updated, false if no copy was updated
         * Throws DALException if something else went wrong
         */
        public Boolean update(DvdCopy copy)
        {
            SqlCommand command = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {

                //todo: alternatief om deze zielige code mee te vervangen
                //wat bedoel je hiermee? 

                command = new SqlCommand("UPDATE DvdCopy " +
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
                    if (command.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to update copy", ex);
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
         * Updates the stock-status for a copy
         * Returns true if the copy was updated, false if no copy was updated
         * Throws a DALException if something else went wrong
         */ 
        public Boolean updateStockStatus(DvdCopy dvdCopy, Boolean in_stock)
        {
            SqlCommand command = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("UPDATE DvdCopy SET in_stock = @in_stock WHERE dvd_copy_id = @dvd_copy_id", cnn);
                command.Parameters.Add(new SqlParameter("@in_stock", in_stock));
                command.Parameters.Add(new SqlParameter("@dvd_copy_id", dvdCopy.dvd_copy_id));
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
                    throw new DALException("Failed to update copy stock-status", ex);
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
         * Sets all copies back to in_stock = true
         * Returns true if all the copies were succelfully updated, false if no copies were updated
         * Throws a DALException if something else went wrong
         */
        public Boolean deleteAll()
        {
            SqlCommand command = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("UPDATE DvdCopy SET in_stock = 1;", cnn);
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
                    throw new DALException("Failed to reset all the copies", ex);
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
         * Creates a DvdCopy-Object
         */
        private DvdCopy createDvdCopy(SqlDataReader reader)
        {
            return new DvdCopy
            {
                dvd_copy_id = Convert.ToInt32(reader["dvd_copy_id"]),
                dvdinfo = new DvdInfoDAO().getByID(reader["dvd_info_id"].ToString()),           //Throws NoRecordException
                type = new DvdCopyTypeDAO().getByID(reader["copy_type_id"].ToString()),         //Throws NoRecordException
                serialnumber = Convert.ToString(reader["serialnumber"]),
                note = Convert.ToString(reader["note"]),
                in_stock = Convert.ToBoolean(reader["in_stock"])
            };
        }

        /*
         * Returns a list of dvdcopies for a dvd
         * Throws a NoRecordException if no records were found
         * Throws a DALException if something else went wrong
         */
        /*
        public List<DvdCopy> getAllCopiesForDvdInfo(DvdInfo dvdInfo)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {       
                command = new SqlCommand("SELECT * FROM DvdCopy WHERE dvd_info_id = @dvd_info_id", cnn);
                command.Parameters.Add(new SqlParameter("@dvd_info_id", dvdInfo.dvd_info_id));
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<DvdCopy> dvdCopies = new List<DvdCopy>();
                        while (reader.Read())
                        {
                            dvdCopies.Add(createDvdCopy(reader));       //Throws NoRecordException
                        }
                        return dvdCopies;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get all copies for dvdinfo", ex);
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
                throw new NoRecordException("No records were found - DvdCopyDAO getAllCopiesForDvdInfo()");
            }
        }*/

        /*
         *  Adds a dvdCopy for a dvd
         *  Returns true if insert was successful, false if no rows were inserted
         *  Throws a DALException if something went wrong
         */
        /*
        public Boolean addCopiesForDvd(DvdInfo dvdInfo)
        {
            SqlCommand command = null;
            Boolean status = true;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                Random rnd = new Random();

                for (int i = 0; i < 12; i++) //add 12 copies
                {
                    command = new SqlCommand("INSERT INTO DvdCopy (dvd_info_id,copy_type_id,serialnumber,note,in_stock)" +
                    "VALUES(@dvd_info_id,@copy_type_id,@serialnumber,@note,@in_stock)", cnn);
                    command.Parameters.Add(new SqlParameter("@dvd_info_id", dvdInfo.dvd_info_id));
                    command.Parameters.Add(new SqlParameter("@copy_type_id", i < 10 ? 2 : 1)); //insert 10 buy copies and 2 rent copies
                    command.Parameters.Add(new SqlParameter("@serialnumber", rnd.Next(99999999))); //insert random number as serial number
                    command.Parameters.Add(new SqlParameter("@note", ""));
                    command.Parameters.Add(new SqlParameter("@in_stock", true));
                    try
                    {
                        cnn.Open();
                        if (command.ExecuteNonQuery() == 0) {
                            status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new DALException("Failed to insert coppies for dvd", ex);
                    }
                    finally
                    {
                        if (cnn != null)
                        {
                            cnn.Close();
                        }
                    }                   
                }
                return status;
            }
        }*/

        /*
         * Returns a List with dvdCopies for a certain dvdInfo that are in stock and for rent
         */
        /*
        public List<DvdCopy> getAllInStockRentCopiesForDvdInfo(DvdInfo dvdInfo)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                //is hier nog met hardcoded type_id
                command = new SqlCommand("SELECT * FROM DvdCopy WHERE dvd_info_id = @dvd_info_id AND copy_type_id = 1 AND in_stock = 1;", cnn);
                command.Parameters.Add(new SqlParameter("@dvd_info_id", dvdInfo.dvd_info_id));
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<DvdCopy> dvdCopies = new List<DvdCopy>();
                        while (reader.Read())
                        {
                            dvdCopies.Add(createDvdCopy(reader));
                        }
                        return dvdCopies;
                    }

                }
                catch (Exception ex)
                {

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
                return null;
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
         * */
    }
}