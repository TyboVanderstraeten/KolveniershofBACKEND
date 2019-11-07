using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KolveniershofBACKEND.Controllers
{
    [ApiController]
    [Route("KolveniershofAPI/[controller]")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityRepository _activityRepository;

        public ActivitiesController(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        /// <summary>
        /// Get all activities
        /// </summary>
        /// <returns>All activities</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Activity>> GetAll()
        {
            return _activityRepository.GetAll().ToList();
        }

        /// <summary>
        /// Get a specific activity
        /// </summary>
        /// <param name="id">The id of the activity</param>
        /// <returns>The activity</returns>
        [HttpGet]
        [Route("{id}")]
        public ActionResult<Activity> GetById(int id)
        {
            return _activityRepository.GetById(id);
        }

        [HttpPost]
        public ActionResult<Activity> Add(ActivityDTO model)
        {
            try
            {
                Activity activityToCreate = new Activity(
                    model.ActivityType,
                    model.Name,
                    model.Description,
                    model.Pictogram);

                _activityRepository.Add(activityToCreate);
                _activityRepository.SaveChanges();
                return CreatedAtAction(nameof(GetById), new { id = activityToCreate.ActivityId }, activityToCreate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public ActionResult<Activity> Edit(ActivityDTO model)
        {
            Activity activityToEdit = _activityRepository.GetById(model.ActivityId);
            activityToEdit.ActivityType = model.ActivityType;
            activityToEdit.Name = model.Name;
            activityToEdit.Description = model.Description;
            activityToEdit.Pictogram = model.Pictogram;
            _activityRepository.SaveChanges();
            return Ok(activityToEdit);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult<Activity> Remove(int id)
        {
            Activity activityToDelete = _activityRepository.GetById(id);
            if (activityToDelete == null)
            {
                return NoContent();
            }
            else
            {
                _activityRepository.Remove(activityToDelete);
                _activityRepository.SaveChanges();
                return Ok(activityToDelete);
            }
        }
    }
}
