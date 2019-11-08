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

        public AttendanceRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
            _customDays = dbContext.CustomDays;
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
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
