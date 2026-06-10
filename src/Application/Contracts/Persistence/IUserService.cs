using Domain.Entities;

namespace Application.Contracts.Persistence;

public interface IUserService
{
    Task SaveUserAsync(User user);
    Task<bool> CheckIfEmailExistsAsync(string email);
    Task<User?> GetUserByLoginAsync(string login);
}