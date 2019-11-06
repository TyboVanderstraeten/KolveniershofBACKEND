using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Repositories.Concrete
{
    public class NoteRepository : INoteRepository
    {
        private readonly DBContext _dbContext;
        private readonly DbSet<CustomDay> _customDays;

        public NoteRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
            _customDays = dbContext.CustomDays;
        }

        public Note GetCustomDayNote(DateTime date, int noteId)
        {
            return _customDays.Include(d => d.Notes)
                  .SingleOrDefault(d => d.Date.Date == date.Date)
                  .Notes
                  .SingleOrDefault(n => n.NoteId == noteId);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
