using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Repositories.Concrete
{
    public class DayActivityRepository : IDayActivityRepository
    {
        private readonly DBContext _dbContext;
        private readonly DbSet<Day> _days;
        private readonly DbSet<CustomDay> _customDays;

        public DayActivityRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
            _days = dbContext.Days;
            _customDays = dbContext.CustomDays;
        }

        public DayActivity GetTemplateDayActivity(string templateName, int weekNr, int dayNr, TimeOfDay timeOfDay, int activityId)
        {
            return _days.Include(d => d.DayActivities).ThenInclude(da => da.Attendances).ThenInclude(a => a.User)
                              .SingleOrDefault(d => d.TemplateName.ToLower().Trim().Equals(templateName.ToLower().Trim()) && d.WeekNr == weekNr && d.DayNr == dayNr)
                              .DayActivities
                              .SingleOrDefault(da => da.ActivityId == activityId && da.TimeOfDay.Equals(timeOfDay));
        }

        public DayActivity GetCustomDayActivity(DateTime date, TimeOfDay timeOfDay, int activityId)
        {
            return _customDays.Include(cd => cd.DayActivities).ThenInclude(da => da.Attendances).ThenInclude(a => a.User)
                              .SingleOrDefault(cd => cd.Date.Date == date.Date)
                              .DayActivities
                              .SingleOrDefault(da => da.ActivityId == activityId && da.TimeOfDay.Equals(timeOfDay));
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
