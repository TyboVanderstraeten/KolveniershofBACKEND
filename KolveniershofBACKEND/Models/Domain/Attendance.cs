using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KolveniershofBACKEND.Models.Domain
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Attendance
    {
        public int DayId { get; set; }
        public int ActivityId { get; set; }
        public int UserId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TimeOfDay TimeOfDay { get; set; }
        [JsonIgnore]
        public DayActivity DayActivity { get; set; }
        public User User { get; set; }
        public string Comment { get; set; }

        protected Attendance()
        {

        }

        public Attendance(DayActivity dayActivity, User user)
        {
            DayId = dayActivity.DayId;
            ActivityId = dayActivity.ActivityId;
            UserId = user.UserId;
            DayActivity = dayActivity;
            User = user;
            TimeOfDay = dayActivity.TimeOfDay;
        }
    }
}
