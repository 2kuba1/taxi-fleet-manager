using Application.Contracts.Persistence;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Persistence.Identity;

namespace Persistence.Contracts;

public sealed class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
{
    public async Task<bool> UserExistsByNameAsync(string username)
    {
        var user = await userManager.FindByNameAsync(username);
        return user != null;
    }

    public async Task<(bool Succeeded, string ErrorMessage)> CreateUserAsync(Guid userId, string login, string email, string temporaryPassword)
    {
        var identityUser = new ApplicationUser
        {
            Id = userId,
            UserName = login,
            Email = email
        };

        var result = await userManager.CreateAsync(identityUser, temporaryPassword);
        
        if (result.Succeeded)
        {
            return (true, string.Empty);
        }

        var firstError = result.Errors.FirstOrDefault()?.Description ?? "Unknown identity error";
        return (false, firstError);
    }

    public async Task<string> GeneratePasswordResetTokenAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
            throw new EmailNotFoundForPasswordReset($"User with email {email} not found for password reset.");
        
        return await userManager.GeneratePasswordResetTokenAsync(user);
    }
}