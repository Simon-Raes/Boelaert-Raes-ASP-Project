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
        public OrderLineType getOrderLineTypeForID(int id)
        {
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {

                SqlCommand command = new SqlCommand("SELECT * FROM OrderLineType WHERE order_line_type_id = @order_line_type_id", cnn);
                command.Parameters.Add(new SqlParameter("@order_line_type_id",id));
                try
                {
                    OrderLineType orderLineType = null;
                    cnn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while(reader.Read()) 
                    {
                        orderLineType = createOrderLineStatus(reader);
                    }                    
                    reader.Close();
                    return orderLineType;

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



        private OrderLineType createOrderLineStatus(SqlDataReader reader)
        {
            OrderLineType orderLineType = new OrderLineType
            {
                id= Convert.ToInt16(reader["order_line_type_id"]),
                name = reader["name"].ToString()
            };
            return orderLineType;
        }


    }
}
