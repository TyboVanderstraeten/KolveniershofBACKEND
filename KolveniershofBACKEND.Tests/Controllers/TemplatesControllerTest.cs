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
            _controller = new TemplatesController(_dayRepository.Object, _activityRepository.Object, _userRepository.Object, _dayActivityRepository.Object, _helperRepository.Object);
        }

        #region TemplateDay
        #region Get
        [Fact]
        public void GetAll_Succeeds()
        {

            _dayRepository.Setup(d => d.GetAll()).Returns(_dummyDBContext.Days);

            ActionResult<IEnumerable<Day>> actionResult = _controller.GetAll();
            var response = actionResult?.Result as OkObjectResult;
            IEnumerable<Day> days = response?.Value as IEnumerable<Day>; 
    
            Assert.Equal(3, days?.Count());
        }

        [Fact]
        public void GetAllByTemplateName_Succeeds()
        {
            string templateName = "eerste_week_eerste_dag";
            IList<Day> daysWeek1 = _dummyDBContext.Days.Where(day => day.TemplateName == templateName).ToList();
            _dayRepository.Setup(d => d.GetAllByTemplateName(templateName)).Returns(daysWeek1);

            ActionResult<IEnumerable<Day>> actionResult = _controller.GetAll(templateName);
            var response = actionResult?.Result as OkObjectResult;
            IEnumerable<Day> days = response?.Value as IEnumerable<Day>;

            Assert.Equal(1, days?.Count());
        }

        [Fact]
        public void GetAllByTemplateName_WrongTemplateName_ReturnsNotFound()
        {
            string templateName = "ag";

            ActionResult<IEnumerable<Day>> actionResult = _controller.GetAll(templateName);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }

        [Fact]
        public void GetAllByTemplateNameAndWeek_Succeeds()
        {
            int weekNr = 1;
            string templateName = "eerste_week_eerste_dag";
            IList<Day> daysWeek1 = _dummyDBContext.Days.Where(day => day.WeekNr == weekNr && day.TemplateName == templateName).ToList();
            _dayRepository.Setup(d => d.GetAllByWeek(templateName, weekNr)).Returns(daysWeek1);

            ActionResult<IEnumerable<Day>> actionResult = _controller.GetAll(templateName, weekNr);
            var response = actionResult?.Result as OkObjectResult;
            IEnumerable<Day> days = response?.Value as IEnumerable<Day>;

            Assert.Equal(1, days?.Count());
        }

        [Fact]
        public void GetByTemplateNameWeekAndDay_Succeeds()
        {
            int dayNr = 1;
            int weekNr = 1;
            string templatename = "eerste_week_eerste_dag";
            _dayRepository.Setup(d => d.GetByWeekAndDay(templatename, weekNr, dayNr)).Returns(_dummyDBContext.Day1);


            ActionResult<Day> actionResult = _controller.GetByWeekAndDay(templatename, weekNr, dayNr);
            var response = actionResult?.Result as OkObjectResult;
            Day day = response?.Value as Day;

            Assert.Equal(4, day.DayActivities.Count);
            Assert.Equal(1, day.DayNr);
            Assert.Equal(1, day.WeekNr);
        }

        [Fact]
        public void GetByTemplateNameWeekAndDay_ReturnsNull()
        {
            int dayNr = 4;
            int weekNr = 1;
            string templatename = "eerste_week_eerste_dag";
            _dayRepository.Setup(d => d.GetByWeekAndDay(templatename, weekNr, dayNr)).Returns((Day)null);
            ActionResult<Day> actionResult = _controller.GetByWeekAndDay(templatename, weekNr, dayNr);
            Assert.Null(actionResult.Value);
        }
        #endregion

        #region Add
        [Fact]
        public void AddTemplateDay_Succeeds()
        {
            DayDTO dayDTO = new DayDTO()
            {
                TemplateName = "tweede_week_eerste_dag",
                DayNr = 1,
                WeekNr = 2
            };

            ActionResult<Day> actionResult = _controller.Add(dayDTO);
            var response = actionResult?.Result as OkObjectResult;
            Day day = response?.Value as Day;


            Assert.Equal("tweede_week_eerste_dag", day.TemplateName);
            _dayRepository.Verify(d => d.Add(It.IsAny<Day>()), Times.Once());
            _dayRepository.Verify(d => d.SaveChanges(), Times.Once());
        }

        [Fact]
        public void AddTemplateDay_TwiceSameDay_ReturnBadRequest()
        {
            Day existingDay = _dummyDBContext.Day6;
            DayDTO newDayDTO = new DayDTO()
            {
                TemplateName = "tweede_week_eerste_dag",
                DayNr = 1,
                WeekNr = 2
            };
            _dayRepository.Setup(d => d.GetByWeekAndDay(existingDay.TemplateName, existingDay.WeekNr, existingDay.DayNr)).Returns(existingDay);


            ActionResult<Day> actionResult = _controller.Add(newDayDTO);

            Assert.IsType<BadRequestObjectResult>(actionResult?.Result);
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
            _dayRepository.Verify(d => d.Remove(It.IsAny<Day>()), Times.Once());
            _dayRepository.Verify(d => d.SaveChanges(), Times.Once());
        }

        [Fact]
        public void RemoveTemplateDay_NonExistingDay_ReturnsNotFound()
        {
            string templateName = "vijfde_week_zesde_dag";
            int weekNr = 5;
            int dayNr = 6;
            _dayRepository.Setup(d => d.GetByWeekAndDay(templateName, 1, 1)).Returns(_dummyDBContext.Day1);
            
            ActionResult<Day> actionResult = _controller.Remove(templateName, weekNr, dayNr);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }
        #endregion

        #region Add / Remove activity
        [Fact]
        public void AddActivity_Succeeds()
        {

            string templateName = "eerste_week_eerste_dag";
            int weekNr = 1;
            int dayNr = 1;

            DayActivityDTO dayActivityDTO = new DayActivityDTO()
            {
                DayId = 1,
                ActivityId = 1,
                TimeOfDay = TimeOfDay.VOLLEDIG,
                Attendances = null
            };

            _dayRepository.Setup(d => d.GetByWeekAndDay(templateName, weekNr, dayNr)).Returns(_dummyDBContext.Day1);
            _activityRepository.Setup(d => d.GetById(dayActivityDTO.ActivityId)).Returns(_dummyDBContext.Activity1);

            ActionResult<DayActivity> actionResult = _controller.AddActivity(templateName, weekNr, dayNr, dayActivityDTO);
            Assert.Equal(TimeOfDay.VOLLEDIG, actionResult.Value.TimeOfDay);
            _dayRepository.Verify(d => d.SaveChanges(), Times.Once());
        }

        [Fact]
        public void RemoveActivity_Succeeds()
        {

            string templateName = "eerste_week_eerste_dag";
            int weekNr = 1;
            int dayNr = 1;
            int activityId = 1;
            TimeOfDay timeOfDay = TimeOfDay.VOLLEDIG;


            _dayRepository.Setup(d => d.GetByWeekAndDay(templateName, weekNr, dayNr)).Returns(_dummyDBContext.Day1);
            _dayActivityRepository.Setup(d => d.GetTemplateDayActivity(templateName, weekNr, dayNr, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<DayActivity> actionResult = _controller.RemoveActivity(templateName, weekNr, dayNr, activityId, timeOfDay);
            var response = actionResult?.Result as OkObjectResult;
            DayActivity dayActivity = response?.Value as DayActivity;

            Assert.Equal(TimeOfDay.VOLLEDIG, dayActivity.TimeOfDay);
            _dayRepository.Verify(d => d.SaveChanges(), Times.Once());

        }

        [Fact]
        public void RemoveActivity_WrongWeekIdAndDayId_ReturnsNotFound()
        {
            string templateName = "eerste_week_eerste_dag";
            int weekNr = 5;
            int dayNr = 9;
            int activityId = 1;
            TimeOfDay timeOfDay = TimeOfDay.VOLLEDIG;
            _dayRepository.Setup(d => d.GetByWeekAndDay(templateName, 1, 1)).Returns(_dummyDBContext.Day1);

            ActionResult<DayActivity> actionResult = _controller.RemoveActivity(templateName, weekNr, dayNr, activityId, timeOfDay);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _dayRepository.Verify(d => d.SaveChanges(), Times.Never());
        }

        [Fact]
        public void RemoveActivity_RightWeekIdAndTemplateNameButWrongActivityId_ReturnsNotFound()
        {

            string templateName = "eerste_week_eerste_dag";
            int weekNr = 1;
            int dayNr = 1;
            int activityId = 99;
            TimeOfDay timeOfDay = TimeOfDay.VOLLEDIG;
            _dayRepository.Setup(d => d.GetByWeekAndDay(templateName, weekNr, dayNr)).Returns(_dummyDBContext.Day1);
            _dayActivityRepository.Setup(d => d.GetTemplateDayActivity(templateName, 1, 1, timeOfDay, 1)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<DayActivity> actionResult = _controller.RemoveActivity(templateName, weekNr, dayNr, activityId, timeOfDay);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _dayRepository.Verify(d => d.SaveChanges(), Times.Never());
        }
        #endregion

        #region Add / Remove Helper
        [Fact]
        public void AddHelper_Succeeds()
        {
            HelperDTO helperDTO = new HelperDTO()
            {
                UserId = 3,
                DayId = 1
            };

            string templateName = "eerste_week_eerste_dag";
            int weekNr = 1;
            int dayNr = 1;

            _dayRepository.Setup(d => d.GetByWeekAndDay(templateName, weekNr, dayNr)).Returns(_dummyDBContext.Day1);
            _userRepository.Setup(d => d.GetById(helperDTO.UserId)).Returns(_dummyDBContext.U1);

            ActionResult<Helper> actionResult = _controller.AddHelper(templateName, weekNr, dayNr, helperDTO);
            var response = actionResult?.Result as OkObjectResult;
            Helper helper = response?.Value as Helper;

            Assert.Equal("Tybo", helper.User.FirstName);
            _dayRepository.Verify(d => d.SaveChanges(), Times.Once());
        }

        [Fact]
        public void AddHelper_WrongDayIdAndTemplateName_ReturnsNotFound()
        {
            HelperDTO helperDTO = new HelperDTO()
            {
                UserId = 3,
                DayId = 1
            };
            string templateName = "eerste_week_eerste_dag";
            int weekNr = 15;
            int dayNr = -1;
            _dayRepository.Setup(d => d.GetByWeekAndDay(templateName, 1, 1)).Returns(_dummyDBContext.Day1);
            _userRepository.Setup(u => u.GetById(helperDTO.UserId)).Returns(_dummyDBContext.U3);

            ActionResult<Helper> actionResult = _controller.AddHelper(templateName, weekNr, dayNr, helperDTO);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _dayRepository.Verify(d => d.SaveChanges(), Times.Never());
        }

        [Fact]
        public void AddHelper_RightWeekIdAndDayIdButWrongUserId_ReturnsNotFound()
        {
            HelperDTO helperDTO = new HelperDTO()
            {
                UserId = 30,
                DayId = 1
            };
            string templateName = "eerste_week_eerste_dag";
            int weekNr = 1;
            int dayNr = 1;
            _userRepository.Setup(u => u.GetById(1)).Returns(_dummyDBContext.U1);
            _dayRepository.Setup(d => d.GetByWeekAndDay(templateName, weekNr, dayNr)).Returns(_dummyDBContext.Day1);

            ActionResult<Helper> actionResult = _controller.AddHelper(templateName, weekNr, dayNr, helperDTO);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _dayRepository.Verify(d => d.SaveChanges(), Times.Never());
        }

        [Fact]
        public void RemoveHelper_Succeeds()
        {
            string templateName = "eerste_week_eerste_dag";
            int weekNr = 1;
            int dayNr = 1;
            int userId = 3;
            _dayRepository.Setup(d => d.GetByWeekAndDay(templateName, weekNr, dayNr)).Returns(_dummyDBContext.Day1);
            _helperRepository.Setup(d => d.GetTemplateDayHelper(templateName, weekNr, dayNr, userId)).Returns(_dummyDBContext.Helper2);

            ActionResult<Helper> actionResult = _controller.RemoveHelper(templateName, weekNr, dayNr, userId);
            var response = actionResult?.Result as OkObjectResult;
            Helper helper = response?.Value as Helper;

            Assert.Equal("Tim", helper?.User.FirstName);
            _dayRepository.Verify(d => d.SaveChanges(), Times.Once());
        }

        [Fact]
        public void RemoveHelper_WrongDayIdAndTemplateName_ReturnsNotFound()
        {
            string templateName = "tiende_week_eerste_dag";
            int weekNr = 4;
            int dayNr = 20;
            int userId = 3;
            _dayRepository.Setup(d => d.GetByWeekAndDay("eerste_week_tweede_dag", 1, 2)).Returns(_dummyDBContext.Day1);
            _helperRepository.Setup(d => d.GetTemplateDayHelper("eerste_week_tweede_dag", 1, 2, 1)).Returns(_dummyDBContext.Helper1);

            ActionResult<Helper> actionResult = _controller.RemoveHelper(templateName, weekNr, dayNr, userId);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _dayRepository.Verify(d => d.SaveChanges(), Times.Never());
        }

        [Fact]
        public void RemoveHelper_RightWeekIdAndDayIdButWrongUserId_ReturnsNotFound()
        {
            string templateName = "eerste_week_eerste_dag";
            int weekNr = 1;
            int dayNr = 1;
            int userId = 33;
            _helperRepository.Setup(d => d.GetTemplateDayHelper(templateName, weekNr, dayNr, 1)).Returns(_dummyDBContext.Helper1);
            _dayRepository.Setup(d => d.GetByWeekAndDay(templateName, weekNr, dayNr)).Returns(_dummyDBContext.Day1);

            ActionResult<Helper> actionResult = _controller.RemoveHelper(templateName, weekNr, dayNr, userId);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _dayRepository.Verify(d => d.SaveChanges(), Times.Never());
        }
        #endregion

        #endregion
    }
}
