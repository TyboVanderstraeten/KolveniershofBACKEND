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

        public BusSchemesController(IBusDriverRepository busDriverRepository, IDriverRepository driverRepository)
        {
            _busDriverRepository = busDriverRepository;
            _driverRepository = driverRepository;
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
                return NotFound();
            }
            else
            {
                return Ok(busDrivers);
            }
        }

        /// <summary>
        /// Update the driver for a particular day
        /// </summary>
        /// <param name="busDriverDTO">The particular day with the id of the driver and time of day</param>
        [HttpPut]
        [Route("edit")]
        public ActionResult<BusDriver> Edit(BusDriverDTO busDriverDTO)
        {
            var busDriver = _busDriverRepository.GetBusDriverByDayIdDriverIdAndTimeOfDay(busDriverDTO.DayId, busDriverDTO.OriginalDriverId, busDriverDTO.TimeOfDay);

            if (busDriver == null)
            {
                return NotFound();
            }

            if (busDriver.Driver.DriverId == busDriverDTO.NewDriverId)
            {
                return BadRequest();
            }

            var newDriver = _driverRepository.GetById(busDriverDTO.NewDriverId);

            if (newDriver == null)
            {
                return NotFound();
            }

            var existingBusDriver = _busDriverRepository.GetBusDriverByDayIdDriverIdAndTimeOfDay(busDriverDTO.DayId, newDriver.DriverId, busDriverDTO.TimeOfDay);

            if(existingBusDriver != null)
            {
                return Forbid();
            }

            try
            {
                var newBusDriver = new BusDriver(busDriver.Day, newDriver, busDriverDTO.TimeOfDay, busDriver.BusColor);
                _busDriverRepository.Remove(busDriver);

                _busDriverRepository.Add(newBusDriver);
                _busDriverRepository.SaveChanges();

                return Ok(newBusDriver);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
