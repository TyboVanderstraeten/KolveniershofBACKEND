using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Tests.Data;
using System;
using Xunit;

namespace KolveniershofBACKEND.Tests.Models
{
    public class DayActivityTest
    {
        private DummyDBContext _dummyDbContext;
        private DayActivity _dayActivityWithAttendances, _dayActivityWithoutAttendances;
        private Attendance _attendance6 { get; set; }

        public DayActivityTest()
        {
            _dummyDbContext = new DummyDBContext();
            _dayActivityWithAttendances = _dummyDbContext.DayActivity3;
            _dayActivityWithoutAttendances = _dummyDbContext.DayActivity9;
            _attendance6 = _dummyDbContext.Attendance6;
        }

        [Fact]
        public void Attendances_DayActivityWithAttendances_ReturnsLengthOfAttendances()
        {
            Assert.Equal(1, _dayActivityWithAttendances.Attendances.Count);
        }

        [Fact]
        public void Attendances_DayActivityWithoutAttendances_ReturnsZero()
        {
            Assert.Equal(0, _dayActivityWithoutAttendances.Attendances.Count);
        }

        #region AddAttendance
        [Fact]
        public void AddAttendance_DayActivityWithAttendances_ReturnsAttendancesLengthPlusOne()
        {
            var length = _dayActivityWithAttendances.Attendances.Count;

            _dayActivityWithAttendances.AddAttendance(_attendance6);

            Assert.Equal(length + 1, _dayActivityWithAttendances.Attendances.Count);
        }

        [Fact]
        public void AddAttendance_SameAttendanceToDayActivityWithAttendances_ThrowsArgumentException()
        {
            _dayActivityWithAttendances.AddAttendance(_attendance6);

            Assert.Throws<ArgumentException>(() => _dayActivityWithAttendances.AddAttendance(_attendance6));
        }
        #endregion

        #region RemoveWeekendDay
        [Fact]
        public void RemoveAttendance_DayActivityWithAttendances_ReturnsAttendancesLengthMinusOne()
        {
            var length = _dayActivityWithAttendances.Attendances.Count;

            _dayActivityWithAttendances.RemoveAttendance(_dummyDbContext.Attendance3);

            Assert.Equal(length - 1, _dayActivityWithAttendances.Attendances.Count);
        }

        [Fact]
        public void RemoveAttendance_AttendanceNotInDayActivityWithAttendances_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _dayActivityWithAttendances.RemoveAttendance(_attendance6));
        }
        #endregion
    }
}
