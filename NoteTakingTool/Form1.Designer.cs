﻿namespace NoteTakingTool
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NotebookTreeView = new System.Windows.Forms.TreeView();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.WriteCurrentNote_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NotebookTreeView
            // 
            this.NotebookTreeView.Location = new System.Drawing.Point(12, 12);
            this.NotebookTreeView.Name = "NotebookTreeView";
            this.NotebookTreeView.Size = new System.Drawing.Size(236, 514);
            this.NotebookTreeView.TabIndex = 0;
            this.NotebookTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.NotebookTreeView_AfterSelect_1);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(254, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(639, 514);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 538);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(232, 55);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // WriteCurrentNote_Button
            // 
            this.WriteCurrentNote_Button.Location = new System.Drawing.Point(278, 542);
            this.WriteCurrentNote_Button.Name = "WriteCurrentNote_Button";
            this.WriteCurrentNote_Button.Size = new System.Drawing.Size(197, 50);
            this.WriteCurrentNote_Button.TabIndex = 3;
            this.WriteCurrentNote_Button.Text = "WriteCurrentNote";
            this.WriteCurrentNote_Button.UseVisualStyleBackColor = true;
            this.WriteCurrentNote_Button.Click += new System.EventHandler(this.WriteCurrentNote_Button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 601);
            this.Controls.Add(this.WriteCurrentNote_Button);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.NotebookTreeView);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView NotebookTreeView;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button WriteCurrentNote_Button;
    }
}

