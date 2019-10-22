using System.Collections.Generic;

namespace KolveniershofBACKEND.Models.Domain
{
    public class Day
    {
        public int DayId { get; set; }
        public int WeekNr { get; set; }
        public int DayNr { get; set; }
        public ICollection<DayActivity> DayActivities { get; set; }
        public ICollection<Helper> Helpers { get; set; }

        protected Day()
        {
            DayActivities = new List<DayActivity>();
            Helpers = new List<Helper>();
        }

        public Day(int weekNr, int dayNr)
        {
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
    }
}
