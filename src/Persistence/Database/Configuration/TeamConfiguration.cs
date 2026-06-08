using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.Configuration;

internal sealed class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams", "business");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);

        builder.HasOne(t => t.Owner)
            .WithOne()
            .HasForeignKey<Team>(t => t.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.CarFleet)
            .WithOne(cf => cf.Team)
            .HasForeignKey<CarFleet>(cf => cf.TeamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}