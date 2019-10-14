using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Models.Domain
{
    public class DayActivity
    {
        public int DayId { get; set; }
        public int ActivityId { get; set; }
        public Day Day { get; set; }
        public Activity Activity { get; set; }
        public TimeOfDay TimeOfDay { get; set; }

        protected DayActivity()
        {

        }

        public DayActivity(Day day, Activity activity, TimeOfDay timeOfDay)
        {
            DayId = day.DayId;
            ActivityId = activity.ActivityId;
            Day = day;
            Activity = activity;
            TimeOfDay = timeOfDay;
        }
    }
}
