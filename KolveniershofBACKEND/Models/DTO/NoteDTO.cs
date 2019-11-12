using KolveniershofBACKEND.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace KolveniershofBACKEND.Models.DTO
{
    public class NoteDTO
    {
        public int NoteId { get; set; }
        public int DayId { get; set; }
        [Required(ErrorMessage = "NoteType is required")]
        public NoteType NoteType { get; set; }
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
    }
}
