using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KolveniershofBACKEND.Data.Mappers
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.ToTable("Activity");
            builder.HasKey(a => a.ActivityId);
            builder.Property(a => a.ActivityType).HasConversion(new EnumToStringConverter<ActivityType>()).IsRequired();
            builder.Property(a => a.Name).IsRequired();
            builder.Property(a => a.Description).IsRequired(false);
            builder.Property(a => a.Pictogram).IsRequired();                
        }
    }
}
