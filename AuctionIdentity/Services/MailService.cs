using AuctionIdentity.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AuctionIdentity.Services
{
    public class MailService : IMailService
    {
        public void SendEmail()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your Name", "sasha.baginsky@gmail.com"));
            message.To.Add(new MailboxAddress("Recipient Name", "sanya.baginsky@gmail.com"));
            message.Subject = "Test Email";

            // Тело письма
            message.Body = new TextPart("plain")
            {
                Text = @"Hello, this is a test email!"
            };

            // Отправка письма
            using (var client = new SmtpClient())
            {

                // Подключаемся к SMTP-серверу Gmail
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                // Указываем свои учетные данные
                // Используйте созданный пароль приложения вместо вашего основного пароля
                client.Authenticate("sasha.baginsky@gmail.com", "aiph kcee pvvj atwp");

                // Отправляем письмо
                client.Send(message);
                Console.WriteLine("Email sent successfully!");

                client.Disconnect(true);

            }
        }
    }
}
