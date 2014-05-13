using LayeredBusinessModel.Domain;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.DAO
{
    public class TokenDAO : DAO
    {

        public List<Token> getTokensForCustomer(Customer customer) {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM Tokens where customer_id=@customer_id", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id", customer.customer_id));
                
                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<Token> tokens = new List<Token>();
                        while (reader.Read())
                        {
                            tokens.Add(createToken(reader));
                        }
                        return tokens;
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

        public Token getTokenByToken(String token)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("SELECT * FROM Tokens WHERE token = @token", cnn);
                command.Parameters.Add(new SqlParameter("@token", token));

                try
                {
                    cnn.Open();
                    reader = command.ExecuteReader();
                    if(reader.HasRows)
                    {
                        reader.Read();                    
                        return createToken(reader);
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

        //public Boolean removeToken(String token) {
        //    using (var cnn = new SqlConnection(sDatabaseLocatie))
        //    {
        //        SqlCommand command = new SqlCommand("DELETE FROM Tokens WHERE token = @token", cnn);
        //        command.Parameters.Add(new SqlParameter("@token", token));
        //        try
        //        {
        //            cnn.Open();
        //            command.ExecuteNonQuery();
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        finally
        //        {
        //            cnn.Close();
        //        }
        //        return false;
        //    }
        //}

        public Boolean removeToken(Token token)
        {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("DELETE FROM Tokens WHERE token = @token", cnn);
                command.Parameters.Add(new SqlParameter("@token", token.token));
                try
                {
                    cnn.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (cnn != null)
                    {
                        cnn.Close();
                    }
                }
                return false;
            }
        }

        public Boolean removeTokensForCustomer(Customer customer) {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("DELETE FROM Tokens WHERE customer_id = @customer_id", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id",  customer.customer_id));
                try
                {
                    cnn.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (cnn != null)
                    {
                        cnn.Close();
                    }
                }
                return false;
            }
        }

        public Boolean addToken(Token token) 
        {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {

                command = new SqlCommand("INSERT INTO Tokens (tokenstatus_id,customer_id,token,timestamp)" +
                "VALUES(@tokenstatus_id,@customer_id,@token,@timestamp)", cnn);

                command.Parameters.Add(new SqlParameter("@tokenstatus_id", getTokenStatusID(token.status)));
                command.Parameters.Add(new SqlParameter("@customer_id", token.customer.customer_id));
                command.Parameters.Add(new SqlParameter("@token", token.token));
                command.Parameters.Add(new SqlParameter("@timestamp", token.timestamp));

                try
                {
                    cnn.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (cnn != null)
                    {
                        cnn.Close();
                    }
                }
                return false;
            }
        }

        protected Token createToken(SqlDataReader reader)
        {
            return new Token
            {
                status = getTokenStatus(Convert.ToInt16(reader["tokenstatus_id"])),
                token = reader["token"].ToString(),
                customer = makeCustomer(Convert.ToInt16(reader["customer_id"])),
                timestamp = Convert.ToDateTime(reader["timestamp"])  
            };
        }

        private TokenStatus getTokenStatus(int i)
        {
            switch (i)
            {
                case 1: return TokenStatus.VERIFICATION;
                case 2: return TokenStatus.RECOVERY;
            }
            return TokenStatus.EMPTY;
        }

        private int getTokenStatusID(TokenStatus t) 
        {
            switch(t) {
                case TokenStatus.EMPTY: 
                    return -1;
                case TokenStatus.VERIFICATION:
                    return 1;
                case TokenStatus.RECOVERY:
                    return 2;
            }
            return -1;
        }
           
        private Customer makeCustomer(int i)
        {
            Customer c = new Customer();
            c.customer_id = i;
            return c;
        }
    }
}
