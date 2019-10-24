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
                              .Include(cd => cd.DayActivities)
                              .Include(cd => cd.Helpers)
                              .ToList();
        }

        public IEnumerable<CustomDay> GetAllInRange(DateTime start, DateTime end)
        {
            return _customDays.Where(cd => cd.Date.Date >= start.Date && cd.Date.Date <= end.Date)
                              .Include(cd => cd.Notes)
                              .Include(cd => cd.DayActivities)
                              .Include(cd => cd.Helpers)
                              .ToList();
        }

        public IEnumerable<User> GetAbsentUsersForDay(DateTime date)
        {
            return (IEnumerable<User>)_customDays.Where(d => d.Date.Date == date.Date)
                                                 .Select(d => d.DayActivities
                                                       .Where(da => da.Activity.ActivityType.Equals(ActivityType.AFWEZIG))
                                                       .Select(da => da.Attendances
                                                           .Select(a => a.User)))
                                                 .ToList();
        }

        public IEnumerable<User> GetSickUsersForDay(DateTime date)
        {
            return (IEnumerable<User>)_customDays.Where(d => d.Date.Date == date.Date)
                                                 .Select(d => d.DayActivities
                                                       .Where(da => da.Activity.ActivityType.Equals(ActivityType.ZIEK))
                                                       .Select(da => da.Attendances
                                                           .Select(a => a.User)))
                                                 .ToList();
        }

        public IEnumerable<Note> GetNotesForDay(DateTime date)
        {
            return (IEnumerable<Note>)_customDays.Where(d => d.Date.Date == date.Date)
                                                 .Select(d => d.Notes)
                                                 .ToList();
        }

        public IEnumerable<Helper> GetHelpersForDay(DateTime date)
        {
            return (IEnumerable<Helper>)_customDays.Where(d => d.Date.Date == date.Date)
                                                   .Select(d => d.Helpers)
                                                   .ToList();
        }

        public CustomDay GetByDate(DateTime date)
        {
            return _customDays.Include(cd => cd.Notes)
                              .Include(cd => cd.DayActivities)
                              .Include(cd => cd.Helpers)
                              .SingleOrDefault(cd => cd.Date.Date == date.Date);
        }

        public CustomDay GetById(int id)
        {
            return _customDays.Include(cd => cd.Notes)
                              .Include(cd => cd.DayActivities)
                              .Include(cd => cd.Helpers)
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
