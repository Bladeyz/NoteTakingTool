using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

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
                , notebookID = notebookID
                , noteTitle = noteTitle
                , noteContent = noteContent
                , isArchived = false
            };
            notes.Add(note);
        }
        public string ReturnNoteContent(int noteIndex)
        {
            return notes[noteIndex].noteContent;
        }
        public void UpdateNoteContent(int noteIndex,  string noteContent)
        {
            notes[noteIndex].noteContent = noteContent;
        }
        public void WriteNotebookToJSON()
        {
            string jsonExport = JsonConvert.SerializeObject(this);

            string filePath = Path.Combine(Environment.CurrentDirectory, @"NotebookFiles", String.Concat(notebookName, ".json"));
            try
            {
                using (StreamWriter writeJSON = new StreamWriter(filePath))
                {
                    writeJSON.WriteLine(jsonExport);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }
        public void LoadTestNotes(int numberOfNotes)
        {
            for (int i = 0; i < numberOfNotes; i++)
            {
                string noteTitle = String.Format("NoteTitle:{0}", notes.Count());
                string noteContent = String.Format("NoteContents:{0}", notes.Count());
                AddNote(noteTitle, noteContent);
            }
        }
        public IEnumerator<Note> GetEnumerator()
        {
            // returns an enumerator of notes
            // allows foreach loops to be used on the notebook to loop through each note
            return notes.GetEnumerator();
        }
    }
}
