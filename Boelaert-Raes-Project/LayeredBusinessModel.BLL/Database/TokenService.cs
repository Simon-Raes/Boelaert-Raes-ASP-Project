using LayeredBusinessModel.Domain;
using LayeredBusinessModel.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace LayeredBusinessModel.BLL
{
    public class TokenService
    {
        public Token getByID(String token_id)
        {
            Token token = new TokenDAO().getByToken(token_id);          //throws Throws NoRecordException           
            //customer opvragen en in token plaatsen
            token.customer = new CustomerService().getByID(token.customer.customer_id.ToString());      //throws Throws NoRecordException || DALException            
            return token;
        }

        public List<Token> getByCustomer(Customer customer)
        {
            return new TokenDAO().getByCustomer(customer);          //throws Throws NoRecordException                    
        }

        public Boolean add(Token token)
        {
            return new TokenDAO().add(token);
        }

        public Boolean deleteToken(Token token)
        {
            return new TokenDAO().delete(token);
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
