using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Models.Domain
{
    public class Presence
    {
        public int DayId { get; set; }
        public int ActivityId { get; set; }
        public int UserId { get; set; }
        public DayActivity DayActivity { get; set; }
        public User User { get; set; }
        // Nullable? Set value later?
        public string Comment { get; set; }

        protected Presence()
        {

        }

        public Presence(DayActivity dayActivity, User user, string comment)
        {
            DayId = dayActivity.DayId;
            ActivityId = dayActivity.ActivityId;
            UserId = user.UserId;
            DayActivity = dayActivity;
            User = user;
            Comment = comment;
        }
    }
}
