using KolveniershofBACKEND.Models.Domain;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface IActivityRepository
    {
        IEnumerable<Activity> GetAll();
        Activity GetById(int id);
        void Add(Activity activity);
        void Remove(Activity activity);
        void SaveChanges();
    }
}
