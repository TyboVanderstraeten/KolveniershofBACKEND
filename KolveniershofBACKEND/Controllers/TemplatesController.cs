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

        public TemplatesController(IDayRepository dayRepository, IActivityRepository activityRepository,
            IUserRepository userRepository)
        {
            _dayRepository = dayRepository;
            _activityRepository = activityRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("template/all")]
        public ActionResult<IEnumerable<Day>> GetAllTemplateDays()
        {
            return _dayRepository.GetAll().ToList();
        }

        [HttpGet]
        [Route("template/all/name/{templateName}")]
        public ActionResult<IEnumerable<Day>> GetAllTemplateDaysByName(string templateName)
        {
            return _dayRepository.GetAllByTemplateName(templateName).ToList();
        }

        [HttpGet]
        [Route("template/week/{templateName}/{weekNr}")]
        public ActionResult<IEnumerable<Day>> GetAllTemplateDaysByWeek(string templateName, int weekNr)
        {
            return _dayRepository.GetAllByWeek(templateName, weekNr).ToList();
        }

        [HttpGet]
        [Route("template/day/{id}")]
        public ActionResult<Day> GetTemplateDayById(int id)
        {
            return _dayRepository.GetById(id);
        }

        [HttpGet]
        [Route("template/week/day/{templateName}/{weekNr}/{dayNr}")]
        public ActionResult<Day> GetTemplateDayByWeekAndDay(string templateName, int weekNr, int dayNr)
        {
            return _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
        }

        [HttpPost]
        [Route("template/new")]
        public ActionResult<Day> AddTemplateDay(DayDTO model)
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

        [HttpPost]
        [Route("template/activity/new/{templateName}/{weekNr}/{dayNr}")]
        public ActionResult<DayActivity> AddActivityToTemplateDay(string templateName, int weekNr, int dayNr, DayActivityDTO model)
        {
            Day dayToEdit = _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
            Activity activity = _activityRepository.GetById(model.ActivityId);
            DayActivity dayActivityToAdd = new DayActivity(dayToEdit, activity, model.TimeOfDay);
            dayToEdit.AddDayActivity(dayActivityToAdd);
            _dayRepository.SaveChanges();
            return dayActivityToAdd;
        }

        [HttpPost]
        [Route("template/helper/new/{templateName}/{weekNr}/{dayNr}")]
        public ActionResult<Helper> AddHelperToTemplateDay(string templateName, int weekNr, int dayNr, HelperDTO model)
        {
            Day dayToEdit = _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
            User user = _userRepository.GetById(model.UserId);
            Helper helperToAdd = new Helper(dayToEdit, user);
            dayToEdit.AddHelper(helperToAdd);
            _dayRepository.SaveChanges();
            return helperToAdd;
        }

        [HttpDelete]
        [Route("template/activity/delete/{templateName}/{weekNr}/{dayNr}/{id}/{timeOfDay}")]
        public ActionResult<DayActivity> RemoveActivityFromTemplateDay(string templateName, int weekNr, int dayNr, int id, TimeOfDay timeOfDay)
        {
            Day dayToEdit = _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
            DayActivity dayActivityToRemove =
                    dayToEdit.DayActivities.SingleOrDefault(da => da.DayId == dayToEdit.DayId && da.ActivityId == id && da.TimeOfDay.Equals(timeOfDay));
            dayToEdit.RemoveDayActivity(dayActivityToRemove);
            _dayRepository.SaveChanges();
            return dayActivityToRemove;
        }

        [HttpDelete]
        [Route("template/helper/delete/{templateName}/{weekNr}/{dayNr}/{id}")]
        public ActionResult<Helper> RemoveHelperFromTemplateDay(string templateName, int weekNr, int dayNr, int id)
        {
            Day dayToEdit = _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
            Helper helperToRemove = dayToEdit.Helpers.SingleOrDefault(h => h.DayId == dayToEdit.DayId && h.UserId == id);
            dayToEdit.RemoveHelper(helperToRemove);
            _dayRepository.SaveChanges();
            return helperToRemove;
        }

        [HttpDelete]
        [Route("template/remove/{templateName}/{weekNr}/{dayNr}")]
        public ActionResult<Day> RemoveTemplateDay(string templateName, int weekNr, int dayNr)
        {
            Day dayToRemove = _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
            _dayRepository.Remove(dayToRemove);
            _dayRepository.SaveChanges();
            return dayToRemove;
        }
    }
}