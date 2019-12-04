using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Models.Domain.Enums;

namespace KolveniershofBACKEND.Models.DTO
{
    public class BusDriverDTO
    {
        public int DayId { get; set; }
        public int DriverId { get; set; }
        public TimeOfDay TimeOfDay { get; set; }
        public BusColor BusColor { get; set; }
  }
}
