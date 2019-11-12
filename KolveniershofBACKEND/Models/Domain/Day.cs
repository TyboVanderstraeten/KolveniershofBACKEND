using System.Collections.Generic;

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
            Helpers.Add(helper);
        }

        public void RemoveHelper(Helper helper)
        {
            Helpers.Remove(helper);
        }

        public void AddDayActivity(DayActivity dayActivity)
        {
            DayActivities.Add(dayActivity);
        }

        public void RemoveDayActivity(DayActivity dayActivity)
        {
            DayActivities.Remove(dayActivity);
        }
    }
}
