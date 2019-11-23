using KolveniershofBACKEND.Controllers;
using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.DTO;
using KolveniershofBACKEND.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KolveniershofBACKEND.Tests.Controllers
{
    public class BusSchemesControllerTest
    {
        private Mock<IBusDriverRepository> _busDriverRepository;
        private Mock<IDriverRepository> _driverRepository;
        private DummyDBContext _dummyDBContext;
        private BusSchemesController _controller;

        public BusSchemesControllerTest()
        {
            _busDriverRepository = new Mock<IBusDriverRepository>();
            _driverRepository = new Mock<IDriverRepository>();
            _controller = new BusSchemesController(_busDriverRepository.Object, _driverRepository.Object);
            _dummyDBContext = new DummyDBContext();
        }

        #region GET
        [Fact]
        public void BusScheme_GetByValidWeek_ReturnsOKResult()
        {
            const int weekId = 1;

            _busDriverRepository.Setup(b => b.GetBusDriversByWeek(weekId)).Returns(_dummyDBContext.BusDriversForWeek1);

            ActionResult<IEnumerable<BusDriver>> actionResult = _controller.GetBusScheme(weekId);
            var response = actionResult?.Result as OkObjectResult;
            List<BusDriver> busDrivers = response?.Value as List<BusDriver>;

            Assert.Equal(_dummyDBContext.BusDriversForWeek1.Count, busDrivers.Count);
        }

        [Fact]
        public void BusScheme_GetByInvalidWeek_ReturnsNotFound()
        {
            const int weekId = 16;

            _busDriverRepository.Setup(b => b.GetBusDriversByWeek(1)).Returns(_dummyDBContext.BusDriversForWeek1);

            ActionResult<IEnumerable<BusDriver>> actionResult = _controller.GetBusScheme(weekId);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
        }
        #endregion

        #region EDIT
        [Fact]
        public void Edit_AllInfoValid_EditsBusDriver()
        {
            BusDriverDTO busDriverDTO = new BusDriverDTO()
            {
                DayId = _dummyDBContext.Day1.DayId,
                NewDriverId = 3,
                OriginalDriverId = 1,
                TimeOfDay = TimeOfDay.OCHTEND
            };
            _busDriverRepository.Setup(b => b.GetBusDriverByDayIdDriverIdAndTimeOfDay(busDriverDTO.DayId, busDriverDTO.OriginalDriverId, busDriverDTO.TimeOfDay))
                                .Returns(_dummyDBContext.BusDriver1);
            _driverRepository.Setup(d => d.GetById(busDriverDTO.NewDriverId)).Returns(_dummyDBContext.Driver3);

            ActionResult<BusDriver> actionResult = _controller.Edit(busDriverDTO);
            var response = actionResult?.Result as OkObjectResult;
            BusDriver editedBusDriver = response?.Value as BusDriver;

            Assert.Equal("Makker", editedBusDriver.Driver.Name);
            _busDriverRepository.Verify(a => a.SaveChanges(), Times.Once());
        }

        [Fact]
        public void Edit_WrongDayIdAndWrongOriginalDriverId_ReturnsNotFound()
        {
            BusDriverDTO busDriverDTO = new BusDriverDTO()
            {
                DayId = 69,
                NewDriverId = 3,
                OriginalDriverId = -4,
                TimeOfDay = TimeOfDay.OCHTEND
            };
            _busDriverRepository.Setup(b => b.GetBusDriverByDayIdDriverIdAndTimeOfDay(1, 1, busDriverDTO.TimeOfDay)).Returns(_dummyDBContext.BusDriver1);
            _driverRepository.Setup(d => d.GetById(busDriverDTO.NewDriverId)).Returns(_dummyDBContext.Driver3);

            ActionResult<BusDriver> actionResult = _controller.Edit(busDriverDTO);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _busDriverRepository.Verify(a => a.SaveChanges(), Times.Never());
        }

        [Fact]
        public void Edit_RightDayIdAndOriginalDriverIdButWrongNewDriverId_ReturnsNotFound()
        {
            BusDriverDTO busDriverDTO = new BusDriverDTO()
            {
                DayId = _dummyDBContext.Day1.DayId,
                NewDriverId = -3,
                OriginalDriverId = 1,
                TimeOfDay = TimeOfDay.OCHTEND
            };
            _busDriverRepository.Setup(b => b.GetBusDriverByDayIdDriverIdAndTimeOfDay(busDriverDTO.DayId, busDriverDTO.OriginalDriverId, busDriverDTO.TimeOfDay))
                                .Returns(_dummyDBContext.BusDriver1);
            _driverRepository.Setup(d => d.GetById(3)).Returns(_dummyDBContext.Driver3);

            ActionResult<BusDriver> actionResult = _controller.Edit(busDriverDTO);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _busDriverRepository.Verify(a => a.SaveChanges(), Times.Never());
        }

        [Fact]
        public void Edit_SameOriginalDriverIdAndNewDriverId_ReturnsForbid()
        {
            BusDriverDTO busDriverDTO = new BusDriverDTO()
            {
                DayId = _dummyDBContext.Day1.DayId,
                NewDriverId = 1,
                OriginalDriverId = 1,
                TimeOfDay = TimeOfDay.OCHTEND
            };
            _busDriverRepository.Setup(b => b.GetBusDriverByDayIdDriverIdAndTimeOfDay(busDriverDTO.DayId, busDriverDTO.OriginalDriverId, busDriverDTO.TimeOfDay))
                                .Returns(_dummyDBContext.BusDriver1);
            _driverRepository.Setup(d => d.GetById(3)).Returns(_dummyDBContext.Driver3);

            ActionResult<BusDriver> actionResult = _controller.Edit(busDriverDTO);

            Assert.IsType<ForbidResult>(actionResult?.Result);
            _busDriverRepository.Verify(a => a.SaveChanges(), Times.Never());
        }
        #endregion
    }
}
