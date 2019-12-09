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
            var response = actionResult?.Result as OkObjectResult;
            IEnumerable<CustomDay> days = response?.Value as IEnumerable<CustomDay>;

            Assert.Equal(_dummyDBContext.CustomDays.Length, days.Count());

        }

        [Fact]
        public void GetAllInRange()
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(1);
            IList<CustomDay> days = _dummyDBContext.CustomDays.Where(day => day.Date >= startDate && day.Date <= endDate).ToList();
            _customDayRepository.Setup(c => c.GetAllInRange(startDate, endDate)).Returns(days);

            ActionResult<IEnumerable<CustomDay>> actionResult = _controller.GetAll(startDate, endDate);
            var response = actionResult?.Result as OkObjectResult;
            IEnumerable<CustomDay> daysResult = response?.Value as IEnumerable<CustomDay>;


            Assert.Equal(2, daysResult.Count());
        }

        [Fact]
        public void GetAllInRange_WrongDates_ReturnsNotFound()
        {
            DateTime startDate = new DateTime(1945, 1, 1);
            DateTime endDate = new DateTime(1945, 12, 1);
            _customDayRepository.Setup(a => a.GetAllInRange(new DateTime(2019, 11, 1), new DateTime(2019, 11, 30))).Returns(_dummyDBContext.CustomDays);

            ActionResult<IEnumerable<CustomDay>> actionResult = _controller.GetAll(startDate, endDate);

            Assert.IsType<NotFoundResult>(actionResult?.Result);

        }

        [Fact]
        public void GetByDate_Succeeds()
        {
            DateTime date = DateTime.Today.AddDays(1);
            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay2);

            ActionResult<CustomDay> actionResult = _controller.GetByDate(date);
            var response = actionResult?.Result as OkObjectResult;
            CustomDay day = response?.Value as CustomDay;


            Assert.Equal(2, day.DayNr);
        }

        [Fact]
        public void GetByDate_WrongDate_ReturnsNotFound()
        {
            DateTime date = new DateTime(1945, 1, 1);
            _customDayRepository.Setup(a => a.GetByDate(new DateTime(2019, 11, 21))).Returns(_dummyDBContext.CustomDay1);

            ActionResult<CustomDay> actionResult = _controller.GetByDate(date);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }

        [Fact]
        public void GetForUser_Succeeds()
        {
            int userId = 1;
            DateTime date = DateTime.Today;
            _customDayRepository.Setup(d => d.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);

            ActionResult<CustomDay> actionResult = _controller.GetForUser(userId, date);
            var response = actionResult?.Result as OkObjectResult;
            CustomDay customDay = response?.Value as CustomDay;

            Assert.Equal("chocomousse", customDay.Dessert);
        }

        [Fact]
        public void GetForUser_WrongDate_ReturnsNotFound()
        {
            int userId = 1;
            DateTime date = new DateTime(1945, 1, 1);
            _customDayRepository.Setup(a => a.GetByDate(new DateTime(2019, 11, 21))).Returns(_dummyDBContext.CustomDay1);

            ActionResult<CustomDay> actionResult = _controller.GetForUser(userId, date);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }

        [Fact]
        public void GetForUser_RightDateButWrongUserId_ReturnsNotFound()
        {
            int userId = -15;
            DateTime date = DateTime.Today;
            _customDayRepository.Setup(d => d.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);
            _userRepository.Setup(u => u.GetById(1)).Returns(_dummyDBContext.U1);

            ActionResult<CustomDay> actionResult = _controller.GetForUser(userId, date);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }

        [Fact]
        public void GetAbsentUsersForDate_Succeeds()
        {
            IList<User> absents = new List<User>();
            absents.Add(_dummyDBContext.U2);
            _customDayRepository.Setup(c => c.GetAbsentUsersForDay(DateTime.Today)).Returns(absents);

            ActionResult<IEnumerable<User>> actionResult = _controller.GetAbsent(DateTime.Today);
            var response = actionResult?.Result as OkObjectResult;
            IEnumerable<User> absentsResult = response?.Value as IEnumerable<User>;


            Assert.Equal(1, absentsResult.Count());
        }

        [Fact]
        public void GetAbsentUsers_WrongDate_ReturnsNotFound()
        {
            DateTime date = new DateTime(1945, 1, 1);
            _customDayRepository.Setup(a => a.GetByDate(new DateTime(2019, 11, 21))).Returns(_dummyDBContext.CustomDay1);

            ActionResult<IEnumerable<User>> actionResult = _controller.GetAbsent(date);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }

        [Fact]
        public void GetSickUsers_WrongDate_ReturnsNotFound()
        {
            DateTime date = new DateTime(1945, 1, 1);
            _customDayRepository.Setup(a => a.GetByDate(new DateTime(2019, 11, 21))).Returns(_dummyDBContext.CustomDay1);

            ActionResult<IEnumerable<User>> actionResult = _controller.GetSick(date);

            Assert.IsType<NotFoundResult>(actionResult?.Result);

        }

        [Fact]
        public void GetNotesForDate_Succeeds()
        {
            DateTime date = DateTime.Today;
            _customDayRepository.Setup(d => d.GetNotesForDay(date)).Returns(_dummyDBContext.Notes);

            ActionResult<IEnumerable<Note>> actionResult = _controller.GetNotes(date);
            var response = actionResult?.Result as OkObjectResult;
            IEnumerable<Note> notesResult = response?.Value as IEnumerable<Note>;

            Assert.Equal(2, notesResult.Count());
        }

        [Fact]
        public void GetNotesForDate_WrongDate_ReturnsNotFound()
        {
            DateTime date = new DateTime(1945, 1, 1);
            _customDayRepository.Setup(a => a.GetByDate(new DateTime(2019, 11, 21))).Returns(_dummyDBContext.CustomDay1);

            ActionResult<IEnumerable<Note>> actionResult = _controller.GetNotes(date);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }

        [Fact]
        public void GetHelpersForDate_WrongDate_ReturnsNotFound()
        {
            DateTime date = new DateTime(1945, 1, 1);
            _customDayRepository.Setup(a => a.GetByDate(new DateTime(2019, 11, 21))).Returns(_dummyDBContext.CustomDay1);

            ActionResult<IEnumerable<Note>> actionResult = _controller.GetNotes(date);

            Assert.IsType<NotFoundResult>(actionResult?.Result);

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
            var response = actionResult?.Result as OkObjectResult;
            CustomDay customDay = response?.Value as CustomDay;



            Assert.Equal(4, customDay.DayActivities.Count);
            _customDayRepository.Verify(a => a.Add(It.IsAny<CustomDay>()), Times.Once);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Once);

        }

        [Fact]
        public void AddCustomDay_NonExistingWeekNrAndDayNr_ReturnsNotFound()
        {
            CustomDayDTO dayDTO = new CustomDayDTO()
            {
                TemplateName = "eerste_week_eerste_dag",
                DayNr = 8,
                WeekNr = 5,
                Date = DateTime.Today,
                PreDish = "Kervelsoep",
                MainDish = "Kip",
                Dessert = "Chocomousse",
                Notes = null
            };
            _dayRepository.Setup(a => a.GetByWeekAndDay(dayDTO.TemplateName, 1, 1)).Returns(_dummyDBContext.CustomDay1);

            ActionResult<CustomDay> actionResult = _controller.Add(dayDTO);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Never);
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
            var response = actionResult?.Result as OkObjectResult;
            CustomDay customDay = response?.Value as CustomDay;

            Assert.Equal("Chocomousse", customDay.Dessert);
            Assert.Equal(dayDTO.DayNr, customDay.DayNr);
            Assert.Equal(dayDTO.DayId, customDay.DayId);

            _customDayRepository.Verify(a => a.SaveChanges(), Times.Once);
        }

        [Fact]
        public void EditCustomDay_WrongDate_ReturnsNotFound()
        {
            DateTime date = new DateTime(1945, 1, 1);
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
            _customDayRepository.Setup(c => c.GetByDate(new DateTime(2019, 11, 21))).Returns(_dummyDBContext.CustomDay1);

            ActionResult<CustomDay> actionResult = _controller.Edit(date, dayDTO);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Never);
        }
        #endregion

        #region Delete CustomDay
        [Fact]
        public void DeleteCustomDay_RightDate_DeletesCustomDay()
        {
            DateTime date = DateTime.Today;
            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);

            ActionResult<CustomDay> actionResult = _controller.Remove(date);
            var response = actionResult?.Result as OkObjectResult;
            CustomDay customDay = response?.Value as CustomDay;

            Assert.IsType<OkObjectResult>(actionResult?.Result);
            Assert.Equal("eerste_week_eerste_dag", customDay.TemplateName);
            _customDayRepository.Verify(a => a.Remove(It.IsAny<CustomDay>()), Times.Once);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteCustomDay_WrongDate_DeletesCustomDay()
        {
            DateTime date = new DateTime(1945, 1, 1);
            _customDayRepository.Setup(c => c.GetByDate(DateTime.Today)).Returns(_dummyDBContext.CustomDay1);

            ActionResult<CustomDay> actionResult = _controller.Remove(date);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Never);
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
            var response = actionResult?.Result as OkObjectResult;
            DayActivity dayActivity = response?.Value as DayActivity;

            Assert.Equal(TimeOfDay.VOORMIDDAG, dayActivity.TimeOfDay);

            _customDayRepository.Verify(a => a.SaveChanges(), Times.Once);
        }

        [Fact]
        public void AddActivityToCustomDay_WrongDate_ReturnsNotFound()
        {
            DateTime date = new DateTime(1945, 1, 1);
            DayActivityDTO dayActivityDTO = new DayActivityDTO()
            {
                ActivityId = 2,
                TimeOfDay = TimeOfDay.VOORMIDDAG,
                Attendances = null
            };
            _customDayRepository.Setup(c => c.GetByDate(DateTime.Today)).Returns(_dummyDBContext.CustomDay1);

            ActionResult<DayActivity> actionResult = _controller.AddActivity(date, dayActivityDTO);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Never);
        }

        [Fact]
        public void AddActivityToCustomDay_RightDateButWrongActivityId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            DayActivityDTO dayActivityDTO = new DayActivityDTO()
            {
                ActivityId = -2,
                TimeOfDay = TimeOfDay.VOORMIDDAG,
                Attendances = null
            };
            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);
            _activityRepository.Setup(a => a.GetById(2)).Returns(_dummyDBContext.Activity2);

            ActionResult<DayActivity> actionResult = _controller.AddActivity(date, dayActivityDTO);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Never);
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
            var response = actionResult?.Result as OkObjectResult;
            DayActivity dayActivity = response?.Value as DayActivity;

            Assert.Equal("Koken", dayActivity.Activity.Name);

            _customDayRepository.Verify(a => a.SaveChanges(), Times.Once);
        }

        [Fact]
        public void RemoveActivityForCustomDay_WrongDate_ReturnsNotFound()
        {
            DateTime date = new DateTime(1945, 1, 1);
            int activityId = 1;
            TimeOfDay timeOfDay = TimeOfDay.OCHTEND;
            _customDayRepository.Setup(c => c.GetByDate(DateTime.Today)).Returns(_dummyDBContext.CustomDay1);
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity2);

            ActionResult<DayActivity> actionResult = _controller.RemoveActivity(date, activityId, timeOfDay);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Never);
        }

        [Fact]
        public void RemoveActivityForCustomDay_RightDateButWrongActivityId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            int activityId = -3;
            TimeOfDay timeOfDay = TimeOfDay.NAMIDDAG;
            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, 2)).Returns(_dummyDBContext.DayActivity2);

            ActionResult<DayActivity> actionResult = _controller.RemoveActivity(date, activityId, timeOfDay);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Never);
        }
        #endregion

        #region Add / Remove helper
        [Fact]
        public void AddHelper_RightDateButWrongUserId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            HelperDTO helperDTO = new HelperDTO()
            {
                UserId = 747

            };
            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);
            _userRepository.Setup(u => u.GetById(2)).Returns(_dummyDBContext.U2);

            ActionResult<Helper> actionResult = _controller.AddHelper(date, helperDTO);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Never);
        }

        [Fact]
        public void AddHelper_WrongDate_ReturnsNotFound()
        {
            DateTime date = new DateTime(1945, 1, 1);
            HelperDTO helperDTO = new HelperDTO()
            {
                UserId = 2
            };
            _customDayRepository.Setup(c => c.GetByDate(DateTime.Today)).Returns(_dummyDBContext.CustomDay1);

            ActionResult<Helper> actionResult = _controller.AddHelper(date, helperDTO);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Never);

        }

        [Fact]
        public void RemoveHelper_Succeeds()
        {
            DateTime date = DateTime.Today;
            int userId = 3;
            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);
            _helperRepository.Setup(h => h.GetCustomDayHelper(date, userId)).Returns(_dummyDBContext.Helper2);

            ActionResult<Helper> actionResult = _controller.RemoveHelper(date, userId);
            var response = actionResult?.Result as OkObjectResult;
            Helper helper = response?.Value as Helper;

            Assert.Equal("Tim", helper.User.FirstName);

            _customDayRepository.Verify(c => c.SaveChanges(), Times.Once());
        }

        [Fact]
        public void RemoveHelper_WrongDate_ReturnsNotFound()
        {
            DateTime date = new DateTime(1945, 1, 1);
            int userId = 3;
            _customDayRepository.Setup(c => c.GetByDate(DateTime.Today)).Returns(_dummyDBContext.CustomDay1);

            ActionResult<Helper> actionResult = _controller.RemoveHelper(date, userId);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Never);
        }

        [Fact]
        public void RemoveHelper_RightDateButWrongUserId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            int userId = 33;
            _helperRepository.Setup(h => h.GetCustomDayHelper(date, 2)).Returns(_dummyDBContext.Helper2);
            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);

            ActionResult<Helper> actionResult = _controller.RemoveHelper(date, userId);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Never);
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
            var response = actionResult?.Result as OkObjectResult;
            Note note = response?.Value as Note;

            Assert.Equal(NoteType.VERVOER, note.NoteType);
            Assert.Equal(noteDTO.Content, note.Content);

            _customDayRepository.Verify(c => c.SaveChanges(), Times.Once());
        }

        [Fact]
        public void AddNote_WrongDate_ReturnsNotFound()
        {
            DateTime date = new DateTime(1944, 1, 1);
            _customDayRepository.Setup(c => c.GetByDate(DateTime.Today)).Returns(_dummyDBContext.CustomDay1);

            NoteDTO noteDTO = new NoteDTO()
            {
                NoteType = NoteType.VERVOER,
                Content = "Jantje zijn moeder gaat hem brengen met de auto, hij zal de bus dus niet nemen"
            };

            ActionResult<Note> actionResult = _controller.AddNote(date, noteDTO);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Never);
        }

        [Fact]
        public void RemoveNote_Succeeds()
        {
            DateTime date = DateTime.Today;
            int noteId = 2;

            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);
            _noteRepository.Setup(n => n.GetCustomDayNote(date, noteId)).Returns(_dummyDBContext.Note2);

            ActionResult<Note> actionResult = _controller.RemoveNote(date, noteId);
            var response = actionResult?.Result as OkObjectResult;
            Note note = response?.Value as Note;

            Assert.Equal(NoteType.VARIA, note.NoteType);

            _customDayRepository.Verify(c => c.SaveChanges(), Times.Once());
        }

        [Fact]
        public void RemoveNote_WrongDate_ReturnsNotFound()
        {
            DateTime date = new DateTime(1945, 1, 1);
            int noteId = 2;
            _customDayRepository.Setup(c => c.GetByDate(DateTime.Today)).Returns(_dummyDBContext.CustomDay1);
            _noteRepository.Setup(n => n.GetCustomDayNote(DateTime.Today, noteId)).Returns(_dummyDBContext.Note2);

            ActionResult<Note> actionResult = _controller.RemoveNote(date, noteId);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Never);
        }

        [Fact]
        public void RemoveNote_RightDateButWrongNoteId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            int noteId = 29;
            _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay1);
            _noteRepository.Setup(n => n.GetCustomDayNote(DateTime.Today, 2)).Returns(_dummyDBContext.Note2);

            ActionResult<Note> actionResult = _controller.RemoveNote(date, noteId);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _customDayRepository.Verify(a => a.SaveChanges(), Times.Never);
        }
        #endregion

    }
}

