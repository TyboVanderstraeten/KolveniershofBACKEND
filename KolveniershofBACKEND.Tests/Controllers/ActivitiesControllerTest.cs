using KolveniershofBACKEND.Controllers;
using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.DTO;
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

        #region Get

        [Fact]
        public void GetAllActivities()
        {
            _activityRepository.Setup(a => a.GetAll()).Returns(_dummyDBContext.Activities);
            ActionResult<IEnumerable<Activity>> actionResult = _controller.GetAll();
            IList<Activity> activitiesInModel = actionResult.Value as IList<Activity>;
            Assert.Equal(7, activitiesInModel.Count);
        }

        [Fact]
        public void GetById_GivesActivity()
        {
            int activityId = 1;
            _activityRepository.Setup(a => a.GetById(activityId)).Returns(_dummyDBContext.Activity1);
            ActionResult<Activity> actionResult = _controller.GetById(activityId);
            Activity activity = actionResult.Value;
            Assert.Equal("Testatelier", activity.Name);
        }

        [Fact]
        public void GetById_GivesNull()
        {
            int activityId = 8;
            _activityRepository.Setup(a => a.GetById(activityId)).Returns((Activity)null);
            ActionResult<Activity> actionResult = _controller.GetById(activityId);
            Assert.Null(actionResult.Value);
        }
        #endregion

        [Fact]
        public void AddActivity_Succeeds()
        {

            ActivityDTO activityDTO = new ActivityDTO()
            {
                ActivityType = ActivityType.ATELIER,
                Name = "Zwemmen",
                Description = "Samen met de vriendengroep gaan zwemmen in het stedelijk zwembad",
                Pictogram = null
            };
            
            ActionResult<Activity> actionResult = _controller.Add(activityDTO);
            CreatedAtActionResult actionResult2 = actionResult.Result as CreatedAtActionResult; //good? --> not isolated?
            Activity activity = actionResult2.Value as Activity;
            Assert.Equal("GetById", actionResult2.ActionName);
            Assert.Equal("Zwemmen", activity.Name);
        }

        //when validation is added --> new test for badrequest!


        [Fact]
        public void EditActivity_Succeeds()
        {
            ActivityDTO activityDTO = new ActivityDTO()
            {
                ActivityId = 2,
                ActivityType = ActivityType.ATELIER,
                Name = "Zwemmen",
                Description = "",
                Pictogram = null
            };
            _activityRepository.Setup(m => m.GetById(activityDTO.ActivityId)).Returns(_dummyDBContext.Activity2);

            ActionResult<Activity> actionResult = _controller.Edit(activityDTO);
            OkObjectResult actionResult2 = actionResult.Result as OkObjectResult;
            Activity activity = actionResult2.Value as Activity;

            Assert.Equal("Zwemmen", activity.Name);
            Assert.Equal("", activity.Description);

        }

    }
}
