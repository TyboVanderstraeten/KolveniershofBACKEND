namespace KolveniershofBACKEND.Models.DTO
{
    public class AttendanceDTO
    {
        public int DayId { get; set; }
        public int ActivityId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
    }
}
