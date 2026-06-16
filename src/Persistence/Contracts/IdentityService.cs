using Application.Contracts.Persistence;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Persistence.Identity;

namespace Persistence.Contracts;

public sealed class IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IIdentityService
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
            Email = email,
            EmailConfirmed = false,
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

    /// <summary>
    /// Determines whether the provided login and password credentials are valid.
    /// </summary>
    /// <param name="login">String that represents user's login credential.</param>
    /// <param name="password">String that represents user's password.</param>
    /// <returns>True if credentials are valid, allowing subsequent methods to generate tokens; otherwise, false.</returns>
    public async Task<bool> CheckLoginCredentialsAsync(string login, string password)
    {
        var user = await userManager.FindByNameAsync(login);
            
        if (user == null)
            return false;
        
        if(!user.EmailConfirmed)
            return false;
        
        var result = await signInManager.CheckPasswordSignInAsync(user, password, true);

        return result.Succeeded;
    }

    public async Task<bool> CheckPasswordResetTokenAsync(string token, string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
            return false;

        var isValid =
            await userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", token);

        return isValid;
    }

    public async Task SetupPasswordAsync(string token, string email, string temporaryPassword, string newPassword)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
            throw new UserNotFoundException($"User not found.");
 
        if(userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash!, temporaryPassword) == PasswordVerificationResult.Failed)
            throw new InvalidResetTokenException("Invalid reset token.");
        
        var result = await userManager.ResetPasswordAsync(user, token, newPassword);
        
        if(result.Errors.Any())
            throw new InvalidResetTokenException("Invalid reset token.");

        user.EmailConfirmed = true;
        await userManager.UpdateAsync(user);
    }
}