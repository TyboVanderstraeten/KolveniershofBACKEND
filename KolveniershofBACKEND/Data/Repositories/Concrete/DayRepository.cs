using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace KolveniershofBACKEND.Data.Repositories.Concrete
{
    public class DayRepository : IDayRepository
    {
        private readonly DBContext _dbContext;
        private readonly DbSet<Day> _days;
        private readonly DbSet<User> _users;
        private readonly DbSet<Activity> _activities;

        public DayRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
            _days = dbContext.Days;
            _users = dbContext.Users;
            _activities = dbContext.Activities;
        }


        public IEnumerable<Day> GetAll()
        {
            return _days.Where(d => !(d is CustomDay))
                        .Include(d => d.DayActivities).ThenInclude(da => da.Activity)
                        .Include(d => d.Helpers).ThenInclude(h => h.User)
                        .OrderBy(d => d.TemplateName).ThenBy(d => d.WeekNr).ThenBy(d => d.DayNr)
                        .ToList();
        }

        public IEnumerable<Day> GetAllByWeek(string templateName, int weekNr)
        {
            return _days.Where(d => !(d is CustomDay) && d.WeekNr == weekNr
            && d.TemplateName.ToLower().Trim().Equals(templateName.ToLower().Trim()))
                        .Include(d => d.DayActivities).ThenInclude(da => da.Activity)
                        .Include(d => d.Helpers).ThenInclude(h => h.User)
                        .ToList();
        }

        public IEnumerable<Day> GetAllByTemplateName(string templateName)
        {
            return _days.Where(d => !(d is CustomDay)
            && d.TemplateName.ToLower().Trim().Equals(templateName.ToLower().Trim()))
                        .Include(d => d.DayActivities).ThenInclude(da => da.Activity)
                        .Include(d => d.Helpers).ThenInclude(h => h.User)
                        .ToList();
        }
        public Day GetById(int id)
        {
            return _days.Where(d => !(d is CustomDay))
                        .Include(d => d.DayActivities).ThenInclude(da => da.Activity)
                        .Include(d => d.Helpers).ThenInclude(h => h.User)
                        .SingleOrDefault(d => d.DayId == id);
        }

        public Day GetByWeekAndDay(string templateName, int weekNr, int dayNr)
        {
            return _days.Where(d => !(d is CustomDay)
            && d.TemplateName.ToLower().Trim().Equals(templateName.ToLower().Trim()))
                        .Include(d => d.DayActivities).ThenInclude(da => da.Activity)
                        .Include(d => d.Helpers).ThenInclude(h => h.User)
                        .SingleOrDefault(d => d.WeekNr == weekNr && d.DayNr == dayNr);
        }

        public IEnumerable<User> GetPossibleHelpers(string templateName, int weekNr, int dayNr)
        {
            return _users.Where(u => u.UserType.Equals(UserType.STAGIAIR) || u.UserType.Equals(UserType.VRIJWILLIGER))
                         .Except(_days.Where(d => !(d is CustomDay)
                                      && d.TemplateName.ToLower().Trim().Equals(templateName.ToLower().Trim())
                                      && d.WeekNr == weekNr
                                      && d.DayNr == dayNr)
                                      .SelectMany(d => d.Helpers).Include(h => h.User)
                                      .Select(h => h.User)
                                      .ToList())
                                      .ToList();
        }

        public IEnumerable<Activity> GetPossibleDayActivities(string templateName, int weekNr, int dayNr, TimeOfDay timeOfDay)
        {
            return _activities.Except(_days.Where(d => !(d is CustomDay)
                                      && d.TemplateName.ToLower().Trim().Equals(templateName.ToLower().Trim())
                                      && d.WeekNr == weekNr
                                      && d.DayNr == dayNr)
                                      .SelectMany(d => d.DayActivities).Include(da => da.Activity)
                                      .Where(da => da.TimeOfDay.Equals(timeOfDay))
                                      .Select(da => da.Activity)
                                      .Except(_activities.Where(a => a.ActivityType.Equals(ActivityType.AFWEZIG) || a.ActivityType.Equals(ActivityType.ZIEK)))
                                      .ToList())
                                      .ToList();
        }


        public void Add(Day day)
        {
            _days.Add(day);
        }

        public void Remove(Day day)
        {
            _days.Remove(day);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
