using MimeKit;
using Sella_API.Model;
namespace Sella_API.Helpers
{
    public class EmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public void SendEmail(EmailModel emailModel)
        {
            var EmailMsg = new MimeMessage();
            var From = configuration["EmailSettings:From"];
            EmailMsg.From.Add(new MailboxAddress("Sella", From));
            EmailMsg.To.Add(new MailboxAddress(emailModel.TO, emailModel.TO));
            EmailMsg.Subject = emailModel.Subject;
            EmailMsg.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(emailModel.Content)
            };

            using (var Client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    Client.Connect(configuration["EmailSettings:SmtpServer"], 465, true);
                    Client.Authenticate(configuration["EmailSettings:From"],
                        configuration["EmailSettings:Password"]);
                    Client.Send(EmailMsg);

                }
                catch (Exception Ex)
                {
                    throw;
                }
                finally
                {
                    Client.Disconnect(true);
                    Client.Dispose();
                }
            }
        }
    }
}
