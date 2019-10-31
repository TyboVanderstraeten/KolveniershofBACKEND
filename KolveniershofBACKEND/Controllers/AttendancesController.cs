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
        private readonly IUserRepository _userRepository;

        public AttendancesController(ICustomDayRepository customDayRepository, IUserRepository userRepository)
        {
            _customDayRepository = customDayRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("activity/{date}/{id}/{timeOfDay}")]
        public ActionResult<IEnumerable<Attendance>> GetAllAttendancesForActivity(DateTime date, int id, TimeOfDay timeOfDay)
        {
            DayActivity dayActivity = _customDayRepository
                                            .GetByDate(date)
                                            .DayActivities
                                            .SingleOrDefault(da => da.ActivityId == id && da.TimeOfDay.Equals(timeOfDay));
            return dayActivity.Attendances.ToList();
        }

        [HttpPost]
        [Route("activity/{date}/{timeOfDay}/{activityId}/{userId}")]
        public ActionResult<Attendance> AddAttendanceToActivity(DateTime date, TimeOfDay timeOfDay, int activityId, int userId)
        {
            DayActivity dayActivity = _customDayRepository
                                            .GetByDate(date)
                                            .DayActivities
                                            .SingleOrDefault(da => da.ActivityId == activityId && da.TimeOfDay.Equals(timeOfDay));
            User user = _userRepository.GetById(userId);
            Attendance attendanceToAdd = new Attendance(dayActivity, user);
            dayActivity.AddAttendance(attendanceToAdd);
            _customDayRepository.SaveChanges();
            return attendanceToAdd;
        }

    }
}
