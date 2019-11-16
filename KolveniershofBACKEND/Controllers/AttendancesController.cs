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
        [Route("{date}/{timeOfDay}/activity/{activityId}")]
        public ActionResult<IEnumerable<Attendance>> GetAll(DateTime date, int activityId, TimeOfDay timeOfDay)
        {
            IEnumerable<Attendance> attendances = _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId).Attendances.ToList();
            if (attendances == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(attendances);
            }
        }


        /// <summary>
        /// Get all client attendances for an activity on a specific day
        /// </summary>
        /// <param name="date">The date of the activity</param>
        /// <param name="activityId">The id of the activity</param>
        /// <param name="timeOfDay">The time of day of the activity</param>
        /// <returns>The client attendances</returns>
        [HttpGet]
        [Route("clients/{date}/{timeOfDay}/activity/{activityId}")]
        public ActionResult<IEnumerable<Attendance>> GetAllClients(DateTime date, int activityId, TimeOfDay timeOfDay)
        {
            IEnumerable<Attendance> attendances = _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId).Attendances.Where(a => a.User.UserType.Equals(UserType.CLIENT)).ToList();
            if (attendances == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(attendances);
            }

        }

        /// <summary>
        /// Get all personnel attendances for an activity on a specific day
        /// </summary>
        /// <param name="date">The date of the activity</param>
        /// <param name="activityId">The id of the activity</param>
        /// <param name="timeOfDay">The time of day of the activity</param>
        /// <returns>The personnel attendances</returns>
        [HttpGet]
        [Route("personnel/{date}/{timeOfDay}/activity/{activityId}")]
        public ActionResult<IEnumerable<Attendance>> GetAllPersonnel(DateTime date, int activityId, TimeOfDay timeOfDay)
        {

            IEnumerable<Attendance> attendances = _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId).Attendances.Where(a => !a.User.UserType.Equals(UserType.CLIENT)).ToList();
            if (attendances == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(attendances);
            }
        }


        ///<summary>
        /// Get all clients that are not yet attending a specific activity
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <param name="timeOfDay">The time of day of the activity</param>
        /// <param name="activityId">The id of the activity</param>
        /// <returns>The clients that are not yet attending the activity</returns>
        [HttpGet]
        [Route("{date}/{timeOfDay}/activity/{activityId}/possibleclients")]
        public ActionResult<IEnumerable<User>> GetPossibleClients(DateTime date, TimeOfDay timeOfDay, int activityId)
        {
            IEnumerable<User> users = _attendanceRepository.GetPossibleClients(date, timeOfDay, activityId);
            if (users == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(users);
            }
        }

        ///<summary>
        /// Get all personnel that are not yet attending a specific activity
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <param name="timeOfDay">The time of day of the activity</param>
        /// <param name="activityId">The id of the activity</param>
        /// <returns>The personnel that are not yet attending the activity</returns>
        [HttpGet]
        [Route("{date}/{timeOfDay}/activity/{activityId}/possiblepersonnel")]
        public ActionResult<IEnumerable<User>> GetPossiblePersonnel(DateTime date, TimeOfDay timeOfDay, int activityId)
        {
            IEnumerable<User> users = _attendanceRepository.GetPossiblePersonnel(date, timeOfDay, activityId);
            if (users == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(users);
            }
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
        [Route("{date}/{timeOfDay}/activity/{activityId}/user/{userId}")]
        public ActionResult<Attendance> Add(DateTime date, TimeOfDay timeOfDay, int activityId, int userId)
        {
            DayActivity dayActivity = _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId);
            if (dayActivity == null)
            {
                return NotFound();
            }
            else
            {
                User user = _userRepository.GetById(userId);
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        Attendance attendanceToAdd = new Attendance(dayActivity, user);
                        dayActivity.AddAttendance(attendanceToAdd);
                        _dayActivityRepository.SaveChanges();
                        return Ok(attendanceToAdd);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
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
        [Route("{date}/{timeOfDay}/activity/{activityId}/user/{userId}")]
        public ActionResult<Attendance> Remove(DateTime date, TimeOfDay timeOfDay, int activityId, int userId)
        {
            DayActivity dayActivity = _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId);
            if (dayActivity == null)
            {
                return NotFound();
            }
            else
            {
                Attendance attendanceToRemove = _attendanceRepository.GetForUser(date, timeOfDay, activityId, userId);
                if (attendanceToRemove == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        dayActivity.RemoveAttendance(attendanceToRemove);
                        _dayActivityRepository.SaveChanges();
                        return Ok(attendanceToRemove);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
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
        [Route("comment/{date}/{timeOfDay}/activity/{activityId}/user/{userId}")]
        public ActionResult<Attendance> AddComment(DateTime date, TimeOfDay timeOfDay, int activityId, int userId, CommentDTO model)
        {
            Attendance attendanceToEdit = _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId).Attendances.SingleOrDefault(a => a.UserId == userId);
            if (attendanceToEdit == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    attendanceToEdit.Comment = model.Comment;
                    _dayActivityRepository.SaveChanges();
                    return Ok(attendanceToEdit);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
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
        [Route("comment/{date}/{timeOfDay}/activity/{activityId}/user/{userId}")]
        public ActionResult<Attendance> RemoveComment(DateTime date, TimeOfDay timeOfDay, int activityId, int userId)
        {
            Attendance attendanceToEdit = _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId).Attendances.SingleOrDefault(a => a.UserId == userId);
            if (attendanceToEdit == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    attendanceToEdit.Comment = null;
                    _dayActivityRepository.SaveChanges();
                    return Ok(attendanceToEdit);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
