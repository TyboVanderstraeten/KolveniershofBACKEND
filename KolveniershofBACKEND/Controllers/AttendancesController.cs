using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.DTO;
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
        [Route("{date}/{timeOfDay}/{activityId}")]
        public ActionResult<IEnumerable<Attendance>> GetAll(DateTime date, int activityId, TimeOfDay timeOfDay)
        {
            DayActivity dayActivity = _customDayRepository
                                            .GetByDate(date)
                                            .DayActivities
                                            .SingleOrDefault(da => da.ActivityId == activityId && da.TimeOfDay.Equals(timeOfDay));
            return dayActivity.Attendances.ToList();
        }

        [HttpGet]
        [Route("clients/{date}/{timeOfDay}/{activityId}")]
        public ActionResult<IEnumerable<Attendance>> GetAllClients(DateTime date, int activityId, TimeOfDay timeOfDay)
        {
            DayActivity dayActivity = _customDayRepository
                                            .GetByDate(date)
                                            .DayActivities
                                            .SingleOrDefault(da => da.ActivityId == activityId && da.TimeOfDay.Equals(timeOfDay));
            return dayActivity.Attendances.Where(a => a.User.UserType.Equals(UserType.CLIENT)).ToList();
        }


        [HttpGet]
        [Route("personnel/{date}/{timeOfDay}/{activityId}")]
        public ActionResult<IEnumerable<Attendance>> GetAllPersonnel(DateTime date, int activityId, TimeOfDay timeOfDay)
        {
            DayActivity dayActivity = _customDayRepository
                                            .GetByDate(date)
                                            .DayActivities
                                            .SingleOrDefault(da => da.ActivityId == activityId && da.TimeOfDay.Equals(timeOfDay));
            return dayActivity.Attendances.Where(a => !(a.User.UserType.Equals(UserType.CLIENT))).ToList();
        }

        [HttpPost]
        [Route("{date}/{timeOfDay}/{activityId}/{userId}")]
        public ActionResult<Attendance> Add(DateTime date, TimeOfDay timeOfDay, int activityId, int userId)
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
        [Route("{date}/{timeOfDay}/{activityId}/{userId}")]
        public ActionResult<Attendance> Remove(DateTime date, TimeOfDay timeOfDay, int activityId, int userId)
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

        [HttpPost]
        [Route("comment/{date}/{timeOfDay}/{activityId}/{userId}")]
        public ActionResult<Attendance> AddComment(DateTime date, TimeOfDay timeOfDay, int activityId, int userId, CommentDTO model)
        {
            DayActivity dayActivity = _customDayRepository
                                          .GetByDate(date)
                                          .DayActivities
                                          .SingleOrDefault(da => da.ActivityId == activityId && da.TimeOfDay.Equals(timeOfDay));
            Attendance attendanceToEdit = dayActivity.Attendances.SingleOrDefault(a => a.UserId == userId);
            attendanceToEdit.Comment = model.Comment;
            _customDayRepository.SaveChanges();
            return attendanceToEdit;
        }

        [HttpDelete]
        [Route("comment/{date}/{timeOfDay}/{activityId}/{userId}")]
        public ActionResult<Attendance> RemoveComment(DateTime date, TimeOfDay timeOfDay, int activityId, int userId)
        {
            DayActivity dayActivity = _customDayRepository
                                          .GetByDate(date)
                                          .DayActivities
                                          .SingleOrDefault(da => da.ActivityId == activityId && da.TimeOfDay.Equals(timeOfDay));
            Attendance attendanceToEdit = dayActivity.Attendances.SingleOrDefault(a => a.UserId == userId);
            attendanceToEdit.Comment = null;
            _customDayRepository.SaveChanges();
            return attendanceToEdit;
        }
    }
}
