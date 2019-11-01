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
        [Route("weekendday/{date}/{userId}")]
        public ActionResult<WeekendDay> GetWeekendDayFromUser(DateTime date, int userId)
        {
            return _userRepository.GetById(userId).WeekendDays.SingleOrDefault(wd => wd.Date.Date == date.Date);
        }

        [HttpPost]
        [Route("weekendday/new/")]
        public ActionResult<WeekendDay> AddWeekendDayToUser(WeekendDayDTO model)
        {
            WeekendDay weekendDayToAdd = new WeekendDay(model.Date, model.Comment);
            User user = _userRepository.GetById(model.UserId);
            user.AddWeekendDay(weekendDayToAdd);
            _userRepository.SaveChanges();
            return weekendDayToAdd;
        }

        [HttpPut]
        [Route("weekendday/edit/{weekendDayId}/{userId}")]
        public ActionResult<WeekendDay> EditWeekendDayFromUser(int weekendDayId, int userId, CommentDTO model)
        {
            WeekendDay weekendDayToEdit = _userRepository.GetById(userId).WeekendDays.SingleOrDefault(w => w.WeekendDayId == weekendDayId);
            weekendDayToEdit.Comment = model.Comment;
            _userRepository.SaveChanges();
            return weekendDayToEdit;
        }

        [HttpDelete]
        [Route("weekendday/delete/{weekendDayId}/{userId}")]
        public ActionResult<WeekendDay> RemoveWeekendDayFromUser(int weekendDayId, int userId)
        {
            User user = _userRepository.GetById(userId);
            WeekendDay weekendDayToRemove = user.WeekendDays.SingleOrDefault(w => w.WeekendDayId == weekendDayId);
            user.RemoveWeekendDay(weekendDayToRemove);
            _userRepository.SaveChanges();
            return weekendDayToRemove;
        }
    }
}