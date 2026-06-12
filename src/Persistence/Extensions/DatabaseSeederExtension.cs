using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.Database;
using Persistence.Database.Seeders;


namespace Persistence.Extensions;

public static class DatabaseSeederExtension
{
    public static async Task SeedDatabaseAsync(this IHost app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var context =  scope.ServiceProvider.GetService<AppDbContext>();
        if (context != null)
        {
            await UserTeamRoleSeeder.SeedAsync(scope.ServiceProvider);
        }
    }
}