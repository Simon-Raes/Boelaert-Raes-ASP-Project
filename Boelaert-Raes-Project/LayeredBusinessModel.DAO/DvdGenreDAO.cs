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
    public class DvdGenreDAO : DAO
    {
        public void addDvdGenre(int genre_id, int dvd_info_id)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            
            SqlCommand command = new SqlCommand("INSERT INTO DvdGenre " +
            "(dvd_info_id, genre_id) " +
            "VALUES (@dvd_info_id, @genre_id)", cnn);
            command.Parameters.Add(new SqlParameter("@dvd_info_id", dvd_info_id));
            command.Parameters.Add(new SqlParameter("@genre_id", genre_id));
            
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
}
