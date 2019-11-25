using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Data.Repositories.Concrete
{
    public class DriverRepository : IDriverRepository
    {
        private readonly DBContext _dbContext;
        private readonly DbSet<Driver> _drivers;

        public DriverRepository(DBContext dBContext)
        {
            _dbContext = dBContext;
            _drivers = _dbContext.Drivers;
        }

        public void Add(Driver newDriver)
        {
            _drivers.Add(newDriver);
        }

        public IEnumerable<Driver> GetAllDrivers()
        {
            return _drivers.ToList();
        }

        public Driver GetById(int driverId)
        {
            return _drivers.SingleOrDefault(d => d.DriverId == driverId);
        }

        public Driver GetByName(string name)
        {
            return _drivers.SingleOrDefault(d => d.Name == name);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
