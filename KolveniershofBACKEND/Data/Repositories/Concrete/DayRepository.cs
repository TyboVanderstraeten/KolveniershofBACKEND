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
            return _days.Where(d => !(d is CustomDay) && !(d is WeekendDay))
                        .Include(d => d.DayActivities).ThenInclude(da => da.Activity)
                        .Include(d => d.Helpers).ThenInclude(h => h.User)
                        .ToList();
        }

        public IEnumerable<Day> GetAllByWeek(int weekNr)
        {
            return _days.Where(d => !(d is CustomDay) && !(d is WeekendDay) && d.WeekNr == weekNr)
                        .Include(d => d.DayActivities).ThenInclude(da => da.Activity)
                        .Include(d => d.Helpers).ThenInclude(h => h.User)
                        .ToList();
        }

        public Day GetById(int id)
        {
            return _days.Where(d => !(d is CustomDay) && !(d is WeekendDay))
                        .Include(d => d.DayActivities).ThenInclude(da => da.Activity)
                        .Include(d => d.Helpers).ThenInclude(h => h.User)
                        .SingleOrDefault(d => d.DayId == id);
        }

        public Day GetByWeekAndDay(int weekNr, int dayNr)
        {
            return _days.Where(d => !(d is CustomDay) && !(d is WeekendDay))
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
