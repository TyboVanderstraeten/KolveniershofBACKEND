using KolveniershofBACKEND.Models.Domain;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Models.DTO
{
    public class DayActivityDTO
    {
        public int DayId { get; set; }
        public int ActivityId { get; set; }
        public TimeOfDay TimeOfDay { get; set; }
        public ICollection<AttendanceDTO> Attendances { get; set; }
    }
}
