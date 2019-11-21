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
        public void GetAllActivities_Succeeds()
        {
            _activityRepository.Setup(a => a.GetAll()).Returns(_dummyDBContext.Activities);

            ActionResult<IEnumerable<Activity>> actionResult = _controller.GetAll();
            var response = actionResult?.Result as OkObjectResult;
            List<Activity> activities = response?.Value as List<Activity>;

            Assert.Equal(200, response?.StatusCode);
            Assert.Equal(7, activities.Count);
        }

        [Fact]
        public void GetById_GivesActivity()
        {
            int activityId = 1;
            _activityRepository.Setup(a => a.GetById(activityId)).Returns(_dummyDBContext.Activity1);

            ActionResult<Activity> actionResult = _controller.GetById(activityId);
            var response = actionResult?.Result as OkObjectResult;
            Activity activityThatCameBackFromResponse = response?.Value as Activity;

            Assert.Equal("Testatelier", activityThatCameBackFromResponse.Name);
        }

        [Fact]
        public void GetById_NotExistingId_ReturnsNotFound()
        {
            int activityId = 8;
            _activityRepository.Setup(a => a.GetById(activityId)).Returns((Activity)null);

            ActionResult<Activity> actionResult = _controller.GetById(activityId);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
        #endregion

        #region Add
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
            var response = actionResult.Result as OkObjectResult;
            Activity newActivity = response?.Value as Activity;

            Assert.Equal(200, response?.StatusCode);
            Assert.Equal("Zwemmen", newActivity.Name);
            _activityRepository.Verify(a => a.Add(It.IsAny<Activity>()), Times.Once());
            _activityRepository.Verify(a => a.SaveChanges(), Times.Once());
        }

        /*[Fact]
        public void AddActivity_Fails_NameIsNull()
         {
             ActivityDTO activityDTO = new ActivityDTO()
             {
                 ActivityType = ActivityType.ATELIER,
                 Name = null,
                 Description = "Samen met de vriendengroep gaan zwemmen in het stedelijk zwembad",
                 Pictogram = null
             };

             ActionResult<Activity> actionResult = _controller.Add(activityDTO);
             Assert.IsType<BadRequestResult>(actionResult.Result);
             _activityRepository.Verify(a => a.Add(It.IsAny<Activity>()), Times.Never());
             _activityRepository.Verify(a => a.SaveChanges(), Times.Never());
         }*/



        #endregion

        #region Edit
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
            _activityRepository.Verify(a => a.SaveChanges(), Times.Once());
        }

        [Fact]
        public void EditActivity_NonExistingActivityId_ReturnsNotFound()
        {
            ActivityDTO activityDTO = new ActivityDTO()
            {
                ActivityId = 66,
                ActivityType = ActivityType.ATELIER,
                Name = "Zwemmen",
                Description = "",
                Pictogram = null
            };
            
            ActionResult<Activity> actionResult = _controller.Edit(activityDTO);

            Assert.IsType<NotFoundResult>(actionResult?.Result);
            _activityRepository.Verify(a => a.SaveChanges(), Times.Never());
        }

        /*[Fact]
        public void EditActivity_Fails_NameIsNull()
        {
            ActivityDTO activityDTO = new ActivityDTO()
            {
                ActivityId = 2,
                ActivityType = ActivityType.ATELIER,
                Name = null,
                Description = "",
                Pictogram = null
            };
            _activityRepository.Setup(m => m.GetById(activityDTO.ActivityId)).Returns(_dummyDBContext.Activity2);

            ActionResult<Activity> actionResult = _controller.Edit(activityDTO);
            Assert.IsType<BadRequestResult>(actionResult.Result);
          
            _activityRepository.Verify(a => a.SaveChanges(), Times.Never());
        }*/
        #endregion

        #region Remove
        [Fact]
        public void RemoveActivity_Succeeds()
        {

            int activityId = 1;
            _activityRepository.Setup(a => a.GetById(activityId)).Returns(_dummyDBContext.Activity1);

            ActionResult<Activity> actionResult = _controller.Remove(activityId);
            OkObjectResult okObjectResult = actionResult.Result as OkObjectResult;
            Activity activity = okObjectResult.Value as Activity;
            Assert.Equal("Testatelier", activity.Name);
            Assert.NotNull(activity);
            _activityRepository.Verify(a => a.Remove(It.IsAny<Activity>()), Times.Once());
            _activityRepository.Verify(a => a.SaveChanges(), Times.Once());
        }

        [Fact]
        public void RemoveActivity_Fails_AcitivyDoesntExist()
        {
            int activityId = 100;
            _activityRepository.Setup(a => a.GetById(activityId)).Returns((Activity)null);

            ActionResult<Activity> actionResult = _controller.Remove(activityId);
            Assert.IsType<NotFoundResult>(actionResult.Result);
            _activityRepository.Verify(a => a.Remove(It.IsAny<Activity>()), Times.Never());
            _activityRepository.Verify(a => a.SaveChanges(), Times.Never());

        } 
        #endregion

    }
}
