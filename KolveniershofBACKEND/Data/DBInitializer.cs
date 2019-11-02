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
                #region Activities
                Activity a1 = new Activity(ActivityType.ATELIER, "Testatelier", "Dit is een testatelier", "test.picto");
                Activity a2 = new Activity(ActivityType.ATELIER, "Koken", "We gaan koken", "koken.picto");
                Activity a3 = new Activity(ActivityType.ATELIER, "Zwemmen", "Baantjes zwemmen", "zwemmen.picto");
                Activity a4 = new Activity(ActivityType.AFWEZIG, "Afwezig", "Afwezig", "afwezig.picto");
                Activity a5 = new Activity(ActivityType.ZIEK, "Ziek", "Ziek", "ziek.picto");
                Activity a6 = new Activity(ActivityType.VERVOER, "Bus", "Bus", "bus.picto");
                Activity a7 = new Activity(ActivityType.KOFFIE, "Koffie", "Koffie", "Koffie.picto");
                _dbContext.Activities.Add(a1);
                _dbContext.Activities.Add(a2);
                _dbContext.Activities.Add(a3);
                _dbContext.Activities.Add(a4);
                _dbContext.Activities.Add(a5);
                _dbContext.Activities.Add(a6);
                _dbContext.Activities.Add(a7);
                #endregion

                #region User

                User u1 = new User(UserType.BEGELEIDER, "Tybo", "Vanderstraeten", "tybo@hotmail.com", "", null);
                await _userManager.CreateAsync(new IdentityUser() { Email = u1.Email, UserName = u1.Email, EmailConfirmed = true, }, "P@ssword1");

            User u2 = new User(UserType.CLIENT, "Rob", "De Putter", "rob@hotmail.com", "", 2);
                await _userManager.CreateAsync(new IdentityUser() { Email = u2.Email, UserName = u2.Email, EmailConfirmed = true, }, "P@ssword1");

                User u3 = new User(UserType.STAGIAIR, "Tim", "Geldof", "tim@hotmail.com", "", null);
                await _userManager.CreateAsync(new IdentityUser() { Email = u3.Email, UserName = u3.Email, EmailConfirmed = true, }, "P@ssword1");

                User u4 = new User(UserType.VRIJWILLIGER, "Dean", "Vandamme", "dean@hotmail.com", "", null);
                await _userManager.CreateAsync(new IdentityUser() { Email = u4.Email, UserName = u4.Email, EmailConfirmed = true, }, "P@ssword1");

                User u5 = new User(UserType.CLIENT, "Alihan", "Fevziev", "xaml@hotmail.com", "", 2);
                await _userManager.CreateAsync(new IdentityUser() { Email = u5.Email, UserName = u5.Email, EmailConfirmed = true, }, "P@ssword1");
                _dbContext.Users.Add(u1);
                _dbContext.Users.Add(u2);
                _dbContext.Users.Add(u3);
                _dbContext.Users.Add(u4);
                _dbContext.Users.Add(u5);
                #endregion

                #region TemplateDay
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Day day = new Day(i + 1, j + 1);
                        DayActivity dayActivity1 = new DayActivity(day, a1, TimeOfDay.VOORMIDDAG);
                        DayActivity dayActivity2 = new DayActivity(day, a2, TimeOfDay.VOORMIDDAG);
                        DayActivity dayActivity3 = new DayActivity(day, a3, TimeOfDay.NAMIDDAG);
                        DayActivity dayActivity4 = new DayActivity(day, a4, TimeOfDay.VOLLEDIG);
                        DayActivity dayActivity5 = new DayActivity(day, a5, TimeOfDay.VOLLEDIG);
                        DayActivity dayActivity6 = new DayActivity(day, a6, TimeOfDay.OCHTEND);
                        DayActivity dayActivity7 = new DayActivity(day, a7, TimeOfDay.OCHTEND);
                        DayActivity dayActivity8 = new DayActivity(day, a7, TimeOfDay.AVOND);
                        DayActivity dayActivity9 = new DayActivity(day, a6, TimeOfDay.AVOND);
                        day.AddDayActivity(dayActivity1);
                        day.AddDayActivity(dayActivity2);
                        day.AddDayActivity(dayActivity3);
                        day.AddDayActivity(dayActivity4);
                        day.AddDayActivity(dayActivity5);
                        day.AddDayActivity(dayActivity6);
                        day.AddDayActivity(dayActivity7);
                        day.AddDayActivity(dayActivity8);
                        day.AddDayActivity(dayActivity9);
                        Helper helper1 = new Helper(day, u3);
                        Helper helper2 = new Helper(day, u4);
                        day.AddHelper(helper1);
                        day.AddHelper(helper2);
                        _dbContext.Days.Add(day);
                    }
                }
                #endregion

                _dbContext.SaveChanges();
        }
    }
}
