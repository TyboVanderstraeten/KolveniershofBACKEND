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

        public WeekendsController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("{date}/{userId}")]
        public ActionResult<WeekendDay> Get(DateTime date, int userId)
        {
            return _userRepository.GetById(userId).WeekendDays.SingleOrDefault(wd => wd.Date.Date == date.Date);
        }

        [HttpPost]
        public ActionResult<WeekendDay> Add(WeekendDayDTO model)
        {
            WeekendDay weekendDayToAdd = new WeekendDay(model.Date, model.Comment);
            User user = _userRepository.GetById(model.UserId);
            user.AddWeekendDay(weekendDayToAdd);
            _userRepository.SaveChanges();
            return weekendDayToAdd;
        }

        [HttpPut]
        [Route("{weekendDayId}/{userId}")]
        public ActionResult<WeekendDay> Edit(int weekendDayId, int userId, CommentDTO model)
        {
            WeekendDay weekendDayToEdit = _userRepository.GetById(userId).WeekendDays.SingleOrDefault(w => w.WeekendDayId == weekendDayId);
            weekendDayToEdit.Comment = model.Comment;
            _userRepository.SaveChanges();
            return weekendDayToEdit;
        }

        [HttpDelete]
        [Route("{weekendDayId}/{userId}")]
        public ActionResult<WeekendDay> Remove(int weekendDayId, int userId)
        {
            User user = _userRepository.GetById(userId);
            WeekendDay weekendDayToRemove = user.WeekendDays.SingleOrDefault(w => w.WeekendDayId == weekendDayId);
            user.RemoveWeekendDay(weekendDayToRemove);
            _userRepository.SaveChanges();
            return weekendDayToRemove;
        }
    }
}