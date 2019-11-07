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
    public class WeekendsController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IWeekendDayRepository _weekendDayRepository;

        public WeekendsController(IUserRepository userRepository, IWeekendDayRepository weekendDayRepository)
        {
            _userRepository = userRepository;
            _weekendDayRepository = weekendDayRepository;
        }

        /// <summary>
        /// Get a specific weekendday
        /// </summary>
        /// <param name="date">The date of the weekendday</param>
        /// <param name="userId">The id of the user</param>
        /// <returns>The weekendday</returns>
        [HttpGet]
        [Route("{date}/{userId}")]
        public ActionResult<WeekendDay> Get(DateTime date, int userId)
        {
            return _weekendDayRepository.GetByDate(date, userId);
        }

        /// <summary>
        /// Create a new weekendday
        /// </summary>
        /// <param name="model">The weekendday</param>
        /// <returns>The weekendday</returns>
        [HttpPost]
        public ActionResult<WeekendDay> Add(WeekendDayDTO model)
        {
            WeekendDay weekendDayToAdd = new WeekendDay(model.Date, model.Comment);
            User user = _userRepository.GetById(model.UserId);
            user.AddWeekendDay(weekendDayToAdd);
            _userRepository.SaveChanges();
            return weekendDayToAdd;
        }

        /// <summary>
        /// Edit a weekendday
        /// </summary>
        /// <param name="model">The weekendday</param>
        /// <param name="userId">The id of the user</param>
        /// <returns>The weekendday</returns>
        [HttpPut]
        [Route("{date}/{userId}")]
        public ActionResult<WeekendDay> Edit(DateTime date, int userId, CommentDTO model)
        {
            WeekendDay weekendDayToEdit = _weekendDayRepository.GetByDate(date, userId);
            weekendDayToEdit.Comment = model.Comment;
            _userRepository.SaveChanges();
            return weekendDayToEdit;
        }

        /// <summary>
        /// Remove a weekendday
        /// </summary>
        /// <param name="date">The date of the weekendday</param>
        /// <param name="userId">The id of the user</param>
        /// <returns>The weekendday</returns>
        [HttpDelete]
        [Route("{date}/{userId}")]
        public ActionResult<WeekendDay> Remove(DateTime date, int userId)
        {
            WeekendDay weekendDayToRemove = _weekendDayRepository.GetByDate(date, userId);
            User user = _userRepository.GetById(userId);
            user.RemoveWeekendDay(weekendDayToRemove);
            _userRepository.SaveChanges();
            return weekendDayToRemove;
        }
    }
}