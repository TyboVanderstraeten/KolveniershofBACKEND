using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofBACKEND.Models.DTO
{
    public class CustomWeekendDayDTO
    {
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
    }
}
