using KolveniershofBACKEND.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
