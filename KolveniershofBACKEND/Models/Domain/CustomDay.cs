using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Models.Domain
{
    public class CustomDay : Day
    {
        public DateTime Date { get; set; }
        // Menu nullable? Set value later?
        public string Menu { get; set; }

        protected CustomDay() : base()
        {

        }

        public CustomDay(int weekNr, int dayNr, DateTime date, string menu) : base(weekNr, dayNr)
        {
            Date = date;
            Menu = menu;
        }
    }
}
