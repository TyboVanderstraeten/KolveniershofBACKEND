using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.Domain.Enums;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
  public interface IBusDriverRepository
  {
    IEnumerable<BusDriver> GetBusDriversByWeek(int weekNr);
    IEnumerable<BusDriver> GetBusDriversByDayId(int dayId);
    BusDriver GetBusDriverByDayIdBusColorAndTimeOfDay(int dayId, BusColor busColor, TimeOfDay timeOfDay);
    IEnumerable<int> GetAllWeeks();
    void Add(BusDriver busDriver);
    void Remove(BusDriver busDriver);
    void SaveChanges();
  }
}
