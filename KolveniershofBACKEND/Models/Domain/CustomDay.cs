﻿using System;
using System.Collections.Generic;

namespace KolveniershofBACKEND.Models.Domain
{
    public class CustomDay : Day
    {
        public DateTime Date { get; set; }
        public string Menu { get; set; }
        public ICollection<Note> Notes { get; set; }

        protected CustomDay() : base()
        {
            Notes = new List<Note>();
        }

        public CustomDay(int weekNr, int dayNr, DateTime date, string menu) : base(weekNr, dayNr)
        {
            Date = date;
            Menu = menu;
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