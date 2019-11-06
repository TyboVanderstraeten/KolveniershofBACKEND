using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface INoteRepository
    {
        Note GetCustomDayNote(DateTime date, int noteId);
        void SaveChanges();
    }
}
