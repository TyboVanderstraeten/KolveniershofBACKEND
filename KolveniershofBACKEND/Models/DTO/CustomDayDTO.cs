using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KolveniershofBACKEND.Models.DTO
{
    public class CustomDayDTO : DayDTO
    {
        public DateTime Date { get; set; }
        public string PreDish { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Main dish is required")]
        public string MainDish { get; set; }
        public string Dessert { get; set; }
        public ICollection<NoteDTO> Notes { get; set; }
    }
}
