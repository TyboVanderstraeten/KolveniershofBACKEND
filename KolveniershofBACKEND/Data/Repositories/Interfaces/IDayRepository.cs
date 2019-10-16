using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface IDayRepository
    {
        IEnumerable<Day> GetAll();
        Day GetById(int id);
        void Add(Day day);
        void Remove(Day day);
        void SaveChanges();
    }
}
