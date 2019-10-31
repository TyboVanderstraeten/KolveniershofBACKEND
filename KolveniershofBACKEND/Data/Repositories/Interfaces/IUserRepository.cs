using KolveniershofBACKEND.Models.Domain;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        IEnumerable<User> GetAllFromGroup(int group);
        IEnumerable<User> GetAllWithType(UserType userType);
        IEnumerable<Attendance> GetAttendancesFromUser(int id);
        // This is going to need some other method as well
        IEnumerable<WeekendDay> GetWeekendDaysFromUser(int id);
        User GetById(int id);
        User GetByEmail(string email);
        void Add(User user);
        void Remove(User user);
        void SaveChanges();
    }
}
