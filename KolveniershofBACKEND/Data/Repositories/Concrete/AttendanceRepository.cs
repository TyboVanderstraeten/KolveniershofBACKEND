using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Repositories.Concrete
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly DBContext _dbContext;
        private readonly DbSet<CustomDay> _customDays;
        private readonly DbSet<User> _users;

        public AttendanceRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
            _customDays = dbContext.CustomDays;
            _users = dbContext.Users;
        }

        public Attendance GetForUser(DateTime date, TimeOfDay timeOfDay, int activityId, int userId)
        {
            return _customDays.Include(cd => cd.DayActivities).ThenInclude(da => da.Attendances).ThenInclude(a => a.User)
                                      .SingleOrDefault(cd => cd.Date.Date == date.Date)
                                      .DayActivities
                                      .SingleOrDefault(da => da.ActivityId == activityId && da.TimeOfDay.Equals(timeOfDay))
                                      .Attendances
                                      .SingleOrDefault(a => a.UserId == userId);
        }

        public IEnumerable<User> GetPossibleClients(DateTime date, TimeOfDay timeOfDay, int activityId)
        {
            return _users.Where(u => u.UserType.Equals(UserType.CLIENT))
                    .Except(_customDays.Where(cd => cd.Date.Date == date.Date)
                                       .SelectMany(cd => cd.DayActivities)
                                       .Where(da => da.TimeOfDay.Equals(timeOfDay) && da.ActivityId == activityId)
                                       .SelectMany(da => da.Attendances).Include(a => a.User)
                                       .Select(a => a.User))
                                       .ToList();
        }

        public IEnumerable<User> GetPossiblePersonnel(DateTime date, TimeOfDay timeOfDay, int activityId)
        {
            return _users.Where(u => !u.UserType.Equals(UserType.CLIENT))
                     .Except(_customDays.Where(cd => cd.Date.Date == date.Date)
                           .SelectMany(cd => cd.DayActivities)
                           .Where(da => da.TimeOfDay.Equals(timeOfDay) && da.ActivityId == activityId)
                           .SelectMany(da => da.Attendances).Include(a => a.User)
                           .Select(a => a.User))
                           .ToList();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
