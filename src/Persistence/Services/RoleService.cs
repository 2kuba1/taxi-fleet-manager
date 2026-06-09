using Application.Contracts.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Services;

public class RoleService(AppDbContext dbContext) : IRoleService
{
    public async Task<Role?> GetRoleByNameAsync(string roleName)
    {
        return await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
    }
}