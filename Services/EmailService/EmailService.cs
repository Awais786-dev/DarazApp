using System.Net.Mail;
using System.Net;

namespace DarazApp.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost = "smtp.gmail.com";// "smtp.gmail.com";
        private readonly string _smtpUsername = "ravian78692@gmail.com"; // Your SMTP username
        private readonly string _smtpPassword = "vluixpdkuyozxyfd"; // Your SMTP password
        private readonly int _smtpPort = 587; // Your SMTP port

        public async Task SendConfirmationEmail(string email, string confirmationLink)
        {
            try
            {
                MailAddress fromAddress = new MailAddress(_smtpUsername, "Daraz.Pk");
                MailAddress toAddress = new MailAddress(email);
                string subject = "Email Confirmation";
                string body = $@"
                    <html>
                    <body>
                        <p> Happy Shopping On Daraz.pk</p>
                        <p>Please confirm your email by clicking on the link below:</p>
                        <p><a href='{confirmationLink}'>Click Here</a> to confirm your email.</p>
                    </body>
                    </html>";


                using (var smtpClient = new SmtpClient(_smtpHost, _smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                    smtpClient.EnableSsl = true;

                    MailMessage mailMessage = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true  // Set to true if you want to send HTML email
                    };

                    await smtpClient.SendMailAsync(mailMessage);

                }
            }
            catch (SmtpException smtpEx)
            {
                // Handle SMTP exceptions (e.g., connection issues, authentication issues)
                throw new Exception("SMTP Error: " + smtpEx.Message, smtpEx);
            }
            catch (Exception ex)
            {
                // Handle exceptions (logging, rethrow, etc.)
                throw new Exception("Error sending confirmation email", ex);
            }
        }


    }
}
