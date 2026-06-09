using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Identity;

namespace Persistence.Database;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) 
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<CarFleet> CarFleets { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<ShiftReport> ShiftReports { get; set; }
    public DbSet<WorkShift> WorkShifts { get; set; }
    public DbSet<Team> Teams { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("identity");
     
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var properties = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(string));

            foreach (var property in properties)
            {
                property.SetIsUnicode(true); 
            }
        }
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in base.ChangeTracker.Entries<BaseEntity>()
                     .Where(q => q.State is EntityState.Added or EntityState.Modified))
        {
            entry.Entity.UpdatedAt = GetValidUtcNow();

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = GetValidUtcNow();
            }
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
    
    private static DateTime GetValidUtcNow()
    {
        var utcNow = DateTime.UtcNow;

        if (utcNow == DateTime.MaxValue || utcNow == DateTime.MinValue)
        {
            utcNow = DateTime.UtcNow.AddMinutes(-1);
        }

        return utcNow;
    }   
}