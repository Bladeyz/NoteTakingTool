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
            logicalFilePath = "C:/Users/James/Desktop/noteshere";
        }

        public int notebookID;
        public string notebookName;
        public List<Note> notes;
        public bool isArchived;
        public string logicalFilePath;

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

        public void WriteNotebookToJSON(Notebook notebook)
        {
            string jsonExport = JsonConvert.SerializeObject(notebook);

            string filePath = Path.Combine(Environment.CurrentDirectory, @"NotebookFiles", notebook.notebookName, @".json");
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
    }
}
