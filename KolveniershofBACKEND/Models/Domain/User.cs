using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace KolveniershofBACKEND.Models.Domain
{
    public class User
    {
        public int UserId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public UserType UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public int? Group { get; set; }
        public int? DegreeOfLimitation { get; set; }
        public ICollection<WeekendDay> WeekendDays { get; set; }

        protected User()
        {
            WeekendDays = new List<WeekendDay>();
        }

        public User(UserType userType, string firstName, string lastName,
            string email, string profilePicture, int? group, int? degreeOfLimitation)
        {
            UserType = userType;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            ProfilePicture = profilePicture;
            Group = group;
            DegreeOfLimitation = degreeOfLimitation;
            WeekendDays = new List<WeekendDay>();
        }

        public void AddWeekendDay(WeekendDay weekendDays)
        {
            if (WeekendDays.SingleOrDefault(wd => wd.WeekendDayId == weekendDays.WeekendDayId || wd.Date == weekendDays.Date) == null)
            {
                WeekendDays.Add(weekendDays);
            }
            else
            {
                throw new ArgumentException("WeekendDay already exists");
            }
        }

        public void RemoveWeekendDay(WeekendDay weekendDays)
        {
            if (WeekendDays.SingleOrDefault(wd => wd.WeekendDayId == weekendDays.WeekendDayId) != null)
            {
                WeekendDays.Remove(weekendDays);
            }
            else
            {
                throw new ArgumentException("Weekendday doesn't exist");
            }
        }
    }
}
