using KolveniershofBACKEND.Models.Domain;
using KolveniershofBACKEND.Tests.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KolveniershofBACKEND.Tests.Models
{
    public class CustomDayTest
    {
        private DummyDBContext _dummyDBContext;
        private CustomDay _customDayWithNotes, _customDayWithoutNotes;
        private Note _note1;

        public CustomDayTest()
        {
            _dummyDBContext = new DummyDBContext();
            _customDayWithNotes = _dummyDBContext.CustomDay1;
            _customDayWithoutNotes = _dummyDBContext.CustomDay4;
            _note1 = _dummyDBContext.Note4;
        }

        [Fact]
        public void Notes_CustomDayWithNotes_ReturnsLengthOfNotes()
        {
            Assert.Equal(2, _customDayWithNotes.Notes.Count);
        }

        [Fact]
        public void Notes_CustomDayWithoutNotes_ReturnsZero()
        {
            Assert.Equal(0, _customDayWithoutNotes.Notes.Count);
        }

        #region AddNote
        [Fact]
        public void AddNote_CustomDayWithNotes_AddsNewNote()
        {
            var length = _customDayWithNotes.Notes.Count;

            _customDayWithNotes.AddNote(_note1);

            Assert.Equal(length + 1, _customDayWithNotes.Notes.Count);
        }

        [Fact]
        public void AddNote_SameNoteToCustomDayWithNotes_ThrowsArgumentException()
        {
            _customDayWithNotes.AddNote(_note1);

            Assert.Throws<ArgumentException>(() => _customDayWithNotes.AddNote(_note1));
        }
        #endregion

        #region RemoveNote
        [Fact]
        public void RemoveNote_CustomDayWithNotes_RemovesNote()
        {
            var length = _customDayWithNotes.Notes.Count;

            _customDayWithNotes.RemoveNote(_dummyDBContext.Note3);

            Assert.Equal(length - 1, _customDayWithNotes.Notes.Count);
        }

        [Fact]
        public void RemoveNote_NoteNotInCustomDayWithNotes_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _customDayWithNotes.RemoveNote(_dummyDBContext.Note4));
        }
        #endregion
    }
}
