using KolveniershofBACKEND.Models.Domain;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        IEnumerable<User> GetAllFromGroup(int group);
        IEnumerable<User> GetAllWithType(UserType userType);
        User GetById(int id);
        User GetByEmail(string email);
        void Add(User user);
        void Remove(User user);
        void SaveChanges();
    }
}
