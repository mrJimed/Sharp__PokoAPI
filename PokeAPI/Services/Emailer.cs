using MailKit.Net.Smtp;
using MimeKit;
using PokeAPI.Models;

namespace PokeAPI.Services
{
    public class Emailer
    {
        string fromEmail;
        string password;

        public Emailer(string fromEmail, string password)
        {
            this.fromEmail = fromEmail;
            this.password = password;
        }

        public async Task SendEmail(FightStatMail mail)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("PokeAPI", fromEmail));
            emailMessage.To.Add(new MailboxAddress("", mail.Email));
            emailMessage.Subject = "Fight statistic";
            emailMessage.Body = new TextPart("Paint")
            {
                Text = mail.Message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 465, true);
                await client.AuthenticateAsync(fromEmail, password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
