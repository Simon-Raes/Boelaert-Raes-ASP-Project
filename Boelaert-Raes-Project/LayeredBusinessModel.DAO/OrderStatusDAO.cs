using CustomException;
using LayeredBusinessModel.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.DAO
{
    public class OrderStatusDAO : DAO
    {
        /*
         * Returns an Orderstatus based on an ID
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public OrderStatus getOrderStatusByID(String id) 
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM OrderStatus WHERE orderstatus_id = @orderstatus_id", cnn);
                command.Parameters.Add(new SqlParameter("@orderstatus_id", id));
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if(reader.HasRows)
                    {
                        reader.Read();
                        return createOrderStatus(reader);
                    }
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to get orderstatus based on an ID", ex);
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
                throw new NoRecordException("No records were found - OrderStatusDAO getOrderStatusByID()");
            }        
        }

        private OrderStatus createOrderStatus(SqlDataReader reader)
        {
            return new OrderStatus
            {
                id = Convert.ToInt16(reader["orderstatus_id"]),
                name = reader["name"].ToString()
            };
        }
 
    }
}
