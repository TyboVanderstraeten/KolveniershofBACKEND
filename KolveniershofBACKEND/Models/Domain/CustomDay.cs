using System;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Models.Domain
{
    public class CustomDay : Day
    {
        public DateTime Date { get; set; }
        public string PreDish { get; set; }
        public string MainDish { get; set; }
        public string Dessert { get; set; }
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
            Notes.Add(note);
        }

        public void RemoveNote(Note note)
        {
            Notes.Remove(note);
        }
    }
}
