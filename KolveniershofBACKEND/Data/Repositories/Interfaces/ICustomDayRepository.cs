using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface ICustomDayRepository
    {
        IEnumerable<CustomDay> GetAll();
        IEnumerable<CustomDay> GetAllInRange(DateTime start, DateTime end);
        CustomDay GetById(int id);
        CustomDay GetByDate(DateTime date);
        void Add(CustomDay customDay);
        void Remove(CustomDay customDay);
        void SaveChanges();
    }
}
