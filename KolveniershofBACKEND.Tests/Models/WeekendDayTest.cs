using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Tests.Data;
using System;
using Xunit;

namespace KolveniershofBACKEND.Tests.Models
{
    public class WeekendDayTest
    {
        private DummyDBContext _dummyDBContext;

        public WeekendDayTest()
        {
            _dummyDBContext = new DummyDBContext();
        }

        [Fact]
        public void WeekendDay_ValidConstructor_CreatesObject()
        {
            WeekendDay testWeekendDay = _dummyDBContext.GamingWithBestFriendOn24112019;

            Assert.Equal("gamen met beste vriend", testWeekendDay.Comment);
            Assert.Equal(new DateTime(2019, 11, 24), testWeekendDay.Date);
        }
    }
}
