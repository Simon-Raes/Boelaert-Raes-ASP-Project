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
    //Superclass for all DAO classes so the connectionstring only has to be changed in one location.
    public class DAO
    {
        protected string strSQL;
        //pick one:
        protected string sDatabaseLocatie = ConfigurationManager.ConnectionStrings["local"].ConnectionString; //local testing database
        //protected string sDatabaseLocatie = ConfigurationManager.ConnectionStrings["ProjectConnection"].ConnectionString; //online host database
        protected SqlConnection cnn;
    }
}
