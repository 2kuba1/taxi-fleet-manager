using Application.Contracts.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Services;

public sealed class UserService(AppDbContext dbContext) : IUserService
{
    public async Task SaveUserAsync(User user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> CheckIfEmailExistsAsync(string email)
    {
        return await dbContext.Users.AsNoTracking().AnyAsync(u => u.Email == email);
    }
}