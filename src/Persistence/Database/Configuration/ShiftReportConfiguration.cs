using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.Configuration;

internal sealed class ShiftReportConfiguration : IEntityTypeConfiguration<ShiftReport>
{
    public void Configure(EntityTypeBuilder<ShiftReport> builder)
    {
        builder.ToTable("ShiftReports", "business");
        builder.HasKey(sr => sr.Id);

        builder.Property(sr => sr.OdometerPhotoUrl).IsRequired().HasMaxLength(2048);
        builder.Property(sr => sr.ReportStatus).IsRequired().HasConversion<string>();

        builder.OwnsOne(sr => sr.KilometersDriven, kd =>
        {
            kd.Property(k => k.Value).HasColumnName("KilometersDriven").IsRequired();
        });

        builder.OwnsOne(sr => sr.CardTransactionsSum, cts =>
        {
            cts.Property(c => c.Value).HasColumnName("CardTransactionsSum").IsRequired();
        });

        builder.HasOne(sr => sr.Car)
            .WithMany()
            .HasForeignKey(sr => sr.CarId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict); 

        builder.HasOne(sr => sr.User)
            .WithMany(u => u.ShiftReports)
            .HasForeignKey(sr => sr.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}