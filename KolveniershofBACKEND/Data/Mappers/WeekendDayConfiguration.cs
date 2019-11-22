using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Mappers
{
    public class WeekendDayConfiguration : IEntityTypeConfiguration<WeekendDay>
    {
        public void Configure(EntityTypeBuilder<WeekendDay> builder)
        {
            builder.ToTable("WeekendDay");
            builder.HasKey(w => w.WeekendDayId);
            builder.Property(w => w.Date).IsRequired();
            builder.Property(w => w.Comment).IsRequired(false);
        }
    }
}
