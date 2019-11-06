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

        public AttendancesController(IUserRepository userRepository, IDayActivityRepository dayActivityRepository)
        {
            _userRepository = userRepository;
            _dayActivityRepository = dayActivityRepository;
        }

        [HttpGet]
        [Route("{date}/{timeOfDay}/{activityId}")]
        public ActionResult<IEnumerable<Attendance>> GetAll(DateTime date, int activityId, TimeOfDay timeOfDay)
        {
            return _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId).Attendances.ToList();
        }

        [HttpGet]
        [Route("clients/{date}/{timeOfDay}/{activityId}")]
        public ActionResult<IEnumerable<Attendance>> GetAllClients(DateTime date, int activityId, TimeOfDay timeOfDay)
        {
            return _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId).Attendances.Where(a => a.User.UserType.Equals(UserType.CLIENT)).ToList();
        }


        [HttpGet]
        [Route("personnel/{date}/{timeOfDay}/{activityId}")]
        public ActionResult<IEnumerable<Attendance>> GetAllPersonnel(DateTime date, int activityId, TimeOfDay timeOfDay)
        {
            return _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId).Attendances.Where(a => !(a.User.UserType.Equals(UserType.CLIENT))).ToList();
        }

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

        [HttpDelete]
        [Route("{date}/{timeOfDay}/{activityId}/{userId}")]
        public ActionResult<Attendance> Remove(DateTime date, TimeOfDay timeOfDay, int activityId, int userId)
        {
            DayActivity dayActivity = _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId);
            Attendance attendanceToRemove = dayActivity.Attendances.SingleOrDefault(a => a.UserId == userId);
            dayActivity.RemoveAttendance(attendanceToRemove);
            _dayActivityRepository.SaveChanges();
            return attendanceToRemove;
        }

        [HttpPost]
        [Route("comment/{date}/{timeOfDay}/{activityId}/{userId}")]
        public ActionResult<Attendance> AddComment(DateTime date, TimeOfDay timeOfDay, int activityId, int userId, CommentDTO model)
        {
            Attendance attendanceToEdit = _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId).Attendances.SingleOrDefault(a => a.UserId == userId);
            attendanceToEdit.Comment = model.Comment;
            _dayActivityRepository.SaveChanges();
            return attendanceToEdit;
        }

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
