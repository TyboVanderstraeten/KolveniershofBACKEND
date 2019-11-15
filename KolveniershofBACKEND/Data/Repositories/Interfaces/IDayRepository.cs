using KolveniershofBACKEND.Models.Domain;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface IDayRepository
    {
        IEnumerable<Day> GetAll();
        IEnumerable<Day> GetAllByWeek(string templateName, int weekNr);
        IEnumerable<Day> GetAllByTemplateName(string templateName);
        Day GetById(int id);
        Day GetByWeekAndDay(string templateName, int weekNr, int dayNr);
        IEnumerable<User> GetPossibleHelpers(string templateName, int weekNr, int dayNr);
        IEnumerable<Activity> GetPossibleDayActivities(string templateName, int weekNr, int dayNr,TimeOfDay timeOfDay);
        void Add(Day day);
        void Remove(Day day);
        void SaveChanges();
    }
}
