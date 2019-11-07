using KolveniershofBACKEND.Controllers;
using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Tests.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace KolveniershofBACKEND.Tests.Controllers
{
    public class DaysControllerTest
    {
        private Mock<IDayRepository> _dayRepository;
        private Mock<ICustomDayRepository> _customDayRepository;
        private Mock<IActivityRepository> _activityRepository;
        private Mock<IDayActivityRepository> _dayActivityRepository;
        private Mock<IHelperRepository> _helperRepository;
        private Mock<IUserRepository>_userRepository;
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

        //#region CustomDay
        //#region Get
        //[Fact]
        //public void GetAllCustomDays_Succeeds()
        //{
        //    _customDayRepository.Setup(a => a.GetAll()).Returns(_dummyDBContext.CustomDays);
        //    ActionResult<IEnumerable<Day>> actionResult = _controller.GetAllCustomDays();
        //    IList<Day> days = actionResult.Value.ToList();
        //    Assert.Equal(3, days.Count);
        //}

        //[Fact]
        //public void GetAllCustomDaysInRange()
        //{
        //    DateTime startDate = DateTime.Today;
        //    DateTime endDate = DateTime.Today.AddDays(1);
        //    IList<CustomDay> days = _dummyDBContext.CustomDays.Where(day => day.Date >= startDate && day.Date <= endDate).ToList();
        //    _customDayRepository.Setup(c => c.GetAllInRange(startDate, endDate)).Returns(days);

        //    ActionResult<IEnumerable<Day>> actionResult = _controller.GetAllCustomDaysInRange(startDate, endDate);
        //    IList<Day> daysResult = actionResult.Value.ToList();
        //    Assert.Equal(2, daysResult.Count);
        //}

        //[Fact]
        //public void GetCustomDayById_Succeeds()
        //{
        //    int customDayId = 1;
        //    _customDayRepository.Setup(c => c.GetById(customDayId)).Returns(_dummyDBContext.CustomDay1);
        //    ActionResult<Day> actionResult = _controller.GetCustomDayById(customDayId);
        //    CustomDay day = actionResult.Value as CustomDay;
        //    Assert.Equal(DateTime.Today, day.Date);
        //}

        //[Fact]
        //public void GetCustomDayByDate_Succeeds()
        //{
        //    DateTime date = DateTime.Today.AddDays(1);
        //    _customDayRepository.Setup(c => c.GetByDate(date)).Returns(_dummyDBContext.CustomDay2);
        //    ActionResult<Day> actionResult = _controller.GetCustomDayByDate(date);
        //    CustomDay day = actionResult.Value as CustomDay;
        //    Assert.Equal(2, day.DayNr);
        //}

        //[Fact]
        //public void GetAbsentUsersForDate_Succeeds()
        //{
        //    IList<User> absents = new List<User>();
        //    absents.Add(_dummyDBContext.U2);
        //    _customDayRepository.Setup(c => c.GetAbsentUsersForDay(DateTime.Today)).Returns(absents);
        //    ActionResult<IEnumerable<User>> actionResult = _controller.GetAbsentUsersForDay(DateTime.Today);
        //    IList<User> absentsResult = actionResult.Value.ToList();
        //    Assert.Equal(1, absentsResult.Count);
        //}
        //#endregion

        //[Fact]
        //public void AddCustomDay_Succeeds()
        //{

        //    CustomDayDTO dayDTO = new CustomDayDTO()
        //    {
        //        DayNr = 1,
        //        WeekNr = 2,
        //        Date = DateTime.Today,
        //        PreDish = "Kervelsoep",
        //        MainDish = "Kip",
        //        Dessert = "Chocomousse",
        //        Notes = null
        //    };

        //    _dayRepository.Setup(c => c.GetByWeekAndDay(dayDTO.WeekNr, dayDTO.DayNr)).Returns(_dummyDBContext.CustomDay2);

        //    ActionResult<CustomDay> actionResult = _controller.AddCustomDay(dayDTO);
        //    CustomDay customDay = actionResult.Value;
        //    Assert.Equal(4, customDay.DayActivities.Count);
        //}



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
        //#endregion
    }
}
