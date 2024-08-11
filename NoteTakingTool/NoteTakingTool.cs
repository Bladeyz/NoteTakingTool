using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoteTakingTool
{
    public partial class NoteTakingTool : Form
    {
        private TreeViewManager treeViewManager;

        public NoteTakingTool()
        {
            InitializeComponent();
            treeViewManager = new TreeViewManager() { treeView = NotebookTreeView };
            treeViewManager.LoadNotebooksFromFile();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            treeViewManager.AddNewNotebook();
        }
        private void NotebookTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode selectedNode = NotebookTreeView.GetNodeAt(e.X, e.Y);

            if (selectedNode != null)
            {
                // Set the selected indexes. If a child node is selected then return the index of the parent for the notebook.
                int selectedNotebook = selectedNode.Parent is null ? selectedNode.Index : selectedNode.Parent.Index;
                int selectedNote = selectedNode.Parent is null ? -1 : selectedNode.Index;
                treeViewManager.SetSelectedNotebookIndexes(selectedNotebook, selectedNote);

                if (e.Button == MouseButtons.Right)
                {
                    NotebookTreeView.SelectedNode = selectedNode;
                    treeViewManager.ShowContextMenuStrip();
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (selectedNode.Parent != null)
                    {
                        UpdateActiveNote();
                        treeViewManager.SetActiveNotebookIndexes(selectedNotebook, selectedNote);

                        string noteContent = treeViewManager.ReturnActiveNoteContent();

                        richTextBox1.Text = noteContent;
                    }
                }
            }
        }
        //private void NotebookTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        //{

        //}
        private void LoadNotes_Button_Click(object sender, EventArgs e)
        {
            UpdateActiveNote();
            treeViewManager.LoadNotebooksFromFile();
        }
        private void WriteCurrentNote_Button_Click(object sender, EventArgs e)
        {
            UpdateActiveNote();
            treeViewManager.WriteNotebooksToFile();
        }
        private void NoteTakingTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateActiveNote();
            treeViewManager.WriteNotebooksToFile();
        }
        private void UpdateActiveNote()
        {
            treeViewManager.UpdateActiveNoteContent(richTextBox1.Text);
        }
    }
}
