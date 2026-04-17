using MailKit.Net.Smtp;
using MimeKit;

namespace InventoryMgtSystem.Handlers
{
    public class EmailService
    {
        public void SendEmail(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("kwonimong@gmail.com"));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            using var smtp = new SmtpClient();

            try
            {
                smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                smtp.Authenticate("kwonimong@gmail.com", "Kaushalee@123");

                smtp.Send(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email Error: " + ex.Message);
                throw;
            }
            finally
            {
                smtp.Disconnect(true);
            }
        }
    }
}
