namespace Application.Contracts.Infrastructure;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string message);
    Task SendWelcomeEmailAsync(string email, string login, string temporaryPassword, string restToken);
}