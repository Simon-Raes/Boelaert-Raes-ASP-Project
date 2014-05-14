using CustomException;
using LayeredBusinessModel.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.DAO
{
    public class DvdCopyTypeDAO : DAO
    {
        /*
         * Returs a DvdCopyType based on an ID
         * Throws a NoRecordException if no records were found
         * Throws a DALException if something else went wrong
         */
        public DvdCopyType getByID(String id)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {   
                command = new SqlCommand("SELECT * FROM DvdCopyType WHERE copy_type_id =@copy_type_id", cnn);
                command.Parameters.Add(new SqlParameter("@copy_type_id", id));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return createDvdCopyType(reader);
                    }
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to get a type based on an ID", ex);
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
                throw new NoRecordException("No records were found - DvdCopyTypeDAO getTypeForID()");
            }
        }

        /*
         * Returns a DvdCopyType based on a name
         * Throws a NoRecordException if no records were found
         * Throws a DALException if something else went wrong
         */
        public DvdCopyType getByName(String name)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM DvdCopyType WHERE name =@name", cnn);
                command.Parameters.Add(new SqlParameter("@name", name));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if(reader.HasRows) 
                    {
                        reader.Read();
                        return createDvdCopyType(reader);                    
                    }
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to get a type based on an ID", ex);
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
                throw new NoRecordException("No records were found - DvdCopyTypeDAO getTypeByName()");
            }
        }

        private DvdCopyType createDvdCopyType(SqlDataReader reader)
        {
            return new DvdCopyType()
            {
                id = Convert.ToInt16(reader["copy_type_id"]),
                name = reader["name"].ToString()
            };
        }
    }
}
