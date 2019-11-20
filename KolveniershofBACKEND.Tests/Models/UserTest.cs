using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Tests.Data;
using System;
using Xunit;

namespace KolveniershofBACKEND.Tests.Models
{
    public class UserTest
    {
        private DummyDBContext _dummyDbContext;
        private User userWithoutWeekendDays;
        private User userWithOneWeekendDay;
        private WeekendDay weekendDay1, weekendDay2, weekendDay3;

        public UserTest()
        {
            _dummyDbContext = new DummyDBContext();
            userWithoutWeekendDays = _dummyDbContext.U2;
            userWithOneWeekendDay = _dummyDbContext.U4;
            weekendDay1 = _dummyDbContext.GoingOutWithGirlfriendOn24112019;
            weekendDay2 = _dummyDbContext.PicknickingWithParentsOn23112019;
            weekendDay3 = _dummyDbContext.GamingWithBestFriendOn24112019;
        }

        [Fact]
        public void WeekendDays_UserWithoutWeekendDays_ReturnsZero()
        {
            Assert.Equal(0, userWithoutWeekendDays.WeekendDays.Count);
        }

        [Fact]
        public void UserWithOneWeekendDay_ReturnsOne()
        {
            Assert.Equal(1, userWithOneWeekendDay.WeekendDays.Count);
        }

        #region AddWeekendDay
        [Fact]
        public void AddWeekendDay_UserWithoutWeekenddays_ReturnsOne()
        {
            WeekendDay weekendDay = weekendDay2;
            userWithoutWeekendDays.AddWeekendDay(weekendDay);

            Assert.Equal(1, userWithoutWeekendDays.WeekendDays.Count);
        }

        [Fact]
        public void AddWeekendDay_SameWeekendDayToUserWithOneWeekendDay_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => userWithOneWeekendDay.AddWeekendDay(weekendDay1));
        }

        [Fact]
        public void AddWeekendDay_NewWeekendDayWithExistingDate_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => userWithOneWeekendDay.AddWeekendDay(weekendDay3));
        }
        #endregion

        #region RemoveWeekendDay
        [Fact]
        public void RemoveWeekendDay_UserWithOneWeekendDay_ReturnsZero()
        {
            userWithOneWeekendDay.RemoveWeekendDay(weekendDay1);

            Assert.Equal(0, userWithOneWeekendDay.WeekendDays.Count);
        }

        [Fact]
        public void RemoveWeekendDay_WeekendDayThatsNotInTheListOfUserWithOneWeekendDay_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => userWithOneWeekendDay.RemoveWeekendDay(weekendDay3));
        }
        #endregion
    }
}
