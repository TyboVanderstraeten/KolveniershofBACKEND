using KolveniershofBACKEND.Controllers;
using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.DTO;
using KolveniershofBACKEND.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace KolveniershofBACKEND.Tests.Controllers
{
    public class WeekendsControllerTest
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IWeekendDayRepository> _weekendDayRepository;
        private WeekendsController _controller;
        private DummyDBContext _dummyDBContext;

        public WeekendsControllerTest()
        {
            _weekendDayRepository = new Mock<IWeekendDayRepository>();
            _userRepository = new Mock<IUserRepository>();
            _controller = new WeekendsController(_userRepository.Object, _weekendDayRepository.Object);
            _dummyDBContext = new DummyDBContext();
        }

        #region GET
        [Fact]
        public void WeekendDay_GetByRightDateAndUserId_ReturnsOKResult()
        {
            DateTime date = DateTime.Today;
            int userId = 4;
            _weekendDayRepository.Setup(w => w.GetByDate(date, userId)).Returns(_dummyDBContext.GoingOutWithGirlfriendOn24112019);

            ActionResult<WeekendDay> actionResult = _controller.Get(date, userId);
            var response = actionResult?.Result as OkObjectResult;
            WeekendDay weekend = response?.Value as WeekendDay;

            Assert.IsType<OkObjectResult>(actionResult?.Result);
            Assert.Equal("afspreken met liefje", weekend.Comment);
            Assert.Equal(4, weekend.UserId);
        }

        [Fact]
        public void WeekendDay_WrongUserId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            int userId = 4;
            _weekendDayRepository.Setup(w => w.GetByDate(date, 3)).Returns(_dummyDBContext.GoingOutWithGirlfriendOn24112019);

            ActionResult<WeekendDay> actionResult = _controller.Get(date, userId);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }

        [Fact]
        public void WeekendDay_WrongDateRightUserId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            int userId = 4;
            _weekendDayRepository.Setup(w => w.GetByDate(date.AddDays(1), 4)).Returns(_dummyDBContext.GoingOutWithGirlfriendOn24112019);

            ActionResult<WeekendDay> actionResult = _controller.Get(date, userId);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }

        [Fact]
        public void WeekendDay_WrongDateWrongUserId_ReturnsNotFound()
        {
            DateTime date = DateTime.Today;
            int userId = 4;
            _weekendDayRepository.Setup(w => w.GetByDate(date.AddDays(1), 3)).Returns(_dummyDBContext.GoingOutWithGirlfriendOn24112019);

            ActionResult<WeekendDay> actionResult = _controller.Get(date, userId);
        
            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }
        #endregion

        #region ADD
        [Fact]
        public void AddWeekendDay_RightUserId_ReturnsOKResult()
        {
            int userId = 3;
            WeekendDayDTO dto = new WeekendDayDTO()
            {
                Comment = "de auto wassen met papa",
                Date = DateTime.Today,
                UserId = userId,
                WeekendDayId = 4
            };
            _userRepository.Setup(u => u.GetById(userId)).Returns(_dummyDBContext.U3);

            ActionResult<WeekendDay> actionResult = _controller.Add(dto);
            var response = actionResult?.Result as OkObjectResult;
            WeekendDay weekendDay = response?.Value as WeekendDay;

            Assert.IsType<OkObjectResult>(actionResult?.Result);
            _userRepository.Verify(u => u.SaveChanges(), Times.Once);
            Assert.Equal(dto.Comment, weekendDay.Comment);
        }

        [Fact]
        public void AddWeekendDay_WrongUserId_ReturnsNotFound()
        {
            int userId = 3;
            WeekendDayDTO dto = new WeekendDayDTO()
            {
                Comment = "de auto wassen met papa",
                Date = DateTime.Today,
                UserId = userId,
                WeekendDayId = 4
            };
            _userRepository.Setup(u => u.GetById(4)).Returns(_dummyDBContext.U3);

            ActionResult<WeekendDay> actionResult = _controller.Add(dto);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }
        #endregion

        #region EDIT
        [Fact]
        public void Edit_RightDateAndUserId_EditsWeekendDay()
        {
            int userId = 4;
            DateTime date = new DateTime(2019, 11, 24);
            CommentDTO newDTO = new CommentDTO()
            {
                Comment = "nieuwe commentaar"
            };
            _weekendDayRepository.Setup(w => w.GetByDate(date, userId)).Returns(_dummyDBContext.GoingOutWithGirlfriendOn24112019);

            ActionResult<WeekendDay> actionResult = _controller.Edit(date, userId, newDTO);
            var response = actionResult?.Result as OkObjectResult;
            WeekendDay weekendDay = response?.Value as WeekendDay;

            Assert.IsType<OkObjectResult>(actionResult?.Result);
            Assert.Equal("nieuwe commentaar", weekendDay.Comment);
        }

        [Fact]
        public void Edit_WrongDateAndWrongUserId_ReturnsNotFound()
        {
            int userId = 4;
            DateTime date = new DateTime(2019, 11, 24);
            CommentDTO newDTO = new CommentDTO()
            {
                Comment = "nieuwe commentaar"
            };
            _weekendDayRepository.Setup(w => w.GetByDate(date.AddDays(1), userId - 1)).Returns(_dummyDBContext.GoingOutWithGirlfriendOn24112019);

            ActionResult<WeekendDay> actionResult = _controller.Edit(date, userId, newDTO);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }
        #endregion

        #region DELETE
        [Fact]
        public void Remove_RightDateAndRightUserId_ReturnsOkResult()
        {
            int userId = 4;
            DateTime date = new DateTime(2019, 11, 24);
            _weekendDayRepository.Setup(w => w.GetByDate(date, userId)).Returns(_dummyDBContext.GoingOutWithGirlfriendOn24112019);
            _userRepository.Setup(u => u.GetById(userId)).Returns(_dummyDBContext.U4);

            ActionResult<WeekendDay> actionResult = _controller.Remove(date, userId);
            var response = actionResult?.Result as OkObjectResult;
            WeekendDay weekendDay = response?.Value as WeekendDay;

            Assert.IsType<OkObjectResult>(actionResult?.Result);
            Assert.Equal("afspreken met liefje", weekendDay.Comment);
        }

        [Fact]
        public void Remove_WrongDate_ReturnsNotFound()
        {
            int userId = 4;
            DateTime date = new DateTime(2019, 11, 24);
            _weekendDayRepository.Setup(w => w.GetByDate(date.AddDays(1), userId)).Returns(_dummyDBContext.GoingOutWithGirlfriendOn24112019);

            ActionResult<WeekendDay> actionResult = _controller.Remove(date, userId);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }

        [Fact]
        public void Remove_WrongUserId_ReturnsNotFound()
        {
            int userId = 4;
            DateTime date = new DateTime(2019, 11, 24);
            _weekendDayRepository.Setup(w => w.GetByDate(date, userId - 1)).Returns(_dummyDBContext.GoingOutWithGirlfriendOn24112019);

            ActionResult<WeekendDay> actionResult = _controller.Remove(date, userId);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }
        #endregion
    }
}
