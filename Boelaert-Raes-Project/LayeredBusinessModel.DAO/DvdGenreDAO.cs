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
        public void addGenreForDvd(Genre genre, DvdInfo dvdInfo)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {

                SqlCommand command = new SqlCommand("INSERT INTO DvdGenre " +
                "(dvd_info_id, genre_id) " +
                "VALUES (@dvd_info_id, @genre_id)", cnn);
                command.Parameters.Add(new SqlParameter("@dvd_info_id", dvdInfo.dvd_info_id));
                command.Parameters.Add(new SqlParameter("@genre_id", genre.genre_id));

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

        public List<int> findRelatedDvdsBasedOnGenre(DvdInfo dvdInfo, int amount)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                List<int> dvdIds = new List<int>();

                SqlCommand command = new SqlCommand("select top(@amount) dvd_info_id from dvdGenre where dvd_info_id != @dvd_info_id and genre_id in (select genre_id from dvdgenre where dvd_info_id = @dvd_info_id) group by dvd_info_id order by COUNT(dvd_info_id) desc  ", cnn);
                command.Parameters.Add(new SqlParameter("@dvd_info_id", amount));
                command.Parameters.Add(new SqlParameter("@amount", amount));
                try
                {
                    cnn.Open();

                    SqlDataReader reader = command.ExecuteReader();

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
}
