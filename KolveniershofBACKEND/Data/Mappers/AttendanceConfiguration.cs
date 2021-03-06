﻿using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KolveniershofBACKEND.Data.Mappers
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.ToTable("Attendance");
            builder.HasKey(a => new { a.DayId, a.ActivityId, a.UserId, a.TimeOfDay });
            builder.Property(a => a.Comment).IsRequired(false);
            builder.HasOne(a => a.DayActivity)
                .WithMany(da => da.Attendances)
                .IsRequired()
                .HasForeignKey(a => new { a.DayId, a.ActivityId, a.TimeOfDay })
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.User)
                .WithMany()
                .IsRequired()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
 