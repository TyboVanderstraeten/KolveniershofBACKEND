using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KolveniershofBACKEND.Data.Mappers
{
    public class HelperConfiguration : IEntityTypeConfiguration<Helper>
    {
        public void Configure(EntityTypeBuilder<Helper> builder)
        {
            builder.ToTable("Helper");
            builder.HasKey(h => new { h.DayId, h.UserId });
            builder.HasOne(h => h.Day)
                .WithMany(d => d.Helpers)
                .IsRequired()
                .HasForeignKey(h => h.DayId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(d => d.User)
                .WithMany()
                .IsRequired()
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
