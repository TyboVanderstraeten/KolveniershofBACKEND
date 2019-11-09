using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Models.Domain
{
    public class BusDriver
    {
        [JsonIgnore]
        public Day Day { get; set; }
        public int DayId { get; set; }

        [JsonIgnore]
        public Driver Driver { get; set; }
        public int DriverId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TimeOfDay TimeOfDay { get; set; }

        protected BusDriver()
        {

        }

        public BusDriver(Day day, Driver driver, TimeOfDay timeOfDay)
        {
            Day = day;
            DayId = day.DayId;
            Driver = driver;
            DriverId = driver.DriverId;
            TimeOfDay = timeOfDay;
        }
    }
}
