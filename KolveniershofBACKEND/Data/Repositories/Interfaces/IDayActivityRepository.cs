using KolveniershofBACKEND.Models.Domain;
using System;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface IDayActivityRepository
    {
        DayActivity GetDayActivity(DateTime date, TimeOfDay timeOfDay, int activityId);
        void SaveChanges();
    }
}
