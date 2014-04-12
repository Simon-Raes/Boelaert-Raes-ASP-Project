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
    public class GenreDAO : DAO
    {
        public List<Genre> getGenres()
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<Genre> genrelist = new List<Genre>();

            SqlCommand command = new SqlCommand("SELECT * FROM genres", cnn);
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    genrelist.Add(createGenre(reader));
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
            return genrelist;
        }


        public List<Genre> getGenresForCategory(int categoryID)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<Genre> genrelist = new List<Genre>();

            SqlCommand command = new SqlCommand("SELECT * FROM genres WHERE category_id = " + categoryID, cnn);
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    genrelist.Add(createGenre(reader));
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
            return genrelist;
        }

        public List<Genre> getGenresForDvd(int dvd_id)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<Genre> genrelist = new List<Genre>();

            SqlCommand command = new SqlCommand("SELECT * FROM Genres "+
                "JOIN DvdGenre "+
                "ON DvdGenre.genre_id = Genres.genre_id "+
                "WHERE DvdGenre.dvd_info_id = " + dvd_id, cnn);
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    genrelist.Add(createGenre(reader));
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
            return genrelist;
        }

        public Genre getGenre(int genreID)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            Genre genre = null;

            SqlCommand command = new SqlCommand("SELECT * FROM genres WHERE genre_id = " + genreID, cnn);
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                reader.Read();
                genre = createGenre(reader);
                reader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                cnn.Close();
            }
            return genre;
        }

        private Genre createGenre(SqlDataReader reader)
        {
            Genre genre = new Genre
            {
                genre_id = Convert.ToInt32(reader["genre_id"]),
                category_id = Convert.ToInt32(reader["category_id"]),
                name = Convert.ToString(reader["name"])
            };
            return genre;
        }
    }


}
