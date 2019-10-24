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

        public SchedulerController(IDayRepository dayRepository, ICustomDayRepository customDayRepository)
        {
            _dayRepository = dayRepository;
            _customDayRepository = customDayRepository;
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
            customDayToCreate.DayActivities = templateDayChosen.DayActivities;
            customDayToCreate.Helpers = templateDayChosen.Helpers;

            _customDayRepository.Add(customDayToCreate);
            _customDayRepository.SaveChanges();
            return customDayToCreate;
        }

        #endregion
    }
}