﻿using BrainHope.Services.DTO.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using TrackRoom.Services.Interfaces;

namespace TrackRoom.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(Message message)
        {
            var email = new MimeMessage
            {
                Subject = message.Subject,
                Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content }
            };
            email.From.Add(new MailboxAddress("Track Room", _emailConfig.From));
            email.To.AddRange(message.To);

            using var client = new SmtpClient();

            try
            {
                //client.Connect(_emailConfig.SmtpServer, 465, SecureSocketOptions.SslOnConnect);
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);

                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                client.Send(email);
                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Email sending failed: " + ex.Message);
            }
        }

    }
}
