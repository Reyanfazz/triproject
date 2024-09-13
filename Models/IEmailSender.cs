namespace certificate.Models
{
    public interface IEmailSender
     {
        Task SendEmailAsync(string email, string subject, string message);
     }
}
