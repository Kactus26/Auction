using AuctionIdentity.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http.HttpResults;
using MimeKit;

namespace AuctionIdentity.Services
{
    public class MailService : IMailService
    {
        public string SendEmail(string email)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("AuctionCEO", "sasha.baginsky@gmail.com"));
            message.To.Add(new MailboxAddress("Favourite Client", email));
            message.Subject = "Confirm your email";

            string code = GeneratePassword();
            message.Body = new TextPart("plain")
            {
                Text = $"This is your code: {code}. If you don't know what is it all about, just ignore this message <3"
            };

            // Отправка письма
            using (var client = new SmtpClient())
            {
                try
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
                catch (Exception ex)
                {
                    return new (ex.Message);
                }

                return new (code);
            }
        }

        private string GeneratePassword()
        {
            Random random = new Random();
            char[] digits = new char[6];

            for (int i = 0; i < 6; i++)
            {
                digits[i] = (char)('0' + random.Next(10));
            }

            return new string(digits);
        }
    }
}
