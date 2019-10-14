using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Mappers
{
    public class DayActivityConfiguration : IEntityTypeConfiguration<DayActivity>
    {
        public void Configure(EntityTypeBuilder<DayActivity> builder)
        {
            builder.ToTable("DayActivity");
            builder.HasKey(da => new { da.DayId, da.ActivityId });
            builder.Property(da => da.TimeOfDay).IsRequired();
            builder.HasOne(da => da.Day)
                .WithMany(d => d.DayActivities)
                .IsRequired()
                .HasForeignKey(da => da.DayId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(da => da.Activity)
                .WithMany()
                .IsRequired()
                .HasForeignKey(da => da.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
