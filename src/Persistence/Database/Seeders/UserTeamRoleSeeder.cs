using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Identity;

namespace Persistence.Database.Seeders;

public static class UserTeamRoleSeeder
{
    private static readonly Guid InitialOwnerUserId = Guid.Parse("99999999-9999-9999-9999-999999999999");
    private static readonly Guid CieszynTeamId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        await context.Database.MigrateAsync();
        
        if (!await context.Roles.AnyAsync())
        {
            await context.Roles.AddRangeAsync(
                Role.Create("Admin"),
                Role.Create("User"),
                Role.Create("Driver")
            );

            await context.SaveChangesAsync();
        }
        
        var identityExists =
            await context.Set<ApplicationUser>()
                .AnyAsync(x => x.Id == InitialOwnerUserId);

        if (identityExists)
            return;

        var ownerRole =
            await context.Roles.FirstAsync(r => r.Name == "User");

        var password = configuration["SeedSettings:InitialOwnerPassword"]!;
        var email = configuration["SeedSettings:InitialOwnerEmail"]!;
        var login = configuration["SeedSettings:InitialOwnerLogin"]!;
        var phoneNumber = configuration["SeedSettings:InitialOwnerPhoneNumber"]!;
        var areaCode = configuration["SeedSettings:InitialOwnerAreaCode"]!;
        var firstName = configuration["SeedSettings:InitialOwnerFirstName"]!;
        var lastName = configuration["SeedSettings:InitialOwnerLastname"]!;

        var identityUser = new ApplicationUser
        {
            Id = InitialOwnerUserId,
            UserName = login,
            Email = email,
        };

        var passwordHasher = new PasswordHasher<ApplicationUser>();

        identityUser.PasswordHash =
            passwordHasher.HashPassword(identityUser, password);

        context.Set<ApplicationUser>().Add(identityUser);

        await context.SaveChangesAsync();

        var owner = User.Create(
            InitialOwnerUserId,
            email,
            login,
            phoneNumber,
            areaCode,
            firstName,
            lastName,
            0f,
            ContractType.B2B,
            ownerRole.Id,
            null
        );

        context.Users.Add(owner);

        await context.SaveChangesAsync();

        var team = Team.Create(
            CieszynTeamId,
            "Cieszyn",
            InitialOwnerUserId
        );

        context.Teams.Add(team);

        await context.SaveChangesAsync();

        owner = await context.Users
            .FirstAsync(x => x.Id == InitialOwnerUserId);

        typeof(User)
            .GetProperty(nameof(User.TeamId))!
            .SetValue(owner, CieszynTeamId);

        await context.SaveChangesAsync();
    }
}