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
    public class GenreDAO
    {
        private string strSQL;
        private string sDatabaseLocatie = ConfigurationManager.ConnectionStrings["ProjectConnection"].ConnectionString;
        private SqlConnection cnn;

        public List<Genre> getGenresForCategory(int categoryID)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<Genre> genrelist = new List<Genre>();

            SqlCommand command = new SqlCommand("SELECT * FROM genres WHERE category_id = "+categoryID, cnn);
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
