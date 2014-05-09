using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using System.Net.Mail;
using System.Net;

namespace LayeredBusinessModel.BLL.Model
{
    public class EmailModel
    {
        /*Send an email asking for user confirmation.*/
        public void sendRegistrationEmail(Customer customer, Token token)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                //todo: put credentials in web.config
                Credentials = new NetworkCredential("taboelaertraesa@gmail.com", "KathoVives"),
                EnableSsl = true
            };
            client.Send("info@TaboelaertRaesa.com", customer.email, "Taboelaert Raesa account verification", "Dear " + customer.name + ",\n\n" +
                "Welcome to Taboelaert Raesa!\n\n" +
                "To complete your registration process, please click the following link: \n" +
                "http://simonraes-001-site1.smarterasp.net/signup.aspx?token=" + token.token + " .\n\n" +
                "\n\nRegards,\nThe Taboelaert Raesa team.");
        }

        /*Send an email alerting the user he has been registered and verified.*/
        public void sendRegistrationCompleteEmail(Customer customer)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                //todo: put credentials in web.config
                Credentials = new NetworkCredential("taboelaertraesa@gmail.com", "KathoVives"),
                EnableSsl = true
            };
            client.Send("info@TaboelaertRaesa.com", customer.email, "Taboelaert Raesa registration completed", "Dear " + customer.name + ",\n\n" +
                "Your Taboelaert Raesa registraton has been completed.\n\n" +
                "Regards,\nThe Taboelaert Raesa team.");
        }

        /*Send the user an email asking him to confirm his password reset request*/
        public void sendPasswordResetRequestEmail(Customer customer, Token token)
        {
            SmtpClient clienta = new SmtpClient("smtp.gmail.com", 587)
            {
                //todo: put credentials in web.config
                Credentials = new NetworkCredential("taboelaertraesa@gmail.com", "KathoVives"),
                EnableSsl = true
            };
            clienta.Send("info@TaboelaertRaesa.com", customer.email, "Password reset request", "Dear " + customer.name + ",\n\n" +
                "We received a request to reset the password on your Taboelaert Raesa account. Click the following URL to complete the process:\n" +
                "http://simonraes-001-site1.smarterasp.net/ForgotPassword.aspx?resetToken=" + token.token + " .\n\n" +
                "If you did not request this reset, you can ignore this email.\n\nRegards,\nThe Taboelaert Raesa team.");
        }

        /*Send the user an email with his new password.*/
        public void sendPasswordResetCompletedEmail(Customer customer)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                //todo: put credentials in web.config
                Credentials = new NetworkCredential("taboelaertraesa@gmail.com", "KathoVives"),
                EnableSsl = true
            };
            client.Send("info@TaboelaertRaesa.com", customer.email, "Your Taboelaert Raesa password has been reset", "Dear " + customer.name + ",\n\n" +
                "Your Taboelaert Raesa password has been reset to '" + CryptographyModel.decryptPassword(customer.password) + "'.\n\n" +
                "For security reasons, we ask you to change this password the next time you log in." +
                "\n\nRegards,\nThe Taboelaert Raesa team.");
        }

        public void sendOrderConfirmationEmail(Customer customer, Order order, List<OrderLine> orderLines, Boolean allInStock)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                //todo: put credentials in web.config
                Credentials = new NetworkCredential("taboelaertraesa@gmail.com", "KathoVives"),
                EnableSsl = true
            };

            //create email content
            String messageContent = "Dear " + customer.name + ",\n\n" +
                "Thank you for your business. Your order information can be found below.\n\n";
            if (!allInStock)
            {
                messageContent += "Some items in this order are currently out of stock. Your order will be dispatched as soon as they become available.\n\n";
            }
            messageContent += "Order " + order.order_id + " (" + order.orderstatus_name + ")\n";
            foreach (OrderLine orderLine in orderLines)
            {
                messageContent += orderLine.dvd_info_name + " (" + orderLine.order_line_type_name;
                if (orderLine.order_line_type_id == 1)
                {
                    messageContent += " from " + orderLine.startdate + " until " + orderLine.enddate;
                }
                messageContent += ")\n";
            }
            messageContent += "\nRegards,\nThe Taboelaert Raesa team.";


            client.Send("info@TaboelaertRaesa.com", customer.email, "Your Taboelaert Raesa order information", messageContent);
        }
    }
}
