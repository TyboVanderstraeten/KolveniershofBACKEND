using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface ICustomDayRepository
    {
        IEnumerable<CustomDay> GetAll();
        IEnumerable<CustomDay> GetAllInRange(DateTime start, DateTime end);
        IEnumerable<User> GetAbsentUsersForDay(DateTime date);
        IEnumerable<User> GetSickUsersForDay(DateTime date);
        //IEnumerable<User> GetAttendedUsersForActivity(DateTime date, int activityId);
        IEnumerable<Note> GetNotesForDay(DateTime date);
        IEnumerable<Helper> GetHelpersForDay(DateTime date);
        CustomDay GetById(int id);
        CustomDay GetByDate(DateTime date);
        void Add(CustomDay customDay);
        void Remove(CustomDay customDay);
        void SaveChanges();
    }
}
