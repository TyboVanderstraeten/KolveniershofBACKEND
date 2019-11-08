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
    public class AttendancesController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IDayActivityRepository _dayActivityRepository;
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendancesController(IUserRepository userRepository, IDayActivityRepository dayActivityRepository,
            IAttendanceRepository attendanceRepository)
        {
            _userRepository = userRepository;
            _dayActivityRepository = dayActivityRepository;
            _attendanceRepository = attendanceRepository;
        }

        /// <summary>
        /// Get all attendances for an activity on a specific day
        /// </summary>
        /// <param name="date">The date of the activity</param>
        /// <param name="activityId">The id of the activity</param>
        /// <param name="timeOfDay">The time of day of the activity</param>
        /// <returns>The attendance</returns>
        [HttpGet]
        [Route("{date}/{timeOfDay}/{activityId}")]
        public ActionResult<IEnumerable<Attendance>> GetAll(DateTime date, int activityId, TimeOfDay timeOfDay)
        {
            return _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId).Attendances.ToList();
        }

        /// <summary>
        /// Get all client attendances for an activity on a specific day
        /// </summary>
        /// <param name="date">The date of the activity</param>
        /// <param name="activityId">The id of the activity</param>
        /// <param name="timeOfDay">The time of day of the activity</param>
        /// <returns>The client attendances</returns>
        [HttpGet]
        [Route("clients/{date}/{timeOfDay}/{activityId}")]
        public ActionResult<IEnumerable<Attendance>> GetAllClients(DateTime date, int activityId, TimeOfDay timeOfDay)
        {
            return _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId).Attendances.Where(a => a.User.UserType.Equals(UserType.CLIENT)).ToList();
        }

        /// <summary>
        /// Get all personnel attendances for an activity on a specific day
        /// </summary>
        /// <param name="date">The date of the activity</param>
        /// <param name="activityId">The id of the activity</param>
        /// <param name="timeOfDay">The time of day of the activity</param>
        /// <returns>The personnel attendances</returns>
        [HttpGet]
        [Route("personnel/{date}/{timeOfDay}/{activityId}")]
        public ActionResult<IEnumerable<Attendance>> GetAllPersonnel(DateTime date, int activityId, TimeOfDay timeOfDay)
        {
            return _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId).Attendances.Where(a => !a.User.UserType.Equals(UserType.CLIENT)).ToList();
        }

        /// <summary>
        /// Create a new attendance for an activity on a specific day
        /// </summary>
        /// <param name="date">The date of the activity</param>
        /// <param name="timeOfDay">The time of day of the activity</param>
        /// <param name="activityId">The id of the activity</param>
        /// <param name="userId">The id of the attending user</param>
        /// <returns>The attendance</returns>
        [HttpPost]
        [Route("{date}/{timeOfDay}/{activityId}/{userId}")]
        public ActionResult<Attendance> Add(DateTime date, TimeOfDay timeOfDay, int activityId, int userId)
        {
            DayActivity dayActivity = _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId);
            User user = _userRepository.GetById(userId);
            Attendance attendanceToAdd = new Attendance(dayActivity, user);
            dayActivity.AddAttendance(attendanceToAdd);
            _dayActivityRepository.SaveChanges();
            return attendanceToAdd;
        }

        /// <summary>
        /// Remove an attendance for an activity on a specific day
        /// </summary>
        /// <param name="date">The date of the activity</param>
        /// <param name="timeOfDay">The time of day of the activity</param>
        /// <param name="activityId">The id of the activity</param>
        /// <param name="userId">The id of the attending user</param>
        /// <returns>The attendance</returns>
        [HttpDelete]
        [Route("{date}/{timeOfDay}/{activityId}/{userId}")]
        public ActionResult<Attendance> Remove(DateTime date, TimeOfDay timeOfDay, int activityId, int userId)
        {
            DayActivity dayActivity = _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId);
            Attendance attendanceToRemove = _attendanceRepository.GetForUser(date, timeOfDay, activityId, userId);
            dayActivity.RemoveAttendance(attendanceToRemove);
            _dayActivityRepository.SaveChanges();
            return attendanceToRemove;
        }

        /// <summary>
        /// Add a comment to an attendance for an activity on a specific day
        /// </summary>
        /// <param name="date">The date of the activity</param>
        /// <param name="timeOfDay">The time of day of the activity</param>
        /// <param name="activityId">The id of the activity</param>
        /// <param name="userId">The id of the attending user</param>
        /// <param name="model">The comment</param>
        /// <returns>The attendance</returns>
        [HttpPost]
        [Route("comment/{date}/{timeOfDay}/{activityId}/{userId}")]
        public ActionResult<Attendance> AddComment(DateTime date, TimeOfDay timeOfDay, int activityId, int userId, CommentDTO model)
        {
            Attendance attendanceToEdit = _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId).Attendances.SingleOrDefault(a => a.UserId == userId);
            attendanceToEdit.Comment = model.Comment;
            _dayActivityRepository.SaveChanges();
            return attendanceToEdit;
        }

        /// <summary>
        /// Remove a comment from an attendance for an activity on a specific day
        /// </summary>
        /// <param name="date">The date of the activity</param>
        /// <param name="timeOfDay">The time of day of the activity</param>
        /// <param name="activityId">The id of the activity</param>
        /// <param name="userId">The id of the attending user</param>
        /// <returns>The attendance</returns>
        [HttpDelete]
        [Route("comment/{date}/{timeOfDay}/{activityId}/{userId}")]
        public ActionResult<Attendance> RemoveComment(DateTime date, TimeOfDay timeOfDay, int activityId, int userId)
        {
            Attendance attendanceToEdit = _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId).Attendances.SingleOrDefault(a => a.UserId == userId);
            attendanceToEdit.Comment = null;
            _dayActivityRepository.SaveChanges();
            return attendanceToEdit;
        }
    }
}
