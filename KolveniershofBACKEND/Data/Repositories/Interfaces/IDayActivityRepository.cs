using KolveniershofBACKEND.Models.Domain;
using System;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface IDayActivityRepository
    {
        DayActivity GetTemplateDayActivity(string templateName, int weekNr, int dayNr, TimeOfDay timeOfDay, int activityId)
        DayActivity GetCustomDayActivity(DateTime date, TimeOfDay timeOfDay, int activityId);
        void SaveChanges();
    }
}
