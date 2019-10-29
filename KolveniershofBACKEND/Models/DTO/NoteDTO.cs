using KolveniershofBACKEND.Models.Domain;

namespace KolveniershofBACKEND.Models.DTO
{
    public class NoteDTO
    {
        public int NoteId { get; set; }
        public int DayId { get; set; }
        public NoteType NoteType { get; set; }
        public string Content { get; set; }
    }
}
