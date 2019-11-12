using KolveniershofBACKEND.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace KolveniershofBACKEND.Models.DTO
{
    public class AttendanceDTO
    {
        public int DayId { get; set; }
        public int ActivityId { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Time of day is required")]
        public TimeOfDay TimeOfDay { get; set; }
        public string Comment { get; set; }
    }
}
