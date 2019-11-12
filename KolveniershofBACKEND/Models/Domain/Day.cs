using System;
using System.Collections.Generic;
using System.Linq;

namespace KolveniershofBACKEND.Models.Domain
{
    public class Day
    {
        public int DayId { get; set; }
        public string TemplateName { get; set; }
        public int WeekNr { get; set; }
        public int DayNr { get; set; }
        public string DayName {
            get {
                switch (DayNr)
                {
                    case 1: return "Maandag";
                    case 2: return "Dinsdag";
                    case 3: return "Woensdag";
                    case 4: return "Donderdag";
                    case 5: return "Vrijdag";
                    default: return "Invalid";
                }
            }
        }
        public ICollection<DayActivity> DayActivities { get; set; }
        public ICollection<Helper> Helpers { get; set; }

        protected Day()
        {
            DayActivities = new List<DayActivity>();
            Helpers = new List<Helper>();
        }

        public Day(string templateName, int weekNr, int dayNr)
        {
            TemplateName = templateName;
            WeekNr = weekNr;
            DayNr = dayNr;
            DayActivities = new List<DayActivity>();
            Helpers = new List<Helper>();
        }

        public void AddHelper(Helper helper)
        {
            if (Helpers.SingleOrDefault(h => h.DayId == helper.DayId && h.UserId == helper.UserId) == null)
            {
                Helpers.Add(helper);
            }
            else
            {
                throw new ArgumentException("Helper already exists");
            }
        }

        public void RemoveHelper(Helper helper)
        {
            if (Helpers.SingleOrDefault(h => h.DayId == helper.DayId && h.UserId == helper.UserId) != null)
            {
                Helpers.Remove(helper);
            }
            else
            {
                throw new ArgumentException("Helper doesn't exist");
            }
        }

        public void AddDayActivity(DayActivity dayActivity)
        {
            if (DayActivities.SingleOrDefault(da => da.DayId == dayActivity.DayId && da.ActivityId == dayActivity.ActivityId) == null)
            {
                DayActivities.Add(dayActivity);
            }
            else
            {
                throw new ArgumentException("DayActivity already exists");
            }
        }

        public void RemoveDayActivity(DayActivity dayActivity)
        {
            if (DayActivities.SingleOrDefault(da => da.DayId == dayActivity.DayId && da.ActivityId == dayActivity.ActivityId) != null)
            {
                DayActivities.Remove(dayActivity);
            }
            else
            {
                throw new ArgumentException("DayActivity doesn't exist");
            }
        }
    }
}
