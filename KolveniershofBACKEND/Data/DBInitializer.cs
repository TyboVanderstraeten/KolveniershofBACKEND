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
                _dbContext.Activities.Add(a1);
                #endregion

                #region Day
                Day day11 = new Day(1, 1);
                Day day12 = new Day(1, 2);
                _dbContext.Days.Add(day11);
                _dbContext.Days.Add(day12);
                #endregion

                #region CustomDay
                CustomDay cday11 = new CustomDay(day11.WeekNr, day11.DayNr,
                    DateTime.Now, "French fries");
                CustomDay cday12 = new CustomDay(day12.WeekNr, day12.DayNr,
                 DateTime.Now.AddDays(1), "Hot dog");
                _dbContext.CustomDays.Add(cday11);
                _dbContext.CustomDays.Add(cday12);
                #endregion
                _dbContext.SaveChanges();
            }
        }
    }
}
