using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Tests.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KolveniershofBACKEND.Tests.Models
{
    public class DriverTest
    {
        private DummyDBContext _dummyDbContext;

        public DriverTest()
        {
            _dummyDbContext = new DummyDBContext();
        }

        [Fact]
        public void Driver_ValidConstuctor_CreatesObject()
        {
            Driver testDriver = _dummyDbContext.Driver2;

            Assert.Equal("Karel", testDriver.Name);
        }
    }
}
