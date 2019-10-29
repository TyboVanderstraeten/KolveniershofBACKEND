using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Tests.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace KolveniershofBACKEND.Tests.Controllers
{
    public class ActivitiesControllerTest
    {
        private Mock<IActivityRepository> _activityRepository;

        private DummyDBContext _dummyDBContext;

        public ActivitiesControllerTest()
        {
            _activityRepository = new Mock<IActivityRepository>();
            _dummyDBContext = new DummyDBContext();
        }
    } 
}
