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
        IEnumerable<Note> GetNotesFromCustomDay(int id);
        CustomDay GetById(int id);
        CustomDay GetByIdWithNotes(int id);
        void Add(CustomDay customDay);
        void Remove(CustomDay customDay);
        void SaveChanges();
    }
}
