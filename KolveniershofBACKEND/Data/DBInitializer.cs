using KolveniershofBACKEND.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System;

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

        public void seedDatabase()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                #region Activities
                Activity a1 = new Activity(ActivityType.ATELIER, "Testatelier", "Dit is een testatelier", "test.picto");
                Activity a2 = new Activity(ActivityType.ATELIER, "Koken", "We gaan koken", "koken.picto");
                _dbContext.Activities.Add(a1);
                #endregion

                #region Day
                Day day11 = new Day(1, 1);
                DayActivity da1 = new DayActivity(day11, a1, TimeOfDay.VOORMIDDAG);
                DayActivity da2 = new DayActivity(day11, a2, TimeOfDay.NAMIDDAG);
                day11.AddDayActivity(da1);
                day11.AddDayActivity(da2);
                day11.AddHelper(new Helper(day11, new User(UserType.STAGIAIR, "Tybo",
                    "Vanderstraeten", "tybo@hotmail.com", "string.jpeg", null)));
                _dbContext.Days.Add(day11);
                #endregion

                _dbContext.SaveChanges();
            }
        }
    }
}
