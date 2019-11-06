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

        [HttpGet]
        public ActionResult<IEnumerable<CustomDay>> GetAll()
        {
            return _customDayRepository.GetAll().ToList();
        }

        [HttpGet]
        [Route("{startDate}/{endDate}")]
        public ActionResult<IEnumerable<CustomDay>> GetAll(DateTime startDate, DateTime endDate)
        {
            return _customDayRepository.GetAllInRange(startDate, endDate).ToList();
        }

        [HttpGet]
        [Route("{dayId}")]
        public ActionResult<CustomDay> GetById(int dayId)
        {
            return _customDayRepository.GetById(dayId);
        }

        [HttpGet]
        [Route("{date}")]
        public ActionResult<CustomDay> GetByDate(DateTime date)
        {
            return _customDayRepository.GetByDate(date);
        }


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

        [HttpGet]
        [Route("absent/{date}")]
        public ActionResult<IEnumerable<User>> GetAbsent(DateTime date)
        {
            return _customDayRepository.GetAbsentUsersForDay(date).ToList();
        }

        [HttpGet]
        [Route("sick/{date}")]
        public ActionResult<IEnumerable<User>> GetSick(DateTime date)
        {
            return _customDayRepository.GetSickUsersForDay(date).ToList();
        }

        [HttpGet]
        [Route("notes/{date}")]
        public ActionResult<IEnumerable<Note>> GetNotes(DateTime date)
        {
            return _customDayRepository.GetNotesForDay(date).ToList();
        }

        [HttpGet]
        [Route("helpers/{date}")]
        public ActionResult<IEnumerable<Helper>> GetHelpers(DateTime date)
        {
            return _customDayRepository.GetHelpersForDay(date).ToList();
        }

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

        [HttpDelete]
        [Route("{date}")]
        public ActionResult<CustomDay> Remove(DateTime date)
        {
            CustomDay dayToRemove = _customDayRepository.GetByDate(date);
            _customDayRepository.Remove(dayToRemove);
            _customDayRepository.SaveChanges();
            return dayToRemove;
        }

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