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
         */
        public DvdCopyType getTypeForID(String id)
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

        /*
         * Returns a DvdCopyType based on a name
         */
        public DvdCopyType getTypeByName(String name)
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
