using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoteTakingTool
{
    internal class TreeViewManager
    {
        internal TreeViewManager()
        {
            notebooks = new List<Notebook>();
        }

        private List<Notebook> notebooks;

        public void LoadNotebooksFromFile(string notebookDirectoryPath)
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"NotebookFiles");
            string[] NotebooksInDirectory = Directory.GetFiles(path);

            foreach(string notebook in  NotebooksInDirectory)
            {
                using(StreamReader reader = new StreamReader(notebook))
                {
                    string notebookJson = reader.ReadToEnd();
                    notebooks.Add(JsonConvert.DeserializeObject<Notebook>(notebookJson));
                }
            }
        }

        public void LoadTreeView(TreeView treeView)
        {
            treeView.Nodes.Clear();
            foreach(Notebook notebook in notebooks)
            {
                int notebookIndex = treeView.Nodes.Count;
                treeView.Nodes.Add(notebook.notebookName);
                
                foreach(Note note in notebook)
                {
                    treeView.Nodes[notebookIndex].Nodes.Add(note.noteTitle);
                }
            }
        }
        public void WriteNotebooksToJSON()
        {
            foreach(Notebook notebook in notebooks)
            {
                notebook.WriteNotebookToJSON(notebook);
            }
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
            }
            else
            {
                MessageBox.Show("NOTE NOT ADDED");
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


    }
}
