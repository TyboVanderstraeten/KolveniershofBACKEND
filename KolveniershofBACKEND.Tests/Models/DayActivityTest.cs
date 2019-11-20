using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Tests.Data;
using System;
using Xunit;

namespace KolveniershofBACKEND.Tests.Models
{
    public class DayActivityTest
    {
        private DummyDBContext _dummyDbContext;
        public DayActivity DayActivityWithAttendances, DayActivityWithoutAttendances;
        public Attendance Attendance6 { get; set; }

        public DayActivityTest()
        {
            _dummyDbContext = new DummyDBContext();
            DayActivityWithAttendances = _dummyDbContext.DayActivity3;
            DayActivityWithoutAttendances = _dummyDbContext.DayActivity9;
            Attendance6 = _dummyDbContext.Attendance6;
        }

        [Fact]
        public void Attendances_DayActivityWithAttendances_ReturnsLengthOfAttendances()
        {
            Assert.Equal(1, DayActivityWithAttendances.Attendances.Count);
        }

        [Fact]
        public void Attendances_DayActivityWithoutAttendances_ReturnsZero()
        {
            Assert.Equal(0, DayActivityWithoutAttendances.Attendances.Count);
        }

        #region AddAttendance
        [Fact]
        public void AddAttendance_DayActivityWithAttendances_ReturnsAttendancesLengthPlusOne()
        {
            var length = DayActivityWithAttendances.Attendances.Count;

            DayActivityWithAttendances.AddAttendance(Attendance6);

            Assert.Equal(length + 1, DayActivityWithAttendances.Attendances.Count);
        }

        [Fact]
        public void AddAttendance_SameAttendanceToDayActivityWithAttendances_ThrowsArgumentException()
        {
            DayActivityWithAttendances.AddAttendance(Attendance6);

            Assert.Throws<ArgumentException>(() => DayActivityWithAttendances.AddAttendance(Attendance6));
        }
        #endregion

        #region RemoveWeekendDay
        [Fact]
        public void RemoveAttendance_AttendanceNotInDayActivityWithAttendances_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => DayActivityWithAttendances.RemoveAttendance(Attendance6));
        }
        #endregion
    }
}
