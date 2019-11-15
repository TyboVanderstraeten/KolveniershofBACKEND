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

        public CustomDayRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
            _customDays = dbContext.CustomDays;
        }

        public IEnumerable<CustomDay> GetAll()
        {
            return _customDays.Include(cd => cd.Notes)
                              .Include(cd => cd.DayActivities).ThenInclude(da => da.Activity)
                              .Include(cd => cd.DayActivities).ThenInclude(da => da.Attendances).ThenInclude(a => a.User)
                              .Include(cd => cd.Helpers).ThenInclude(h => h.User)
                              .ToList();
        }

        public IEnumerable<CustomDay> GetAllInRange(DateTime start, DateTime end)
        {
            return _customDays.Where(cd => cd.Date.Date >= start.Date && cd.Date.Date <= end.Date)
                .Include(cd => cd.Notes)
                .Include(cd => cd.DayActivities).ThenInclude(da => da.Activity)
                .Include(cd => cd.DayActivities).ThenInclude(da => da.Attendances).ThenInclude(a => a.User)
                .Include(cd => cd.Helpers).ThenInclude(h => h.User)
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
                              .ToList();

            // All day activities attended by a person in a range
            IEnumerable<DayActivity> dayActivitiesAttendedByUser =
                                        _customDays.Where(cd => cd.Date.Date >= start.Date && cd.Date.Date <= end.Date)
                                        .Include(cd => cd.DayActivities).ThenInclude(da => da.Attendances).ThenInclude(a => a.User)
                                        .SelectMany(cd => cd.DayActivities)
                                        .Where(da => da.Attendances.Any(a => a.UserId == userId))
                                        .ToList();

            // Replace customdays dayactivities with those attended
            foreach(var customDay in customDaysRange) {
                customDay.DayActivities = dayActivitiesAttendedByUser.Where(da=>da.DayId==customDay.DayId).ToList();
            }
            
            return customDaysRange.ToList();
        }

        public IEnumerable<User> GetAbsentUsersForDay(DateTime date)
        {
            return _customDays.Where(d => d.Date.Date == date.Date)
                              .SelectMany(d => d.DayActivities
                                    .Where(da => da.Activity.ActivityType.Equals(ActivityType.AFWEZIG))
                                    .SelectMany(da => da.Attendances
                                            .Select(a => a.User)))
                              .ToList();
        }

        public IEnumerable<User> GetSickUsersForDay(DateTime date)
        {
            return _customDays.Where(d => d.Date.Date == date.Date)
                              .SelectMany(d => d.DayActivities
                                    .Where(da => da.Activity.ActivityType.Equals(ActivityType.ZIEK))
                                    .SelectMany(da => da.Attendances
                                            .Select(a => a.User)))
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
