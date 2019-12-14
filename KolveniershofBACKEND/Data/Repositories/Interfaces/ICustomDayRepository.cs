using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface ICustomDayRepository
    {
        IEnumerable<CustomDay> GetAll();
        IEnumerable<CustomDay> GetAllInRange(DateTime start, DateTime end);
        IEnumerable<CustomDay> GetAllInRangeForUser(DateTime start, DateTime end, int userId);
        IEnumerable<CustomDay> GetAllInRangeForUserNoHelpers(DateTime start, DateTime end, int userId);
        IEnumerable<User> GetAbsentUsersForDay(DateTime date);
        IEnumerable<User> GetSickUsersForDay(DateTime date);
        IEnumerable<Note> GetNotesForDay(DateTime date);
        IEnumerable<Helper> GetHelpersForDay(DateTime date);
        IEnumerable<User> GetPossibleHelpers(DateTime date);
        IEnumerable<Activity> GetPossibleDayActivities(DateTime date, TimeOfDay timeOfDay);
        CustomDay GetById(int id);
        CustomDay GetByDate(DateTime date);
        CustomDay GetByDateNoHelpers(DateTime date);
        void Add(CustomDay customDay);
        void Remove(CustomDay customDay);
        void SaveChanges();
    }
}
