using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface IAttendanceRepository
    {
        Attendance GetForUser(DateTime date, TimeOfDay timeOfDay, int activityId, int userId);
        void SaveChanges();
    }
}
