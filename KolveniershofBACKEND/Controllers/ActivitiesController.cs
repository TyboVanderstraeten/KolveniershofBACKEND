using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpGet]
        public ActionResult<IEnumerable<Activity>> GetAll()
        {
            return _activityRepository.GetAll().ToList();
        }

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
                    (ActivityType)model.ActivityType,
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
