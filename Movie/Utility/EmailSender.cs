
using System.Net.Mail;
using System.Net;
using System.Configuration;
using Microsoft.Extensions.Options;

namespace Movie.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            this._emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            //using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
            //{
            //    client.EnableSsl = true;
            //    client.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password);

            //    var mail = new MailMessage
            //    {
            //        From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
            //        Subject = subject,
            //        Body = body,
            //        IsBodyHtml = true
            //    };
            //    mail.To.Add(toEmail);

            //    await client.SendMailAsync(mail);
            //}



            using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password);

                var mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.SenderEmail,_emailSettings.SenderName),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = body
                };
                mail.To.Add(toEmail);
                await client.SendMailAsync(mail);
            }
        }
    }
}

