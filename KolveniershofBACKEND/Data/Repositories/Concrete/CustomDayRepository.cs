using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KolveniershofBACKEND.Data.Repositories.Concrete
{
    public class CustomDayRepository : ICustomDayRepository
    {
        private readonly DBContext _dbContext;
        private readonly DbSet<CustomDay> _customDays;
        private readonly DbSet<User> _users;
        private readonly DbSet<Activity> _activities;

        public CustomDayRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
            _customDays = dbContext.CustomDays;
            _users = dbContext.Users;
            _activities = dbContext.Activities;
        }

        public IEnumerable<CustomDay> GetAll()
        {
            return _customDays.Include(cd => cd.Notes)
                              .Include(cd => cd.DayActivities).ThenInclude(da => da.Activity)
                              .Include(cd => cd.DayActivities).ThenInclude(da => da.Attendances).ThenInclude(a => a.User)
                              .Include(cd => cd.Helpers).ThenInclude(h => h.User)
                              .OrderBy(cd => cd.Date)
                              .ToList();
        }

        public IEnumerable<CustomDay> GetAllInRange(DateTime start, DateTime end)
        {
            return _customDays.Where(cd => cd.Date.Date >= start.Date && cd.Date.Date <= end.Date)
                .Include(cd => cd.Notes)
                .Include(cd => cd.DayActivities).ThenInclude(da => da.Activity)
                .Include(cd => cd.DayActivities).ThenInclude(da => da.Attendances).ThenInclude(a => a.User)
                .Include(cd => cd.Helpers).ThenInclude(h => h.User)
                .OrderBy(cd => cd.Date)
                .ToList();
        }

        public IEnumerable<CustomDay> GetAllInRangeForUser(DateTime start, DateTime end, int userId)
        {
            // All customdays in a range
            IEnumerable<CustomDay> customDaysRange = _customDays.Where(cd => cd.Date.Date >= start.Date && cd.Date.Date <= end.Date)
                              .Include(cd => cd.Notes)
                              .Include(cd => cd.DayActivities).ThenInclude(da => da.Activity)
                              .Include(cd => cd.DayActivities).ThenInclude(da => da.Attendances).ThenInclude(a => a.User)
                              .Include(cd => cd.Helpers).ThenInclude(h => h.User)
                              .OrderBy(cd => cd.Date)
                              .ToList();

            // Replace customdays dayactivities with those attended
            foreach (var customDay in customDaysRange)
            {
                customDay.DayActivities = customDay.DayActivities.Where(da => da.Attendances.Any(a => a.UserId == userId)).ToList();
            }

            return customDaysRange.OrderBy(cd=>cd.Date).ToList();
        }

        public IEnumerable<User> GetAbsentUsersForDay(DateTime date)
        {
            return _customDays.Where(d => d.Date.Date == date.Date)
                              .SelectMany(d => d.DayActivities
                                    .Where(da => da.Activity.ActivityType.Equals(ActivityType.AFWEZIG))
                                    .SelectMany(da => da.Attendances
                                            .Select(a => a.User)))
                                            .OrderBy(u => u.FirstName).ThenBy(u => u.LastName)
                              .ToList();
        }

        public IEnumerable<User> GetSickUsersForDay(DateTime date)
        {
            return _customDays.Where(d => d.Date.Date == date.Date)
                              .SelectMany(d => d.DayActivities
                                    .Where(da => da.Activity.ActivityType.Equals(ActivityType.ZIEK))
                                    .SelectMany(da => da.Attendances
                                            .Select(a => a.User)))
                                            .OrderBy(u => u.FirstName).ThenBy(u => u.LastName)
                              .ToList();
        }

        public IEnumerable<Note> GetNotesForDay(DateTime date)
        {
            return _customDays.Where(d => d.Date.Date == date.Date)
                              .SelectMany(d => d.Notes)
                              .ToList();
        }

        public IEnumerable<Helper> GetHelpersForDay(DateTime date)
        {
            return _customDays.Where(d => d.Date.Date == date.Date)
                              .SelectMany(d => d.Helpers).Include(h => h.User)
                              .OrderBy(h => h.User.FirstName).ThenBy(h => h.User.LastName)
                              .ToList();
        }

        public IEnumerable<User> GetPossibleHelpers(DateTime date)
        {
            return _users.Where(u => u.UserType.Equals(UserType.STAGIAIR) || u.UserType.Equals(UserType.VRIJWILLIGER))
                         .Except(_customDays.Where(d => d.Date.Date == date.Date)
                                      .SelectMany(d => d.Helpers).Include(h => h.User)
                                      .Select(h => h.User))
                                      .OrderBy(u => u.FirstName).ThenBy(u => u.LastName)
                                      .ToList();
        }

        public IEnumerable<Activity> GetPossibleDayActivities(DateTime date, TimeOfDay timeOfDay)
        {
            return _activities.Except(_customDays.Where(d => d.Date.Date == date.Date)
                                      .SelectMany(d => d.DayActivities).Include(da => da.Activity)
                                      .Where(da => da.TimeOfDay.Equals(timeOfDay))
                                      .Select(da => da.Activity)
                                      )
                                      .Except(_activities.Where(a => a.ActivityType.Equals(ActivityType.AFWEZIG) || a.ActivityType.Equals(ActivityType.ZIEK)))
                                      .OrderBy(a => a.Name).ThenBy(a => a.ActivityType)
                                      .ToList();
        }

        public CustomDay GetByDate(DateTime date)
        {
            return _customDays.Include(cd => cd.Notes)
                              .Include(cd => cd.DayActivities).ThenInclude(da => da.Activity)
                              .Include(cd => cd.DayActivities).ThenInclude(da => da.Attendances).ThenInclude(a => a.User)
                              .Include(cd => cd.Helpers).ThenInclude(h => h.User)
                              .SingleOrDefault(cd => cd.Date.Date == date.Date);
        }

        public CustomDay GetById(int id)
        {
            return _customDays.Include(cd => cd.Notes)
                              .Include(cd => cd.DayActivities).ThenInclude(da => da.Activity)
                              .Include(cd => cd.DayActivities).ThenInclude(da => da.Attendances).ThenInclude(a => a.User)
                              .Include(cd => cd.Helpers).ThenInclude(h => h.User)
                              .SingleOrDefault(cd => cd.DayId == id);
        }

        public void Add(CustomDay customDay)
        {
            _customDays.Add(customDay);
        }

        public void Remove(CustomDay customDay)
        {
            _customDays.Remove(customDay);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
