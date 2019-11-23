using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Repositories.Concrete
{
    public class DriverRepository : IDriverRepository
    {
        private readonly DBContext _dbContext;

        public DriverRepository(DBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public IEnumerable<Driver> GetAllDrivers()
        {
            return _dbContext.Drivers.ToList();
        }

        public Driver GetById(int driverId)
        {
            return _dbContext.Drivers.SingleOrDefault(d => d.DriverId == driverId);
        }
    }
}
