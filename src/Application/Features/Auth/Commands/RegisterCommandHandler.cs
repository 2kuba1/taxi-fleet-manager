using System.Text;
using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Cortex.Mediator.Commands;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Features.Auth.Commands;

public sealed class RegisterCommandHandler(
    IIdentityService identityService,
    IUserService userService,
    IEmailService emailService,
    IRoleService roleService,
    IUnitOfWork unitOfWork) : ICommandHandler<RegisterCommand>
{
    public async Task Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (await userService.CheckIfEmailExistsAsync(command.Email))
            throw new EmailAlreadyExistsException($"Email {command.Email} is already registered.");
        
        var login = GenerateCustomLogin(command.FirstName, command.LastName);

        var suffix = 1;
        var finalLogin = login;
        while (await identityService.UserExistsByNameAsync(finalLogin))
        {
            finalLogin = $"{login}{suffix}";
            suffix++;
        }

        var userId = Guid.NewGuid();
        var temporaryPassword = GenerateTemporaryPassword();

        var role = await roleService.GetRoleByNameAsync("Driver");

        if (role == null)
        {
            throw new RoleNotFoundException($"Role 'Driver' not found.");
        }
        
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var identityResult =
                await identityService.CreateUserAsync(userId, finalLogin, command.Email, temporaryPassword);

            if (!identityResult.Succeeded)
                throw new UserCreationFailedException($"User creation failed: {identityResult.ErrorMessage}");
            
            var domainUser = User.Create(userId,
                command.Email,
                finalLogin,
                command.PhoneNumber,
                command.AreaCode,
                command.FirstName,
                command.LastName,
                command.KilometerRate,
                command.ContractType,
                role.Id,
                command.TeamId);

            await userService.SaveUserAsync(domainUser);
        }
        catch(Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
        
        var resetToken = await identityService.GeneratePasswordResetTokenAsync(command.Email);
        await emailService.SendWelcomeEmailAsync(command.Email, finalLogin, temporaryPassword, resetToken);
    }
    
    private static string GenerateCustomLogin(string firstName, string lastName)
    {
        var cleanFirst = firstName.Replace(" ", "").ToLower();
        var cleanLast = lastName.Replace(" ", "").ToLower();

        var partFirst = cleanFirst.Length >= 3 ? cleanFirst.Substring(0, 3) : cleanFirst.PadRight(3, 'x');
        var partLast = cleanLast.Length >= 2 ? cleanLast.Substring(0, 2) : cleanLast.PadRight(2, 'x');

        return $"{partFirst}{partLast}";
    }

    private static string GenerateTemporaryPassword()
    {
        const string lowercase = "abcdefgijklmnopqrstuqvwxyz";
        const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string digits = "1234567890";
        const string specials = "!@#$%^&*";
        
        var random = new Random();
        var password = new StringBuilder();

        password.Append(lowercase[random.Next(lowercase.Length)]);
        password.Append(uppercase[random.Next(uppercase.Length)]);
        password.Append(digits[random.Next(digits.Length)]);
        password.Append(specials[random.Next(specials.Length)]);

        const string allChars = lowercase + uppercase + digits + specials;
        for (int i = 0; i < 6; i++)
        {
            password.Append(allChars[random.Next(allChars.Length)]);
        }

        return new string(password.ToString().ToCharArray().OrderBy(s => random.Next()).ToArray());
    }
}