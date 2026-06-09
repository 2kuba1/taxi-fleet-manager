using Domain.Entities;

namespace Application.Contracts.Persistence;

public interface IRoleService
{
    Task<Role?> GetRoleByNameAsync(string roleName);
}