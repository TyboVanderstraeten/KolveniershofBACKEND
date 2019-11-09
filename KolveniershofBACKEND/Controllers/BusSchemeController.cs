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
        /// Get the bus scheme from a templateDay
        /// </summary>
        /// <param name="dayId">The id of an particular day</param>
        [HttpGet]
        [Route("{dayId}")]
        public ActionResult<IEnumerable<BusDriver>> GetBusScheme(int dayId)
        {
            return _busDriverRepository.GetDriversForDay(dayId).ToList();
        }
    }
}
