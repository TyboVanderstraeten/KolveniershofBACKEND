using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace KolveniershofBACKEND.Data.Repositories.Concrete
{
  public class BusDriverRepository : IBusDriverRepository
  {
    private readonly DBContext _dbContext;
    private readonly DbSet<BusDriver> _busDrivers;

    public BusDriverRepository(DBContext dBContext)
    {
      _dbContext = dBContext;
      _busDrivers = _dbContext.BusDrivers;
    }

    public void Add(BusDriver busDriver)
    {
      _busDrivers.Add(busDriver);
    }

    public IEnumerable<int> GetAllWeeks()
    {
      return _busDrivers.Select(b => b.Day.WeekNr).Distinct();
    }

    public BusDriver GetBusDriverByDayIdBusColorAndTimeOfDay(int dayId, BusColor busColor, TimeOfDay timeOfDay)
    {
      return _busDrivers
          .Include(b => b.Day)
          .Include(b => b.Driver)
          .SingleOrDefault(d => d.DayId == dayId && d.BusColor == busColor && d.TimeOfDay == timeOfDay);
    }

    public IEnumerable<BusDriver> GetBusDriversByDayId(int dayId)
    {
      return _busDrivers
          .Include(b => b.Day)
          .Include(b => b.Driver)
          .Where(b => b.DayId == dayId).ToList();
    }

    // The reason I 'hardcode' summer is because it doesn't matter for this functionality
    // summer or winter the busses need to drive
    public IEnumerable<BusDriver> GetBusDriversByWeek(int weekNr)
    {
      return _busDrivers
          .Include(b => b.Day)
          .Include(b => b.Driver)
          .Where(b => b.Day.WeekNr == weekNr && b.Day.TemplateName == "zomer")
          .ToList();
    }

    public void Remove(BusDriver busDriver)
    {
      _busDrivers.Remove(busDriver);
    }

    public void SaveChanges()
    {
      _dbContext.SaveChanges();
    }
  }
}
