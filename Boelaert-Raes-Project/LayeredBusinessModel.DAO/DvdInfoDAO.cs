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
    public class DvdInfoDAO : DAO
    {
        public List<DvdInfo> getAllDvdInfos()
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<DvdInfo> dvdlist = new List<DvdInfo>();

            SqlCommand command = new SqlCommand("SELECT * FROM DvdInfo", cnn);
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dvdlist.Add(createDvdInfo(reader));
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
            return dvdlist;
        }

        public List<DvdInfo> searchDvdWithText(String searchText)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<DvdInfo> dvdlist = new List<DvdInfo>();

            //todo: parameters
            //beter met CONTAINS dan wildcards+LIKE?
            SqlCommand command = new SqlCommand("SELECT * FROM DvdInfo WHERE name LIKE '%" + searchText + "%' OR barcode LIKE '%" + searchText + "%' OR author LIKE '%" + searchText + "%';", cnn);
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dvdlist.Add(createDvdInfo(reader));
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
            return dvdlist;
        }

        public List<DvdInfo> searchDvdWithTextAndCategory(String searchText, String categoryID)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<DvdInfo> dvdlist = new List<DvdInfo>();

            //todo: parameters
            //beter met CONTAINS dan wildcards+LIKE?    

            //todo: fix bug
            //haalt een record op voor elk genre van een dvdInfo (bv dvdInfo met 3 genres zal 3 keer in de resultset zitten, film met 1 genre 1 keer, etc...)

            SqlCommand command = new SqlCommand("SELECT * " + //"SELECT DvdInfo.dvd_info_id, DvdInfo.name, DvdInfo.year, DvdInfo.barcode, DvdInfo.author, DvdInfo.image "
            "FROM DvdInfo " +
            "INNER JOIN DvdGenre " +
            "ON DvdInfo.dvd_info_id = DvdGenre.dvd_info_id " +
            "INNER JOIN Genres " +
            "ON DvdGenre.genre_id = Genres.genre_id " +
            "WHERE Genres.category_id = " + categoryID +
            "AND (DvdInfo.name LIKE '%" + searchText + "%' OR DvdInfo.barcode LIKE '%" + searchText + "%' OR DvdInfo.author LIKE '%" + searchText + "%')" , cnn);

            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dvdlist.Add(createDvdInfo(reader));
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
            return dvdlist;
        }

        public List<DvdInfo> searchDvdWithTextAndGenre(String searchText, String genreID)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<DvdInfo> dvdlist = new List<DvdInfo>();

            //todo: parameters    

            SqlCommand command = new SqlCommand("SELECT * " + //SELECT DvdInfo.dvd_info_id, DvdInfo.name, DvdInfo.year, DvdInfo.barcode, DvdInfo.author, DvdInfo.image 
            "FROM DvdInfo " +
            "INNER JOIN DvdGenre " +
            "ON DvdInfo.dvd_info_id = DvdGenre.dvd_info_id " +
            "WHERE DvdGenre.genre_id = " + genreID +
            " AND (name LIKE '%" + searchText + "%' OR barcode LIKE '%" + searchText + "%' OR author LIKE '%" + searchText + "%')", cnn);


            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dvdlist.Add(createDvdInfo(reader));
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
            return dvdlist;
        }

        public List<DvdInfo> getLatestDvds(int amount)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<DvdInfo> dvdlist = new List<DvdInfo>();
            SqlCommand sql = new SqlCommand("select top " + amount + " * from DvdInfo", cnn);

            try
            {
               
                cnn.Open();
                SqlDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    dvdlist.Add(createDvdInfo(reader));
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cnn.Close();
            }

            return dvdlist;
        }


        private DvdInfo createDvdInfo(SqlDataReader reader)
        {
            //gebruik van object initializer ipv constructor
            DvdInfo dvd = new DvdInfo
            {
                dvd_info_id = Convert.ToInt32(reader["dvd_info_id"]),
                name = Convert.ToString(reader["name"]),
                year = Convert.ToString(reader["year"]),
                barcode = Convert.ToString(reader["barcode"]),
                author = Convert.ToString(reader["author"]),
                image = Convert.ToString(reader["image"]),
                descripion = Convert.ToString(reader["description"]),
                rent_price = float.Parse(reader["rent_price"].ToString()),
                buy_price = float.Parse(reader["buy_price"].ToString()),
                date_added = Convert.ToDateTime(reader["date_added"])
            };
            return dvd;
        }
    }
}
