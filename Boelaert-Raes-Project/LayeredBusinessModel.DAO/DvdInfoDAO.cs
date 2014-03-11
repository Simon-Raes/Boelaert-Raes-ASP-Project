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
    public class DvdInfoDAO
    {
        private string strSQL;
        private string sDatabaseLocatie = ConfigurationManager.ConnectionStrings["ProjectConnection"].ConnectionString;
        private SqlConnection cnn;

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

        public List<DvdInfo> getAllWithTitleSearch(String searchText)
        {
            cnn = new SqlConnection(sDatabaseLocatie);
            List<DvdInfo> dvdlist = new List<DvdInfo>();
            
            //todo: parameters
            //beter met CONTAINS dan wildcards+LIKE?
            SqlCommand command = new SqlCommand("SELECT * FROM DvdInfo WHERE name LIKE '%"+searchText+"%';", cnn);
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


        private DvdInfo createDvdInfo(SqlDataReader reader)
        {
            //gebruik van object initializer ipv constructor
            DvdInfo dvd = new DvdInfo
            {
                dvd_info_id = Convert.ToInt32(reader["dvd_info_id"]),
                name = Convert.ToString(reader["name"]),
                code = Convert.ToString(reader["code"]),
                author = Convert.ToString(reader["author"]),
                image = Convert.ToString(reader["image"]),
            };
            return dvd;
        }
    }
}
