using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Identity;

namespace Persistence.Database.Configuration;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "business");
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Email).IsRequired()
            .HasMaxLength(256);
        builder.Property(u => u.Login).IsRequired()
            .HasMaxLength(8);
        builder.Property(u => u.FirstName).IsRequired()
            .HasMaxLength(100);
        builder.Property(u => u.LastName).IsRequired()
            .HasMaxLength(100);
        builder.Property(u => u.ContractType).HasDefaultValue(ContractType.Mandate)
            .HasConversion<string>()
            .HasSentinel(ContractType.Mandate);

        builder.OwnsOne(u => u.PhoneNumber, pn =>
        {
            pn.Property(p => p.Number).HasColumnName("PhoneNumber").IsRequired().HasMaxLength(10);
            pn.Property(a => a.AreaCode).HasColumnName("PhoneAreaCode").IsRequired().HasMaxLength(5);
        });

        builder.OwnsOne(u => u.KilometerRate, kr =>
        {
            kr.Property(k => k.Value).HasColumnName("KilometerRate").IsRequired();
        });

        builder.HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<User>(u => u.Id)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(u => u.Team)
            .WithMany(t => t.Users)
            .HasForeignKey(u => u.TeamId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}