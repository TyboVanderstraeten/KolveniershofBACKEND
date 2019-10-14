using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Models.Domain
{
    public class Day
    {
        public int DayId { get; set; }
        public int WeekNr { get; set; }
        public int DayNr { get; set; }

        protected Day()
        {

        }

        public Day(int weekNr, int dayNr)
        {
            WeekNr = weekNr;
            DayNr = dayNr;
        }
    }
}
