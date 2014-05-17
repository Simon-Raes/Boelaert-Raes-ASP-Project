using LayeredBusinessModel.Domain;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomException;

namespace LayeredBusinessModel.DAO
{
    public class TokenDAO : DAO
    {
        /*
         * Returns a list with tokens for customer
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public List<Token> getByCustomer(Customer customer) {
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
                    throw new DALException("Failed to get tokens for customer", ex);
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
                throw new NoRecordException("No records were found - TokenDAO getTokensForCustomer()");
            }
        }

        /*
         * Returns a token based on the tokenstring
         * Throws NoRecordException if no records were found
         * Throws DALException if something else went wrong
         */
        public Token getByToken(String token)
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
                    throw new DALException("Failed to get token based on tokenstring", ex);
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
                throw new NoRecordException("No records were found - TokenDAO getTokenByToken()");
            }
        }

        /*
         * Removes a token
         * Returns true if records were deleted, false if not
         * Throws DALException if something else went wrong
         */
        public Boolean delete(Token token)
        {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("DELETE FROM Tokens WHERE token = @token", cnn);
                command.Parameters.Add(new SqlParameter("@token", token.token));
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
                    throw new DALException("Failed to delete token", ex);
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
         * Adds a tokens
         * Returns true if records were inserted, false if not
         * Throws DALException if something else went wrong
         */
        public Boolean add(Token token) 
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
                    if (command.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw new DALException("Failed to add a tokens", ex);
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

        protected Token createToken(SqlDataReader reader)
        {
            return new Token
            {
                status = getTokenStatus(Convert.ToInt16(reader["tokenstatus_id"])),
                token = reader["token"].ToString(),
                customer = makeCustomer(Convert.ToString(reader["customer_id"])),
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
        
        /*
         * Creates a Customer-Object
         */ 
        private Customer makeCustomer(String i)
        {
            CustomerDAO customerDAO = new CustomerDAO();
            return customerDAO.getByID(i);
            //Customer c = new Customer();
            //c.customer_id = i;
            //return c;
        }

        /*
         * Removes tokens for customer
         * Returns true if records were deleted, false if not
         * Throws DALException if something else went wrong
         */
        /*
        public Boolean deleteByCustomer(Customer customer) {
            SqlCommand command = null;
            using (var cnn = new SqlConnection(sDatabaseLocatie))
            {
                command = new SqlCommand("DELETE FROM Tokens WHERE customer_id = @customer_id", cnn);
                command.Parameters.Add(new SqlParameter("@customer_id",  customer.customer_id));
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
                    throw new DALException("Failed to delete tokens for customers", ex);
                }
                finally
                {
                    if (cnn != null)
                    {
                        cnn.Close();
                    }
                }
            }
        }*/

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
    }
}
