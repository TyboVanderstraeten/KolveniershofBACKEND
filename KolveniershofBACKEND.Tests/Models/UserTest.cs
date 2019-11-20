using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Tests.Data;
using System;
using Xunit;

namespace KolveniershofBACKEND.Tests.Models
{
    public class UserTest
    {
        private DummyDBContext _dummyDbContext;
        private User _userWithoutWeekendDays;
        private User _userWithOneWeekendDay;
        private WeekendDay _weekendDay1, _weekendDay2, _weekendDay3;

        public UserTest()
        {
            _dummyDbContext = new DummyDBContext();
            _userWithoutWeekendDays = _dummyDbContext.U2;
            _userWithOneWeekendDay = _dummyDbContext.U4;
            _weekendDay1 = _dummyDbContext.GoingOutWithGirlfriendOn24112019;
            _weekendDay2 = _dummyDbContext.PicknickingWithParentsOn23112019;
            _weekendDay3 = _dummyDbContext.GamingWithBestFriendOn24112019;
        }

        [Fact]
        public void WeekendDays_UserWithoutWeekendDays_ReturnsZero()
        {
            Assert.Equal(0, _userWithoutWeekendDays.WeekendDays.Count);
        }

        [Fact]
        public void UserWithOneWeekendDay_ReturnsOne()
        {
            Assert.Equal(1, _userWithOneWeekendDay.WeekendDays.Count);
        }

        #region AddWeekendDay
        [Fact]
        public void AddWeekendDay_UserWithoutWeekenddays_ReturnsOne()
        {
            WeekendDay weekendDay = _weekendDay2;
            _userWithoutWeekendDays.AddWeekendDay(weekendDay);

            Assert.Equal(1, _userWithoutWeekendDays.WeekendDays.Count);
        }

        [Fact]
        public void AddWeekendDay_SameWeekendDayToUserWithOneWeekendDay_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _userWithOneWeekendDay.AddWeekendDay(_weekendDay1));
        }

        [Fact]
        public void AddWeekendDay_NewWeekendDayWithExistingDate_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _userWithOneWeekendDay.AddWeekendDay(_weekendDay3));
        }
        #endregion

        #region RemoveWeekendDay
        [Fact]
        public void RemoveWeekendDay_UserWithOneWeekendDay_ReturnsZero()
        {
            _userWithOneWeekendDay.RemoveWeekendDay(_weekendDay1);

            Assert.Equal(0, _userWithOneWeekendDay.WeekendDays.Count);
        }

        [Fact]
        public void RemoveWeekendDay_WeekendDayThatsNotInTheListOfUserWithOneWeekendDay_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _userWithOneWeekendDay.RemoveWeekendDay(_weekendDay3));
        }
        #endregion
    }
}
