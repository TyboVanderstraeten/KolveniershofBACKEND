using KolveniershofBACKEND.Controllers;
using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.DTO;
using KolveniershofBACKEND.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace KolveniershofBACKEND.Tests.Controllers
{
    public class SchedulerControllerTest
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<ICustomDayRepository> _customDayRepository;
        private Mock<IActivityRepository> _activityRepository;
        private Mock<IDayRepository> _dayRepository;

        private DummyDBContext _dummyDBContext;
        private SchedulerController _controller;
 
        public SchedulerControllerTest()
        {
            _userRepository = new Mock<IUserRepository>();
            _customDayRepository = new Mock<ICustomDayRepository>();
            _activityRepository = new Mock<IActivityRepository>();
            _dayRepository = new Mock<IDayRepository>();
            _dummyDBContext = new DummyDBContext();
            _controller = new SchedulerController(_dayRepository.Object, _customDayRepository.Object, _activityRepository.Object, _userRepository.Object);
        }

        #region TemplateDay
        #region Get
        [Fact]
        public void GetAllTemplateDays_Succeeds()
        {

            _dayRepository.Setup(d => d.GetAll()).Returns(_dummyDBContext.Days);
            ActionResult<IEnumerable<Day>> actionResult = _controller.GetAllTemplateDays();
            IList<Day> days = actionResult.Value as IList<Day>;
            Assert.Equal(3, days.Count);
        }

        [Fact]
        public void GetAllTemplateDaysByWeekNr_Succeeds()
        {
            int weekNr = 1;
            IList<Day> daysWeek1 = _dummyDBContext.Days.Where(day => day.WeekNr == weekNr).ToList();
            _dayRepository.Setup(d => d.GetAllByWeek(weekNr)).Returns(daysWeek1);
            ActionResult<IEnumerable<Day>> actionResult = _controller.GetAllTemplateDaysByWeek(weekNr);
            IList<Day> days = actionResult.Value as IList<Day>;
            Assert.Equal(3, days.Count);
        }


        [Fact]
        public void GetTemplateDayById_Succeeds()
        {
            int dayId = 1;
            _dayRepository.Setup(d => d.GetById(dayId)).Returns(_dummyDBContext.Day1);
            ActionResult<Day> actionResult = _controller.GetTemplateDayById(dayId);
            Assert.Equal(4, actionResult.Value.DayActivities.Count);
            Assert.Equal(1, actionResult.Value.DayNr);
        }

        [Fact]
        public void GetTemplateDayById_ReturnsNull()
        {
            int dayId = 4;
            _dayRepository.Setup(d => d.GetById(dayId)).Returns((Day)null);
            ActionResult<Day> actionResult = _controller.GetTemplateDayById(dayId);
            Assert.Null(actionResult.Value);
        }

        [Fact]
        public void GetTemplateDayByWeekAndDay_Succeeds()
        {
            int dayNr = 1;
            int weekNr = 1;
            _dayRepository.Setup(d => d.GetByWeekAndDay(weekNr, dayNr)).Returns(_dummyDBContext.Day1);
            ActionResult<Day> actionResult = _controller.GetTemplateDayByWeekAndDay(weekNr, dayNr);
            Assert.Equal(4, actionResult.Value.DayActivities.Count);
            Assert.Equal(1, actionResult.Value.DayNr);
            Assert.Equal(1, actionResult.Value.WeekNr);
        }

        [Fact]
        public void GetTemplateDayByWeekAndDay_ReturnsNull()
        {
            int dayNr = 4;
            int weekNr = 1;
            _dayRepository.Setup(d => d.GetByWeekAndDay(weekNr, dayNr)).Returns((Day)null);
            ActionResult<Day> actionResult = _controller.GetTemplateDayByWeekAndDay(weekNr, dayNr);
            Assert.Null(actionResult.Value);
        }
        #endregion

        #region Add
        [Fact]
        public void AddTemplateDay_Succeeds()
        {
            DayActivityDTO dayActivityDTO1 = new DayActivityDTO()
            {
                DayId = 1,
                ActivityId = 1,
                TimeOfDay = TimeOfDay.AVOND,
                Attendances = null
            };

            DayActivityDTO dayActivityDTO2 = new DayActivityDTO()
            {
                DayId = 1,
                ActivityId = 2,
                TimeOfDay = TimeOfDay.VOORMIDDAG,
                Attendances = null
            };

            HelperDTO helperDTO = new HelperDTO()
            {
                UserId = 1,
                DayId = 1
            };
            DayDTO dayDTO = new DayDTO()
            {
                DayActivities = new[] { dayActivityDTO1, dayActivityDTO2 },
                Helpers = new[] { helperDTO },
                DayNr = 1,
                WeekNr = 2
            };

            ActionResult<Day> actionResult = _controller.AddTemplateDay(dayDTO);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Day dayResult = okObjectResult.Value as Day;
            Assert.Equal(2, dayResult.DayActivities.Count);
        }
        #endregion

        #region Edit
        [Fact]
        public void AddTemplateDay_Edit()
        {
            DayActivityDTO dayActivityDTO1 = new DayActivityDTO()
            {
                DayId = 1,
                ActivityId = 1,
                TimeOfDay = TimeOfDay.AVOND,
                Attendances = null
            };

            DayActivityDTO dayActivityDTO2 = new DayActivityDTO()
            {
                DayId = 1,
                ActivityId = 2,
                TimeOfDay = TimeOfDay.VOORMIDDAG,
                Attendances = null
            };

            HelperDTO helperDTO = new HelperDTO()
            {
                UserId = 1,
                DayId = 1
            };
            DayDTO dayDTO = new DayDTO()
            {
                DayId = 1,
                DayActivities = new[] { dayActivityDTO1, dayActivityDTO2 },
                Helpers = new[] { helperDTO },
                DayNr = 1,
                WeekNr = 2
            };

            _dayRepository.Setup(d => d.GetById(dayDTO.DayId)).Returns(_dummyDBContext.Day1);
            ActionResult<Day> actionResult = _controller.EditTemplateDay(dayDTO);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Day dayResult = okObjectResult.Value as Day;
            Assert.Equal(2, dayResult.DayActivities.Count);
        }
        #endregion
        #endregion

        #region Get
        [Fact]
        public void GetAllCustomDays_Succeeds()
        {
            _customDayRepository.Setup(a => a.GetAll()).Returns(_dummyDBContext.CustomDays);
            ActionResult<IEnumerable<Day>> actionResult = _controller.GetAllCustomDays();
            IList<Day> days = actionResult.Value.ToList();
            Assert.Equal(3, days.Count);
        }

        [Fact]
        public void GetAllCustomDaysInRange()
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(1);
            IList<CustomDay> days = _dummyDBContext.CustomDays.Where(day => day.Date >= startDate && day.Date <= endDate).ToList();
            _customDayRepository.Setup(c => c.GetAllInRange(startDate, endDate)).Returns(days);

            ActionResult<IEnumerable<Day>> actionResult = _controller.GetAllCustomDaysInRange(startDate, endDate);
            IList<Day> daysResult = actionResult.Value.ToList();
            Assert.Equal(2, daysResult.Count);
        }

        [Fact]
        public void GetCustomDayById_Succeeds()
        {
            int customDayId = 1;
            _customDayRepository.Setup(c => c.GetById(customDayId)).Returns(_dummyDBContext.CustomDay1);
            ActionResult<Day> actionResult = _controller.GetCustomDayById(customDayId);
            CustomDay day = actionResult.Value as CustomDay;
            Assert.Equal(DateTime.Today, day.Date);
        }

        [Fact]
        public void GetCustomDayByDate_Succeeds()
        {
            DateTime date = DateTime.Today.AddDays(1);
            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay2);
            ActionResult<Day> actionResult = _controller.GetCustomDayByDate(date);
            CustomDay day = actionResult.Value as CustomDay;
            Assert.Equal(2, day.DayNr);
        }

        [Fact]
        public void GetAbsentUsersForDate_Succeeds()
        {
            IList<User> absents = new List<User>();
            absents.Add(_dummyDBContext.U2);
            _customDayRepository.Setup(c => c.GetAbsentUsersForDay(DateTime.Today)).Returns(absents);
            ActionResult<IEnumerable<User>> actionResult = _controller.GetAbsentUsersForDay(DateTime.Today);
            IList<User> absentsResult = actionResult.Value.ToList();
            Assert.Equal(1, absentsResult.Count);
        } 
        #endregion




    }
}
