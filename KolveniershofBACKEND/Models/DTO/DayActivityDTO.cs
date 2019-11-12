using KolveniershofBACKEND.Models.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KolveniershofBACKEND.Models.DTO
{
    public class DayActivityDTO
    {
        public int DayId { get; set; }
        public int ActivityId { get; set; }
        [Required(ErrorMessage = "Time of day is required")]
        public TimeOfDay TimeOfDay { get; set; }
        public ICollection<AttendanceDTO> Attendances { get; set; }
    }
}
