using KolveniershofBACKEND.Data.Mappers;
using KolveniershofBACKEND.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KolveniershofBACKEND.Data
{
    public class DBContext : IdentityDbContext
    {
        public DBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new AttendanceConfiguration());
            builder.ApplyConfiguration(new CustomDayConfiguration());
            builder.ApplyConfiguration(new DayActivityConfiguration());
            builder.ApplyConfiguration(new DayConfiguration());
            builder.ApplyConfiguration(new HelperConfiguration());
            builder.ApplyConfiguration(new NoteConfiguration());
            builder.ApplyConfiguration(new ActivityConfiguration());

        }

        public DbSet<Activity> Activities { get; set; }
        // 'new' because DBContext.Users is an existing namespace
        public new DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<CustomDay> CustomDays { get; set; }
    }
}
