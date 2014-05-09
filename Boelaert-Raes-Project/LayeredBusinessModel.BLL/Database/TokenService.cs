﻿using LayeredBusinessModel.Domain;
using LayeredBusinessModel.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace LayeredBusinessModel.BLL.Database
{
    class TokenService
    {
        public Token checkToken(String t)
        {
            TokenDAO tokenDAO = new TokenDAO();
            Token token = tokenDAO.getTokenByToken(t);
            if (token != null)
            {
                //token bestaat wel
                //customer opvragen en in token plaatsen
                CustomerService c = new CustomerService();
                token.customer = c.getCustomerByID(token.customer.customer_id);
            }
            return token;
        }

        public Boolean deleteTokenbyToken(String token)
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
