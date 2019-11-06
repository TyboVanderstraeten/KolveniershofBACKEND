using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KolveniershofBACKEND.Data.Mappers
{
    public class DayConfiguration : IEntityTypeConfiguration<Day>
    {
        public void Configure(EntityTypeBuilder<Day> builder)
        {
            builder.ToTable("Day");
            builder.HasKey(d => d.DayId);
            builder.Property(d => d.TemplateName).IsRequired();
            builder.Property(d => d.WeekNr).IsRequired();
            builder.Property(d => d.DayNr).IsRequired();
            builder.HasDiscriminator<string>("DayType")
                .HasValue<Day>("template_day")
                .HasValue<CustomDay>("custom_day");
        }
    }
}
