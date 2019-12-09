using KolveniershofBACKEND.Models.Domain;
using Xunit;

namespace KolveniershofBACKEND.Tests.Models
{
    public class ActivityTest
    {
        [Fact]
        public void Activity_ValidConstuctor_CreatesObject()
        {
            Activity testActivity = new Activity(ActivityType.EVENEMENT, "testNaam", "testDescriptie", "picto.jpeg");

            Assert.Equal(ActivityType.EVENEMENT, testActivity.ActivityType);
            Assert.Equal("testNaam", testActivity.Name);
            Assert.Equal("testDescriptie", testActivity.Description);
            Assert.Equal("picto.jpeg", testActivity.Pictogram);
        }
    }
}
