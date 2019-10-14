using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KolveniershofBACKEND.Data.Mappers
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.ToTable("Note");
            builder.HasKey(n => n.NoteId);
            builder.Property(n => n.NoteType).IsRequired();
            builder.Property(n => n.Content).IsRequired();
        }
    }
}
