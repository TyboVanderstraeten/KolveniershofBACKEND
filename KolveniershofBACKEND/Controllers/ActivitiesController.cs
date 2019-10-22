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
        public ActionResult<IEnumerable<ActivityDTO>> GetAll()
        {
            return _activityRepository.GetAll().Select(a => new ActivityDTO(a)).ToList();
        }

        [HttpGet]
        [Route("{Id}")]
        public ActionResult<ActivityDTO> GetById(int id)
        {
            return new ActivityDTO(_activityRepository.GetById(id));
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

        [HttpDelete]
        [Route("{Id}")]
        public ActionResult Remove(int id)
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
