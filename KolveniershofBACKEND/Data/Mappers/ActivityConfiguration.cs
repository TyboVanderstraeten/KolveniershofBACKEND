﻿using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Mappers
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.ToTable("Activity");
            builder.HasKey(a => a.ActivityId);
            builder.Property(a => a.ActivityType).IsRequired();
            builder.Property(a => a.Name).IsRequired();
            builder.Property(a => a.Description).IsRequired();
            builder.Property(a => a.Pictogram).IsRequired();
        }
    }
}
