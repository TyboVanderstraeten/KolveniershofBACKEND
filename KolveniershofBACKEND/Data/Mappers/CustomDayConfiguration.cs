using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KolveniershofBACKEND.Data.Mappers
{
    public class CustomDayConfiguration : IEntityTypeConfiguration<CustomDay>
    {
        public void Configure(EntityTypeBuilder<CustomDay> builder)
        {
            builder.Property(cd => cd.Date).IsRequired();
            builder.Property(cd => cd.Menu).IsRequired();
            builder.HasMany(cd => cd.Notes)
                .WithOne()
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
