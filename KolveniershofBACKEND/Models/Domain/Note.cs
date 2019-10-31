namespace KolveniershofBACKEND.Models.Domain
{
    public class Note
    {
        public int NoteId { get; set; }
        public int DayId { get; set; }
        public NoteType NoteType { get; set; }
        public string Content { get; set; }

        protected Note()
        {

        }

        public Note(NoteType noteType, string content)
        {
            NoteType = noteType;
            Content = content;
        }
    }
}
