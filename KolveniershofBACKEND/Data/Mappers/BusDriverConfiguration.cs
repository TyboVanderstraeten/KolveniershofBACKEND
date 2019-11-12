using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KolveniershofBACKEND.Data.Mappers
{
    public class BusDriverConfiguration : IEntityTypeConfiguration<BusDriver>
    {
        public void Configure(EntityTypeBuilder<BusDriver> builder)
        {
            builder.ToTable("BusDriver");
            builder.Property(b => b.TimeOfDay).HasConversion(new EnumToStringConverter<TimeOfDay>()).IsRequired();
            builder.Property(b => b.BusColor).HasConversion(new EnumToStringConverter<BusColor>()).IsRequired();
            builder.HasKey(b => new { b.DayId, b.DriverId, b.TimeOfDay });
        }
    }
}
