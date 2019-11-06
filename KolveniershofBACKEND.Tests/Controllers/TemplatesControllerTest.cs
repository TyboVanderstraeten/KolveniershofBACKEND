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
    public class TemplatesControllerTest
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IDayActivityRepository> _dayActivityRepository;
        private Mock<IActivityRepository> _activityRepository;
        private Mock<IDayRepository> _dayRepository;
        private Mock<IHelperRepository> _helperRepository;

        private DummyDBContext _dummyDBContext;
        private TemplatesController _controller;
 
        public TemplatesControllerTest()
        {
            _userRepository = new Mock<IUserRepository>();
            _dayActivityRepository = new Mock<IDayActivityRepository>();
            _activityRepository = new Mock<IActivityRepository>();
            _dayRepository = new Mock<IDayRepository>();
            _helperRepository = new Mock<IHelperRepository>();
            _dummyDBContext = new DummyDBContext();
            _controller = new TemplatesController(_dayRepository.Object, _activityRepository.Object, _userRepository.Object, _dayActivityRepository.Object,_helperRepository.Object);
        }

        #region TemplateDay
        #region Get
        [Fact]
        public void GetAll_Succeeds()
        {

            _dayRepository.Setup(d => d.GetAll()).Returns(_dummyDBContext.Days);
            ActionResult<IEnumerable<Day>> actionResult = _controller.GetAll();
            IList<Day> days = actionResult.Value as IList<Day>;
            Assert.Equal(3, days.Count);
        }

        [Fact]
        public void GetAllByTemplateName_Succeeds()
        {
            string templateName = "eerste_week_eerste_dag";
            IList<Day> daysWeek1 = _dummyDBContext.Days.Where(day => day.TemplateName == templateName).ToList();

            _dayRepository.Setup(d => d.GetAllByTemplateName(templateName)).Returns(daysWeek1);
            ActionResult<IEnumerable<Day>> actionResult = _controller.GetAll(templateName);
            IList<Day> days = actionResult.Value as IList<Day>;
            Assert.Equal(1, days.Count);
        }

        [Fact]
        public void GetAllByTemplateNameAndWeek_Succeeds()
        {
            int weekNr = 1;
            string templateName = "eerste_week_eerste_dag";
            IList<Day> daysWeek1 = _dummyDBContext.Days.Where(day => day.WeekNr == weekNr && day.TemplateName == templateName).ToList();

            _dayRepository.Setup(d => d.GetAllByWeek(templateName,weekNr)).Returns(daysWeek1);
            ActionResult<IEnumerable<Day>> actionResult = _controller.GetAll(templateName,weekNr);
            IList<Day> days = actionResult.Value as IList<Day>;
            Assert.Equal(1, days.Count);
        }


        [Fact]
        public void GetById_Succeeds()
        {
            int dayId = 1;
            _dayRepository.Setup(d => d.GetById(dayId)).Returns(_dummyDBContext.Day1);
            ActionResult<Day> actionResult = _controller.GetById(dayId);
            Assert.Equal(4, actionResult.Value.DayActivities.Count);
            Assert.Equal(1, actionResult.Value.DayNr);
        }

        [Fact]
        public void GetById_ReturnsNull()
        {
            int dayId = 4;
            _dayRepository.Setup(d => d.GetById(dayId)).Returns((Day)null);
            ActionResult<Day> actionResult = _controller.GetById(dayId);
            Assert.Null(actionResult.Value);
        }

        [Fact]
        public void GetByTemplateNameWeekAndDay_Succeeds()
        {
            int dayNr = 1;
            int weekNr = 1;
            string templatename = "eerste_week_eerste_dag";
            _dayRepository.Setup(d => d.GetByWeekAndDay(templatename,weekNr, dayNr)).Returns(_dummyDBContext.Day1);
            ActionResult<Day> actionResult = _controller.GetByWeekAndDay(templatename,weekNr, dayNr);
            Assert.Equal(4, actionResult.Value.DayActivities.Count);
            Assert.Equal(1, actionResult.Value.DayNr);
            Assert.Equal(1, actionResult.Value.WeekNr);
        }

        [Fact]
        public void GetByTemplateNameWeekAndDay_ReturnsNull()
        {
            int dayNr = 4;
            int weekNr = 1;
            string templatename = "eerste_week_eerste_dag";
            _dayRepository.Setup(d => d.GetByWeekAndDay(templatename, weekNr, dayNr)).Returns((Day)null);
            ActionResult<Day> actionResult = _controller.GetByWeekAndDay(templatename,weekNr, dayNr);
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
                TemplateName = "tweede_week_eerste_dag",
                DayActivities = new[] { dayActivityDTO1, dayActivityDTO2 },
                Helpers = new[] { helperDTO },
                DayNr = 1,
                WeekNr = 2
            };

            ActionResult<Day> actionResult = _controller.Add(dayDTO);
            Day dayResult = actionResult.Value;
            Assert.Equal("tweede_week_eerste_dag", dayResult.TemplateName);
        }
        #endregion

        #region Remove
        [Fact]
        public void Remove_Succeeds()
        {
            string templateName = "eerste_week_eerste_dag";
            int weekNr = 1;
            int dayNr = 1;

            _dayRepository.Setup(d => d.GetByWeekAndDay(templateName, weekNr, dayNr)).Returns(_dummyDBContext.Day1);
            ActionResult<Day> actionResult = _controller.Remove(templateName, weekNr, dayNr);
            Assert.Equal(templateName, actionResult.Value.TemplateName);
        } 
        #endregion
        #endregion






    }
}
