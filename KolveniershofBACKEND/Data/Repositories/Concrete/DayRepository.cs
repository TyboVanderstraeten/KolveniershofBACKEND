using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Repositories.Concrete
{
    public class DayRepository : IDayRepository
    {
        private DBContext _dbContext;
        private DbSet<Day> _days;

        public DayRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
            _days = dbContext.Days;
        }


        public IEnumerable<Day> GetAll()
        {
            return _days.Include(d => d.DayActivities)
                        .Include(d => d.Helpers)
                        .ToList();
        }

        public IEnumerable<Day> GetAllByWeek(int weekNr)
        {
            return _days.Where(d => d.WeekNr == weekNr)
                        .Include(d => d.DayActivities)
                        .Include(d => d.Helpers)
                        .ToList();
        }

        public Day GetById(int id)
        {
            return _days.Include(d => d.DayActivities)
                        .Include(d => d.Helpers)
                        .SingleOrDefault(d => d.DayId == id);
        }

        public Day GetByWeekAndDay(int weekNr, int dayNr)
        {
            return _days.Include(d => d.DayActivities)
                        .Include(d => d.Helpers)
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
