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
            var actualLengthAttendances = _dummyDBContext.DayActivity1.Attendances.Count();
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<IEnumerable<Attendance>> actionResult = _controller.GetAll(date, activityId, timeOfDay);
            var response = actionResult?.Result as OkObjectResult;
            IEnumerable<Attendance> attendances = response?.Value as IEnumerable<Attendance>;

            Assert.Equal(actualLengthAttendances, attendances?.Count());
        }

        [Fact]
        public void GetAll_NonExistingActivityId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            int activityId = 99;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, 1)).Returns(_dummyDBContext.DayActivity1);


            ActionResult<IEnumerable<Attendance>> actionResult = _controller.GetAll(date, activityId, timeOfDay);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
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
            var response = actionResult?.Result as OkObjectResult;
            IEnumerable<Attendance> attendancesResponse = response?.Value as IEnumerable<Attendance>;

            Assert.Equal(attendances.Count, attendancesResponse.Count());
        }

        [Fact]
        public void GetAllClients_NonExistingActivityId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            int activityId = 99;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, 1)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<IEnumerable<Attendance>> actionResult = _controller.GetAllClients(date, activityId, timeOfDay);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }

        [Fact]
        public void GetAllPersonnel_Succeeds()
        {
            DateTime date = DateTime.Today;
            int activityId = 1;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;


            IList<Attendance> attendances = _dummyDBContext.Attendances1.Where(attendance => attendance.User.UserType != UserType.CLIENT).ToList();
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<IEnumerable<Attendance>> actionResult = _controller.GetAllPersonnel(date, activityId, timeOfDay);
            var response = actionResult?.Result as OkObjectResult;
            IEnumerable<Attendance> attendancesResponse = response?.Value as IEnumerable<Attendance>;


            Assert.Equal(attendances.Count, attendancesResponse.Count());
        }

        [Fact]
        public void GetAllPersonnel_NonExistingActivityId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            int activityId = 99;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, 1)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<IEnumerable<Attendance>> actionResult = _controller.GetAllPersonnel(date, activityId, timeOfDay);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }

        #endregion

        #region Add
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
            var response = actionResult?.Result as OkObjectResult;
            Attendance newAttendance = response?.Value as Attendance;

            Assert.Equal("Tybo", newAttendance.User.FirstName);
            _dayActivityRepository.Verify(a => a.SaveChanges(), Times.Once());
        }

        [Fact]
        public void AddAttendance_NonExistingUserId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;
            int activityId = 1;
            int userId = 40;
            _userRepository.Setup(d => d.GetById(1)).Returns(_dummyDBContext.U1);
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<Attendance> actionResult = _controller.Add(date, timeOfDay, activityId, userId);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }

        [Fact]
        public void AddAttendance_NonExistingDayActivityId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;
            int activityId = 150;
            int userId = 1;
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, 1)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<Attendance> actionResult = _controller.Add(date, timeOfDay, activityId, userId);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }
        #endregion

        #region Remove
        [Fact]
        public void RemoveAttendance_Succeeds()
        {
            DateTime date = DateTime.Today;
            TimeOfDay timeOfDay = TimeOfDay.VOLLEDIG;
            int activityId = 5;
            int userId = 3;
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);
            _attendanceRepository.Setup(d => d.GetForUser(date, timeOfDay, activityId, userId)).Returns(_dummyDBContext.Attendance6);

            ActionResult<Attendance> actionResult = _controller.Remove(date, timeOfDay, activityId, userId);
            var response = actionResult?.Result as OkObjectResult;
            Attendance attendance = response?.Value as Attendance;

            Assert.Equal("Tim", attendance?.User.FirstName);
            _dayActivityRepository.Verify(d => d.SaveChanges(), Times.Once());
        }

        [Fact]
        public void RemoveAttendance_NonExistingDayActivityId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            TimeOfDay timeOfDay = TimeOfDay.VOLLEDIG;
            int activityId = 55;
            int userId = 3;
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, 1)).Returns(_dummyDBContext.DayActivity1);
            _attendanceRepository.Setup(d => d.GetForUser(date, timeOfDay, activityId, userId)).Returns(_dummyDBContext.Attendance6);

            ActionResult<Attendance> actionResult = _controller.Remove(date, timeOfDay, activityId, userId);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }

        [Fact]
        public void RemoveAttendance_NonExistingUserId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            TimeOfDay timeOfDay = TimeOfDay.VOLLEDIG;
            int activityId = 1;
            int userId = -3;
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);
            _attendanceRepository.Setup(d => d.GetForUser(date, timeOfDay, activityId, 3)).Returns(_dummyDBContext.Attendance6);

            ActionResult<Attendance> actionResult = _controller.Remove(date, timeOfDay, activityId, userId);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }
        #endregion

        #region Add / Remove comment
        [Fact]
        public void AddComment_Succeeds()
        {
            DateTime date = DateTime.Today;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;
            int activityId = 1;
            int userId = 2;
            CommentDTO commentDTO = new CommentDTO()
            {
                Comment = "Dit was een zeer leuke activiteit"
            };
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<Attendance> actionResult = _controller.AddComment(date, timeOfDay, activityId, userId, commentDTO);
            var response = actionResult?.Result as OkObjectResult;
            Attendance attendanceToEdit = response?.Value as Attendance;

            Assert.Equal(commentDTO.Comment, attendanceToEdit?.Comment);
            _dayActivityRepository.Verify(d => d.SaveChanges(), Times.Once());
        }

        [Fact]
        public void AddComment_NonExistingActivityId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;
            int activityId = 99;
            int userId = 2;
            CommentDTO commentDTO = new CommentDTO()
            {
                Comment = "Dit was een zeer leuke activiteit"
            };
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, 1)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<Attendance> actionResult = _controller.AddComment(date, timeOfDay, activityId, userId, commentDTO);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }

        [Fact]
        public void AddComment_RightActivityIdButNonExistingUserId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;
            int activityId = 1;
            int userId = -2;
            CommentDTO commentDTO = new CommentDTO()
            {
                Comment = "Dit was een zeer leuke activiteit"
            };
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<Attendance> actionResult = _controller.AddComment(date, timeOfDay, activityId, userId, commentDTO);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }

        [Fact]
        public void RemoveComment_Succeeds()
        {
            DateTime date = DateTime.Today;
            TimeOfDay timeOfDay = TimeOfDay.VOLLEDIG;
            int activityId = 1;
            int userId = 2;
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<Attendance> actionResult = _controller.RemoveComment(date, timeOfDay, activityId, userId);
            var response = actionResult?.Result as OkObjectResult;
            Attendance attendance = response?.Value as Attendance;

            Assert.Equal(timeOfDay, attendance.TimeOfDay);
            _dayActivityRepository.Verify(d => d.SaveChanges(), Times.Once());
        }

        [Fact]
        public void RemoveComment_NonExistingActivityId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;
            int activityId = 99;
            int userId = 2;
            CommentDTO commentDTO = new CommentDTO()
            {
                Comment = "Dit was een zeer leuke activiteit"
            };
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, 1)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<Attendance> actionResult = _controller.RemoveComment(date, timeOfDay, activityId, userId);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }

        [Fact]
        public void RemoveComment_RightActivityIdButNonExistingUserId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            TimeOfDay timeOfDay = TimeOfDay.AVOND;
            int activityId = 1;
            int userId = -2;
            CommentDTO commentDTO = new CommentDTO()
            {
                Comment = "Dit was een zeer leuke activiteit"
            };
            _dayActivityRepository.Setup(d => d.GetCustomDayActivity(date, timeOfDay, activityId)).Returns(_dummyDBContext.DayActivity1);

            ActionResult<Attendance> actionResult = _controller.RemoveComment(date, timeOfDay, activityId, userId);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }
        #endregion

    }
}
