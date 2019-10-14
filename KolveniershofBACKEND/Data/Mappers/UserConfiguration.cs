using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Mappers
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(u => u.UserId);
            builder.Property(u => u.UserType).IsRequired();
            builder.Property(u => u.FirstName).IsRequired();
            builder.Property(u => u.LastName).IsRequired();
            builder.Property(u => u.ProfilePicture).IsRequired();
            builder.Property(u => u.Group).IsRequired(false);
        }
    }
}
