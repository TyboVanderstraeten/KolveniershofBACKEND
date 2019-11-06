using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface IHelperRepository
    {
        Helper GetTemplateDayHelper(string templateName, int weekNr, int dayNr, int userId);
        Helper GetCustomDayHelper(DateTime date, int userId);
        void SaveChanges();
    }
}
