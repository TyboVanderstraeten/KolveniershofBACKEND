using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace KolveniershofBACKEND.Controllers
{
    [ApiController]
    [Route("KolveniershofAPI/[controller]")]
    public class TemplatesController : ControllerBase
    {

        private readonly IDayRepository _dayRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDayActivityRepository _dayActivityRepository;
        private readonly IHelperRepository _helperRepository;

        public TemplatesController(IDayRepository dayRepository, IActivityRepository activityRepository,
            IUserRepository userRepository, IDayActivityRepository dayActivityRepository,
            IHelperRepository helperRepository)
        {
            _dayRepository = dayRepository;
            _activityRepository = activityRepository;
            _userRepository = userRepository;
            _dayActivityRepository = dayActivityRepository;
            _helperRepository = helperRepository;
        }

        /// <summary>
        /// Get all template days
        /// </summary>
        /// <returns>All template days</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Day>> GetAll()
        {
            IEnumerable<Day> days = _dayRepository.GetAll().ToList();
            if (days == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(days);
            }
        }

        /// <summary>
        /// Get all template days from a specific template
        /// </summary>
        /// <param name="templateName">The name of the template</param>
        /// <returns>All template days from a specific template</returns>
        [HttpGet]
        [Route("{templateName}")]
        public ActionResult<IEnumerable<Day>> GetAll(string templateName)
        {
            IEnumerable<Day> days = _dayRepository.GetAllByTemplateName(templateName).ToList();
            if (days == null || !days.Any())
            {
                return NotFound();
            }
            else
            {
                return Ok(days);
            }
        }

        /// <summary>
        /// Get all template days from a specific template and a specific week
        /// </summary>
        /// <param name="templateName">The name of the template</param>
        /// <param name="weekNr">The number of the week</param>
        /// <returns>All template days from a specific template and a specific week</returns>
        [HttpGet]
        [Route("{templateName}/{weekNr}")]
        public ActionResult<IEnumerable<Day>> GetAll(string templateName, int weekNr)
        {
            IEnumerable<Day> days = _dayRepository.GetAllByWeek(templateName, weekNr).ToList();
            if (days == null || !days.Any())
            {
                return NotFound();
            }
            else
            {
                return Ok(days);
            }
        }

        /// <summary>
        /// Get all distinct template names
        /// </summary>
        /// <returns>All distinct template names</returns>
        [HttpGet]
        [Route("templatenames")]
        public ActionResult<IEnumerable<string>> GetAllTemplateNames()
        {
            IEnumerable<string> names = _dayRepository.GetAllTemplateNames().ToList();
            if (names == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(names);
            }
        }

        /// <summary>
        /// Get a template day from a specific template, week and day
        /// </summary>
        /// <param name="templateName">The name of the template</param>
        /// <param name="weekNr">The number of the week</param>
        /// <param name="dayNr">The number of the day</param>
        /// <returns>The template day</returns>
        [HttpGet]
        [Route("{templateName}/{weekNr}/{dayNr}")]
        public ActionResult<Day> GetByWeekAndDay(string templateName, int weekNr, int dayNr)
        {
            Day day = _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
            if (day == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(day);
            }
        }

        ///<summary>
        /// Get all helpers that are not yet helping on a specific template day
        /// </summary>
        /// <param name="templateName">The name of the template</param>
        /// <param name="weekNr">The number of the week</param>
        /// <param name="dayNr">The number of the day</param>
        /// <returns>The helpers that are not yet helping on the template day</returns>
        [HttpGet]
        [Route("{templateName}/{weekNr}/{dayNr}/possiblehelpers")]
        public ActionResult<User> GetPossibleHelpers(string templateName, int weekNr, int dayNr)
        {
            IEnumerable<User> users = _dayRepository.GetPossibleHelpers(templateName, weekNr, dayNr);
            if (users == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(users);
            }
        }

        ///<summary>
        /// Get all day activities that are not yet added to a specific template day
        /// </summary>
        /// <param name="templateName">The name of the template</param>
        /// <param name="weekNr">The number of the week</param>
        /// <param name="dayNr">The number of the day</param>
        /// <param name="timeOfDay">The time of day</param>
        /// <returns>The day activities that are not yet added to the template day</returns>
        [HttpGet]
        [Route("{templateName}/{weekNr}/{dayNr}/{timeOfDay}/possibledayactivities")]
        public ActionResult<Activity> GetPossibleDayActivities(string templateName, int weekNr, int dayNr, TimeOfDay timeOfDay)
        {
            IEnumerable<Activity> activities = _dayRepository.GetPossibleDayActivities(templateName, weekNr, dayNr, timeOfDay);
            if (activities == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(activities);
            }
        }

        /// <summary>
        /// Create a new template day
        /// </summary>
        /// <param name="model">The template day</param>
        /// <returns>The template day</returns>
        [HttpPost]
        public ActionResult<Day> Add(DayDTO model)
        {
            Day day = _dayRepository.GetByWeekAndDay(model.TemplateName, model.WeekNr, model.DayNr);
            if (day != null)
            {
                return BadRequest("A day with this weekNr and dayNr already exists for this template");
            }
            else
            {
                try
                {
                    Day dayToAdd = new Day(model.TemplateName, model.WeekNr, model.DayNr);
                    Activity activitySick = _activityRepository.GetAllIncludingSickAbsent().SingleOrDefault(a => a.ActivityType == ActivityType.ZIEK);
                    Activity activityAbsent = _activityRepository.GetAllIncludingSickAbsent().SingleOrDefault(a => a.ActivityType == ActivityType.AFWEZIG);
                    dayToAdd.AddDayActivity(new DayActivity(dayToAdd, activitySick, TimeOfDay.VOLLEDIG));
                    dayToAdd.AddDayActivity(new DayActivity(dayToAdd, activityAbsent, TimeOfDay.VOLLEDIG));
                    _dayRepository.Add(dayToAdd);
                    _dayRepository.SaveChanges();
                    return Ok(dayToAdd);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        /// <summary>
        /// Remove a template day
        /// </summary>
        /// <param name="templateName">The name of the template</param>
        /// <param name="weekNr">The number of the week</param>
        /// <param name="dayNr">The number of the day</param>
        /// <returns>The template day</returns>
        [HttpDelete]
        [Route("{templateName}/{weekNr}/{dayNr}")]
        public ActionResult<Day> Remove(string templateName, int weekNr, int dayNr)
        {
            Day dayToRemove = _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
            if (dayToRemove == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    _dayRepository.Remove(dayToRemove);
                    _dayRepository.SaveChanges();
                    return dayToRemove;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        /// <summary>
        /// Add an activity to a template day
        /// </summary>
        /// <param name="templateName">The name of the template</param>
        /// <param name="weekNr">The number of the week</param>
        /// <param name="dayNr">The number of the day</param>
        /// <param name="model">The activity</param>
        /// <returns>The activity</returns>
        [HttpPost]
        [Route("{templateName}/{weekNr}/{dayNr}/activity")]
        public ActionResult<DayActivity> AddActivity(string templateName, int weekNr, int dayNr, DayActivityDTO model)
        {
            Day dayToEdit = _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
            Activity activity = _activityRepository.GetById(model.ActivityId);
            DayActivity dayActivityToAdd = new DayActivity(dayToEdit, activity, model.TimeOfDay);
            dayToEdit.AddDayActivity(dayActivityToAdd);
            _dayRepository.SaveChanges();
            return dayActivityToAdd;
        }

        /// <summary>
        /// Remove an activity from a template day
        /// </summary>
        /// <param name="templateName">The name of the template</param>
        /// <param name="weekNr">The number of the week</param>
        /// <param name="dayNr">The number of the day</param>
        /// <param name="activityId">The id of the activity</param>
        /// <param name="timeOfDay">The time of day</param>
        /// <returns>The activity</returns>
        [HttpDelete]
        [Route("{templateName}/{weekNr}/{dayNr}/{timeOfDay}/activity/{activityId}")]
        public ActionResult<DayActivity> RemoveActivity(string templateName, int weekNr, int dayNr, int activityId, TimeOfDay timeOfDay)
        {
            Day dayToEdit = _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
            if (dayToEdit == null)
            {
                return NotFound();
            }
            else
            {
                DayActivity dayActivityToRemove = _dayActivityRepository.GetTemplateDayActivity(templateName, weekNr, dayNr, timeOfDay, activityId);
                if (dayActivityToRemove == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        dayToEdit.RemoveDayActivity(dayActivityToRemove);
                        _dayRepository.SaveChanges();
                        return Ok(dayActivityToRemove);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Add a helper to a template day
        /// </summary>
        /// <param name="templateName">The name of the template</param>
        /// <param name="weekNr">The number of the week</param>
        /// <param name="dayNr">The number of the day</param>
        /// <param name="model">The helper</param>
        /// <returns>The helper</returns>
        [HttpPost]
        [Route("{templateName}/{weekNr}/{dayNr}/helper")]
        public ActionResult<Helper> AddHelper(string templateName, int weekNr, int dayNr, HelperDTO model)
        {
            Day dayToEdit = _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
            if (dayToEdit == null)
            {
                return NotFound();
            }
            else
            {
                User user = _userRepository.GetById(model.UserId);
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        Helper helperToAdd = new Helper(dayToEdit, user);
                        dayToEdit.AddHelper(helperToAdd);
                        _dayRepository.SaveChanges();
                        return Ok(helperToAdd);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Remove a helper from a template day
        /// </summary>
        /// <param name="templateName">The name of the template</param>
        /// <param name="weekNr">The number of the week</param>
        /// <param name="dayNr">The number of the day</param>
        /// <param name="userId">The id of the helper</param>
        /// <returns>The helper</returns>
        [HttpDelete]
        [Route("{templateName}/{weekNr}/{dayNr}/helper/{userId}")]
        public ActionResult<Helper> RemoveHelper(string templateName, int weekNr, int dayNr, int userId)
        {
            Day dayToEdit = _dayRepository.GetByWeekAndDay(templateName, weekNr, dayNr);
            if (dayToEdit == null)
            {
                return NotFound();
            }
            else
            {
                Helper helperToRemove = _helperRepository.GetTemplateDayHelper(templateName, weekNr, dayNr, userId);
                if (helperToRemove == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        dayToEdit.RemoveHelper(helperToRemove);
                        _dayRepository.SaveChanges();
                        return Ok(helperToRemove);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }
    }
}