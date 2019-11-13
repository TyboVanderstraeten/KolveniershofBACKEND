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

        // The reason i 'hardcode' summer is because it doesn't matter for this functionality
        // summer or winter the busses need to drive
        public IEnumerable<BusDriver> GetDriversByWeek(int weekNr)
        {
            return  _busDrivers.Include(b => b.Day)
                .Include(b => b.Driver)
                .Where(b => b.Day.WeekNr == weekNr && b.Day.TemplateName == "zomer")
                .ToList();
        }
    }
}
