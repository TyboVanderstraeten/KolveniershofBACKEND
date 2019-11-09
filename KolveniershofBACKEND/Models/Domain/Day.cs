using Newtonsoft.Json;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Models.Domain
{
    public class Day
    {
        public int DayId { get; set; }
        public string TemplateName { get; set; }
        public int WeekNr { get; set; }
        public int DayNr { get; set; }
        public ICollection<DayActivity> DayActivities { get; set; }
        public ICollection<Helper> Helpers { get; set; }
        [JsonIgnore]
        public ICollection<BusDriver> BusDrivers { get; set; }

        protected Day()
        {
            DayActivities = new List<DayActivity>();
            Helpers = new List<Helper>();
            BusDrivers = new List<BusDriver>();
        }

        public Day(string templateName, int weekNr, int dayNr)
        {
            TemplateName = templateName;
            WeekNr = weekNr;
            DayNr = dayNr;
            DayActivities = new List<DayActivity>();
            Helpers = new List<Helper>();
            BusDrivers = new List<BusDriver>();
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

        public void AddBusDriver(BusDriver busDriver)
        {
            BusDrivers.Add(busDriver);
        }

        public void RemoveBusDriver(BusDriver busDriver)
        {
            BusDrivers.Remove(busDriver);
        }
    }
}
