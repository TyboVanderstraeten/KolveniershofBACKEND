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
    public class DaysControllerTest
    {
        private Mock<IDayRepository> _dayRepository;
        private Mock<ICustomDayRepository> _customDayRepository;
        private Mock<IActivityRepository> _activityRepository;
        private Mock<IDayActivityRepository> _dayActivityRepository;
        private Mock<IHelperRepository> _helperRepository;
        private Mock<IUserRepository> _userRepository;
        private Mock<INoteRepository> _noteRepository;

        private DummyDBContext _dummyDBContext;
        private DaysController _controller;

        public DaysControllerTest()
        {
            _dayRepository = new Mock<IDayRepository>();
            _customDayRepository = new Mock<ICustomDayRepository>();
            _activityRepository = new Mock<IActivityRepository>();
            _dayActivityRepository = new Mock<IDayActivityRepository>();
            _helperRepository = new Mock<IHelperRepository>();
            _userRepository = new Mock<IUserRepository>();
            _noteRepository = new Mock<INoteRepository>();

            _dummyDBContext = new DummyDBContext();
            _controller = new DaysController(_dayRepository.Object, _customDayRepository.Object, _activityRepository.Object, _userRepository.Object,
                _dayActivityRepository.Object, _helperRepository.Object, _noteRepository.Object);
        }





        #region Get
        [Fact]
        public void GetAll_Succeeds()
        {
            _customDayRepository.Setup(a => a.GetAll()).Returns(_dummyDBContext.CustomDays);
            ActionResult<IEnumerable<CustomDay>> actionResult = _controller.GetAll();
            IList<CustomDay> days = actionResult.Value.ToList();
            Assert.Equal(3, days.Count);
        }

        [Fact]
        public void GetAllInRange()
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(1);
            IList<CustomDay> days = _dummyDBContext.CustomDays.Where(day => day.Date >= startDate && day.Date <= endDate).ToList();
            _customDayRepository.Setup(c => c.GetAllInRange(startDate, endDate)).Returns(days);

            ActionResult<IEnumerable<CustomDay>> actionResult = _controller.GetAll(startDate, endDate);
            IList<CustomDay> daysResult = actionResult.Value.ToList();
            Assert.Equal(2, daysResult.Count);
        }

        [Fact]
        public void GetById_Succeeds()
        {
            int customDayId = 1;
            _customDayRepository.Setup(c => c.GetById(customDayId)).Returns(_dummyDBContext.CustomDay1);
            ActionResult<CustomDay> actionResult = _controller.GetById(customDayId);
            CustomDay day = actionResult.Value;
            Assert.Equal(DateTime.Today, day.Date);
        }

        [Fact]
        public void GetByDate_Succeeds()
        {
            DateTime date = DateTime.Today.AddDays(1);
            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay2);
            ActionResult<CustomDay> actionResult = _controller.GetByDate(date);
            CustomDay day = actionResult.Value;
            Assert.Equal(2, day.DayNr);
        }

        [Fact]
        public void GetForUser_Succeeds()
        {
            int userId = 1;
            DateTime date = DateTime.Today;
            _customDayRepository.Setup(d => d.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);
            ActionResult<CustomDay> actionResult = _controller.GetForUser(userId, date);
            Assert.Equal("chocomousse", actionResult.Value.Dessert);

        }

        [Fact]
        public void GetAbsentUsersForDate_Succeeds()
        {
            IList<User> absents = new List<User>();
            absents.Add(_dummyDBContext.U2);
            _customDayRepository.Setup(c => c.GetAbsentUsersForDay(DateTime.Today)).Returns(absents);
            ActionResult<IEnumerable<User>> actionResult = _controller.GetAbsent(DateTime.Today);
            IList<User> absentsResult = actionResult.Value.ToList();
            Assert.Equal(1, absentsResult.Count);
        }

        [Fact]
        public void GetNotesForDate_Succeeds()
        {
            DateTime date = DateTime.Today;
            _customDayRepository.Setup(d => d.GetNotesForDay(date)).Returns(_dummyDBContext.Notes);
            ActionResult<IEnumerable<Note>> actionResult = _controller.GetNotes(date);
            IList<Note> notesResult = actionResult.Value as IList<Note>;
            Assert.Equal(2, notesResult.Count);
        }
        #endregion

        #region Add
        [Fact]
        public void AddCustomDay_Succeeds()
        {

            CustomDayDTO dayDTO = new CustomDayDTO()
            {
                TemplateName = "eerste_week_eerste_dag",
                DayNr = 1,
                WeekNr = 1,
                Date = DateTime.Today,
                PreDish = "Kervelsoep",
                MainDish = "Kip",
                Dessert = "Chocomousse",
                Notes = null
            };

            _dayRepository.Setup(c => c.GetByWeekAndDay(dayDTO.TemplateName, dayDTO.WeekNr, dayDTO.DayNr)).Returns(_dummyDBContext.Day1);

            ActionResult<CustomDay> actionResult = _controller.Add(dayDTO);
            CustomDay customDay = actionResult.Value;
            Assert.Equal(4, customDay.DayActivities.Count);
        } 
        #endregion



        //[Fact] //TODO MICHAEL
        //public void AddActivityToDay_Succeeds()
        //{

        //}

        //[Fact] //TODO MICHAEL
        //public void AddHelperToDay_Succeeds()
        //{

        //}

        //[Fact] //TODO MICHAEL
        //public void AddNoteToDay_Succeeds()
        //{

        //}

    }
}
