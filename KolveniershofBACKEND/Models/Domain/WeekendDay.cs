using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Models.Domain
{
    public class WeekendDay
    {
        public int WeekendDayId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }

        protected WeekendDay()
        {

        }

        public WeekendDay(DateTime date, string comment = "")
        {
            Date = date;
            Comment = comment;
        }
    }
}
