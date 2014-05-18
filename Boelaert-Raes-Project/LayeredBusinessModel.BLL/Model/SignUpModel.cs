using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;

namespace LayeredBusinessModel.BLL
{
    public class SignUpModel
    {
        /*Starts the sign up process.*/
        public Boolean beginSignUpProcess(Customer user)
        {
            CustomerService customerService = new CustomerService();
            Boolean addedUser = false;
            if (customerService.addCustomer(user))
            {
                addedUser = true;
                //need to get the customer from the database so we have his customer_id
                Customer customer = customerService.getByEmail(user.email);             //Throws NoRecordException || DALException
                Token token = new Token
                {
                    customer = customer,
                    status = TokenStatus.VERIFICATION,
                    timestamp = DateTime.Now,
                    token = Util.randomString(10)
                };
                if (new TokenService().add(token))
                {
                    //success
                }

                //send email asking for verification
                EmailModel emailModel = new EmailModel();
                emailModel.sendRegistrationVerificationEmail(token);
            }
            else
            {
                addedUser = false;
            }
            return addedUser;
        }

        /*Verifies the user of the supplied token*/
        public Boolean completeSignUpProcess(Token token)
        {
            Boolean status = true;

            //edit customer object to set verified to true
            Customer customer = token.customer;
            customer.isVerified = true;
            if (!new CustomerService().updateCustomer(customer))
            {
                status = false;
            }

            //send a second email confirming the verification
            new EmailModel().sendRegistrationCompleteEmail(customer);

            //delete the token since it will never be used again
            if (!new TokenService().deleteToken(token))
            {
                status = false;
            }
            return status;
        }

        /*Sends a verification email to the supplied email address. Will only work if the user actually has an unused verification token in the database.*/
        public Boolean sendVerificationForEmail(String email)
        {
            Boolean status = false;

            Customer customer = new CustomerService().getByEmail(email);          //Throws NoRecordException || DALException                        
            List<Token> tokens = new TokenService().getByCustomer(customer);            //throws Throws NoRecordException
            
            Token verificationToken = null;
            foreach (Token token in tokens)
            {
                if (token.status == TokenStatus.VERIFICATION)
                {
                    verificationToken = token;
                }
            }
            if (verificationToken != null)
            {
                status = true;
                new EmailModel().sendRegistrationVerificationEmail(verificationToken);
            }
            return status;
        }
    }
}
