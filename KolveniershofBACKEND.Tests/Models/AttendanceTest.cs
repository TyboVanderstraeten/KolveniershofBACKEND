using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Tests.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KolveniershofBACKEND.Tests.Models
{
    public class AttendanceTest
    {
        private DummyDBContext _dummyDBContext;

        public AttendanceTest()
        {
            _dummyDBContext = new DummyDBContext();
        }

        [Fact]
        public void Note_ValidConstuctor_CreatesObject()
        {
            Attendance testAttendance = new Attendance(_dummyDBContext.DayActivity10, _dummyDBContext.U3);

            Assert.Equal("Tim", testAttendance.User.FirstName);
            Assert.Equal(TimeOfDay.NAMIDDAG, testAttendance.DayActivity.TimeOfDay);
        }
    }
}
