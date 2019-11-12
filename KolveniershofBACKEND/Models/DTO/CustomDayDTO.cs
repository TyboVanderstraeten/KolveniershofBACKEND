using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KolveniershofBACKEND.Models.DTO
{
    public class CustomDayDTO : DayDTO
    {
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Pre-dish is required")]
        public string PreDish { get; set; }
        [Required(ErrorMessage = "Main dish is required")]
        public string MainDish { get; set; }
        [Required(ErrorMessage = "Dessert is required")]
        public string Dessert { get; set; }
        public ICollection<NoteDTO> Notes { get; set; }
    }
}
