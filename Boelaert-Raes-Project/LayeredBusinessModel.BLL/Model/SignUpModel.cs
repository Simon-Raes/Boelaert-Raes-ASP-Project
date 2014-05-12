using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL.Database;

namespace LayeredBusinessModel.BLL.Model
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
                //create token
                TokenService tokenService = new TokenService();
                //need to get the customer from the database so we have his customer_id
                Customer customer = customerService.getCustomerWithEmail(user.email);
                Token token = new Token
                {
                    customer = customer,
                    status = TokenStatus.VERIFICATION,
                    timestamp = DateTime.Now,
                    token = Util.randomString(10)
                };
                tokenService.addToken(token);

                //send email asking for verification
                EmailModel emailModel = new EmailModel();
                emailModel.sendRegistrationEmail(customer, token);
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
            CustomerService customerService = new CustomerService();
            if (!customerService.updateCustomer(customer))
            {
                status = false;
            }

            //send a second email confirming the verification
            EmailModel emailModel = new EmailModel();
            emailModel.sendRegistrationCompleteEmail(customer);

            //delete the token since it will never be used again
            TokenService tokenService = new TokenService();
            if(!tokenService.deleteToken(token))
            {
                status = false;
            }
            
            return status;
        }
    }
}
