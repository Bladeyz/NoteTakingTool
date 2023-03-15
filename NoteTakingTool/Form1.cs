using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoteTakingTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            notebook = new Notebook();
            currentNoteIndex = -1;
        }

        private Notebook notebook;
        private int currentNoteIndex;


        private void button1_Click(object sender, EventArgs e)
        {
            notebook.LoadTestNotes(10);

            foreach(Note note in notebook)
            {
                NotebookTreeView.Nodes.Add(note.noteTitle);
            }  
        }

        private void NotebookTreeView_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            int treeViewIndex = e.Node.Index;
            string noteContent = notebook.ReturnNoteContents(treeViewIndex);

            // if we have a previously selected note and the text is not blank then
            // copy the current text back to the note in question
            if (currentNoteIndex != -1 & richTextBox1.Text != "")
            {
                notebook.notes[currentNoteIndex].noteContent = richTextBox1.Text;
            }

            currentNoteIndex = treeViewIndex;
            richTextBox1.Text = noteContent;
        }
    }
}
