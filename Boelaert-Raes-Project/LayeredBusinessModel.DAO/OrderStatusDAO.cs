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

        public OrderStatus getOrderStatusByID(int id) 
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM OrderStatus WHERE orderstatus_id = @orderstatus_id", cnn);
                command.Parameters.Add(new SqlParameter("@orderstatus_id", id));
                try
                {
                    OrderStatus orderstatus = null;

                    cnn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        orderstatus = createOrderStatus(reader);
                    }   
                    reader.Close();
                    return orderstatus;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    cnn.Close();
                }
                return null;
            }        
        }

        private OrderStatus createOrderStatus(SqlDataReader reader)
        {
            OrderStatus orderstatus = new OrderStatus
            {
                id = Convert.ToInt16(reader["orderstatus_id"]),
                name = reader["name"].ToString()
            };
            return orderstatus;
        }
 
    }
}
