﻿using System;
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
    public class GenreDAO : DAO
    {
        /*
         * Returns a genre based on an ID
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public Genre getByID(String genreID)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM genres WHERE genre_id = @genre_id", cnn);
                command.Parameters.Add(new SqlParameter("@genre_id", genreID));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return createGenre(reader);         //Throws NoRecordException || DALException
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get a genre based on an ID", ex);
                }
                finally
                {
                    if (cnn != null)
                    {
                        cnn.Close();
                    }
                }
                throw new NoRecordException("No records were found - GenreDAO getGenre()");
            }
        }

        /*
         * Returns a list with all the genres for a certain category
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<Genre> getByCategory(String categoryID)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM genres WHERE category_id = @cat_id", cnn);
                command.Parameters.Add(new SqlParameter("@cat_id", categoryID));
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<Genre> genrelist = new List<Genre>();
                        while (reader.Read())
                        {
                            genrelist.Add(createGenre(reader));         //Throws NoRecordException || DALException
                        }
                        return genrelist;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get all the genres for a certain category", ex);
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
                throw new NoRecordException("No records were found - GenreDAO getGenresForCategory()");
            }
        }

        /*
         * Returns a list with all the genres for a certain dvd
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<Genre> getByDvd(String dvd_id)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM Genres " +
                    "JOIN DvdGenre " +
                    "ON DvdGenre.genre_id = Genres.genre_id " +
                    "WHERE DvdGenre.dvd_info_id = @dvdinfo_id", cnn);
                command.Parameters.Add(new SqlParameter("@dvdinfo_id", dvd_id));
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<Genre> genrelist = new List<Genre>();
                        while (reader.Read())
                        {
                            genrelist.Add(createGenre(reader));         //Throws NoRecordException || DALException
                        }
                        return genrelist;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get all the genres for a certain dvd", ex);
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
                throw new NoRecordException("No records were found - GenreDAO getGenresForDvd()");
            }
        }

        /*
         * Returns a list with all the genres
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<Genre> getAll()
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM genres", cnn);
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<Genre> genrelist = new List<Genre>();
                        while (reader.Read())
                        {
                            genrelist.Add(createGenre(reader));     //Throws NoRecordException || DALException
                        }
                        return genrelist;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is NoRecordException || ex is DALException)
                    {
                        throw;
                    }
                    throw new DALException("Failed to get all the genres", ex);
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
                throw new NoRecordException("No records were found - GenreDAO getGenres()");
            }
        }

        private Genre createGenre(SqlDataReader reader)
        {
            return new Genre
            {
                genre_id = Convert.ToInt32(reader["genre_id"]),
                category = new CategoryDAO().getByID(reader["category_id"].ToString()),         //Throws NoRecordException || DALException
                name = Convert.ToString(reader["name"])
            };
        }
    }
}
