using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Net.Configuration;


namespace LayeredBusinessModel.BLL.Model
{
    public class EmailModel
    {
        //todo: a way to get the urls from a relative location so no domain-url hardcoding is requried


        /*Send an email asking for user confirmation.*/
        public void sendRegistrationEmail(Token token)
        {
            getSmtpClient().Send("info@TaboelaertRaesa.com", token.customer.email, "Taboelaert Raesa account verification", "Dear " + token.customer.name + ",\n\n" +
                "Thank you for registering with Taboelaert Raesa.\n\n" +
                "To complete your registration process, please click the following link: \n" +
                "http://simonraes-001-site1.smarterasp.net/signup.aspx?token=" + token.token + " \n\n" +
                "Regards,\nThe Taboelaert Raesa team.");
        }

        /*Send an email alerting the user he has been registered and verified.*/
        public void sendRegistrationCompleteEmail(Customer customer)
        {
            getSmtpClient().Send("info@TaboelaertRaesa.com", customer.email, "Taboelaert Raesa registration completed", "Dear " + customer.name + ",\n\n" +
                "Your registration has been completed. Welcome to Taboelaert Raesa!\n\n" +
                "Regards,\nThe Taboelaert Raesa team.");
        }

        /*Send the user an email asking him to confirm his password reset request*/
        public void sendPasswordResetRequestEmail(Customer customer, Token token)
        {
            getSmtpClient().Send("info@TaboelaertRaesa.com", customer.email, "Password reset request", "Dear " + customer.name + ",\n\n" +
                "We received a request to reset the password on your Taboelaert Raesa account. Click the following URL to complete the process:\n" +
                "http://simonraes-001-site1.smarterasp.net/ForgotPassword.aspx?resetToken=" + token.token + " \n\n" +
                "If you did not request this reset, you can ignore this email.\n\nRegards,\nThe Taboelaert Raesa team.");
        }

        /*Send the user an email with his new password.*/
        public void sendPasswordResetCompletedEmail(Customer customer)
        {
            getSmtpClient().Send("info@TaboelaertRaesa.com", customer.email, "Your Taboelaert Raesa password has been reset", "Dear " + customer.name + ",\n\n" +
                "Your Taboelaert Raesa password has been reset to '" + CryptographyModel.decryptPassword(customer.password) + "'.\n\n" +
                "For security reasons, we ask you to change this password the next time you log in." +
                "\n\nRegards,\nThe Taboelaert Raesa team.");
        }

        //todo: include orderLine en Order price in email!
        public void sendOrderConfirmationEmail(Customer customer, Order order, List<OrderLine> orderLines, Boolean allInStock, String currency)
        {
            //create HTML email content
            String messageContent = "Dear " + customer.name + ",<br /><br />" +
                "Thank you for your business. Your order information can be found below.<br /><br />";
            if (!allInStock)
            {
                messageContent += "Some items in this order are currently out of stock. Your order will be dispatched as soon as they become available.<br /><br />";
            }
            messageContent += "Order " + order.order_id + " (" + order.orderstatus.name + ")<br />";
            messageContent += "<table>";
            double orderTotal = 0;
            foreach (OrderLine orderLine in orderLines)
            {
                messageContent += "<tr><td style='padding: 5px; border: 1px solid #ddd;'>";
                messageContent += orderLine.dvdInfo.name+"</td>";
                if (orderLine.orderLineType.id == 1)
                {
                    messageContent += "<td style='padding: 5px; border: 1px solid #ddd;'>" + orderLine.orderLineType.name + " from " + orderLine.startdate + " until " + orderLine.enddate + "</td>";
                    messageContent += "<td style='padding: 5px; border: 1px solid #ddd;'>" + currency + " " + orderLine.dvdInfo.rent_price + "</td>";
                    orderTotal += orderLine.dvdInfo.rent_price;
                }
                else
                {
                    messageContent += "<td style='padding: 5px; border: 1px solid #ddd;'>"+orderLine.orderLineType.name+"</td>";
                    messageContent += "<td style='padding: 5px; border: 1px solid #ddd;'>"+ currency + " " + orderLine.dvdInfo.buy_price + "</td>";
                    orderTotal += orderLine.dvdInfo.buy_price;
                }
                
                messageContent += "</tr>";                
            }
            messageContent += "</table>";


            messageContent += "</br>";
            messageContent += "Total " + currency + " " +Math.Round(orderTotal, 2);


            messageContent += "<br /><br />Regards,<br />The Taboelaert Raesa team.";
            
            
            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress("info@taboelaertraesa.com");
            message.From = fromAddress;

            message.To.Add(order.customer.email);
            message.Subject = "Your Taboelaert Raesa order information";
            message.IsBodyHtml = true;
            message.Body = messageContent;

            getSmtpClient().Send(message);
        }

        /*Returns SMTP credentials from web.config*/
        private SmtpClient getSmtpClient()
        {
            var client = new System.Net.Mail.SmtpClient();
            var credential = (System.Net.NetworkCredential)client.Credentials;
            client.EnableSsl = true;
            string strHost = client.Host;
            int port = client.Port;

            return client;
        }
    }
}
