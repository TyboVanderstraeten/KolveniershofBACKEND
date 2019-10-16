using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
