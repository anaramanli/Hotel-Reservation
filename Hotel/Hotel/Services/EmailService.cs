//using Hotel.Interfaces;
//using System.Net;
//using System.Net.Mail;

//namespace Hotel.Services
//{
//    public class EmailService : IEmailService
//    {
//        private readonly IConfiguration _configuration;

//        public EmailService(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }


//        public async Task SendMailAsync(string emailTo, string subject, string body, bool isHtml = false)
//        {
//            SmtpClient smtpClient = new SmtpClient(_configuration["Email:Host"], Convert.ToInt32(_configuration["Email:Port"]));
//            smtpClient.EnableSsl = true;
//            smtpClient.Credentials = new NetworkCredential(_configuration["Email:LoginEmail"], _configuration["Email:Password"]);

//            MailAddress from = new MailAddress(_configuration["Email:LoginEmail"], "Caspian Horizon");
//            MailAddress to = new MailAddress(emailTo);

//            MailMessage message = new MailMessage(from, to);
//            message.Subject = subject;
//            message.Body = body;
//            message.IsBodyHtml = isHtml;

//            await smtpClient.SendMailAsync(message);

//        }

//    }
//}
using Hotel.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Hotel.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string emailTo, string subject, string body, bool isHtml = false)
        {
            using (var smtpClient = new SmtpClient(_configuration["Email:Host"], Convert.ToInt32(_configuration["Email:Port"])))
            {
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(_configuration["Email:LoginEmail"], _configuration["Email:Password"]);

                var from = new MailAddress(_configuration["Email:LoginEmail"], "Caspian Horizon");
                var to = new MailAddress(emailTo);

                using (var message = new MailMessage(from, to))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = isHtml;

                    await smtpClient.SendMailAsync(message);
                }
            }
        }

        public async Task SendMailWithEmbeddedImageAsync(string emailTo, string subject, string body, byte[] imageBytes, string contentId, bool isHtml = false)
        {
            using (var smtpClient = new SmtpClient(_configuration["Email:Host"], Convert.ToInt32(_configuration["Email:Port"])))
            {
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(_configuration["Email:LoginEmail"], _configuration["Email:Password"]);

                var from = new MailAddress(_configuration["Email:LoginEmail"], "Caspian Horizon");
                var to = new MailAddress(emailTo);

                using (var message = new MailMessage(from, to))
                {
                    message.Subject = subject;
                    message.IsBodyHtml = isHtml;

                    // Create the alternate view for HTML content
                    var htmlView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

                    // Create the linked resource for the image
                    using (var imageStream = new MemoryStream(imageBytes))
                    {
                        var linkedResource = new LinkedResource(imageStream, MediaTypeNames.Image.Png)
                        {
                            ContentId = contentId,
                            TransferEncoding = TransferEncoding.Base64
                        };

                        // Add the linked resource to the alternate view
                        htmlView.LinkedResources.Add(linkedResource);

                        // Attach the alternate view to the email message
                        message.AlternateViews.Add(htmlView);

                        await smtpClient.SendMailAsync(message);
                    }
                }
            }
        }
    }
}

