using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Models.Domain
{
    // This class' purpose is to link interns and volunteers to a day planning (first piece of text on the paper example)
    public class Helper
    {
        public int DayId { get; set; }
        public int UserId { get; set; }
        public Day Day { get; set; }
        public User User { get; set; }

        protected Helper()
        {

        }

        public Helper(Day day, User user)
        {
            DayId = day.DayId;
            UserId = User.UserId;
            Day = day;
            User = user;
        }
    }
}
