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

        public List<int> findRelatedDvdsBasedOnGenre(int dvdId)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<int> dvdIds = new List<int>();

            SqlCommand sql = new SqlCommand("select top(4) dvd_info_id from dvdGenre where dvd_info_id != " + dvdId + " and genre_id in (select genre_id from dvdgenre where dvd_info_id = " + dvdId + ") group by dvd_info_id order by COUNT(dvd_info_id) desc  ", cnn);

            try
            {
                cnn.Open();

                SqlDataReader reader = sql.ExecuteReader();

                while (reader.Read())
                {
                    dvdIds.Add(Convert.ToInt32(reader["dvd_info_id"]));
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
            return dvdIds;
        }
    }
}
