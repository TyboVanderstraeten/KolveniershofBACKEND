using KolveniershofBACKEND.Models.Domain;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface IBusDriverRepository
    {
        IEnumerable<BusDriver> GetDriversByWeek(int weekNr);
        BusDriver GetBusDriverByDayIdDriverIdAndTimeOfDay(int dayId, int driverId, TimeOfDay timeOfDay);
        void Add(BusDriver busDriver);
        void Remove(BusDriver busDriver);
        void SaveChanges();
    }
}
