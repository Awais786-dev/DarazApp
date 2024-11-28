namespace DarazApp.Services.EmailService
{
    public interface IEmailService
    {
        Task SendConfirmationEmail(string email, string confirmationLink);

    }
}
