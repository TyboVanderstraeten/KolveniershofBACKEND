using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Mappers
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.ToTable("Attendance");
            builder.HasKey(a => new { a.DayId, a.ActivityId, a.UserId });
            builder.Property(a => a.DayActivity).IsRequired();
            builder.Property(a => a.Comment).IsRequired();
            builder.HasOne(a => a.DayActivity)
                .WithMany(da => da.Attendances)
                .IsRequired()
                // Not sure if this works?
                .HasForeignKey(a => new { a.DayId, a.ActivityId })
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
