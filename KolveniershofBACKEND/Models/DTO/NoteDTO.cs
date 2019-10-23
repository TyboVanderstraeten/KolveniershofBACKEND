namespace KolveniershofBACKEND.Models.DTO
{
    public class NoteDTO
    {
        public int NoteId { get; set; }
        public NoteDTO NoteType { get; set; }
        public string Content { get; set; }
    }
}
