using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Models.Domain
{
    public class CustomWeekendDay : Day
    {
        public int UserId { get; set; } 
        public DateTime Date { get; set; }
        public string Comment { get; set; }

        protected CustomWeekendDay() : base()
        {

        }

        public CustomWeekendDay(int weekNr, int dayNr, DateTime date) : base(weekNr, dayNr)
        {
            Date = date;
        }
    }
}
