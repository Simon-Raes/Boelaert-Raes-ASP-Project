using LayeredBusinessModel.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.DAO
{
    public class OrderLineTypeDAO : DAO
    {
        public OrderLineType getOrderLineTypeForID(String id)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM OrderLineType WHERE order_line_type_id = @order_line_type_id", cnn);
                command.Parameters.Add(new SqlParameter("@order_line_type_id",id));
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return createOrderLineStatus(reader);                        
                    }
                }
                catch (Exception ex)
                {

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
                return null;
            }
        }

        private OrderLineType createOrderLineStatus(SqlDataReader reader)
        {
            return new OrderLineType
            {
                id= Convert.ToInt16(reader["order_line_type_id"]),
                name = reader["name"].ToString()
            };
        }
    }
}
