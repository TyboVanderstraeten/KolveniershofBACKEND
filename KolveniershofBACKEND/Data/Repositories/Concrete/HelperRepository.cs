using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Repositories.Concrete
{
    public class HelperRepository : IHelperRepository
    {
        private readonly DBContext _dbContext;
        private readonly DbSet<Day> _days;

        public HelperRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
            _days = dbContext.Days;
        }

        public Helper GetTemplateDayHelper(string templateName, int weekNr, int dayNr, int userId)
        {
            return _days.Include(d => d.Helpers).ThenInclude(h => h.User)
                              .SingleOrDefault(d => d.TemplateName.ToLower().Trim().Equals(templateName.ToLower().Trim()) && d.WeekNr == weekNr && d.DayNr == dayNr)
                              .Helpers
                              .SingleOrDefault(h => h.UserId == userId);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
