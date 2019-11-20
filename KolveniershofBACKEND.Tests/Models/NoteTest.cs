using KolveniershofBACKEND.Tests.Data;
using KolveniershofBACKEND.Models.Domain;
using Xunit;

namespace KolveniershofBACKEND.Tests.Models
{
    public class NoteTest
    {
        private DummyDBContext _dummyDbContext;

        public NoteTest()
        {
            _dummyDbContext = new DummyDBContext();
        }

        [Fact]
        public void Note_ValidConstuctor_CreatesObject()
        {
            Note testNote = _dummyDbContext.Note1;

            Assert.Equal(NoteType.VERVOER, testNote.NoteType);
            Assert.Equal("Florian neemt de bus niet vandaag", testNote.Content);
        }
    }
}
