﻿using System;
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
        /*Send an email asking for user confirmation.*/
        public void sendRegistrationEmail(Customer customer, Token token)
        {
            var client = getSmtpClient();                      

            client.Send("info@TaboelaertRaesa.com", customer.email, "Taboelaert Raesa account verification", "Dear " + customer.name + ",\n\n" +
                "Welcome to Taboelaert Raesa!\n\n" +
                "To complete your registration process, please click the following link: \n" +
                "http://simonraes-001-site1.smarterasp.net/signup.aspx?token=" + token.token + " .\n\n" +
                "\n\nRegards,\nThe Taboelaert Raesa team.");
        }

        /*Send an email alerting the user he has been registered and verified.*/
        public void sendRegistrationCompleteEmail(Customer customer)
        {
            var client = getSmtpClient();  

            client.Send("info@TaboelaertRaesa.com", customer.email, "Taboelaert Raesa registration completed", "Dear " + customer.name + ",\n\n" +
                "Your Taboelaert Raesa registraton has been completed.\n\n" +
                "Regards,\nThe Taboelaert Raesa team.");
        }

        /*Send the user an email asking him to confirm his password reset request*/
        public void sendPasswordResetRequestEmail(Customer customer, Token token)
        {
            var client = getSmtpClient();  

            client.Send("info@TaboelaertRaesa.com", customer.email, "Password reset request", "Dear " + customer.name + ",\n\n" +
                "We received a request to reset the password on your Taboelaert Raesa account. Click the following URL to complete the process:\n" +
                "http://simonraes-001-site1.smarterasp.net/ForgotPassword.aspx?resetToken=" + token.token + " .\n\n" +
                "If you did not request this reset, you can ignore this email.\n\nRegards,\nThe Taboelaert Raesa team.");
        }

        /*Send the user an email with his new password.*/
        public void sendPasswordResetCompletedEmail(Customer customer)
        {
            var client = getSmtpClient();  

            client.Send("info@TaboelaertRaesa.com", customer.email, "Your Taboelaert Raesa password has been reset", "Dear " + customer.name + ",\n\n" +
                "Your Taboelaert Raesa password has been reset to '" + CryptographyModel.decryptPassword(customer.password) + "'.\n\n" +
                "For security reasons, we ask you to change this password the next time you log in." +
                "\n\nRegards,\nThe Taboelaert Raesa team.");
        }

        //todo: include orderLine en Order price in email!
        public void sendOrderConfirmationEmail(Customer customer, Order order, List<OrderLine> orderLines, Boolean allInStock)
        {
            var client = getSmtpClient();  

            //create email content
            String messageContent = "Dear " + customer.name + ",<br /><br />" +
                "Thank you for your business. Your order information can be found below.<br /><br />";
            if (!allInStock)
            {
                messageContent += "Some items in this order are currently out of stock. Your order will be dispatched as soon as they become available.<br /><br />";
            }
            messageContent += "Order " + order.order_id + " (" + order.orderstatus.name + ")<br />";
            messageContent += "<table>";
            foreach (OrderLine orderLine in orderLines)
            {
                messageContent += "<tr><td style='padding: 5px; border: 1px solid #ddd;'>";
                messageContent += orderLine.dvdInfo.name+"</td>";
                if (orderLine.orderLineType.id == 1)
                {
                    messageContent += "<td style='padding: 5px; border: 1px solid #ddd;'>" + orderLine.orderLineType.name + " from " + orderLine.startdate + " until " + orderLine.enddate + "</td>";
                }
                else
                {
                    messageContent += "<td style='padding: 5px; border: 1px solid #ddd;'></td>";
                }
                messageContent += "</tr>";                
            }
            messageContent += "</table>";
            messageContent += "<br /><br />Regards,<br />The Taboelaert Raesa team.";
            
            
            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress("info@taboelaertraesa.com");
            message.From = fromAddress;

            message.To.Add("simonraes@gmail.com");
            message.Subject = "Your Taboelaert Raesa order information";
            message.IsBodyHtml = true;
            message.Body = messageContent;

            client.Send(message);
        }

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
