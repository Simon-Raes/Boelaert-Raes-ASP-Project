using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;

namespace LayeredBusinessModel.BLL
{
    public class PasswordResetModel
    {
        /*Send the user an email explaining we received a password reset request.*/
        public void sendPasswordResetRequest(Customer user)
        {
            //create a database record for this reset request
            Token token = new Token
            {
                customer = user,
                status = TokenStatus.RECOVERY,
                timestamp = DateTime.Now,
                token = Util.randomString(10)
            };
            if (new TokenService().add(token))
            {
                //succes
            }

            //send the user an email asking if he wants to reset his password
            new EmailModel().sendPasswordResetRequestEmail(user, token);
        }

        /*Checks the incoming password reset request.
         Returns true if the password has been reset and the user received an email with his new password.
         Returns false if something went wrong, no password was reset, no mail was sent.*/
        public Boolean checkResetRequestConfirmation(String token_id)
        {
            Boolean didReset = false;
            TokenService tokenService = new TokenService();
            Token token = tokenService.getByID(token_id);           //Throws NoRecordException
            if (token != null)
            {
                //give the user a new password
                Customer customer = token.customer;
                customer.password = CryptographyModel.encryptPassword(Util.randomString(10));
                new CustomerService().updateCustomer(customer);

                //send the user an email with his new password
                new EmailModel().sendPasswordResetCompletedEmail(customer);
                didReset = true;

                //remove the token from the database so the user can't reset his password again with the same url later
                if (tokenService.deleteToken(token))
                {
                    //success
                }
            }
            return didReset;
        }

        public void resetPassword(Customer user)
        {

        }
    }
}
