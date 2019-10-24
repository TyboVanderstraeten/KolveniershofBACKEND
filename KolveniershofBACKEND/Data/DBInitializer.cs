using KolveniershofBACKEND.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data
{
    public class DBInitializer
    {
        private readonly DBContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public DBInitializer(DBContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task seedDatabase()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                #region Activities
                Activity a1 = new Activity(ActivityType.ATELIER, "Testatelier", "Dit is een testatelier", "test.picto");
                Activity a2 = new Activity(ActivityType.ATELIER, "Koken", "We gaan koken", "koken.picto");
                _dbContext.Activities.Add(a1);
                #endregion

                #region User

                User u1 = new User(UserType.BEGELEIDER, "Tybo", "Vanderstraeten", "tybo@hotmail.com", "string.jpeg", null);
                await _userManager.CreateAsync(new IdentityUser() { Email = u1.Email, UserName = u1.Email }, "P@ssword1");
                #endregion

                #region Day
                Day day11 = new Day(1, 1);
                DayActivity da1 = new DayActivity(day11, a1, TimeOfDay.VOORMIDDAG);
                DayActivity da2 = new DayActivity(day11, a2, TimeOfDay.NAMIDDAG);
                day11.AddDayActivity(da1);
                day11.AddDayActivity(da2);
                day11.AddHelper(new Helper(day11, u1));
                _dbContext.Days.Add(day11);
                #endregion

                _dbContext.SaveChanges();
            }
        }
    }
}
