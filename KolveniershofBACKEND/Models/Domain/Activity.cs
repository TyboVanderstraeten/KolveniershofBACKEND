using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KolveniershofBACKEND.Models.Domain
{
    public class Activity
    {
        public int ActivityId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ActivityType ActivityType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Pictogram { get; set; }

        protected Activity()
        {
        }

        public Activity(ActivityType activityType, string name, string description, string pictogram)
        {
            ActivityType = activityType;
            Name = name;
            Description = description;
            Pictogram = pictogram;
        }
    }
}
