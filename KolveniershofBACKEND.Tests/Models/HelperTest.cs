using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Tests.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KolveniershofBACKEND.Tests.Models
{
    public class HelperTest
    {
        private DummyDBContext _dummyDBContext;

        public HelperTest()
        {
            _dummyDBContext = new DummyDBContext();
        }

        [Fact]
        public void Note_ValidConstuctor_CreatesObject()
        {
            Helper testHelper = _dummyDBContext.Helper5;

            Assert.Equal("eerste_week_derde_dag", testHelper.Day.TemplateName);
            Assert.Equal("Tybo", testHelper.User.FirstName);
        }
    }
}
