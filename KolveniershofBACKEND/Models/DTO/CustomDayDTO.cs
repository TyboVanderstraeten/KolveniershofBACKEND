using System;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Models.DTO
{
    public class CustomDayDTO:DayDTO
    {
        public DateTime Date { get; set; }
        public string PreDish { get; set; }
        public string MainDish { get; set; }
        public string Dessert { get; set; }
        public ICollection<NoteDTO> Notes { get; set; }
    }
}
