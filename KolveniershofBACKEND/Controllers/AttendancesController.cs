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
    public class AttendancesController : ControllerBase
    {
        private readonly ICustomDayRepository _customDayRepository;

        public AttendancesController(ICustomDayRepository customDayRepository)
        {
            _customDayRepository = customDayRepository;
        }

        [HttpGet]
        [Route("activity/{date}/{id}/{timeOfDay}")]
        public ActionResult<IEnumerable<Attendance>> GetAllForActivity(DateTime date, int id, TimeOfDay timeOfDay)
        {
            DayActivity dayActivity = _customDayRepository
                                            .GetByDate(date)
                                            .DayActivities
                                            .SingleOrDefault(da => da.ActivityId == id && da.TimeOfDay.Equals(timeOfDay));
            return dayActivity.Attendances.ToList();
        }

        

    }
}
