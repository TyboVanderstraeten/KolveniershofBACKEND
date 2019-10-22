using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Models.DTO
{
    public class AttendanceDTO
    {
        public int DayId { get; set; }
        public int ActivityId { get; set; }
        public int UserId { get; set; }
        public DayActivity DayActivity { get; set; }
        public User User { get; set; }
        public string Comment { get; set; }

        public AttendanceDTO(Attendance attendance)
        {
            DayId = attendance.DayId;
            ActivityId = attendance.ActivityId;
            UserId = attendance.UserId;
            DayActivity = attendance.DayActivity;
            User = attendance.User;
            Comment = attendance.Comment;
        }
    }
}
