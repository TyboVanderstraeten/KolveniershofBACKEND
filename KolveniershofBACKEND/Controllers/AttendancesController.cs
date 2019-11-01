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

        [HttpGet]
        [Route("activity/clients/{date}/{id}/{timeOfDay}")]
        public ActionResult<IEnumerable<Attendance>> GetAllAttendedClientsForActivity(DateTime date, int id, TimeOfDay timeOfDay)
        {
            DayActivity dayActivity = _customDayRepository
                                            .GetByDate(date)
                                            .DayActivities
                                            .SingleOrDefault(da => da.ActivityId == id && da.TimeOfDay.Equals(timeOfDay));
            return dayActivity.Attendances.Where(a => a.User.UserType.Equals(UserType.CLIENT)).ToList();
        }


        [HttpGet]
        [Route("activity/personnel/{date}/{id}/{timeOfDay}")]
        public ActionResult<IEnumerable<Attendance>> GetAllAttendedPersonnelForActivity(DateTime date, int id, TimeOfDay timeOfDay)
        {
            DayActivity dayActivity = _customDayRepository
                                            .GetByDate(date)
                                            .DayActivities
                                            .SingleOrDefault(da => da.ActivityId == id && da.TimeOfDay.Equals(timeOfDay));
            return dayActivity.Attendances.Where(a => !(a.User.UserType.Equals(UserType.CLIENT))).ToList();
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

        [HttpDelete]
        [Route("activitiy/{date}/{timeOfDay}/{activityId}/{userId}")]
        public ActionResult<Attendance> RemoveAttendanceFromActivity(DateTime date, TimeOfDay timeOfDay, int activityId, int userId)
        {
            DayActivity dayActivity = _customDayRepository
                                            .GetByDate(date)
                                            .DayActivities
                                            .SingleOrDefault(da => da.ActivityId == activityId && da.TimeOfDay.Equals(timeOfDay));
            Attendance attendanceToRemove = dayActivity
                                                .Attendances
                                                .SingleOrDefault(a => a.UserId == userId);
            dayActivity.RemoveAttendance(attendanceToRemove);
            _customDayRepository.SaveChanges();
            return attendanceToRemove;
        }



    }
}
