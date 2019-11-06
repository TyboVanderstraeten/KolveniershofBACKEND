using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface IWeekendDayRepository
    {
        WeekendDay GetByDate(DateTime date, int userId);
        void SaveChanges();
    }
}
