using KolveniershofBACKEND.Controllers;
using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace KolveniershofBACKEND.Tests.Controllers
{
    public class ActivitiesControllerTest
    {
        private Mock<IActivityRepository> _activityRepository;
        private ActivitiesController _controller;
        private DummyDBContext _dummyDBContext;

        public ActivitiesControllerTest()
        {
            _activityRepository = new Mock<IActivityRepository>();
            _controller = new ActivitiesController(_activityRepository.Object);
            _dummyDBContext = new DummyDBContext();
        }
        #region Index

        [Fact]
        public void Index_GetAllActivities()
        {
            _activityRepository.Setup(a => a.GetAll()).Returns(_dummyDBContext.Activities);
            ActionResult<IEnumerable<Activity>> actionResult = _controller.GetAll();
            IList<Activity> activitiesInModel = actionResult.Value as IList<Activity>;
            Assert.Equal(7, activitiesInModel.Count);
        }

        [Fact]
        public void Index_GetById_GivesActivity()
        {
            int activityId = 1;
            _activityRepository.Setup(a => a.GetById(activityId)).Returns(_dummyDBContext.Activity1);
            ActionResult<Activity> actionResult = _controller.GetById(activityId);
            Activity activity = actionResult.Value;
            Assert.Equal("Testatelier", activity.Name);
        }

        [Fact]
        public void Index_GetById_GivesNull()
        {
            int activityId = 8;
            _activityRepository.Setup(a => a.GetById(activityId)).Returns((Activity)null);
            ActionResult<Activity> actionResult = _controller.GetById(activityId);
            Assert.Null(actionResult.Value);
        } 
        #endregion
    } 
}
