using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KolveniershofBACKEND.Controllers
{
    [ApiController]
    [Route("KolveniershofAPI/[controller]")]
    public class BusSchemesController : ControllerBase
    {
        private readonly IBusDriverRepository _busDriverRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly ICustomDayRepository _customDayRepository;

        public BusSchemesController(IBusDriverRepository busDriverRepository, IDriverRepository driverRepository, ICustomDayRepository customDayRepository)
        {
            _busDriverRepository = busDriverRepository;
            _driverRepository = driverRepository;
            _customDayRepository = customDayRepository;
        }

        /// <summary>
        /// Get the bus scheme for a template week
        /// </summary>
        /// <param name="weekNr">The number of a particular week</param>
        /// <returns>All busDrivers with chosen week number</returns>
        [HttpGet]
        [Route("getByWeek/{weekNr}")]
        public ActionResult<IEnumerable<BusDriver>> GetBusScheme(int weekNr)
        {
            var busDrivers = _busDriverRepository.GetBusDriversByWeek(weekNr).ToList();

            if(busDrivers == null || !busDrivers.Any())
            {
                return NotFound();
            }
            else
            {
                return Ok(busDrivers);
            }
        }

        /// <summary>
        /// Get the bus scheme for a day
        /// </summary>
        /// <param name="weekNr">The id of a day</param>
        /// <returns>All busDrivers with chosen day id</returns>
        [HttpGet]
        [Route("{dayId}")]
        public ActionResult<IEnumerable<BusDriver>> GetBusSchemesByDayId(int dayId)
        {
            var busDrivers = _busDriverRepository.GetBusDriversByDayId(dayId);

            if (busDrivers == null || !busDrivers.Any())
            {
                return Ok(new List<BusDriver>());
            }
            else
            {
                return Ok(busDrivers);
            }
        }

        /// <summary>
        /// Update the driver for a particular day
        /// </summary>
        /// <param name="editBusDriverDTO">The particular day with the id of the driver and time of day</param>
        [HttpPut]
        [Route("edit")]
        public ActionResult<BusDriver> Edit(BusDriverDTO busDriverDTO)
        {
            var busDriver = _busDriverRepository.GetBusDriverByDayIdBusColorAndTimeOfDay(busDriverDTO.DayId, busDriverDTO.BusColor, busDriverDTO.TimeOfDay);

            if (busDriver == null)
            {
                return NotFound();
            }

            var newDriver = _driverRepository.GetById(busDriverDTO.DriverId);

            if (newDriver == null)
            {
                return NotFound();
            }

            try
            {
                busDriver.Driver = newDriver;
                busDriver.DriverId = newDriver.DriverId;

                _busDriverRepository.SaveChanges();

                return Ok(busDriver);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new bus driver
        /// </summary>
        /// <param name="addBusDriverDTO">Then new bus driver that will be added</param>
        /// <returns>The new bus driver</returns>
        [HttpPost]
        [Route("add")]
        public ActionResult<BusDriver> Add(BusDriverDTO busDriverDTO)
        {
            var existingBusDriver = _busDriverRepository.GetBusDriverByDayIdBusColorAndTimeOfDay(
                            busDriverDTO.DayId, busDriverDTO.BusColor, busDriverDTO.TimeOfDay);

            if(existingBusDriver != null)
            {
                return BadRequest("Er is al een chauffeur die op die dag rijdt!");
            }

            var day = _customDayRepository.GetById(busDriverDTO.DayId);

            if(day == null)
            {
                return NotFound();
            }

            var driver = _driverRepository.GetById(busDriverDTO.DriverId);

            if(driver == null)
            {
                return NotFound();
            }

            try
            {
                BusDriver newBusDriver = new BusDriver(day, driver, busDriverDTO.TimeOfDay, busDriverDTO.BusColor);

                _busDriverRepository.Add(newBusDriver);
                _busDriverRepository.SaveChanges();

                return Ok(newBusDriver);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
