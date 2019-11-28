using KolveniershofBACKEND.Models.Domain;

namespace KolveniershofBACKEND.Models.DTO
{
    public class BusDriverDTO
    {
        public int DayId { get; set; }
        public int NewDriverId { get; set; }
        public TimeOfDay TimeOfDay { get; set; }
    }
}
