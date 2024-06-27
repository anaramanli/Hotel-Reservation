namespace Hotel.Interfaces
{
    public interface IEmailService
    {
        Task SendMailAsync(string emailTo, string subject, string body, bool isHtml = false);
        Task SendMailWithEmbeddedImageAsync(string emailTo, string subject, string body, byte[] imageBytes, string contentId, bool isHtml = false);
    }
}
