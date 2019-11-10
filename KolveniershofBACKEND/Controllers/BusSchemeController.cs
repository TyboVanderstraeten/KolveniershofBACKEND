using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Controllers
{
    [ApiController]
    [Route("KolveniershofAPI/[controller]")]
    public class BusSchemeController : ControllerBase
    {
        private readonly IBusDriverRepository _busDriverRepository;

        public BusSchemeController(IBusDriverRepository busDriverRepository)
        {
            _busDriverRepository = busDriverRepository;
        }

        /// <summary>
        /// Get the bus scheme for a template week
        /// </summary>
        /// <param name="weekNr">The number of a particular week</param>
        [HttpGet]
        [Route("getByWeek/{weekNr}")]
        public ActionResult<IEnumerable<BusDriver>> GetBusScheme(int weekNr)
        {
            return _busDriverRepository.GetDriversByWeek(weekNr).ToList();
        }
    }
}
