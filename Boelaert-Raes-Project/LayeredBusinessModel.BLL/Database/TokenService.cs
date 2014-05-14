using LayeredBusinessModel.Domain;
using LayeredBusinessModel.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace LayeredBusinessModel.BLL.Database
{
    public class TokenService
    {
        public Token getByID(String token_id)
        {
            TokenDAO tokenDAO = new TokenDAO();
            Token token = tokenDAO.getTokenByToken(token_id);
            if (token != null)
            {
                //token bestaat wel
                //customer opvragen en in token plaatsen
               
                token.customer = new CustomerService().getByID(token.customer.customer_id.ToString());      //throws Throws NoRecordException || DALException
             
            }
            return token;
        }

        public List<Token> getTokensForCustomer(Customer customer)
        {
            TokenDAO tokenDAO = new TokenDAO();
            return tokenDAO.getTokensForCustomer(customer);           
        }

        public void addToken(Token token)
        {
            TokenDAO tokenDAO = new TokenDAO();
            tokenDAO.addToken(token);
        }

        public Boolean deleteToken(Token token)
        {
            TokenDAO tokenDAO = new TokenDAO();
            return tokenDAO.removeToken(token);
        }

        public Boolean addTokenForCustomer(Customer customer, TokenStatus status)
        {
            Token token = new Token()
            {
                customer = customer,
                token = generateToken(),
                timestamp = DateTime.Now,
                status = status
            };
            TokenDAO tokenDAO = new TokenDAO();
            return tokenDAO.addToken(token);
        }


        private String generateToken()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] random = new byte[8];
            rng.GetBytes(random);
            return rng.ToString();  
        }
    }
}
