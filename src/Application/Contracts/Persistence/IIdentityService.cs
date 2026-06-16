namespace Application.Contracts.Persistence;

public interface IIdentityService
{
    Task<bool> UserExistsByNameAsync(string username);
    Task<(bool Succeeded, string ErrorMessage)> CreateUserAsync(Guid userId, string login, string email, string temporaryPassword);
    Task<string> GeneratePasswordResetTokenAsync(string email);
    Task<bool> CheckLoginCredentialsAsync(string login, string password);
    Task<bool>  CheckPasswordResetTokenAsync(string token, string email);
    Task SetupPasswordAsync(string token, string email, string temporaryPassword, string newPassword);
}