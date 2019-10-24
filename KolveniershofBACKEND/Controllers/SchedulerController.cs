﻿using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
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
        [Route("template")]
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
 

        #endregion
    }
}