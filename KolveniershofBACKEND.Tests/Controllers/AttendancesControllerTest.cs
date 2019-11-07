using KolveniershofBACKEND.Controllers;
using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
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
    public class AttendancesControllerTest
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IDayActivityRepository> _dayActivityRepository;
        private Mock<IAttendanceRepository> _attendanceRepository;

        private DummyDBContext _dummyDBContext;
        private AttendancesController _controller;

        public AttendancesControllerTest()
        {
            _userRepository = new Mock<IUserRepository>();
            _dayActivityRepository = new Mock<IDayActivityRepository>();
            _attendanceRepository = new Mock<IAttendanceRepository>();

            _dummyDBContext = new DummyDBContext();

            _controller = new AttendancesController(_userRepository.Object, _dayActivityRepository.Object, _attendanceRepository.Object);
        }

        [Fact]
        public void GetAll_Succeeds()
        {
            DateTime date = DateTime.Today;
            int activityId = 1;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;

            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<IEnumerable<Attendance>> actionResult = _controller.GetAll(date, activityId, timeOfDay);
            IList<Attendance> attendances = actionResult.Value.ToList();

            Assert.Equal(3, attendances.Count);
        }

        [Fact]
        public void GetAllClients_Succeeds()
        {
            DateTime date = DateTime.Today;
            int activityId = 1;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;


            IList<Attendance> attendances = _dummyDBContext.Attendances1.Where(attendance => attendance.User.UserType == UserType.CLIENT).ToList();
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);
            ActionResult<IEnumerable<Attendance>> actionResult = _controller.GetAllClients(date, activityId, timeOfDay);

            Assert.Equal(attendances.Count, actionResult.Value.ToList().Count);
        }

        // Gives clients instead of personnel
        [Fact]
        public void GetAllPersonnel_Succeeds()
        {
            DateTime date = DateTime.Today;
            int activityId = 1;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;


            IList<Attendance> attendances = _dummyDBContext.Attendances1.Where(attendance => attendance.User.UserType != UserType.CLIENT).ToList();
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);
            ActionResult<IEnumerable<Attendance>> actionResult = _controller.GetAllClients(date, activityId, timeOfDay);

            Assert.Equal(attendances.Count, actionResult.Value.ToList().Count);
        }

        [Fact]
        public void AddAttendance_Succeeds()
        {
            DateTime date = DateTime.Today;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;
            int activityId = 1;
            int userId = 1;

            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);
            _userRepository.Setup(d => d.GetById(userId)).Returns(_dummyDBContext.U1);

            ActionResult<Attendance> actionResult = _controller.Add(date, timeOfDay, activityId, userId);

            Assert.Equal("Tybo", actionResult.Value.User.FirstName);
            _dayActivityRepository.Verify(a => a.SaveChanges(), Times.Once());
        }

        [Fact]
        public void RemoveAttendance_Succeeds()
        {
            DateTime date = DateTime.Today;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;
            int activityId = 1;
            int userId = 1;

            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);
            _attendanceRepository.Setup(d => d.GetForUser(date, timeOfDay, activityId, userId)).Returns(_dummyDBContext.Attendance1);

            ActionResult<Attendance> actionResult = _controller.Remove(date, timeOfDay, activityId, userId);
            Assert.Equal("Tybo", actionResult.Value.User.FirstName);
            _dayActivityRepository.Verify(d => d.SaveChanges(), Times.Once());
        }






    }
}
