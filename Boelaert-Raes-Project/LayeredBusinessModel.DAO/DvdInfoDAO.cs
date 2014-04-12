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

        public int addDvdInfo(DvdInfo dvdInfo)
        {
            int dvdInfoId = -1;
            cnn = new SqlConnection(sDatabaseLocatie);
            
            SqlCommand command = new SqlCommand("INSERT INTO DvdInfo" +
            "(name, year, barcode, author, description, rent_price, buy_price, date_added, amount_sold, actors, duration) " +
            "OUTPUT INSERTED.dvd_info_id " +
            "VALUES (@name, @year, @barcode, @author, @description, @rent_price, @buy_price, @date_added, @amount_sold, @actors, @duration)", cnn);
            command.Parameters.Add(new SqlParameter("@name", dvdInfo.name));
            command.Parameters.Add(new SqlParameter("@year", dvdInfo.year));
            command.Parameters.Add(new SqlParameter("@barcode", dvdInfo.barcode));
            command.Parameters.Add(new SqlParameter("@author", dvdInfo.author));
            command.Parameters.Add(new SqlParameter("@description", dvdInfo.descripion));
            command.Parameters.Add(new SqlParameter("@rent_price", dvdInfo.rent_price));
            command.Parameters.Add(new SqlParameter("@buy_price", dvdInfo.buy_price));
            command.Parameters.Add(new SqlParameter("@date_added", dvdInfo.date_added));
            command.Parameters.Add(new SqlParameter("@amount_sold", dvdInfo.amount_sold));

            String actorsString = "";
            for(int i=0;i<dvdInfo.actors.Length;i++)
            {
                actorsString += dvdInfo.actors[i];
                if(i<dvdInfo.actors.Length-1)
                {
                    //more actors to follow, add comma
                    actorsString += ",";
                }
            }
            command.Parameters.Add(new SqlParameter("@actors", actorsString));
            command.Parameters.Add(new SqlParameter("@duration", dvdInfo.duration));

            try
            {
                cnn.Open();
                dvdInfoId = (int)command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //return -1 on error
                dvdInfoId = -1;
            }
            finally
            {
                cnn.Close();
            }
            return dvdInfoId;
        }

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


        public DvdInfo getDvdInfoWithId(String id)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            DvdInfo dvd = null;

            SqlCommand command = new SqlCommand("SELECT * FROM DvdInfo WHERE dvd_info_id = "+id, cnn);
            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dvd = createDvdInfo(reader);
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
            return dvd;
        }


        //todo
        public void updateDvdInfo(DvdInfo dvd)
        {
            cnn = new SqlConnection(sDatabaseLocatie);

            SqlCommand command = new SqlCommand("UPDATE DvdInfo " +
            "SET name = @name, " +
            "year = @year, " +
            "barcode = @barcode, " +
            "author = @author, " +
            "image = @image, " +
            "description = @description, " +
            "rent_price = @rent_price, " +
            "buy_price = @buy_price, " +
           // "date_added = @date_added, " +
            "amount_sold = @amount_sold " +
            "WHERE dvd_info_id = @dvd_info_id", cnn);

            command.Parameters.Add(new SqlParameter("@name", dvd.name));
            command.Parameters.Add(new SqlParameter("@year", dvd.year));
            command.Parameters.Add(new SqlParameter("@barcode", dvd.barcode));
            command.Parameters.Add(new SqlParameter("@author", dvd.author));
            command.Parameters.Add(new SqlParameter("@image", dvd.image));
            command.Parameters.Add(new SqlParameter("@description", dvd.descripion));
            command.Parameters.Add(new SqlParameter("@rent_price", dvd.rent_price));
            command.Parameters.Add(new SqlParameter("@buy_price", dvd.buy_price));
          //  command.Parameters.Add(new SqlParameter("@date_added", dvd.date_added));
            command.Parameters.Add(new SqlParameter("@amount_sold", dvd.amount_sold));
            command.Parameters.Add(new SqlParameter("@dvd_info_id", dvd.dvd_info_id));

            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();

                reader.Close();

            }
            catch (Exception ex)
            {
            }
            finally
            {
                cnn.Close();
            }
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

            
            SqlCommand command = new SqlCommand("SELECT DvdInfo.dvd_info_id, DvdInfo.name, DvdInfo.year, DvdInfo.barcode, DvdInfo.author, DvdInfo.image, " + //"SELECT DvdInfo.dvd_info_id, DvdInfo.name, DvdInfo.year, DvdInfo.barcode, DvdInfo.author, DvdInfo.image "
            "DvdInfo.description, DvdInfo.rent_price, DvdInfo.buy_price, DvdInfo.date_added, DvdInfo.amount_sold, DvdInfo.actors, DvdInfo.duration " +
            "FROM DvdInfo " +
            "INNER JOIN DvdGenre " +
            "ON DvdInfo.dvd_info_id = DvdGenre.dvd_info_id " +
            "INNER JOIN Genres " +
            "ON DvdGenre.genre_id = Genres.genre_id " +
            "WHERE Genres.category_id = " + categoryID +
            "AND (DvdInfo.name LIKE '%" + searchText + "%' OR DvdInfo.barcode LIKE '%" + searchText + "%' OR DvdInfo.author LIKE '%" + searchText + "%') " +
            "GROUP BY DvdInfo.dvd_info_id, DvdInfo.name, DvdInfo.year, DvdInfo.barcode, DvdInfo.author, DvdInfo.image, "+
            "DvdInfo.description, DvdInfo.rent_price, DvdInfo.buy_price, DvdInfo.date_added, DvdInfo.amount_sold, DvdInfo.actors, DvdInfo.duration ", cnn);


            /**
            dvd_info_id = Convert.ToInt32(reader["dvd_info_id"]),
                name = Convert.ToString(reader["name"]),
                year = Convert.ToString(reader["year"]),
                barcode = Convert.ToString(reader["barcode"]),
                author = Convert.ToString(reader["author"]),
                image = Convert.ToString(reader["image"]),
                descripion = Convert.ToString(reader["description"]),
                rent_price = float.Parse(reader["rent_price"].ToString()),
                buy_price = float.Parse(reader["buy_price"].ToString()),
                date_added = Convert.ToDateTime(reader["date_added"]),
                amount_sold = Convert.ToInt32(reader["amount_sold"])
                    */


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
            SqlCommand sql = new SqlCommand("SELECT top " + amount + " * FROM DvdInfo ORDER BY date_added DESC", cnn);

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

        public List<DvdInfo> searchDvdFromYear(String year)
        {
            List<DvdInfo> dvdlist = new List<DvdInfo>();

            cnn = new SqlConnection(sDatabaseLocatie);
            
            SqlCommand command = new SqlCommand("SELECT  * FROM DvdInfo where year = " + year, cnn);
            command.Parameters.Add(new SqlParameter("@year", year));

            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();
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

        public List<DvdInfo> searchDvdFromDirector(String director)
        {
            List<DvdInfo> dvdlist = new List<DvdInfo>();

            cnn = new SqlConnection(sDatabaseLocatie);

            SqlCommand command = new SqlCommand("SELECT  * FROM DvdInfo where author like '%" + director + "%'", cnn);
            command.Parameters.Add(new SqlParameter("@director", director));

            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();
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

        public List<DvdInfo> searchDvdWithActor(String actor)
        {
            List<DvdInfo> dvdlist = new List<DvdInfo>();

            cnn = new SqlConnection(sDatabaseLocatie);

            SqlCommand command = new SqlCommand("SELECT  * FROM DvdInfo where actors like '%" + actor + "%'", cnn);
            command.Parameters.Add(new SqlParameter("@director", actor));

            try
            {
                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();
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


        public List<DvdInfo> getMostPopularDvds(int amount)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<DvdInfo> dvdlist = new List<DvdInfo>();
            SqlCommand command = new SqlCommand("SELECT top " + amount + " * FROM DvdInfo ORDER BY amount_sold DESC", cnn);
            command.Parameters.Add(new SqlParameter("@amount", amount));

            try
            {

                cnn.Open();
                SqlDataReader reader = command.ExecuteReader();
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

        private List<KeyValuePair<int, String>> getMedia(int id)
        {
            List<KeyValuePair<int, string>> media = new List<KeyValuePair<int, string>>();

            cnn = new SqlConnection(sDatabaseLocatie);
            SqlCommand sql = new SqlCommand("Select * from Media where dvd_info_id = " + id + " order by media_type_id DESC", cnn);
            sql.Parameters.Add(new SqlParameter("@id", id));
            try
            {
                cnn.Open();
                SqlDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    KeyValuePair<int, String> mediaObject = new KeyValuePair<int,string>(Convert.ToInt32(reader["media_type_id"]),reader["url"].ToString());
                    media.Add(mediaObject);
                }

            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                cnn.Close();
            }



            return media;
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
                date_added = Convert.ToDateTime(reader["date_added"]),
                amount_sold = Convert.ToInt32(reader["amount_sold"]),
                media = getMedia(Convert.ToInt32(reader["dvd_info_id"])),

                actors = Convert.ToString(reader["actors"]).Split(','),
                
                duration = Convert.ToString(reader["duration"])
            };
            return dvd;
        }
    }
}
