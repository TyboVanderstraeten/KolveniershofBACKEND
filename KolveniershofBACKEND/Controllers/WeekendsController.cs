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
        [Route("{date}/user/{userId}")]
        public ActionResult<WeekendDay> Get(DateTime date, int userId)
        {
            WeekendDay weekendDay = _weekendDayRepository.GetByDate(date, userId);
            if (weekendDay == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(weekendDay);
            }
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
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    user.AddWeekendDay(weekendDayToAdd);
                    _userRepository.SaveChanges();
                    return Ok(weekendDayToAdd);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        /// <summary>
        /// Edit a weekendday
        /// </summary>
        /// <param name="model">The weekendday</param>
        /// <param name="userId">The id of the user</param>
        /// <returns>The weekendday</returns>
        [HttpPut]
        [Route("{date}/user/{userId}")]
        public ActionResult<WeekendDay> Edit(DateTime date, int userId, CommentDTO model)
        {
            WeekendDay weekendDayToEdit = _weekendDayRepository.GetByDate(date, userId);
            if (weekendDayToEdit == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    weekendDayToEdit.Comment = model.Comment;
                    _userRepository.SaveChanges();
                    return Ok(weekendDayToEdit);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        /// <summary>
        /// Remove a weekendday
        /// </summary>
        /// <param name="date">The date of the weekendday</param>
        /// <param name="userId">The id of the user</param>
        /// <returns>The weekendday</returns>
        [HttpDelete]
        [Route("{date}/user/{userId}")]
        public ActionResult<WeekendDay> Remove(DateTime date, int userId)
        {
            WeekendDay weekendDayToRemove = _weekendDayRepository.GetByDate(date, userId);
            if (weekendDayToRemove == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    User user = _userRepository.GetById(userId);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        user.RemoveWeekendDay(weekendDayToRemove);
                        _userRepository.SaveChanges();
                        return Ok(weekendDayToRemove);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}