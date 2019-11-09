using KolveniershofBACKEND.Models.Domain;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface IBusDriverRepository
    {
        IEnumerable<BusDriver> GetDriversForDay(int dayId);
    }
}
