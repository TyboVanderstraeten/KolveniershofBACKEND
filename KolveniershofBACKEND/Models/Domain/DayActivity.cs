using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KolveniershofBACKEND.Models.Domain
{
    [JsonObject(MemberSerialization.OptOut)]
    public class DayActivity
    {
        public int DayId { get; set; }
        public int ActivityId { get; set; }
        [JsonIgnore]
        public Day Day { get; set; }
        public Activity Activity { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TimeOfDay TimeOfDay { get; set; }
        public ICollection<Attendance> Attendances { get; set; }

        protected DayActivity()
        {
            Attendances = new List<Attendance>();
        }

        public DayActivity(Day day, Activity activity, TimeOfDay timeOfDay)
        {
            DayId = day.DayId;
            ActivityId = activity.ActivityId;
            Day = day;
            Activity = activity;
            TimeOfDay = timeOfDay;
            Attendances = new List<Attendance>();
        }

        public void AddAttendance(Attendance attendance)
        {
            if (Attendances.SingleOrDefault(a =>
                a.ActivityId == attendance.ActivityId
                && a.DayId == attendance.DayId
                && a.UserId == attendance.UserId
                && a.TimeOfDay == attendance.TimeOfDay) == null)
            {
                Attendances.Add(attendance);
            }
            else
            {
                throw new ArgumentException("Attendance already exists");
            }
        }

        public void RemoveAttendance(Attendance attendance)
        {
            if (Attendances.SingleOrDefault(a =>
                 (a.ActivityId == attendance.ActivityId
                 && a.DayId == attendance.DayId
                 && a.UserId == attendance.UserId
                 && a.TimeOfDay == attendance.TimeOfDay)) != null)
            {
                Attendances.Remove(attendance);
            }
            else
            {
                throw new ArgumentException("Attendance doesn't exist");
            }
        }
    }
}
