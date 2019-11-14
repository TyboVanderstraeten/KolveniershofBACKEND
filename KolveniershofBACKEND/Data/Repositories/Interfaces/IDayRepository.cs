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
        IEnumerable<User> GetPossibleHelpersForDay(string templateName, int weekNr, int dayNr);
        void Add(Day day);
        void Remove(Day day);
        void SaveChanges();
    }
}
