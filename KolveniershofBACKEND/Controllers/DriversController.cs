using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.DTO;
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

        /// <summary>
        /// Adds new driver
        /// </summary>
        /// <returns>Newly created driver</returns>
        [HttpPost]
        [Route("new")]
        public ActionResult<Driver> AddDriver(DriverDTO driverDTO)
        {
            Driver existingDriver = _driverRepository.GetByName(driverDTO.Name);
            if(existingDriver != null)
            {
                return BadRequest($"Chauffeur met de naam '{driverDTO.Name}' bestaat al!");
            }

            try
            {
                Driver newDriver = new Driver(driverDTO.Name);

                _driverRepository.Add(newDriver);
                _driverRepository.SaveChanges();

                return Ok(newDriver);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}