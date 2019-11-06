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
    public class TemplatesController : ControllerBase
    {

        private readonly IDayRepository _dayRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDayActivityRepository _dayActivityRepository;
        private readonly IHelperRepository _helperRepository;

        public TemplatesController(IDayRepository dayRepository, IActivityRepository activityRepository,
            IUserRepository userRepository, IDayActivityRepository dayActivityRepository,
            IHelperRepository helperRepository)
        {
            _dayRepository = dayRepository;
            _activityRepository = activityRepository;
            _userRepository = userRepository;
            _dayActivityRepository = dayActivityRepository;
            _helperRepository = helperRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Day>> GetAll()
        {
            return _dayRepository.GetAll().ToList();
        }

        [HttpGet]
        [Route("{templateName}")]
        public ActionResult<IEnumerable<Day>> GetAll(string templateName)
        {
            return _dayRepository.GetAllByTemplateName(templateName).ToList();
        }

        [HttpGet]
        [Route("{templateName}/{weekNr}")]
        public ActionResult<IEnumerable<Day>> GetAll(string templateName, int weekNr)
        {
            return _dayRepository.GetAllByWeek(templateName, weekNr).ToList();
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Day> GetById(int id)
        {
            return _dayRepository.GetById(id);
        }

        [HttpGet]
        [Route("{templateName}/{weekNr}/{dayNr}")]
        public ActionResult<Day> GetByWeekAndDay(string templateName, int weekNr, int dayNr)
        {
            return _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
        }

        [HttpPost]
        public ActionResult<Day> Add(DayDTO model)
        {
            if (_dayRepository.GetByWeekAndDay(model.TemplateName, model.WeekNr, model.DayNr) == null)
            {
                Day dayToAdd = new Day(model.TemplateName, model.WeekNr, model.DayNr);
                _dayRepository.Add(dayToAdd);
                _dayRepository.SaveChanges();
                return dayToAdd;
            }
            else
            {
                return BadRequest("A day with this weekNr and dayNr already exists for this template");
            }
        }

        [HttpDelete]
        [Route("{templateName}/{weekNr}/{dayNr}")]
        public ActionResult<Day> Remove(string templateName, int weekNr, int dayNr)
        {
            Day dayToRemove = _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
            _dayRepository.Remove(dayToRemove);
            _dayRepository.SaveChanges();
            return dayToRemove;
        }

        [HttpPost]
        [Route("activity/{templateName}/{weekNr}/{dayNr}")]
        public ActionResult<DayActivity> AddActivity(string templateName, int weekNr, int dayNr, DayActivityDTO model)
        {
            Day dayToEdit = _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
            Activity activity = _activityRepository.GetById(model.ActivityId);
            DayActivity dayActivityToAdd = new DayActivity(dayToEdit, activity, model.TimeOfDay);
            dayToEdit.AddDayActivity(dayActivityToAdd);
            _dayRepository.SaveChanges();
            return dayActivityToAdd;
        }

        [HttpDelete]
        [Route("activity/{templateName}/{weekNr}/{dayNr}/{activityId}/{timeOfDay}")]
        public ActionResult<DayActivity> RemoveActivity(string templateName, int weekNr, int dayNr, int activityId, TimeOfDay timeOfDay)
        {
            Day dayToEdit = _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
            DayActivity dayActivityToRemove = _dayActivityRepository.GetTemplateDayActivity(templateName, weekNr, dayNr, timeOfDay, activityId);
            dayToEdit.RemoveDayActivity(dayActivityToRemove);
            _dayRepository.SaveChanges();
            return dayActivityToRemove;
        }

        [HttpPost]
        [Route("helper/{templateName}/{weekNr}/{dayNr}")]
        public ActionResult<Helper> AddHelper(string templateName, int weekNr, int dayNr, HelperDTO model)
        {
            Day dayToEdit = _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
            User user = _userRepository.GetById(model.UserId);
            Helper helperToAdd = new Helper(dayToEdit, user);
            dayToEdit.AddHelper(helperToAdd);
            _dayRepository.SaveChanges();
            return helperToAdd;
        }


        [HttpDelete]
        [Route("helper/{templateName}/{weekNr}/{dayNr}/{userId}")]
        public ActionResult<Helper> RemoveHelper(string templateName, int weekNr, int dayNr, int userId)
        {
            Day dayToEdit = _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
            Helper helperToRemove = _helperRepository.GetTemplateDayHelper(templateName, weekNr, dayNr, userId)
            dayToEdit.RemoveHelper(helperToRemove);
            _dayRepository.SaveChanges();
            return helperToRemove;
        }
    }
}