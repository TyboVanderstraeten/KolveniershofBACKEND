using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace KolveniershofBACKEND.Data.Repositories.Concrete
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _dbContext;
        private readonly DbSet<User> _users;

        public UserRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
            _users = dbContext.Users;
        }

        public IEnumerable<User> GetAll()
        {
            return _users.ToList();
        }

        public IEnumerable<User> GetAllFromGroup(int group)
        {
            return _users.Where(u => u.Group == group).ToList();
        }

        public IEnumerable<User> GetAllWithType(UserType userType)
        {
            return _users.Where(u => u.UserType == userType).ToList();
        }

        public IEnumerable<Attendance> GetAttendancesFromUser(int id)
        {
            return _users.SingleOrDefault(u => u.UserId == id).Attendances.ToList();
        }

        public IEnumerable<CustomWeekendDay> GetCustomWeekendDaysFromUser(int id)
        {
            return _users.SingleOrDefault(u => u.UserId == id).CustomWeekendDays.ToList();
        }

        public User GetById(int id)
        {
            return _users.SingleOrDefault(u => u.UserId == id);
        }

        public User GetByEmail(string email)
        {
            return _users.SingleOrDefault(u => u.Email.ToLower().Equals(email.ToLower()));
        }

        public void Add(User user)
        {
            _users.Add(user);
        }

        public void Remove(User user)
        {
            _users.Remove(user);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
