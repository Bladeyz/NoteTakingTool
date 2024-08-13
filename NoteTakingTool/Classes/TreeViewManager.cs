using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NoteTakingTool
{
    internal class TreeViewManager
    {
        private List<Notebook> notebooks;
        public TreeView treeView;
        private ContextMenuStrip treeViewContextMenuStrip;
        private int activeNoteIndex, activeNotebookIndex, selectedNoteIndex, selectedNotebookIndex;


        internal TreeViewManager()
        {
            activeNoteIndex = -1;
            activeNotebookIndex = -1;
            selectedNoteIndex = -1;
            selectedNotebookIndex = -1;
            notebooks = new List<Notebook>();
            treeView = new TreeView();
            treeViewContextMenuStrip = new ContextMenuStrip();
            InitialiseContextMenuStrip();
        }

        private void InitialiseContextMenuStrip()
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem addNewNote = new ToolStripMenuItem();
            addNewNote.Text = "Add new note";
            addNewNote.Click += new EventHandler(ContextMenu_AddNote);

            ToolStripMenuItem deleteNote = new ToolStripMenuItem();
            deleteNote.Text = "Delete selected note";
            deleteNote.Click += new EventHandler(ContextMenu_DeleteSelectedNote);

            contextMenuStrip.Items.Add(addNewNote);
            contextMenuStrip.Items.Add(deleteNote);

            treeViewContextMenuStrip = contextMenuStrip;
        }
        public void ShowContextMenuStrip()
        {
            treeView.ContextMenuStrip = treeViewContextMenuStrip;
        }
        public void ContextMenu_AddNote(object sender, EventArgs e)
        {
            AddNewNote();
        }
        public void ContextMenu_DeleteSelectedNote(object sender, EventArgs e)
        {
            if (CheckNoteSelected())
            {
                notebooks[selectedNotebookIndex].DeleteNote(selectedNoteIndex);
                RemoveSelectedNoteFromTreeView();
            }
        }
        public void LoadTreeView()
        {
            if(CheckTreeViewExists())
            {
                treeView.Nodes.Clear();
                foreach (Notebook notebook in notebooks)
                {
                    int notebookIndex = treeView.Nodes.Count;
                    treeView.Nodes.Add(notebook.notebookName);

                    foreach (Note note in notebook)
                    {
                        treeView.Nodes[notebookIndex].Nodes.Add(note.noteTitle);
                    }
                }
            }
        }
        public void RemoveSelectedNoteFromTreeView()
        {
            if (CheckNoteSelected())
            {
                treeView.Nodes[selectedNotebookIndex].Nodes[selectedNoteIndex].Remove();
                selectedNoteIndex = -1;
                activeNoteIndex = -1;
            }
        }
        public string ReturnNoteContent(int notebookIndex, int noteIndex)
        {
            return notebooks[notebookIndex].ReturnNoteContent(noteIndex);
        }
        public string ReturnActiveNoteContent()
        {
            if (CheckNoteActive())
            {
                return notebooks[activeNotebookIndex].ReturnNoteContent(activeNoteIndex);
            }
            else
            {
                return null;
            }
        }
        public void UpdateNoteContent(int notebookIndex, int noteIndex, string noteContent)
        {
            notebooks[notebookIndex].UpdateNoteContent(noteIndex, noteContent);
        }
        public void UpdateActiveNoteContent(string noteContent)
        {
            if (CheckNoteActive())
            {
                notebooks[activeNotebookIndex].UpdateNoteContent(activeNoteIndex, noteContent);
            }
        }
        public void WriteNotebooksToFile()
        {
            foreach (Notebook notebook in notebooks)
            {
                notebook.WriteEncryptedNotebookToFile();
            }
        }
        public void LoadNotebooksFromFile()
        {
            // Method to load all of the notebook JSON files from the directory in to memory
            string filePath = Path.Combine(Environment.CurrentDirectory, @"NotebookFiles");
            string[] notebooksInDirectory = Directory.GetFiles(filePath);

            foreach (string notebook in notebooksInDirectory)
            {
                // Read in the byte array, decrypt and convert back to a JSON string
                byte[] loadedByte = File.ReadAllBytes(notebook);
                string unprotectedJSON = Encoding.UTF8.GetString(DPAPI.UnprotectByte(loadedByte));

                Notebook loadedNotebook = JsonConvert.DeserializeObject<Notebook>(unprotectedJSON);

                bool notebookExists = false;
                foreach (Notebook notebooks in notebooks)
                {
                    if(loadedNotebook.notebookName == notebooks.notebookName)
                    {
                        notebookExists = true;
                    }
                }

                if (!notebookExists)
                {
                    notebooks.Add(loadedNotebook);
                }
            }

            LoadTreeView();
        }
        public void AddNewNotebook()
        {
            string formName = "";
            DialogResult dialogResult = NewNotebookDialog("Add New Notebook", ref formName);

            if (dialogResult == DialogResult.OK & formName != "")
            {
                Notebook newNotebook = new Notebook()
                {
                    notebookID = notebooks.Count()
                    , notebookName = formName
                    , isArchived = false
                };

                notebooks.Add(newNotebook);
                LoadTreeView();
            }
            else
            {
                MessageBox.Show("NOTE NOT ADDED");
            }
        }
        public void AddNewNote()
        {
            string noteTitle = "";
            DialogResult dialogResult = NewNoteDialog("Add New Note", ref noteTitle);

            if(dialogResult == DialogResult.OK && noteTitle != "")
            {
                notebooks[selectedNotebookIndex].AddNote(noteTitle);
                treeView.Nodes[selectedNotebookIndex].Nodes.Add(noteTitle);

                int newNodeIndex = treeView.Nodes[selectedNotebookIndex].Nodes.Count - 1;

                treeView.SelectedNode = treeView.Nodes[selectedNotebookIndex].Nodes[newNodeIndex];
            }
        }
        private DialogResult NewNotebookDialog(string title, ref string formName)
        {
            // this is an example of how to create a dialog popup and pass values back
            // tutorial used: https://www.makeuseof.com/winforms-input-dialog-box-create-display/

            Form form = new Form();
            form.Text = title;
            TextBox textBox = new TextBox();

            // The dialog result is what is returned when the button is pressed
            Button buttonOk = new Button();
            buttonOk.DialogResult = DialogResult.OK;
            buttonOk.Text = "Create new notebook";

            Button buttonCancel = new Button();
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.Text = "Cancel notebook creation";

            // set the size & location of the items on the screen
            textBox.SetBounds(36, 86, 700, 20);
            buttonOk.SetBounds(228, 160, 160, 60);
            buttonCancel.SetBounds(228, 360, 160, 60);

            // add each of the controls we have defined to the form
            form.Controls.AddRange(new Control[] { textBox, buttonOk, buttonCancel });

            // bind the accept/cancel on the form to the created buttons
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            // Show the form
            DialogResult result = form.ShowDialog();

            // set he textbox value on the screen to a variable so we can pass it back
            formName = textBox.Text;

            return result;
        }
        private DialogResult NewNoteDialog(string formTitle, ref string noteTitle)
        {
            // This is an example of how to create a dialog popup and pass values back
            // Tutorial used: https://www.makeuseof.com/winforms-input-dialog-box-create-display/

            Form form = new Form();
            form.Text = formTitle;
            form.Size = new Size(338, 130);
            form.StartPosition = FormStartPosition.CenterScreen;

            // Create each of the on screen objects and set properties
            Label noteTitleLabel = new Label();
            noteTitleLabel.Text = "Note Title:";
            noteTitleLabel.SetBounds(1, 5, 60, 20);

            TextBox noteTitleTextBox = new TextBox();
            noteTitleTextBox.SetBounds(61, 5, 258, 20);

            Button buttonOk = new Button();
            buttonOk.DialogResult = DialogResult.OK;
            buttonOk.Text = "Create new note";
            buttonOk.SetBounds(1, 30, 160, 60);

            Button buttonCancel = new Button();
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.Text = "Cancel note creation";
            buttonCancel.SetBounds(161, 30, 160, 60);

            // Add each of the controls we have defined to the form
            form.Controls.AddRange(new Control[] { noteTitleTextBox, noteTitleLabel, buttonOk, buttonCancel });

            // Bind the accept/cancel on the form to the created buttons
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            // Show the form
            DialogResult result = form.ShowDialog();

            // set he textbox value on the screen to a variable so we can pass it back
            noteTitle = noteTitleTextBox.Text;

            return result;
        }
        private bool CheckTreeViewExists()
        {
            bool treeViewStatus = treeView != null ? true : false;

            if (treeViewStatus)
            {
                return true;
            }
            else
            {
                MessageBox.Show("treeView not correctly initialised");
                return false;
            }
        }
        public void SetActiveNotebookIndexes(int notebookIndex = -1, int noteIndex = -1)
        {
            activeNotebookIndex = notebookIndex;
            activeNoteIndex = noteIndex;
            SetSelectedNotebookIndexes(notebookIndex, noteIndex);
        }
        public void SetSelectedNotebookIndexes(int notebookIndex = -1, int noteIndex = -1)
        {
            selectedNotebookIndex = notebookIndex;
            selectedNoteIndex = noteIndex;
        }
        public bool CheckNoteActive()
        {
            if (activeNotebookIndex == -1 || activeNoteIndex == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool CheckNoteSelected()
        {
            if (selectedNotebookIndex == -1 || selectedNoteIndex == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
