using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteTakingTool
{
    internal class Notebook
    {
        internal Notebook()
        {
            notebookID = 1;
            notebookName = "Tester";
            notes = new List<Note>();
        }

        public int notebookID;
        public string notebookName;
        public List<Note> notes;
        public bool isArchived;

        public void AddNote(string noteTitle, string noteContent)
        {
            int noteID = notes.Count();
            Note note = new Note()
            {
                noteID = noteID
                , noteTitle = noteTitle
                , noteContent = noteContent
                , isArchived = false
            };
            notes.Add(note);
        }

        // returns an enumerator of notes
        // allows foreach loops to be used on the notebook to loop through each note
        public IEnumerator<Note> GetEnumerator()
        {
            return notes.GetEnumerator();
        }

        public void LoadTestNotes(int numberOfNotes)
        {
            for(int i = 0; i < numberOfNotes; i++)
            {
                string noteTitle = String.Format("NoteTitle:{0}", notes.Count());
                string noteContent = String.Format("NoteContents:{0}", notes.Count());
                AddNote(noteTitle, noteContent);
            }
        }

        public string ReturnNoteContents(int noteIndex)
        {
            return notes[noteIndex].noteContent;
        }
    }
}
