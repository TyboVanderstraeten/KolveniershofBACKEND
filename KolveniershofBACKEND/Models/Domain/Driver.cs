using Newtonsoft.Json;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Models.Domain
{
    public class Driver
    {
        public int DriverId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<BusDriver> DaysToDrive { get; set; }

        public Driver(string name)
        {
            Name = name;
            DaysToDrive = new List<BusDriver>();
        }

        protected Driver()
        {
            DaysToDrive = new List<BusDriver>();
        }
    }
}
