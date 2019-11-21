using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.Domain.Enums;
using KolveniershofBACKEND.Tests.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KolveniershofBACKEND.Tests.Models
{
    public class BusDriverTest
    {
        private DummyDBContext _dummyDbContext;

        public BusDriverTest()
        {
            _dummyDbContext = new DummyDBContext();
        }

        [Fact]
        public void BusDriver_ValidConstuctor_CreatesObject()
        {
            BusDriver testBusDriver = _dummyDbContext.BusDriver2;

            Assert.Equal(BusColor.GEEL, testBusDriver.BusColor);
            Assert.Equal("Karel", testBusDriver.Driver.Name);
            Assert.Equal("eerste_week_tweede_dag", testBusDriver.Day.TemplateName);
        }
    }
}
