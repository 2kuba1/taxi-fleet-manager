using System.Net;
using System.Net.Mail;
using Application.Contracts.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class EmailService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IEmailService
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        return Task.CompletedTask;
    }

    public async Task SendWelcomeEmailAsync(string email, string login, string temporaryPassword, string resetToken)
    {
        var username = configuration["Smtp:Username"];
        var  password = configuration["Smtp:Password"];
        
        using var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };

        var hostUrl = configuration["App:FrontendUrl"] ?? "http://localhost:3000";
        var setupPasswordUrl = $"{hostUrl}/setup-password?token={Uri.EscapeDataString(resetToken)}&email={Uri.EscapeDataString(email)}";
        
        var htmlBody = $@"
            <!DOCTYPE html>
            <html lang='pl'>
            <head>
                <meta charset='UTF-8'>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #757575; border-radius: 5px; }}
                    .header {{ background-color: #f8f9fa; padding: 15px; text-align: center; border-radius: 5px 5px 0 0; }}
                    .content {{ padding: 20px; }}
                    .credentials-box {{ background-color: #ffffff; padding: 15px; border-radius: 5px; margin: 20px 0; border-left: 4px solid #E21E26; }}
                    .btn {{ display: inline-block; padding: 10px 20px; color: #ffffff !important; background-color: #E21E26; text-decoration: none; border-radius: 5px; font-weight: bold; margin-top: 15px; }}
                    .footer {{ font-size: 12px; color: #757575; margin-top: 25px; text-align: center; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2>Witaj w Taxi Fleet Manager!</h2>
                    </div>
                    <div class='content'>
                        <p>Kierowco! Twoje konto zostało utworzone pomyślnie.</p>
                        <p>Oto Twoje dane do pierwszego zalogowania:</p>
                        
                        <div class='credentials-box'>
                            <strong>Login:</strong> <code style='font-size: 14px; background: #e9ecef; padding: 2px 6px; border-radius: 3px;'>{login}</code><br/>
                            <strong>Hasło tymczasowe:</strong> <code style='font-size: 14px; background: #e9ecef; padding: 2px 6px; border-radius: 3px;'>{temporaryPassword}</code>
                        </div>

                        <p>Ze względów bezpieczeństwa wymagamy, abyś zmienił hasło tymczasowe przed rozpoczęciem korzystania z systemu. Kliknij w poniższy przycisk, aby ustawić nowe, własne hasło:</p>
                        
                        <p style='text-align: center;'>
                            <a href='{setupPasswordUrl}' class='btn'>Ustaw nowe hasło</a>
                        </p>
                    </div>
                    <div class='footer'>
                        <p>Ta wiadomość została wygenerowana automatycznie. Prosimy na nią nie odpowiadać.</p>
                        <p>&copy; {DateTime.UtcNow.Year} Taxi Fleet Manager</p>
                    </div>
                </div>
            </body>
            </html>";
        
        var mailMessage = new MailMessage()
        {
            From = new MailAddress("no-reply@fleetmanager.com", "Taxi Fleet Manager"),
            To = { email },
            Subject = "Witaj w Taxi Fleet Manager - Dane logowania",
            Body = htmlBody,
            IsBodyHtml = true 
        };
        
        await client.SendMailAsync(mailMessage, CancellationToken.None);
    }
}