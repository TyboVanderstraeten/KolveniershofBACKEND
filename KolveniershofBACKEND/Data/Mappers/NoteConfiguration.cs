using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KolveniershofBACKEND.Data.Mappers
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.ToTable("Note");
            builder.HasKey(n => n.NoteId);
            builder.Property(n => n.NoteType).HasConversion(new EnumToStringConverter<NoteType>()).IsRequired();
            builder.Property(n => n.Content).IsRequired();
        }
    }
}
