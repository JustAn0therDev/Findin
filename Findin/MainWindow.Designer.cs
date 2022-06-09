namespace Findin
{
    partial class MainWindowBackend
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PathTextBox = new System.Windows.Forms.TextBox();
            this.PathLabel = new System.Windows.Forms.Label();
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.SearchLabel = new System.Windows.Forms.Label();
            this.FileTypeLabel = new System.Windows.Forms.Label();
            this.FilePatternsTextBox = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.PathFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.DefaultProgramFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.IgnoreDirectoriesLabel = new System.Windows.Forms.Label();
            this.IgnoreDirectoriesTextBox = new System.Windows.Forms.TextBox();
            this.SearchingLabel = new System.Windows.Forms.Label();
            this.ResultsFoundLabel = new System.Windows.Forms.Label();
            this.ResultListView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // PathTextBox
            // 
            this.PathTextBox.Location = new System.Drawing.Point(69, 18);
            this.PathTextBox.Name = "PathTextBox";
            this.PathTextBox.Size = new System.Drawing.Size(376, 23);
            this.PathTextBox.TabIndex = 0;
            this.PathTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchTextBox_KeyPress);
            // 
            // PathLabel
            // 
            this.PathLabel.AutoSize = true;
            this.PathLabel.Location = new System.Drawing.Point(12, 21);
            this.PathLabel.Name = "PathLabel";
            this.PathLabel.Size = new System.Drawing.Size(31, 15);
            this.PathLabel.TabIndex = 1;
            this.PathLabel.Text = "Path";
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(95, 120);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(350, 23);
            this.SearchTextBox.TabIndex = 4;
            this.SearchTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchTextBox_KeyPress);
            // 
            // SearchLabel
            // 
            this.SearchLabel.AutoSize = true;
            this.SearchLabel.Location = new System.Drawing.Point(12, 123);
            this.SearchLabel.Name = "SearchLabel";
            this.SearchLabel.Size = new System.Drawing.Size(77, 15);
            this.SearchLabel.TabIndex = 5;
            this.SearchLabel.Text = "Regex Search";
            // 
            // FileTypeLabel
            // 
            this.FileTypeLabel.AutoSize = true;
            this.FileTypeLabel.Location = new System.Drawing.Point(12, 53);
            this.FileTypeLabel.Name = "FileTypeLabel";
            this.FileTypeLabel.Size = new System.Drawing.Size(71, 15);
            this.FileTypeLabel.TabIndex = 8;
            this.FileTypeLabel.Text = "File Patterns";
            // 
            // FilePatternsTextBox
            // 
            this.FilePatternsTextBox.Location = new System.Drawing.Point(94, 50);
            this.FilePatternsTextBox.Name = "FilePatternsTextBox";
            this.FilePatternsTextBox.Size = new System.Drawing.Size(351, 23);
            this.FilePatternsTextBox.TabIndex = 2;
            this.FilePatternsTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchTextBox_KeyPress);
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(1162, 11);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(355, 35);
            this.SearchButton.TabIndex = 7;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.Search);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(451, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(223, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Select Folder";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SelectFolderButton_Click);
            // 
            // IgnoreDirectoriesLabel
            // 
            this.IgnoreDirectoriesLabel.AutoSize = true;
            this.IgnoreDirectoriesLabel.Location = new System.Drawing.Point(12, 88);
            this.IgnoreDirectoriesLabel.Name = "IgnoreDirectoriesLabel";
            this.IgnoreDirectoriesLabel.Size = new System.Drawing.Size(100, 15);
            this.IgnoreDirectoriesLabel.TabIndex = 5;
            this.IgnoreDirectoriesLabel.Text = "Ignore Directories";
            // 
            // IgnoreDirectoriesTextBox
            // 
            this.IgnoreDirectoriesTextBox.Location = new System.Drawing.Point(118, 85);
            this.IgnoreDirectoriesTextBox.Name = "IgnoreDirectoriesTextBox";
            this.IgnoreDirectoriesTextBox.Size = new System.Drawing.Size(327, 23);
            this.IgnoreDirectoriesTextBox.TabIndex = 3;
            this.IgnoreDirectoriesTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchTextBox_KeyPress);
            // 
            // SearchingLabel
            // 
            this.SearchingLabel.AutoSize = true;
            this.SearchingLabel.Location = new System.Drawing.Point(12, 632);
            this.SearchingLabel.Name = "SearchingLabel";
            this.SearchingLabel.Size = new System.Drawing.Size(68, 15);
            this.SearchingLabel.TabIndex = 9;
            this.SearchingLabel.Text = "Searching...";
            this.SearchingLabel.Visible = false;
            // 
            // ResultsFoundLabel
            // 
            this.ResultsFoundLabel.AutoSize = true;
            this.ResultsFoundLabel.Location = new System.Drawing.Point(12, 632);
            this.ResultsFoundLabel.Name = "ResultsFoundLabel";
            this.ResultsFoundLabel.Size = new System.Drawing.Size(91, 15);
            this.ResultsFoundLabel.TabIndex = 10;
            this.ResultsFoundLabel.Text = "Results found: 0";
            this.ResultsFoundLabel.Visible = false;
            // 
            // ResultListView
            // 
            this.ResultListView.Location = new System.Drawing.Point(12, 160);
            this.ResultListView.Name = "ResultListView";
            this.ResultListView.Size = new System.Drawing.Size(1505, 469);
            this.ResultListView.TabIndex = 11;
            this.ResultListView.UseCompatibleStateImageBehavior = false;
            // 
            // MainWindowBackend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1522, 655);
            this.Controls.Add(this.ResultListView);
            this.Controls.Add(this.ResultsFoundLabel);
            this.Controls.Add(this.SearchingLabel);
            this.Controls.Add(this.IgnoreDirectoriesTextBox);
            this.Controls.Add(this.IgnoreDirectoriesLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.FilePatternsTextBox);
            this.Controls.Add(this.FileTypeLabel);
            this.Controls.Add(this.SearchLabel);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.PathLabel);
            this.Controls.Add(this.PathTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWindowBackend";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find In";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindowBackend_FormClosing);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox PathTextBox;
        private Label PathLabel;
        private TextBox SearchTextBox;
        private Label SearchLabel;
        private Label FileTypeLabel;
        private TextBox FilePatternsTextBox;
        private Button SearchButton;
        private FolderBrowserDialog PathFolderBrowser;
        private Button button1;
        private OpenFileDialog DefaultProgramFileDialog;
        private Label IgnoreDirectoriesLabel;
        private TextBox IgnoreDirectoriesTextBox;
        private Label SearchingLabel;
        private Label ResultsFoundLabel;
        private ListView ResultListView;
    }
}