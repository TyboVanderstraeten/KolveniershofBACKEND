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
            _customDayRepository.Verify(a => a.Add(It.IsAny<CustomDay>()), Times.Once);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Once);

        }
        #endregion

        #region Edit
        [Fact]
        public void Edit_Succeeds()
        {
            DateTime date = DateTime.Today;

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

            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);
            _dayRepository.Setup(d => d.GetByWeekAndDay(dayDTO.TemplateName, dayDTO.WeekNr, dayDTO.DayNr)).Returns(_dummyDBContext.Day1);

            ActionResult<CustomDay> actionResult = _controller.Edit(date, dayDTO);
            Assert.Equal("Chocomousse", actionResult.Value.Dessert);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Once);
        }
        #endregion


        #region Add / Remove activity
        [Fact]
        public void AddActivityToDay_Succeeds()
        {
            DateTime date = DateTime.Today;

            DayActivityDTO dayActivityDTO = new DayActivityDTO()
            {
                ActivityId = 2,
                TimeOfDay = TimeOfDay.VOORMIDDAG,
                Attendances = null
            };

            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);
            _activityRepository.Setup(a => a.GetById(dayActivityDTO.ActivityId)).Returns(_dummyDBContext.Activity2);

            ActionResult<DayActivity> actionResult = _controller.AddActivity(date, dayActivityDTO);
            Assert.Equal(TimeOfDay.VOORMIDDAG, actionResult.Value.TimeOfDay);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Once);
        }

        [Fact]
        public void RemoveActivity_Succeeds()
        {
            DateTime date = DateTime.Today;
            int activityId = 2;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;

            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity2);

            ActionResult<DayActivity> actionResult = _controller.RemoveActivity(date, activityId, timeOfDay);
            Assert.Equal("Koken", actionResult.Value.Activity.Name);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Once);
        }
        #endregion

        #region Add / Remove helper
        [Fact]
        public void AddHelper_Succeeds()
        {
            DateTime date = DateTime.Today;

            HelperDTO helperDTO = new HelperDTO()
            {
                UserId = 1
            };

            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);
            _userRepository.Setup(u => u.GetById(helperDTO.UserId)).Returns(_dummyDBContext.U1);

            ActionResult<Helper> actionResult = _controller.AddHelper(date, helperDTO);
            Assert.Equal("Tybo", actionResult.Value.User.FirstName);
            _customDayRepository.Verify(c => c.SaveChanges(), Times.Once());
        }

        [Fact]
        public void RemoveHelper_Succeeds()
        {
            DateTime date = DateTime.Today;
            int userId = 1;

            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);
            _helperRepository.Setup(h => h.GetCustomDayHelper(date, userId)).Returns(_dummyDBContext.Helper1);

            ActionResult<Helper> actionResult = _controller.RemoveHelper(date, userId);
            Assert.Equal("Tybo", actionResult.Value.User.FirstName);
            _customDayRepository.Verify(c => c.SaveChanges(), Times.Once());
        }

        #endregion

        #region Add / Remove note
        [Fact]
        public void AddNote_Succeeds()
        {
            DateTime date = DateTime.Today;

            NoteDTO noteDTO = new NoteDTO()
            {
                NoteType = NoteType.VERVOER,
                Content = "Jantje zijn moeder gaat hem brengen met de auto, hij zal de bus dus niet nemen"
            };

            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);
            ActionResult<Note> actionResult = _controller.AddNote(date, noteDTO);
            Assert.Equal(NoteType.VERVOER, actionResult.Value.NoteType);
            _customDayRepository.Verify(c => c.SaveChanges(), Times.Once());
        }

        [Fact]
        public void RemoveNote_Succeeds()
        {
            DateTime date = DateTime.Today;
            int noteId = 2;

            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);
            _noteRepository.Setup(n => n.GetCustomDayNote(date, noteId)).Returns(_dummyDBContext.Note2);

            ActionResult<Note> actionResult = _controller.RemoveNote(date, noteId);
            Assert.Equal(NoteType.VERVOER, actionResult.Value.NoteType);
            _customDayRepository.Verify(c => c.SaveChanges(), Times.Once());
        }
        #endregion

    }
}

