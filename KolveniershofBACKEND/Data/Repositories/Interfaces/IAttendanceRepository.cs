using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface IAttendanceRepository
    {
        IEnumerable<User> GetPossibleClients(DateTime date, TimeOfDay timeOfDay, int activityId);
        IEnumerable<User> GetPossiblePersonnel(DateTime date, TimeOfDay timeOfDay, int activityId);
        Attendance GetForUser(DateTime date, TimeOfDay timeOfDay, int activityId, int userId);
        void SaveChanges();
    }
}
