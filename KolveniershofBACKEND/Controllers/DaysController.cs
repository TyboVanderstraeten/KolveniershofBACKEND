using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace KolveniershofBACKEND.Controllers
{
    [ApiController]
    [Route("KolveniershofAPI/[controller]")]
    public class DaysController : ControllerBase
    {
        private readonly IDayRepository _dayRepository;
        private readonly ICustomDayRepository _customDayRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IDayActivityRepository _dayActivityRepository;
        private readonly IHelperRepository _helperRepository;
        private readonly IUserRepository _userRepository;
        private readonly INoteRepository _noteRepository;

        public DaysController(IDayRepository dayRepository, ICustomDayRepository customDayRepository,
                              IActivityRepository activityRepository, IUserRepository userRepository,
                              IDayActivityRepository dayActivityRepository, IHelperRepository helperRepository,
                              INoteRepository noteRepository)
        {
            _dayRepository = dayRepository;
            _customDayRepository = customDayRepository;
            _activityRepository = activityRepository;
            _userRepository = userRepository;
            _dayActivityRepository = dayActivityRepository;
            _helperRepository = helperRepository;
            _noteRepository = noteRepository;
        }

        /// <summary>
        /// Get all custom days
        /// </summary>
        /// <returns>All custom days</returns>
        [HttpGet]
        public ActionResult<IEnumerable<CustomDay>> GetAll()
        {
            IEnumerable<CustomDay> customDays = _customDayRepository.GetAll().ToList();
            if (customDays == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(customDays);
            }
        }

        /// <summary>
        /// Get all custom days in a range
        /// </summary>
        /// <param name="startDate">The first day (included) </param>
        /// <param name="endDate">The last day (included)</param>
        /// <returns>All custom days in the range</returns>
        [HttpGet]
        [Route("{startDate}/{endDate}")]
        public ActionResult<IEnumerable<CustomDay>> GetAll(DateTime startDate, DateTime endDate)
        {
            IEnumerable<CustomDay> customDays = _customDayRepository.GetAllInRange(startDate, endDate).ToList();
            if (customDays == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(customDays);
            }
        }

        ///<summary>
        /// Get all custom days in a range with those day activities attended by the user
        /// </summary>
        /// <param name="startDate">The first day (included) </param>
        /// <param name="endDate">The last day (included)</param>
        /// <param name="userId">The id of the user</param>
        /// <returns>All custom days in the range with those day activities attended by the user</returns>
        [HttpGet]
        [Route("{startDate}/{endDate}/user/{userId}")]
        public ActionResult<IEnumerable<CustomDay>> GetAllForUser(DateTime startDate, DateTime endDate, int userId)
        {
            IEnumerable<CustomDay> customDays = _customDayRepository.GetAllInRangeForUser(startDate, endDate, userId).ToList();
            if (customDays == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(customDays);
            }
        }

        /// <summary>
        /// Get a specific custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <returns>The custom day</returns>
        [HttpGet]
        [Route("date/{date}")]
        public ActionResult<CustomDay> GetByDate(DateTime date)
        {
            CustomDay customDay = _customDayRepository.GetByDate(date);
            if (customDay == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(customDay);
            }
        }

        /// <summary>
        /// Get a custom day for a user with all his attended activities
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="date">The date of the custom day</param>
        /// <returns>The custom day of the user with his attended activities</returns>
        [HttpGet]
        [Route("{date}/user/{userId}")]
        public ActionResult<CustomDay> GetForUser(int userId, DateTime date)
        {
            CustomDay customDay = _customDayRepository.GetByDate(date);
            if (customDay == null)
            {
                return NotFound();
            }
            else
            {
                IEnumerable<DayActivity> dayActivitiesAttended = customDay.DayActivities.Where(da => da.Attendances.Any(a => a.UserId == userId)).ToList();
                if (dayActivitiesAttended == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        CustomDay customDayUser = new CustomDay(
                            customDay.TemplateName,
                            customDay.WeekNr,
                            customDay.DayNr,
                            customDay.Date,
                            customDay.PreDish,
                            customDay.MainDish,
                            customDay.Dessert
                            );
                        customDayUser.DayId = customDay.DayId;
                        customDayUser.DayActivities = dayActivitiesAttended.ToList();
                        customDayUser.Helpers = customDay.Helpers;
                        customDayUser.Notes = customDay.Notes;
                        return Ok(customDayUser);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Get all absent users for a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <returns>The absent users of the custom day</returns>
        [HttpGet]
        [Route("{date}/absent")]
        public ActionResult<IEnumerable<User>> GetAbsent(DateTime date)
        {
            IEnumerable<User> users = _customDayRepository.GetAbsentUsersForDay(date).ToList();
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
        /// Get all sick users for a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <returns>The sick users of the custom day</returns>
        [HttpGet]
        [Route("{date}/sick")]
        public ActionResult<IEnumerable<User>> GetSick(DateTime date)
        {
            IEnumerable<User> users = _customDayRepository.GetSickUsersForDay(date).ToList();
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
        /// Get all notes for a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <returns>The notes of the custom day</returns>
        [HttpGet]
        [Route("{date}/notes")]
        public ActionResult<IEnumerable<Note>> GetNotes(DateTime date)
        {
            IEnumerable<Note> notes = _customDayRepository.GetNotesForDay(date).ToList();
            if (notes == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(notes);
            }
        }

        /// <summary>
        /// Get all helpers for a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <returns>The helpers of the custom day</returns>
        [HttpGet]
        [Route("{date}/helpers")]
        public ActionResult<IEnumerable<Helper>> GetHelpers(DateTime date)
        {
            IEnumerable<Helper> helpers = _customDayRepository.GetHelpersForDay(date).ToList();
            if (helpers == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(helpers);
            }
        }

        /// <summary>
        /// Create a new custom day based upon a template day
        /// </summary>
        /// <param name="model">The custom day</param>
        /// <returns>The custom day</returns>
        [HttpPost]
        public ActionResult<CustomDay> Add(CustomDayDTO model)
        {
            Day templateDayChosen = _dayRepository.GetByWeekAndDay(model.TemplateName, model.WeekNr, model.DayNr);
            if (templateDayChosen == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    CustomDay customDayToCreate = new CustomDay(templateDayChosen.TemplateName, templateDayChosen.WeekNr, templateDayChosen.DayNr, model.Date, model.PreDish, model.MainDish, model.Dessert);
                    foreach (DayActivity dayActivity in templateDayChosen.DayActivities)
                    {
                        DayActivity dayActivityToAdd = new DayActivity(customDayToCreate, dayActivity.Activity, dayActivity.TimeOfDay);
                        customDayToCreate.AddDayActivity(dayActivityToAdd);
                    }
                    foreach (Helper helper in templateDayChosen.Helpers)
                    {
                        Helper helperToAdd = new Helper(customDayToCreate, helper.User);
                        customDayToCreate.AddHelper(helperToAdd);
                    }
                    _customDayRepository.Add(customDayToCreate);
                    _customDayRepository.SaveChanges();
                    return Ok(customDayToCreate);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        /// <summary>
        /// Edit a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <param name="model">The custom day</param>
        /// <returns>The custom day</returns>
        [HttpPut]
        [Route("{date}")]
        public ActionResult<CustomDay> Edit(DateTime date, CustomDayDTO model)
        {
            CustomDay dayToEdit = _customDayRepository.GetByDate(date);
            if (dayToEdit == null)
            {
                return NotFound();
            }
            else
            {
                if (!dayToEdit.WeekNr.Equals(model.TemplateName) || (dayToEdit.WeekNr != model.WeekNr) || (dayToEdit.DayNr != model.DayNr))
                {
                    try
                    {
                        Day templateDayChosen = _dayRepository.GetByWeekAndDay(model.TemplateName, model.WeekNr, model.DayNr);
                        dayToEdit.TemplateName = templateDayChosen.TemplateName;
                        dayToEdit.WeekNr = templateDayChosen.WeekNr;
                        dayToEdit.DayNr = templateDayChosen.DayNr;
                        dayToEdit.DayActivities = new List<DayActivity>();
                        dayToEdit.Helpers = new List<Helper>();
                        foreach (DayActivity dayActivity in templateDayChosen.DayActivities)
                        {
                            DayActivity dayActivityToAdd = new DayActivity(dayToEdit, dayActivity.Activity, dayActivity.TimeOfDay);
                            dayToEdit.AddDayActivity(dayActivityToAdd);
                        }

                        foreach (Helper helper in templateDayChosen.Helpers)
                        {
                            Helper helperToAdd = new Helper(dayToEdit, helper.User);
                            dayToEdit.AddHelper(helperToAdd);
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
                try
                {
                    dayToEdit.Date = model.Date;
                    dayToEdit.PreDish = model.PreDish;
                    dayToEdit.MainDish = model.MainDish;
                    dayToEdit.Dessert = model.Dessert;
                    _customDayRepository.SaveChanges();
                    return Ok(dayToEdit);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                };

            }
        }

        /// <summary>
        /// Remove a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <returns>The custom day</returns>
        [HttpDelete]
        [Route("{date}")]
        public ActionResult<CustomDay> Remove(DateTime date)
        {
            CustomDay dayToRemove = _customDayRepository.GetByDate(date);
            if (dayToRemove == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    _customDayRepository.Remove(dayToRemove);
                    _customDayRepository.SaveChanges();
                    return Ok(dayToRemove);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        /// <summary>
        /// Add an activity to a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <param name="model">The activity</param>
        /// <returns>The activity</returns>
        [HttpPost]
        [Route("{date}/activity")]
        public ActionResult<DayActivity> AddActivity(DateTime date, DayActivityDTO model)
        {
            CustomDay customDayToEdit = _customDayRepository.GetByDate(date);
            if (customDayToEdit == null)
            {
                return NotFound();
            }
            else
            {
                Activity activity = _activityRepository.GetById(model.ActivityId);
                if (activity == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        DayActivity dayActivityToAdd = new DayActivity(customDayToEdit, activity, model.TimeOfDay);
                        customDayToEdit.AddDayActivity(dayActivityToAdd);
                        _customDayRepository.SaveChanges();
                        return Ok(dayActivityToAdd);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Remove an activity from a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <param name="activityId">The id of the activity</param>
        /// <param name="timeOfDay">The time of day</param>
        /// <returns>The activity</returns>
        [HttpDelete]
        [Route("{date}/{timeOfDay}/activity/{activityId}")]
        public ActionResult<DayActivity> RemoveActivity(DateTime date, int activityId, TimeOfDay timeOfDay)
        {
            CustomDay dayToEdit = _customDayRepository.GetByDate(date);
            if (dayToEdit == null)
            {
                return NotFound();
            }
            else
            {
                DayActivity dayActivityToRemove = _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId);
                if (dayActivityToRemove == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        dayToEdit.RemoveDayActivity(dayActivityToRemove);
                        _customDayRepository.SaveChanges();
                        return Ok(dayActivityToRemove);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Add a helper to a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <param name="model">The helper</param>
        /// <returns>The helper</returns>
        [HttpPost]
        [Route("{date}/helper")]
        public ActionResult<Helper> AddHelper(DateTime date, HelperDTO model)
        {
            CustomDay customDayToEdit = _customDayRepository.GetByDate(date);
            if (customDayToEdit == null)
            {
                return NotFound();
            }
            else
            {
                User user = _userRepository.GetById(model.UserId);
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        Helper helperToAdd = new Helper(customDayToEdit, user);
                        customDayToEdit.AddHelper(helperToAdd);
                        _customDayRepository.SaveChanges();
                        return Ok(helperToAdd);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Remove a helper from a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <param name="userId">The id of the helper</param>
        /// <returns>The helper</returns>
        [HttpDelete]
        [Route("{date}/helper/{userId}")]
        public ActionResult<Helper> RemoveHelper(DateTime date, int userId)
        {
            CustomDay dayToEdit = _customDayRepository.GetByDate(date);
            if (dayToEdit == null)
            {
                return NotFound();
            }
            else
            {
                Helper helperToRemove = _helperRepository.GetCustomDayHelper(date, userId);
                if (helperToRemove == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        dayToEdit.RemoveHelper(helperToRemove);
                        _customDayRepository.SaveChanges();
                        return Ok(helperToRemove);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Add a note to a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <param name="model">The note</param>
        /// <returns>The note</returns>
        [HttpPost]
        [Route("{date}/note")]
        public ActionResult<Note> AddNote(DateTime date, NoteDTO model)
        {
            CustomDay customDayToEdit = _customDayRepository.GetByDate(date);
            if (customDayToEdit == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    Note noteToAdd = new Note(model.NoteType, model.Content);
                    customDayToEdit.AddNote(noteToAdd);
                    _customDayRepository.SaveChanges();
                    return Ok(noteToAdd);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        /// <summary>
        /// Remove a note from a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <param name="noteId">The id of the note</param>
        /// <returns>The note</returns>
        [HttpDelete]
        [Route("{date}/note/{noteId}")]
        public ActionResult<Note> RemoveNote(DateTime date, int noteId)
        {
            CustomDay dayToEdit = _customDayRepository.GetByDate(date);
            if (dayToEdit == null)
            {
                return NotFound();
            }
            else
            {
                Note noteToRemove = _noteRepository.GetCustomDayNote(date, noteId);
                if (noteToRemove == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        dayToEdit.RemoveNote(noteToRemove);
                        _customDayRepository.SaveChanges();
                        return Ok(noteToRemove);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }
    }
}