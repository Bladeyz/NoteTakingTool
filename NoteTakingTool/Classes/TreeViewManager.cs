using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NoteTakingTool
{
    internal class TreeViewManager
    {
        internal TreeViewManager()
        {
            notebooks = new List<Notebook>();
            treeView = new TreeView();
            treeViewContextMenuStrip = new ContextMenuStrip();

            InitialiseContextMenuStrip();
        }

        private List<Notebook> notebooks;
        public TreeView treeView;
        private ContextMenuStrip treeViewContextMenuStrip;
        private int currentContextMenuNodeIndex;

        private void InitialiseContextMenuStrip()
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem addNewNote = new ToolStripMenuItem();
            addNewNote.Text = "Add new note";

            addNewNote.Click += new EventHandler(ContextMenu_AddNote);


            contextMenuStrip.Items.Add(addNewNote);

            treeViewContextMenuStrip = contextMenuStrip;
        }
        public void ShowContextMenuStrip(int nodeIndex)
        {
            currentContextMenuNodeIndex = nodeIndex;
            treeView.ContextMenuStrip = treeViewContextMenuStrip;
        }
        public void ContextMenu_AddNote(object sender, EventArgs e)
        {
            // TODO: This might be redundant?
            // can just have the code to create a note in here....
            AddNewNote();
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
        public string ReturnNoteContent(int notebookIndex, int noteIndex)
        {
            return notebooks[notebookIndex].ReturnNoteContent(noteIndex);
        }
        public void UpdateNoteContent(int notebookIndex, int noteIndex, string noteContent)
        {
            notebooks[notebookIndex].UpdateNoteContent(noteIndex, noteContent);
        }
        public void WriteNotebooksToFile()
        {
            foreach (Notebook notebook in notebooks)
            {
                notebook.WriteEncryptedNotbookToFile();
            }
        }
        public void LoadNotebooksFromFile()
        {
            // Method to load all of the notebook JSON files from the directory in to memory
            string filePath = Path.Combine(Environment.CurrentDirectory, @"NotebookFiles");
            string[] notebooksInDirectory = Directory.GetFiles(filePath);

            foreach (string notebook in notebooksInDirectory)
            {
                byte[] loadedByte = File.ReadAllBytes(notebook);

                string unprotectedJSON = Encoding.UTF8.GetString(DPAPI.UnprotectByte(loadedByte));

                notebooks.Add(JsonConvert.DeserializeObject<Notebook>(unprotectedJSON));
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
            string noteContent = "";
            DialogResult dialogResult = NewNoteDialog("Add New Note", ref noteTitle, ref noteContent);

            if(dialogResult == DialogResult.OK && noteTitle != "" && noteContent != "")
            {
                notebooks[currentContextMenuNodeIndex].AddNote(noteTitle, noteContent);
                LoadTreeView();
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
        private DialogResult NewNoteDialog(string formTitle, ref string noteTitle, ref string noteContent)
        {
            // this is an example of how to create a dialog popup and pass values back
            // tutorial used: https://www.makeuseof.com/winforms-input-dialog-box-create-display/

            Form form = new Form();
            form.Text = formTitle;
            TextBox noteTitleTextBox = new TextBox();
            TextBox noteContentTextBox = new TextBox();

            // The dialog result is what is returned when the button is pressed
            Button buttonOk = new Button();
            buttonOk.DialogResult = DialogResult.OK;
            buttonOk.Text = "Create new note";

            Button buttonCancel = new Button();
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.Text = "Cancel note creation";

            // set the size & location of the items on the screen
            noteTitleTextBox.SetBounds(36, 86, 700, 20);
            noteContentTextBox.SetBounds(36, 110, 700, 20);
            buttonOk.SetBounds(228, 160, 160, 60);
            buttonCancel.SetBounds(228, 360, 160, 60);

            // add each of the controls we have defined to the form
            form.Controls.AddRange(new Control[] { noteTitleTextBox, noteContentTextBox, buttonOk, buttonCancel });

            // bind the accept/cancel on the form to the created buttons
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            // Show the form
            DialogResult result = form.ShowDialog();

            // set he textbox value on the screen to a variable so we can pass it back
            noteTitle = noteTitleTextBox.Text;
            noteContent = noteContentTextBox.Text;

            return result;
        }

        /// <summary>
        ///  Returns true if the treeView object of TreeViewManager is not null
        /// </summary>
        /// <returns>bool</returns>
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
    }
}
