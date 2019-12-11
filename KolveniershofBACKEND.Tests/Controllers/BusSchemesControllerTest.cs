using KolveniershofBACKEND.Controllers;
using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.Domain.Enums;
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
        private Mock<ICustomDayRepository> _customDayRepository;
        private DummyDBContext _dummyDBContext;
        private BusSchemesController _controller;

        public BusSchemesControllerTest()
        {
            _busDriverRepository = new Mock<IBusDriverRepository>();
            _driverRepository = new Mock<IDriverRepository>();
            _customDayRepository = new Mock<ICustomDayRepository>();
            _controller = new BusSchemesController(_busDriverRepository.Object, _driverRepository.Object, _customDayRepository.Object);
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
                DriverId = 3,
                BusColor = BusColor.BEIGE,
                TimeOfDay = TimeOfDay.OCHTEND
            };
            _busDriverRepository.Setup(b => b.GetBusDriverByDayIdBusColorAndTimeOfDay(busDriverDTO.DayId, busDriverDTO.BusColor, busDriverDTO.TimeOfDay))
                                .Returns(_dummyDBContext.BusDriver1);
            _driverRepository.Setup(d => d.GetById(busDriverDTO.DriverId)).Returns(_dummyDBContext.Driver3);

            ActionResult<BusDriver> actionResult = _controller.Edit(busDriverDTO);
            var response = actionResult?.Result as OkObjectResult;
            BusDriver editedBusDriver = response?.Value as BusDriver;

            Assert.Equal("Makker", editedBusDriver.Driver.Name);
            _busDriverRepository.Verify(a => a.SaveChanges(), Times.Once());
        }

        [Fact]
        public void Edit_WrongDayIdAndWrongDriverId_ReturnsNotFound()
        {
            BusDriverDTO busDriverDTO = new BusDriverDTO()
            {
                DayId = 69,
                DriverId = 30,
                BusColor = BusColor.BEIGE,
                TimeOfDay = TimeOfDay.OCHTEND
            };
            _busDriverRepository.Setup(b => b.GetBusDriverByDayIdBusColorAndTimeOfDay(1, BusColor.BEIGE, busDriverDTO.TimeOfDay)).Returns(_dummyDBContext.BusDriver1);
            _driverRepository.Setup(d => d.GetById(3)).Returns(_dummyDBContext.Driver3);

            ActionResult<BusDriver> actionResult = _controller.Edit(busDriverDTO);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _busDriverRepository.Verify(a => a.SaveChanges(), Times.Never());
        }

        [Fact]
        public void Edit_RightDayIdButWrongNewDriverId_ReturnsNotFound()
        {
            BusDriverDTO busDriverDTO = new BusDriverDTO()
            {
                DayId = _dummyDBContext.Day1.DayId,
                DriverId = -3,
                BusColor = BusColor.BLAUW,
                TimeOfDay = TimeOfDay.OCHTEND
            };
            _busDriverRepository.Setup(b => b.GetBusDriverByDayIdBusColorAndTimeOfDay(busDriverDTO.DayId, BusColor.BLAUW, busDriverDTO.TimeOfDay))
                                .Returns(_dummyDBContext.BusDriver1);
            _driverRepository.Setup(d => d.GetById(3)).Returns(_dummyDBContext.Driver3);

            ActionResult<BusDriver> actionResult = _controller.Edit(busDriverDTO);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _busDriverRepository.Verify(a => a.SaveChanges(), Times.Never());
        }
        #endregion
    }
}
