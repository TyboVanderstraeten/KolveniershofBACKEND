using System.Collections.Generic;

namespace KolveniershofBACKEND.Models.DTO
{
    public class DayDTO
    {
        public int DayId { get; set; }
        public string TemplateName { get; set; }
        public int WeekNr { get; set; }
        public int DayNr { get; set; }
        public ICollection<DayActivityDTO> DayActivities { get; set; }
        public ICollection<HelperDTO> Helpers { get; set; }
    }
}
