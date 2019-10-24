using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.DTO;
using Microsoft.AspNetCore.Mvc;
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
        [Route("template")]
        public ActionResult<Day> AddTemplateDay(DayDTO model)
        {
            return null;
        }

        [HttpPut]
        [Route("template")]
        public ActionResult<Day> EditTemplateDay(DayDTO model)
        {
            return null;
        }

        [HttpDelete]
        [Route("template/{id}")]
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

        #endregion
    }
}