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
            return _customDays.Where(cd => cd.Date >= start && cd.Date <= end)
                              .Include(cd => cd.Notes)
                              .Include(cd => cd.DayActivities)
                              .Include(cd => cd.Helpers)
                              .ToList();
        }

        public CustomDay GetByDate(DateTime date)
        {
            return _customDays.Include(cd => cd.Notes)
                              .Include(cd => cd.DayActivities)
                              .Include(cd => cd.Helpers)
                              .SingleOrDefault(cd => cd.Date == date);
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
