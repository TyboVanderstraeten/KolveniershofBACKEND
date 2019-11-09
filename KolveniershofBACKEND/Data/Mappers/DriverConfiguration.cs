using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KolveniershofBACKEND.Data.Mappers
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.ToTable("Driver");
            builder.Property(b => b.BusColor).HasConversion(new EnumToStringConverter<BusColor>()).IsRequired();
        }
    }
}
