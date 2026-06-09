using Application.Contracts.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Database;
using Persistence.Identity;
using Persistence.Services;

namespace Persistence;

public static class PersistenceService
{
    public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DatabaseConnectionString"));
        });

        services.AddIdentityCore<ApplicationUser>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequiredLength = 6;

                opt.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IRoleService, RoleService>();
        
        return services;
    }
}