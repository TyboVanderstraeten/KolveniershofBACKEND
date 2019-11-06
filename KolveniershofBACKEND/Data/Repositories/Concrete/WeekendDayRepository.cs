using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Repositories.Concrete
{
    public class WeekendDayRepository : IWeekendDayRepository
    {
        private readonly DBContext _dbContext;
        private readonly DbSet<User> _users;

        public WeekendDayRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
            _users = dbContext.Users;
        }

        public WeekendDay GetByDate(DateTime date, int userId)
        {
            return _users.Include(u => u.WeekendDays).SingleOrDefault(u => u.UserId == userId)
                         .WeekendDays
                         .SingleOrDefault(w => w.Date.Date == date.Date);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
