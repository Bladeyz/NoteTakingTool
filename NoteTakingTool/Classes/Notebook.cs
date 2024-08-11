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
        public int notebookID;
        public string notebookName;
        public List<Note> notes;
        public bool isArchived;

        internal Notebook()
        {
            notebookID = 1;
            notebookName = "Tester";
            notes = new List<Note>();
            isArchived = false;
        }

        public void AddNote(string noteTitle)
        {
            int noteID = notes.Count();
            Note note = new Note()
            {
                noteID = noteID
                , notebookID = notebookID
                , noteTitle = noteTitle
                , noteContent = null
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
        public void DeleteNote(int noteIndex)
        {
            notes.RemoveAt(noteIndex);
        }
        public void WriteEncryptedNotebookToFile()
        {
            string jsonExport = JsonConvert.SerializeObject(this);

            byte[] encryptedByte = DPAPI.ProtectByte(Encoding.UTF8.GetBytes(jsonExport));

            string filePath = Path.Combine(Environment.CurrentDirectory, @"NotebookFiles", String.Concat(notebookName, ".dat"));
            try
            {
                File.WriteAllBytes(filePath, encryptedByte);
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
                AddNote(noteTitle);
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
