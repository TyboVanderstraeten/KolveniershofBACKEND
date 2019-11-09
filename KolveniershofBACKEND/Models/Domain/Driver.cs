using KolveniershofBACKEND.Models.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Models.Domain
{
    public class Driver
    {
        public int DriverId { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public BusColor BusColor { get; set; }
        public ICollection<BusDriver> DaysToDrive { get; set; }

        public Driver(string name, BusColor busColor)
        {
            Name = name;
            BusColor = busColor;
            DaysToDrive = new List<BusDriver>();
        }

        protected Driver()
        {
            DaysToDrive = new List<BusDriver>();
        }
    }
}
