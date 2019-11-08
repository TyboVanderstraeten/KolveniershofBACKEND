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
            return _customDayRepository.GetAll().ToList();
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
            return _customDayRepository.GetAllInRange(startDate, endDate).ToList();
        }

        /// <summary>
        /// Get a specific custom day
        /// </summary>
        /// <param name="dayId">The id of the custom day</param>
        /// <returns>The custom day</returns>
        [HttpGet]
        [Route("{dayId}")]
        public ActionResult<CustomDay> GetById(int dayId)
        {
            return _customDayRepository.GetById(dayId);
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
            return _customDayRepository.GetByDate(date);
        }

        /// <summary>
        /// Get a custom day for a user with all his attended activities
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="date">The date of the custom day</param>
        /// <returns>The custom day of the user with his attended activities</returns>
        [HttpGet]
        [Route("{date}/{userId}")]
        public ActionResult<CustomDay> GetForUser(int userId, DateTime date)
        {
            CustomDay customDay = _customDayRepository.GetByDate(date);
            IEnumerable<DayActivity> dayActivitiesAttended = customDay.DayActivities.Where(da => da.Attendances.Any(a => a.UserId == userId)).ToList();
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
            return customDayUser;
        }

        /// <summary>
        /// Get all absent users for a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <returns>The absent users of the custom day</returns>
        [HttpGet]
        [Route("absent/{date}")]
        public ActionResult<IEnumerable<User>> GetAbsent(DateTime date)
        {
            return _customDayRepository.GetAbsentUsersForDay(date).ToList();
        }

        /// <summary>
        /// Get all sick users for a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <returns>The sick users of the custom day</returns>
        [HttpGet]
        [Route("sick/{date}")]
        public ActionResult<IEnumerable<User>> GetSick(DateTime date)
        {
            return _customDayRepository.GetSickUsersForDay(date).ToList();
        }

        /// <summary>
        /// Get all notes for a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <returns>The notes of the custom day</returns>
        [HttpGet]
        [Route("notes/{date}")]
        public ActionResult<IEnumerable<Note>> GetNotes(DateTime date)
        {
            return _customDayRepository.GetNotesForDay(date).ToList();
        }

        /// <summary>
        /// Get all helpers for a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <returns>The helpers of the custom day</returns>
        [HttpGet]
        [Route("helpers/{date}")]
        public ActionResult<IEnumerable<Helper>> GetHelpers(DateTime date)
        {
            return _customDayRepository.GetHelpersForDay(date).ToList();
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
            return customDayToCreate;
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
            if (!(dayToEdit.WeekNr.Equals(model.TemplateName)) || (dayToEdit.WeekNr != model.WeekNr) || (dayToEdit.DayNr != model.DayNr))
            {
                Day templateDayChosen = _dayRepository.GetByWeekAndDay(model.TemplateName, model.WeekNr, model.DayNr);
                dayToEdit.TemplateName = templateDayChosen.TemplateName;
                dayToEdit.WeekNr = templateDayChosen.WeekNr;
                dayToEdit.DayNr = templateDayChosen.WeekNr;
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
            dayToEdit.Date = model.Date;
            dayToEdit.PreDish = model.PreDish;
            dayToEdit.MainDish = model.MainDish;
            dayToEdit.Dessert = model.Dessert;
            _customDayRepository.SaveChanges();
            return dayToEdit;
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
            _customDayRepository.Remove(dayToRemove);
            _customDayRepository.SaveChanges();
            return dayToRemove;
        }

        /// <summary>
        /// Add an activity to a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <param name="model">The activity</param>
        /// <returns>The activity</returns>
        [HttpPost]
        [Route("activity/{date}")]
        public ActionResult<DayActivity> AddActivity(DateTime date, DayActivityDTO model)
        {
            CustomDay customDayToEdit = _customDayRepository.GetByDate(date);
            Activity activity = _activityRepository.GetById(model.ActivityId);
            DayActivity dayActivityToAdd = new DayActivity(customDayToEdit, activity, model.TimeOfDay);
            customDayToEdit.AddDayActivity(dayActivityToAdd);
            _customDayRepository.SaveChanges();
            return dayActivityToAdd;
        }

        /// <summary>
        /// Remove an activity from a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <param name="activityId">The id of the activity</param>
        /// <param name="timeOfDay">The time of day</param>
        /// <returns>The activity</returns>
        [HttpDelete]
        [Route("activity/{date}/{timeOfDay}/{activityId}")]
        public ActionResult<DayActivity> RemoveActivity(DateTime date, int activityId, TimeOfDay timeOfDay)
        {
            CustomDay dayToEdit = _customDayRepository.GetByDate(date);
            DayActivity dayActivityToRemove = _dayActivityRepository.GetCustomDayActivity(date, timeOfDay, activityId);
            dayToEdit.RemoveDayActivity(dayActivityToRemove);
            _customDayRepository.SaveChanges();
            return dayActivityToRemove;
        }

        /// <summary>
        /// Add a helper to a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <param name="model">The helper</param>
        /// <returns>The helper</returns>
        [HttpPost]
        [Route("helper/{date}")]
        public ActionResult<Helper> AddHelper(DateTime date, HelperDTO model)
        {
            CustomDay customDayToEdit = _customDayRepository.GetByDate(date);
            User user = _userRepository.GetById(model.UserId);
            Helper helperToAdd = new Helper(customDayToEdit, user);
            customDayToEdit.AddHelper(helperToAdd);
            _customDayRepository.SaveChanges();
            return helperToAdd;
        }

        /// <summary>
        /// Remove a helper from a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <param name="userId">The id of the helper</param>
        /// <returns>The helper</returns>
        [HttpDelete]
        [Route("helper/{date}/{userId}")]
        public ActionResult<Helper> RemoveHelper(DateTime date, int userId)
        {
            CustomDay dayToEdit = _customDayRepository.GetByDate(date);
            Helper helperToRemove = _helperRepository.GetCustomDayHelper(date, userId);
            dayToEdit.RemoveHelper(helperToRemove);
            _customDayRepository.SaveChanges();
            return helperToRemove;
        }

        /// <summary>
        /// Add a note to a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <param name="model">The note</param>
        /// <returns>The note</returns>
        [HttpPost]
        [Route("note/{date}")]
        public ActionResult<Note> AddNote(DateTime date, NoteDTO model)
        {
            CustomDay customDayToEdit = _customDayRepository.GetByDate(date);
            Note noteToAdd = new Note(model.NoteType, model.Content);
            customDayToEdit.AddNote(noteToAdd);
            _customDayRepository.SaveChanges();
            return noteToAdd;
        }

        /// <summary>
        /// Remove a note from a custom day
        /// </summary>
        /// <param name="date">The date of the custom day</param>
        /// <param name="noteId">The id of the note</param>
        /// <returns>The note</returns>
        [HttpDelete]
        [Route("note/{date}/{noteId}")]
        public ActionResult<Note> RemoveNote(DateTime date, int noteId)
        {
            CustomDay dayToEdit = _customDayRepository.GetByDate(date);
            Note noteToRemove = _noteRepository.GetCustomDayNote(date, noteId);
            dayToEdit.RemoveNote(noteToRemove);
            _customDayRepository.SaveChanges();
            return noteToRemove;
        }
    }
}