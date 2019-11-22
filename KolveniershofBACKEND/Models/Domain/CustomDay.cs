using System;
using System.Collections.Generic;
using System.Linq;

namespace KolveniershofBACKEND.Models.Domain
{
    public class CustomDay : Day
    {
        public DateTime Date { get; set; }
        public string PreDish { get; set; }
        public string MainDish { get; set; }
        public string Dessert { get; set; }
        public new string DayName {
            get {
                switch (Date.DayOfWeek)
                {
                    case DayOfWeek.Monday: return "Maandag";
                    case DayOfWeek.Tuesday: return "Dinsdag";
                    case DayOfWeek.Wednesday: return "Woensdag";
                    case DayOfWeek.Thursday: return "Donderdag";
                    case DayOfWeek.Friday: return "Vrijdag";
                    default: return "Invalid";
                }
            }
        }
        public ICollection<Note> Notes { get; set; }

        protected CustomDay() : base()
        {
            Notes = new List<Note>();
        }

        public CustomDay(string templateName, int weekNr, int dayNr, DateTime date, string preDish, string mainDish, string dessert) : base(templateName, weekNr, dayNr)
        {
            Date = date;
            PreDish = preDish;
            MainDish = mainDish;
            Dessert = dessert;
            Notes = new List<Note>();
        }

        public void AddNote(Note note)
        {
            if (Notes.SingleOrDefault(n => n.NoteId == note.NoteId && n.DayId == note.DayId) == null)
            {
                Notes.Add(note);
            }
            else
            {
                throw new ArgumentException("Note already exists");
            }
        }

        public void RemoveNote(Note note)
        {
            if (Notes.SingleOrDefault(n => n.NoteId == note.NoteId && n.DayId == note.DayId) != null)
            {
                Notes.Remove(note);
            }
            else
            {
                throw new ArgumentException("Note doesn't exist");
            }
        }
    }
}
