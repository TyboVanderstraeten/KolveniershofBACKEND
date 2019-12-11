using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Tests.Data;
using System;
using Xunit;

namespace KolveniershofBACKEND.Tests.Models
{
    public class DayTest
    {
        private DummyDBContext _dummyDBContext;
        private Day _dayWithNoDayActivitesAndNoHelpers, _dayWithHelpersAndNoDayActivities;
        private Day _dayWithDayActivitesAndNoHelpers, _dayWithHelpersAndDayActivities;

        public DayTest()
        {
            _dummyDBContext = new DummyDBContext();
            _dayWithHelpersAndDayActivities = _dummyDBContext.Day1;
            _dayWithHelpersAndNoDayActivities = _dummyDBContext.Day4;
            _dayWithDayActivitesAndNoHelpers = _dummyDBContext.Day5;
            _dayWithNoDayActivitesAndNoHelpers = _dummyDBContext.Day6;
        }

        #region Constructor
        [Fact]
        public void HelpersAndDayActivities_DayWithHelpersAndDayActivities_ReturnsHelpersAndDayActivitiesLength()
        {
            Assert.Equal(4, _dayWithHelpersAndDayActivities.DayActivities.Count);
            Assert.Equal(2, _dayWithHelpersAndDayActivities.Helpers.Count);
        }

        [Fact]
        public void HelpersAndDayActivities_DayWithNoHelpersAndDayActivities_ReturnsZeroAndDayActivitesLength()
        {
            Assert.Equal(4, _dayWithDayActivitesAndNoHelpers.DayActivities.Count);
            Assert.Empty(_dayWithDayActivitesAndNoHelpers.Helpers);
        }

        [Fact]
        public void HelpersAndDayActivities_DayWithHelpersAndNoDayActivities_ReturnsHelpersLengthAndZero()
        {
            Assert.Empty(_dayWithHelpersAndNoDayActivities.DayActivities);
            Assert.Equal(2, _dayWithHelpersAndNoDayActivities.Helpers.Count);
        }

        [Fact]
        public void HelpersAndDayActivities_DayWithNoHelpersAndNoDayActivities_ReturnsZeroAndZero()
        {
            Assert.Empty(_dayWithNoDayActivitesAndNoHelpers.DayActivities);
            Assert.Empty(_dayWithNoDayActivitesAndNoHelpers.Helpers);
        }
        #endregion

        #region AddHelper
        [Fact]
        public void AddHelper_DayWithNoDayActivitesAndNoHelpers_AddsNewHelper()
        {
            _dayWithNoDayActivitesAndNoHelpers.AddHelper(_dummyDBContext.Helper1);

            Assert.NotEmpty(_dayWithNoDayActivitesAndNoHelpers.Helpers);
            Assert.Equal(1, _dayWithNoDayActivitesAndNoHelpers.Helpers.Count);
        }

        [Fact]
        public void AddHelper_AddExistingHelperToDayWithNoDayActivitesAndNoHelpers_ThrowsArgumentException()
        {
            _dayWithNoDayActivitesAndNoHelpers.AddHelper(_dummyDBContext.Helper1);

            Assert.Throws<ArgumentException>(() => _dayWithNoDayActivitesAndNoHelpers.AddHelper(_dummyDBContext.Helper1));
        }
        #endregion

        #region RemoveHelper
        [Fact]
        public void RemoveHelper_DayWithHelpersAndActivites_RemovesHelper()
        {
            _dayWithHelpersAndDayActivities.RemoveHelper(_dummyDBContext.Helper2);

            Assert.NotEmpty(_dayWithHelpersAndDayActivities.Helpers);
        }

        [Fact]
        public void RemoveHelper_RemoveHelperNotInListOfDayWithHelpersAndActivites_ThrowsArgumentException()
        {
            _dayWithHelpersAndDayActivities.RemoveHelper(_dummyDBContext.Helper2);

            Assert.Throws<ArgumentException>(() => _dayWithHelpersAndDayActivities.RemoveHelper(_dummyDBContext.Helper2));
        }
        #endregion

        #region AddDayActivity
        [Fact]
        public void DayActivity_DayWithNoDayActivitesAndNoHelpers_AddsDayActivity()
        {
            var length = _dayWithNoDayActivitesAndNoHelpers.DayActivities.Count;
            _dayWithNoDayActivitesAndNoHelpers.AddDayActivity(_dummyDBContext.DayActivity12);
            _dayWithNoDayActivitesAndNoHelpers.AddDayActivity(_dummyDBContext.DayActivity10);
            _dayWithNoDayActivitesAndNoHelpers.AddDayActivity(_dummyDBContext.DayActivity11);

            Assert.NotEmpty(_dayWithNoDayActivitesAndNoHelpers.DayActivities);
            Assert.Equal(length + 3, _dayWithNoDayActivitesAndNoHelpers.DayActivities.Count);
        }

        [Fact]
        public void DayActivity_AddExistingDayActivityToDayWithNoDayActivitesAndNoHelpers_ThrowsArgumentException()
        {
            _dayWithNoDayActivitesAndNoHelpers.AddDayActivity(_dummyDBContext.DayActivity12);

            Assert.Throws<ArgumentException>(() => _dayWithNoDayActivitesAndNoHelpers.AddDayActivity(_dummyDBContext.DayActivity12));
        }
        #endregion

        #region RemoveDayActivity
        [Fact]
        public void RemoveDayActivity_DayWithHelpersAndActivites_RemovesRemoveDayActivity()
        {
            var length = _dayWithHelpersAndDayActivities.DayActivities.Count;
            _dayWithHelpersAndDayActivities.RemoveDayActivity(_dummyDBContext.DayActivity3);

            Assert.Equal(length - 1 ,_dayWithHelpersAndDayActivities.DayActivities.Count);
        }

        [Fact]
        public void RemoveDayActivity_RemoveHelperNotInListOfDayWithHelpersAndActivites_ThrowsArgumentException()
        {
            _dayWithHelpersAndDayActivities.RemoveDayActivity(_dummyDBContext.DayActivity3);

            Assert.Throws<ArgumentException>(() => _dayWithHelpersAndDayActivities.RemoveDayActivity(_dummyDBContext.DayActivity3));
        }
        #endregion
    }
}
