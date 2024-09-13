using System.Net;
using System.Net.Mail;

namespace certificate.Models
{
    public class EmailSender : IEmailSender
    {
       public Task SendEmailAsync(string email, string subject, string message)
        {

            var mail = "tritcontact@gmail.com";
            var pw = "ywbq xmjw ccbc zoyp";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail,pw),


            };

            return client.SendMailAsync(

                new MailMessage(
                    from: mail,
                    to: email,
                    subject, message));
        }
    }
}
