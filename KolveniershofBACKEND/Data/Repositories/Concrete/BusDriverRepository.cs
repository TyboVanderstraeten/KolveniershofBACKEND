using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
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

        public IEnumerable<BusDriver> GetDriversForDay(int dayId)
        {
            var lol =  _busDrivers.Include(b => b.Day).Include(b => b.Driver).Where(b => b.DayId == dayId).ToList();

            return lol;
        }
    }
}
