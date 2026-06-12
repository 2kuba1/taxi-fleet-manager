using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.Configuration;

internal sealed class WorkShiftConfiguration : IEntityTypeConfiguration<WorkShift>
{
    public void Configure(EntityTypeBuilder<WorkShift> builder)
    {
        builder.ToTable("WorkShifts", "business");
        builder.HasKey(ws => ws.Id);

        builder.Property(ws => ws.StartTime).IsRequired();
        builder.Property(ws => ws.EndTime).IsRequired();

        builder.HasOne(ws => ws.User)
            .WithMany(u => u.WorkShifts)
            .HasForeignKey(ws => ws.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}