using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Models.DTO
{
    public class DayDTO
    {
        public int WeekNr { get; set; }
        public int DayNr { get; set; }
        public ICollection<DayActivityDTO> DayActivities { get; set; }
        public ICollection<HelperDTO> Helpers { get; set; }
    }
}
