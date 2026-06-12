using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.Configuration;

internal sealed class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("Cars", "business");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Brand).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Model).IsRequired().HasMaxLength(50);
        builder.Property(c => c.InspectionDate).IsRequired();
        builder.Property(c => c.InsuranceRenewalDate).IsRequired();

        builder.OwnsOne(c => c.LicensePlate, lp =>
        {
            lp.Property(l => l.Value).HasColumnName("LicensePlate").IsRequired().HasMaxLength(15);
        });

        builder.OwnsOne(c => c.VinNumber, vin =>
        {
            vin.Property(v => v.Value).HasColumnName("VinNumber").IsRequired().HasMaxLength(17).IsFixedLength();
        });

        builder.HasOne(c => c.CarFleet)
            .WithMany(f => f.Cars)
            .HasForeignKey(c => c.CarFleetId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}