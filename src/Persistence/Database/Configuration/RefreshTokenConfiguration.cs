using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.Configuration;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens", "business");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.TokenHash)
            .IsRequired()
            .HasMaxLength(500); 

        builder.HasIndex(r => r.TokenHash)
            .IsUnique();

        builder.Property(r => r.ExpiresAtUtc)
            .IsRequired();
        
        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}