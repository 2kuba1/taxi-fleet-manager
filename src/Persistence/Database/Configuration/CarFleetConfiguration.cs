using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.Configuration;

internal sealed class CarFleetConfiguration : IEntityTypeConfiguration<CarFleet>
{
    public void Configure(EntityTypeBuilder<CarFleet> builder)
    {
        builder.ToTable("CarFleets", "business");
        builder.HasKey(cf => cf.Id);

        builder.HasOne(cf => cf.Team)
            .WithOne(t => t.CarFleet)
            .HasForeignKey<CarFleet>(cf => cf.TeamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}