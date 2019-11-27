using KolveniershofBACKEND.Data.Repositories.Interfaces;
using KolveniershofBACKEND.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace KolveniershofBACKEND.Data.Repositories.Concrete
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly DBContext _dbContext;
        private readonly DbSet<Activity> _activities;

        public ActivityRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
            _activities = dbContext.Activities;
        }

        public IEnumerable<Activity> GetAll()
        {
            return _activities.Where(a => !(a.ActivityType.Equals(ActivityType.AFWEZIG) || a.ActivityType.Equals(ActivityType.ZIEK)))
                .OrderBy(a => a.Name).ThenBy(a => a.ActivityType).ToList();
        }

        public IEnumerable<string> GetAllPictograms()
        {
            return _activities.Select(a => a.Pictogram).Distinct().ToList();
        }

        public Activity GetById(int id)
        {
            return _activities.SingleOrDefault(a => a.ActivityId == id);
        }

        public void Add(Activity activity)
        {
            _activities.Add(activity);
        }

        public void Remove(Activity activity)
        {
            _activities.Remove(activity);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }


    }
}
