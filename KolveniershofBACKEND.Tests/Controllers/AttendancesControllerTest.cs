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

        #region Get
        [Fact]
        public void GetAll_Succeeds()
        {
            DateTime date = DateTime.Today;
            int activityId = 1;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;

            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<IEnumerable<Attendance>> actionResult = _controller.GetAll(date, activityId, timeOfDay);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            IList<Attendance> attendances = (okObjectResult.Value as IEnumerable<Attendance>).ToList();
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
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;

            Assert.Equal(attendances.Count, (okObjectResult.Value as IEnumerable<Attendance>).ToList().Count);
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
            ActionResult<IEnumerable<Attendance>> actionResult = _controller.GetAllPersonnel(date, activityId, timeOfDay);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;

            Assert.Equal(attendances.Count, (okObjectResult.Value as IEnumerable<Attendance>).ToList().Count);
        } 
        #endregion

        #region Add
        [Fact]
        public void AddAttendance_Succeeds()
        {
            DateTime date = DateTime.Today;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;
            int activityId = 7;
            int userId = 1;

            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);
            _userRepository.Setup(d => d.GetById(userId)).Returns(_dummyDBContext.UserNew);

            ActionResult<Attendance> actionResult = _controller.Add(date, timeOfDay, activityId, userId);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Attendance attendance = okObjectResult.Value as Attendance;


            Assert.Equal("Florian", attendance.User.FirstName);
            _dayActivityRepository.Verify(a => a.SaveChanges(), Times.Once());
        } 
        #endregion

        #region Remove
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
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Assert.Equal("Tybo",(okObjectResult.Value as Attendance).User.FirstName);
            _dayActivityRepository.Verify(d => d.SaveChanges(), Times.Once());
        } 
        #endregion

        #region Add / Remove comment
        [Fact]
        public void AddComment_Succeeds()
        {
            DateTime date = DateTime.Today;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;
            int activityId = 1;
            int userId = 1;
            CommentDTO commentDTO = new CommentDTO()
            {
                Comment = "Dit was een zeer leuke activiteit"
            };

            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<Attendance> actionResult = _controller.AddComment(date, timeOfDay, activityId, userId, commentDTO);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Attendance attendance = okObjectResult.Value as Attendance;
            Assert.Equal(commentDTO.Comment, attendance.Comment);
            _dayActivityRepository.Verify(d => d.SaveChanges(), Times.Once());
        }

        [Fact]
        public void RemoveComment_Succeeds()
        {
            DateTime date = DateTime.Today;
            TimeOfDay timeOfDay = TimeOfDay.VOLLEDIG;
            int activityId = 1;
            int userId = 1;

            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<Attendance> actionResult = _controller.RemoveComment(date, timeOfDay, activityId, userId);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Attendance attendance = okObjectResult.Value as Attendance;
            Assert.Equal(timeOfDay, attendance.TimeOfDay);
            _dayActivityRepository.Verify(d => d.SaveChanges(), Times.Once());
        }  
        #endregion

    }
}
