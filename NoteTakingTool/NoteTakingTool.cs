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
        public NoteTakingTool()
        {
            InitializeComponent();
            treeViewManager = new TreeViewManager() { treeView = NotebookTreeView };
            activeNoteIndex = -1;
            activeNotebookIndex = -1;

            encryptDecrypt = new EncryptDecrypt();
        }

        private int activeNoteIndex, activeNotebookIndex;
        private TreeViewManager treeViewManager;

        private EncryptDecrypt encryptDecrypt;


        private void button1_Click(object sender, EventArgs e)
        {
            treeViewManager.AddNewNotebook();
        }

        private void NotebookTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode selectedNode = NotebookTreeView.GetNodeAt(e.X, e.Y);

            if (selectedNode != null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    NotebookTreeView.SelectedNode = selectedNode;

                    // if parent is not null we are on a child node, so return the parents index
                    if(selectedNode.Parent != null)
                    {
                        treeViewManager.ShowContextMenuStrip(selectedNode.Parent.Index);
                    }
                    else
                    {
                        treeViewManager.ShowContextMenuStrip(selectedNode.Index);
                    }
                }
                else if(e.Button == MouseButtons.Left)
                {
                    if(selectedNode.Parent != null)
                    {
                        int selectedNotebookIndex = selectedNode.Parent.Index;
                        int selectednoteIndex = selectedNode.Index;
                        string noteContent = treeViewManager.ReturnNoteContent(selectedNotebookIndex, selectednoteIndex);

                        // if the active values are not -1 copy the current text back to the note it is related to
                        if (activeNoteIndex != -1 & activeNotebookIndex != -1 && richTextBox1.Text != "")
                        {
                            treeViewManager.UpdateNoteContent(activeNotebookIndex, activeNoteIndex, richTextBox1.Text);
                        }

                        activeNoteIndex = selectednoteIndex;
                        activeNotebookIndex = selectedNotebookIndex;
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
            treeViewManager.LoadNotebooksFromFile();
        }
        private void WriteCurrentNote_Button_Click(object sender, EventArgs e)
        {
            treeViewManager.WriteNotebooksToFile();
        }
        private void NoteTakingTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            treeViewManager.WriteNotebooksToFile();
        }
    }
}
