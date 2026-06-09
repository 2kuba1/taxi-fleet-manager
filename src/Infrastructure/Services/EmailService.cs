using Application.Contracts.Infrastructure;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        return Task.CompletedTask;
    }

    public Task SendWelcomeEmailAsync(string email, string login, string temporaryPassword, string resetToken)
    {
        return Task.CompletedTask;
    }
}