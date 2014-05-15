using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using LayeredBusinessModel.Domain;
using System.Configuration;
using CustomException;

namespace LayeredBusinessModel.DAO
{
    public class DvdInfoDAO : DAO
    {
        /*
         * Returns a dvdinfo based on an ID
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public DvdInfo getByID(String id)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM DvdInfo WHERE dvd_info_id = @id", cnn);
                command.Parameters.Add(new SqlParameter("@id", id));
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return createDvdInfo(reader);           //Throws NoRecordException
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get the dvdinfo based on an ID", ex);
                }
                finally
                {
                    if (cnn != null)
                    {
                        cnn.Close();
                    }
                }
                throw new NoRecordException("No records were found - DvdInfoDAO getDvdInfoWithId()");
            }
        }

        /*
         * Returns a list with dvd's from a certain year
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<DvdInfo> getByYear(String year)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT  * FROM DvdInfo where year = @year", cnn);
                command.Parameters.Add(new SqlParameter("@year", year));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<DvdInfo> dvdlist = new List<DvdInfo>();
                        while (reader.Read())
                        {
                            dvdlist.Add(createDvdInfo(reader));         //Throws NoRecordException
                        }
                        return dvdlist;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get dvd's from a certain year", ex);
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
                throw new NoRecordException("No records were found - DvdInfoDAO searchDvdFromYear()");
            }

        }

        /*
         * Returns a list with dvd's from a certain director
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<DvdInfo> getByDirector(String director)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT  * FROM DvdInfo where author like @director", cnn);
                command.Parameters.Add(new SqlParameter("@director", "%" + director + "%"));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<DvdInfo> dvdlist = new List<DvdInfo>();
                        while (reader.Read())
                        {
                            dvdlist.Add(createDvdInfo(reader));         //Throws NoRecordException
                        }
                        return dvdlist;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get dvd's from a certain director", ex);
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
                throw new NoRecordException("No records were found - DvdInfoDAO searchDvdFromDirector()");
            }
        }

        /*
         * Returns a list with dvd's from a certain actor
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<DvdInfo> getByActor(String actor)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT  * FROM DvdInfo where actors like @actor", cnn);
                command.Parameters.Add(new SqlParameter("@actor", "%" + actor + "%"));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<DvdInfo> dvdlist = new List<DvdInfo>();
                        while (reader.Read())
                        {
                            dvdlist.Add(createDvdInfo(reader));         //Throws NoRecordException
                        }
                        return dvdlist;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get dvd's from a certain actor", ex);
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
                throw new NoRecordException("No records were found - DvdInfoDAO searchDvdWithActor()");
            }
        }

        /*
         * Returns all DvdInfo's that have a banner image
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong 
         */
        public List<DvdInfo> getAllWithBanner()
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM DvdInfo " +
                "JOIN Media " +
                "ON Media.dvd_info_id = DvdInfo.dvd_info_id " +
                "WHERE Media.media_type_id = 4", cnn);
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<DvdInfo> dvdlist = new List<DvdInfo>();
                        while (reader.Read())
                        {
                            dvdlist.Add(createDvdInfo(reader));         //Throws NoRecordException          
                        }
                        return dvdlist;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get all the dvd's with a banner", ex);
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
                throw new NoRecordException("No records were found - DvdInfoDAO getAllDvdInfosWithBanner()");
            }
        }

        /*
         * Returns a list with dvd's based on searchtext
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<DvdInfo> getByText(String searchText)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM DvdInfo WHERE name LIKE @searchtext OR barcode LIKE @searchtext OR author LIKE @searchtext", cnn);
                command.Parameters.Add(new SqlParameter("@searchtext", "%" + searchText + "%"));
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<DvdInfo> dvdlist = new List<DvdInfo>();
                        while (reader.Read())
                        {
                            dvdlist.Add(createDvdInfo(reader));         //Throws NoRecordException
                        }
                        return dvdlist;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get dvd's based on searchtext", ex);
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
                throw new NoRecordException("No records were found - DvdInfoDAO searchDvdWithText()");
            }
        }

        /*
         * Returns a list with dvd's based on searchtext and category
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<DvdInfo> getByTextCategory(String searchText, String categoryID)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT DvdInfo.dvd_info_id, DvdInfo.name, DvdInfo.year, DvdInfo.barcode, DvdInfo.author, " +
                "DvdInfo.description, DvdInfo.rent_price, DvdInfo.buy_price, DvdInfo.date_added, DvdInfo.amount_sold, DvdInfo.actors, DvdInfo.duration " +
                "FROM DvdInfo " +
                "INNER JOIN DvdGenre " +
                "ON DvdInfo.dvd_info_id = DvdGenre.dvd_info_id " +
                "INNER JOIN Genres " +
                "ON DvdGenre.genre_id = Genres.genre_id " +
                "WHERE Genres.category_id = @cat_id " +
                "AND (DvdInfo.name LIKE @searchtext OR DvdInfo.barcode LIKE @searchtext OR DvdInfo.author LIKE @searchtext) " +
                "GROUP BY DvdInfo.dvd_info_id, DvdInfo.name, DvdInfo.year, DvdInfo.barcode, DvdInfo.author, " +
                "DvdInfo.description, DvdInfo.rent_price, DvdInfo.buy_price, DvdInfo.date_added, DvdInfo.amount_sold, DvdInfo.actors, DvdInfo.duration ", cnn);

                command.Parameters.Add(new SqlParameter("@cat_id", categoryID));
                command.Parameters.Add(new SqlParameter("@searchtext", "%" + searchText + "%"));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<DvdInfo> dvdlist = new List<DvdInfo>();
                        while (reader.Read())
                        {
                            dvdlist.Add(createDvdInfo(reader));         //Throws NoRecordException
                        }
                        return dvdlist;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get dvd's based on searchtext and categorie", ex);
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
                throw new NoRecordException("No records were found - DvdInfoDAO searchDvdWithTextAndCategory()");
            }
        }

        /*
         * Returns a list with dvd's based on searchtext and genre
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<DvdInfo> getByTextAndGenre(String searchText, String genreID)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * " +
                "FROM DvdInfo " +
                "INNER JOIN DvdGenre " +
                "ON DvdInfo.dvd_info_id = DvdGenre.dvd_info_id " +
                "WHERE DvdGenre.genre_id = @genre_id" +
                " AND (name LIKE @searchtext OR barcode LIKE @searchtext OR author LIKE @searchtext)", cnn);

                command.Parameters.Add(new SqlParameter("@genre_id", genreID));
                command.Parameters.Add(new SqlParameter("@searchtext", "%" + searchText + "%"));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<DvdInfo> dvdlist = new List<DvdInfo>();
                        while (reader.Read())
                        {
                            dvdlist.Add(createDvdInfo(reader));         //Throws NoRecordException
                        }
                        return dvdlist;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get dvd's based on searchtext and genre", ex);
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
                throw new NoRecordException("No records were found - DvdInfoDAO searchDvdWithTextAndGenre()");
            }
        }

        /*
         * Returns a list with the latest dvd's
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<DvdInfo> getLatestDvds(int amount)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {

                command = new SqlCommand("SELECT top (@amount) * FROM DvdInfo ORDER BY date_added DESC", cnn);
                command.Parameters.Add(new SqlParameter("@amount", amount));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<DvdInfo> dvdlist = new List<DvdInfo>();
                        while (reader.Read())
                        {
                            dvdlist.Add(createDvdInfo(reader));             //Throws NoRecordException                   
                        }
                        return dvdlist;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get the latest dvd's", ex);
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
                throw new NoRecordException("No records were found - DvdInfoDAO getLatestDvds()");
            }
        }

        /*
         * Returns a list with popular dvd's
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<DvdInfo> getMostPopularDvds(int amount)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT top (@amount) * FROM DvdInfo ORDER BY amount_sold DESC", cnn);
                command.Parameters.Add(new SqlParameter("@amount", amount));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<DvdInfo> dvdlist = new List<DvdInfo>();
                        while (reader.Read())
                        {
                            dvdlist.Add(createDvdInfo(reader));         //Throws NoRecordException             
                        }
                        return dvdlist;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get popular dvd's", ex);
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
                throw new NoRecordException("No records were found - DvdInfoDAO getMostPopularDvds()");
            }
        }

        /*
         * Returns a list with recommendations
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<int> getRecommendations(int[] genres, int amount)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                //even zo laten en zoeken naar een correcte oplossing om een array als parameter mee te geven
                command = new SqlCommand("select top (@amount) dvd_info_id from dvdGenre where genre_id in (" + genres[0] + "," + genres[1] + "," + genres[2] + ") group by dvd_info_id order by COUNT(dvd_info_id) desc  ", cnn);

                command.Parameters.Add(new SqlParameter("@amount", amount));

                //create a string out of the received genres
                //String values = "";
                //for (int i = 1; i <= genres.Length; i++)
                //{
                //    values += genres[i - 1];
                //    if (i < genres.Length)
                //    {
                //        values += ",";
                //    }
                //}

                //sql.Parameters.Add(new SqlParameter("@values", values));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<int> dvdIds = new List<int>();
                        while (reader.Read())
                        {
                            dvdIds.Add(Convert.ToInt32(reader["dvd_info_id"]));
                        }
                        return dvdIds;
                    }
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to get recommendations", ex);
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
                throw new NoRecordException("No records were found - DvdInfoDAO getRecommendations()");
            }
        }

        /*
         * Adds a dvd
         * Return the id from the newly added dvd
         * Throws DALException if something else went wrong
         */ 
        public int add(DvdInfo dvdInfo)
        {
            SqlCommand command = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("INSERT INTO DvdInfo" +
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
                for (int i = 0; i < dvdInfo.actors.Length; i++)
                {
                    actorsString += dvdInfo.actors[i];
                    if (i < dvdInfo.actors.Length - 1)
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
                    return (int)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to add dvdinfo", ex);
                }
                finally
                {
                    if (cnn != null)
                    {
                        cnn.Close();
                    }
                }
            }
        }       

        /*
         * Updates a dvd
         * Returns true if the dvd was updated, false if no dvdwas updated
         * Throws DALException if something else went wrong
         */ 
        public Boolean update(DvdInfo dvd)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {

                command = new SqlCommand("UPDATE DvdInfo " +
                "SET name = @name, " +
                "year = @year, " +
                "barcode = @barcode, " +
                "author = @author, " +
                "description = @description, " +
                "rent_price = @rent_price, " +
                "buy_price = @buy_price, " +
                "amount_sold = @amount_sold " +
                "WHERE dvd_info_id = @dvd_info_id", cnn);

                command.Parameters.Add(new SqlParameter("@name", dvd.name));
                command.Parameters.Add(new SqlParameter("@year", dvd.year));
                command.Parameters.Add(new SqlParameter("@barcode", dvd.barcode));
                command.Parameters.Add(new SqlParameter("@author", dvd.author));
                command.Parameters.Add(new SqlParameter("@description", dvd.descripion));
                command.Parameters.Add(new SqlParameter("@rent_price", dvd.rent_price));
                command.Parameters.Add(new SqlParameter("@buy_price", dvd.buy_price));
                command.Parameters.Add(new SqlParameter("@amount_sold", dvd.amount_sold));
                command.Parameters.Add(new SqlParameter("@dvd_info_id", dvd.dvd_info_id));

                try
                {
                    cnn.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    return false;                    
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to update dvd", ex);
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
            }
        }

        /*
         * Returns a list with media for a dvd
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */ 
        private List<KeyValuePair<int, String>> getMedia(String id)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("Select * from Media where dvd_info_id = @id order by media_type_id DESC", cnn);
                command.Parameters.Add(new SqlParameter("@id", id));
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<KeyValuePair<int, String>> media = new List<KeyValuePair<int, String>>();
                        while (reader.Read())
                        {
                            KeyValuePair<int, String> mediaObject = new KeyValuePair<int, String>(Convert.ToInt32(reader["media_type_id"]), reader["url"].ToString());
                            media.Add(mediaObject);
                        }
                        return media;
                    }
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to get media for dvd", ex);
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
                throw new NoRecordException("No records were found - DvdInfoDAO getMedia()");
            }
        }

        /*
         * Creates a DvdInfo-Object
         */ 
        private DvdInfo createDvdInfo(SqlDataReader reader)
        {
            return new DvdInfo
            {
                dvd_info_id = Convert.ToInt32(reader["dvd_info_id"]),
                name = Convert.ToString(reader["name"]),
                year = Convert.ToString(reader["year"]),
                barcode = Convert.ToString(reader["barcode"]),
                author = Convert.ToString(reader["author"]),
                descripion = Convert.ToString(reader["description"]),
                rent_price = float.Parse(reader["rent_price"].ToString()),
                buy_price = float.Parse(reader["buy_price"].ToString()),
                date_added = Convert.ToDateTime(reader["date_added"]),
                amount_sold = Convert.ToInt32(reader["amount_sold"]),
                media = getMedia(reader["dvd_info_id"].ToString()),         //Throws NoRecordException
                actors = Convert.ToString(reader["actors"]).Split(','),                
                duration = Convert.ToString(reader["duration"])
            };
        }

        /*
         * Returns a List with all the dvd's
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        /*
        public List<DvdInfo> getAll()
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM DvdInfo", cnn);
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<DvdInfo> dvdlist = new List<DvdInfo>();
                        while (reader.Read())
                        {
                            dvdlist.Add(createDvdInfo(reader));
                        }
                        return dvdlist;
                    }
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to get all the dvd's", ex);
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
                throw new NoRecordException("No records were found - DvdInfoDAO getAllDvdInfos()");
            }
        }*/
    }
}
