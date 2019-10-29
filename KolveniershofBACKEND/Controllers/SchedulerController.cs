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
    public class SchedulerController : ControllerBase
    {
        private readonly IDayRepository _dayRepository;
        private readonly ICustomDayRepository _customDayRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IUserRepository _userRepository;

        public SchedulerController(IDayRepository dayRepository, ICustomDayRepository customDayRepository,
            IActivityRepository activityRepository, IUserRepository userRepository)
        {
            _dayRepository = dayRepository;
            _customDayRepository = customDayRepository;
            _activityRepository = activityRepository;
            _userRepository = userRepository;
        }

        #region Template methods

        [HttpGet]
        [Route("template/all")]
        public ActionResult<IEnumerable<Day>> GetAllTemplateDays()
        {
            return _dayRepository.GetAll().ToList();
        }

        [HttpGet]
        [Route("template/week/{weekNr}")]
        public ActionResult<IEnumerable<Day>> GetAllTemplateDaysByWeek(int weekNr)
        {
            return _dayRepository.GetAllByWeek(weekNr).ToList();
        }

        [HttpGet]
        [Route("template/day/{id}")]
        public ActionResult<Day> GetTemplateDayById(int id)
        {
            return _dayRepository.GetById(id);
        }

        [HttpGet]
        [Route("template/week/day/{weekNr}/{dayNr}")]
        public ActionResult<Day> GetTemplateDayByWeekAndDay(int weekNr, int dayNr)
        {
            return _dayRepository.GetByWeekAndDay(weekNr, dayNr);
        }

        [HttpPost]
        [Route("template/new")]
        public ActionResult<Day> AddTemplateDay(DayDTO model)
        {
            return null;
        }

        [HttpPost]
        [Route("template/activity/new/{weekNr}/{dayNr")]
        public ActionResult<DayActivity> AddActivityToTemplateDay(int weekNr, int dayNr, DayActivityDTO model)
        {
            Day dayToEdit = _dayRepository.GetByWeekAndDay(weekNr, dayNr);
            Activity activity = _activityRepository.GetById(model.ActivityId);
            DayActivity dayActivityToAdd = new DayActivity(dayToEdit, activity, model.TimeOfDay);
            dayToEdit.AddDayActivity(dayActivityToAdd);
            _dayRepository.SaveChanges();
            return dayActivityToAdd;
        }

        [HttpPost]
        [Route("template/helper/new/{weekNr}/{dayNr")]
        public ActionResult<Helper> AddHelperToTemplateDay(int weekNr, int dayNr, HelperDTO model)
        {
            Day dayToEdit = _dayRepository.GetByWeekAndDay(weekNr, dayNr);
            User user = _userRepository.GetById(model.UserId);
            Helper helperToAdd = new Helper(dayToEdit, user);
            dayToEdit.AddHelper(helperToAdd);
            _dayRepository.SaveChanges();
            return helperToAdd;
        }

        [HttpPut]
        [Route("template/edit")]
        public ActionResult<Day> EditTemplateDay(DayDTO model)
        {
            return null;
        }

        [HttpDelete]
        [Route("template/remove/{id}")]
        public ActionResult<Day> RemoveTemplateDay(int id)
        {
            return null;
        }

        #endregion

        #region CustomDay methods

        [HttpGet]
        [Route("custom/all")]
        public ActionResult<IEnumerable<Day>> GetAllCustomDays()
        {
            return _customDayRepository.GetAll().ToList();
        }

        [HttpGet]
        [Route("custom/range/{start}/{end}")]
        public ActionResult<IEnumerable<Day>> GetAllCustomDaysInRange(DateTime start, DateTime end)
        {
            return _customDayRepository.GetAllInRange(start, end).ToList();
        }

        [HttpGet]
        [Route("custom/day/{id}")]
        public ActionResult<Day> GetCustomDayById(int id)
        {
            return _customDayRepository.GetById(id);
        }

        [HttpGet]
        [Route("custom/day/date/{date}")]
        public ActionResult<Day> GetCustomDayByDate(DateTime date)
        {
            return _customDayRepository.GetByDate(date);
        }

        [HttpGet]
        [Route("custom/day/absent/{date}")]
        public ActionResult<IEnumerable<User>> GetAbsentUsersForDay(DateTime date)
        {
            return _customDayRepository.GetAbsentUsersForDay(date).ToList();
        }

        [HttpGet]
        [Route("custom/day/sick/{date}")]
        public ActionResult<IEnumerable<User>> GetSickUsersForDay(DateTime date)
        {
            return _customDayRepository.GetSickUsersForDay(date).ToList();
        }

        [HttpGet]
        [Route("custom/day/activity/attended/clients/{date}/{id}")]
        public ActionResult<IEnumerable<User>> GetAttendedClientsForActivity(DateTime date, int id)
        {
            return _customDayRepository.GetAttendedClientsForActivity(date, id).ToList();
        }

        [HttpGet]
        [Route("custom/day/activity/attended/personnel/{date}/{id}")]
        public ActionResult<IEnumerable<User>> GetAttendedPersonnelForActivity(DateTime date, int id)
        {
            return _customDayRepository.GetAttendedPersonnelForActivity(date, id).ToList();
        }

        [HttpGet]
        [Route("custom/day/notes/{date}")]
        public ActionResult<IEnumerable<Note>> GetNotesForDay(DateTime date)
        {
            return _customDayRepository.GetNotesForDay(date).ToList();
        }

        [HttpGet]
        [Route("custom/day/helpers/{date}")]
        public ActionResult<IEnumerable<Helper>> GetHelpersForDay(DateTime date)
        {
            return _customDayRepository.GetHelpersForDay(date).ToList();
        }

        [HttpPost]
        [Route("custom/day/new")]
        public ActionResult<CustomDay> AddCustomDay(CustomDayDTO model)
        {
            // Choose template day
            Day templateDayChosen = _dayRepository.GetByWeekAndDay(model.WeekNr, model.DayNr);

            // Create custom day
            CustomDay customDayToCreate = new CustomDay(templateDayChosen.WeekNr, templateDayChosen.DayNr, model.Date, model.Menu);

            // Inject template collections into customday collections
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

            /*
             * Now you can call other methods, for instance:
             * - Adding helpers
             * - Adding activities
             * - Adding notes
             * - ...
             */
        }

        [HttpPost]
        [Route("custom/day/add/activity/{date}")]
        public ActionResult<DayActivity> AddActivityToDay(DateTime date, DayActivityDTO model)
        {
            CustomDay customDayToEdit = _customDayRepository.GetByDate(date);
            Activity activity = _activityRepository.GetById(model.ActivityId);
            DayActivity dayActivityToAdd = new DayActivity(customDayToEdit, activity, model.TimeOfDay);
            customDayToEdit.AddDayActivity(dayActivityToAdd);
            _customDayRepository.SaveChanges();
            return dayActivityToAdd;
        }

        [HttpPost]
        [Route("custom/day/add/helper/{date}")]
        public ActionResult<Helper> AddHelperToDay(DateTime date, HelperDTO model)
        {
            CustomDay customDayToEdit = _customDayRepository.GetByDate(date);
            User user = _userRepository.GetById(model.UserId);
            Helper helperToAdd = new Helper(customDayToEdit, user);
            customDayToEdit.AddHelper(helperToAdd);
            _customDayRepository.SaveChanges();
            return helperToAdd;
        }

        [HttpPost]
        [Route("custom/day/add/note/{date}")]
        public ActionResult<Note> AddNoteToDay(DateTime date, NoteDTO model)
        {
            CustomDay customDayToEdit = _customDayRepository.GetByDate(date);
            Note noteToAdd = new Note(model.NoteType, model.Content);
            customDayToEdit.AddNote(noteToAdd);
            _customDayRepository.SaveChanges();
            return noteToAdd;
        }

        #endregion
    }
}