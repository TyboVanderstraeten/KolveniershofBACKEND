using KolveniershofBACKEND.Models.Domain;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Data.Repositories.Interfaces
{
    public interface IDriverRepository
    {
        IEnumerable<Driver> GetAllDrivers();
        Driver GetById(int driverId);
    }
}
