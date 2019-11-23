using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KolveniershofBACKEND.Controllers
{
    [Route("KolveniershofAPI/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly IDriverRepository _driverRepository;

        public DriversController(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        /// <summary>
        /// Get all drivers
        /// </summary>
        /// <returns>All drivers</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Driver>> GetAllDrivers()
        {
            return _driverRepository.GetAllDrivers().ToList();
        }
    }
}