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
            this.ResultsListBox = new System.Windows.Forms.ListBox();
            this.FileTypeLabel = new System.Windows.Forms.Label();
            this.FileTypeTextBox = new System.Windows.Forms.TextBox();
            this.IgnoreCaseCheckBox = new System.Windows.Forms.CheckBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.PathFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.SetDefaultProgramPathButton = new System.Windows.Forms.Button();
            this.DefaultProgramFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.IgnoreDirectoriesLabel = new System.Windows.Forms.Label();
            this.IgnoreDirectoriesTextBox = new System.Windows.Forms.TextBox();
            this.SearchingLabel = new System.Windows.Forms.Label();
            this.ResultsFoundLabel = new System.Windows.Forms.Label();
            this.LoadingDirectoryLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PathTextBox
            // 
            this.PathTextBox.Location = new System.Drawing.Point(49, 12);
            this.PathTextBox.Name = "PathTextBox";
            this.PathTextBox.Size = new System.Drawing.Size(376, 23);
            this.PathTextBox.TabIndex = 0;
            this.PathTextBox.Leave += new System.EventHandler(this.UpdateFileDictionary);
            // 
            // PathLabel
            // 
            this.PathLabel.AutoSize = true;
            this.PathLabel.Location = new System.Drawing.Point(12, 15);
            this.PathLabel.Name = "PathLabel";
            this.PathLabel.Size = new System.Drawing.Size(31, 15);
            this.PathLabel.TabIndex = 1;
            this.PathLabel.Text = "Path";
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(60, 114);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(365, 23);
            this.SearchTextBox.TabIndex = 4;
            this.SearchTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchTextBox_KeyPress);
            // 
            // SearchLabel
            // 
            this.SearchLabel.AutoSize = true;
            this.SearchLabel.Location = new System.Drawing.Point(12, 117);
            this.SearchLabel.Name = "SearchLabel";
            this.SearchLabel.Size = new System.Drawing.Size(42, 15);
            this.SearchLabel.TabIndex = 5;
            this.SearchLabel.Text = "Search";
            // 
            // ResultsListBox
            // 
            this.ResultsListBox.FormattingEnabled = true;
            this.ResultsListBox.ItemHeight = 15;
            this.ResultsListBox.Location = new System.Drawing.Point(12, 175);
            this.ResultsListBox.Name = "ResultsListBox";
            this.ResultsListBox.Size = new System.Drawing.Size(1003, 454);
            this.ResultsListBox.TabIndex = 8;
            this.ResultsListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ResultsListBox_MouseDoubleClick);
            // 
            // FileTypeLabel
            // 
            this.FileTypeLabel.AutoSize = true;
            this.FileTypeLabel.Location = new System.Drawing.Point(12, 47);
            this.FileTypeLabel.Name = "FileTypeLabel";
            this.FileTypeLabel.Size = new System.Drawing.Size(56, 15);
            this.FileTypeLabel.TabIndex = 8;
            this.FileTypeLabel.Text = "File types";
            // 
            // FileTypeTextBox
            // 
            this.FileTypeTextBox.Location = new System.Drawing.Point(74, 44);
            this.FileTypeTextBox.Name = "FileTypeTextBox";
            this.FileTypeTextBox.Size = new System.Drawing.Size(351, 23);
            this.FileTypeTextBox.TabIndex = 2;
            this.FileTypeTextBox.Leave += new System.EventHandler(this.UpdateFileDictionary);
            // 
            // IgnoreCaseCheckBox
            // 
            this.IgnoreCaseCheckBox.AutoSize = true;
            this.IgnoreCaseCheckBox.Location = new System.Drawing.Point(431, 116);
            this.IgnoreCaseCheckBox.Name = "IgnoreCaseCheckBox";
            this.IgnoreCaseCheckBox.Size = new System.Drawing.Size(88, 19);
            this.IgnoreCaseCheckBox.TabIndex = 5;
            this.IgnoreCaseCheckBox.Text = "Ignore Case";
            this.IgnoreCaseCheckBox.UseVisualStyleBackColor = true;
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(660, 12);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(355, 23);
            this.SearchButton.TabIndex = 7;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.Search);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(431, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(223, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Select Folder";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SelectFolderButton_Click);
            // 
            // SetDefaultProgramPathButton
            // 
            this.SetDefaultProgramPathButton.Location = new System.Drawing.Point(12, 143);
            this.SetDefaultProgramPathButton.Name = "SetDefaultProgramPathButton";
            this.SetDefaultProgramPathButton.Size = new System.Drawing.Size(319, 23);
            this.SetDefaultProgramPathButton.TabIndex = 6;
            this.SetDefaultProgramPathButton.Text = "Set Default Program Path";
            this.SetDefaultProgramPathButton.UseVisualStyleBackColor = true;
            this.SetDefaultProgramPathButton.Click += new System.EventHandler(this.SetDefaultProgramPathButton_Click);
            // 
            // IgnoreDirectoriesLabel
            // 
            this.IgnoreDirectoriesLabel.AutoSize = true;
            this.IgnoreDirectoriesLabel.Location = new System.Drawing.Point(12, 82);
            this.IgnoreDirectoriesLabel.Name = "IgnoreDirectoriesLabel";
            this.IgnoreDirectoriesLabel.Size = new System.Drawing.Size(100, 15);
            this.IgnoreDirectoriesLabel.TabIndex = 5;
            this.IgnoreDirectoriesLabel.Text = "Ignore Directories";
            // 
            // IgnoreDirectoriesTextBox
            // 
            this.IgnoreDirectoriesTextBox.Location = new System.Drawing.Point(118, 79);
            this.IgnoreDirectoriesTextBox.Name = "IgnoreDirectoriesTextBox";
            this.IgnoreDirectoriesTextBox.Size = new System.Drawing.Size(307, 23);
            this.IgnoreDirectoriesTextBox.TabIndex = 3;
            this.IgnoreDirectoriesTextBox.Leave += new System.EventHandler(this.UpdateFileDictionary);
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
            // LoadingDirectoryLabel
            // 
            this.LoadingDirectoryLabel.AutoSize = true;
            this.LoadingDirectoryLabel.Location = new System.Drawing.Point(905, 151);
            this.LoadingDirectoryLabel.Name = "LoadingDirectoryLabel";
            this.LoadingDirectoryLabel.Size = new System.Drawing.Size(110, 15);
            this.LoadingDirectoryLabel.TabIndex = 12;
            this.LoadingDirectoryLabel.Text = "Loading Directory...";
            this.LoadingDirectoryLabel.Visible = false;
            // 
            // MainWindowBackend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 655);
            this.Controls.Add(this.LoadingDirectoryLabel);
            this.Controls.Add(this.ResultsFoundLabel);
            this.Controls.Add(this.SearchingLabel);
            this.Controls.Add(this.IgnoreDirectoriesTextBox);
            this.Controls.Add(this.IgnoreDirectoriesLabel);
            this.Controls.Add(this.SetDefaultProgramPathButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.IgnoreCaseCheckBox);
            this.Controls.Add(this.FileTypeTextBox);
            this.Controls.Add(this.FileTypeLabel);
            this.Controls.Add(this.ResultsListBox);
            this.Controls.Add(this.SearchLabel);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.PathLabel);
            this.Controls.Add(this.PathTextBox);
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
        private ListBox ResultsListBox;
        private Label FileTypeLabel;
        private TextBox FileTypeTextBox;
        private CheckBox IgnoreCaseCheckBox;
        private Button SearchButton;
        private FolderBrowserDialog PathFolderBrowser;
        private Button button1;
        private Button SetDefaultProgramPathButton;
        private OpenFileDialog DefaultProgramFileDialog;
        private Label IgnoreDirectoriesLabel;
        private TextBox IgnoreDirectoriesTextBox;
        private Label SearchingLabel;
        private Label ResultsFoundLabel;
        private Label LoadingDirectoryLabel;
    }
}