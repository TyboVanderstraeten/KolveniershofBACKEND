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

        public DayRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
            _days = dbContext.Days;
        }


        public IEnumerable<Day> GetAll()
        {
            return _days.Where(d => !(d is CustomDay))
                        .Include(d => d.DayActivities).ThenInclude(da => da.Activity)
                        .Include(d => d.Helpers).ThenInclude(h => h.User)
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
