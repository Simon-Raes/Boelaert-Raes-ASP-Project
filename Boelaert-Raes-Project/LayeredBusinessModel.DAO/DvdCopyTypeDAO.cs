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
        public DvdCopyType getTypeForID(int id)
        {            
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                DvdCopyType type = null;

                SqlCommand command = new SqlCommand("SELECT * FROM DvdCopyType WHERE copy_type_id =@copy_type_id", cnn);
                command.Parameters.Add(new SqlParameter("@copy_type_id", id));

                try
                {
                    cnn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        type = createDvdCopyType(reader);
                    }

                    reader.Close();
                    return type;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    cnn.Close();
                }
                return type;
            }

        }

        private DvdCopyType createDvdCopyType(SqlDataReader reader)
        {
            DvdCopyType type = new DvdCopyType()
            {
                id = Convert.ToInt16(reader["copy_type_id"]),
                name = reader["name"].ToString()
            };
            return type;
        }

    }
}
