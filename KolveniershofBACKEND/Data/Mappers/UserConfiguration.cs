﻿using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KolveniershofBACKEND.Data.Mappers
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(u => u.UserId);
            builder.Property(u => u.UserType).HasConversion(new EnumToStringConverter<UserType>()).IsRequired();
            builder.Property(u => u.FirstName).IsRequired();
            builder.Property(u => u.LastName).IsRequired();
            builder.Property(u => u.Email).IsRequired(false);
            builder.Property(u => u.ProfilePicture).IsRequired(false);
            builder.Property(u => u.Group).IsRequired(false);
            builder.Property(u => u.DegreeOfLimitation).IsRequired(false);
            builder.HasMany(u => u.WeekendDays)
                .WithOne()
                .IsRequired()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
