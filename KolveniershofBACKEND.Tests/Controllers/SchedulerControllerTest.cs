using KolveniershofBACKEND.Controllers;
using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Tests.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace KolveniershofBACKEND.Tests.Controllers
{
    public class SchedulerControllerTest
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<ICustomDayRepository> _customDayRepository;
        private Mock<IActivityRepository> _activityRepository;
        private Mock<IDayRepository> _dayRepository;

        private DummyDBContext _dummyDBContext;
        private SchedulerController _controller;
 
        public SchedulerControllerTest()
        {
            _userRepository = new Mock<IUserRepository>();
            _customDayRepository = new Mock<ICustomDayRepository>();
            _activityRepository = new Mock<IActivityRepository>();
            _dayRepository = new Mock<IDayRepository>();
            _dummyDBContext = new DummyDBContext();
            _controller = new SchedulerController(_dayRepository.Object, _customDayRepository.Object, _activityRepository.Object, _userRepository.Object);
        }

    }
}
