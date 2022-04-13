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
            this.SuspendLayout();
            // 
            // PathTextBox
            // 
            this.PathTextBox.Location = new System.Drawing.Point(157, 12);
            this.PathTextBox.Name = "PathTextBox";
            this.PathTextBox.Size = new System.Drawing.Size(1236, 23);
            this.PathTextBox.TabIndex = 0;
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
            this.SearchTextBox.Location = new System.Drawing.Point(157, 78);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(1236, 23);
            this.SearchTextBox.TabIndex = 2;
            this.SearchTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchTextBox_KeyPress);
            // 
            // SearchLabel
            // 
            this.SearchLabel.AutoSize = true;
            this.SearchLabel.Location = new System.Drawing.Point(12, 82);
            this.SearchLabel.Name = "SearchLabel";
            this.SearchLabel.Size = new System.Drawing.Size(42, 15);
            this.SearchLabel.TabIndex = 5;
            this.SearchLabel.Text = "Search";
            // 
            // ResultsListBox
            // 
            this.ResultsListBox.FormattingEnabled = true;
            this.ResultsListBox.ItemHeight = 15;
            this.ResultsListBox.Location = new System.Drawing.Point(12, 160);
            this.ResultsListBox.MaximumSize = new System.Drawing.Size(1475, 484);
            this.ResultsListBox.MinimumSize = new System.Drawing.Size(1475, 484);
            this.ResultsListBox.Name = "ResultsListBox";
            this.ResultsListBox.Size = new System.Drawing.Size(1475, 484);
            this.ResultsListBox.TabIndex = 5;
            // 
            // FileTypeLabel
            // 
            this.FileTypeLabel.AutoSize = true;
            this.FileTypeLabel.Location = new System.Drawing.Point(12, 47);
            this.FileTypeLabel.Name = "FileTypeLabel";
            this.FileTypeLabel.Size = new System.Drawing.Size(249, 15);
            this.FileTypeLabel.TabIndex = 8;
            this.FileTypeLabel.Text = "In these file types (e.g. \"*.cs\", separated by \";\")";
            // 
            // FileTypeTextBox
            // 
            this.FileTypeTextBox.Location = new System.Drawing.Point(267, 44);
            this.FileTypeTextBox.Name = "FileTypeTextBox";
            this.FileTypeTextBox.Size = new System.Drawing.Size(1126, 23);
            this.FileTypeTextBox.TabIndex = 1;
            // 
            // IgnoreCaseCheckBox
            // 
            this.IgnoreCaseCheckBox.AutoSize = true;
            this.IgnoreCaseCheckBox.Location = new System.Drawing.Point(1399, 82);
            this.IgnoreCaseCheckBox.Name = "IgnoreCaseCheckBox";
            this.IgnoreCaseCheckBox.Size = new System.Drawing.Size(88, 19);
            this.IgnoreCaseCheckBox.TabIndex = 3;
            this.IgnoreCaseCheckBox.Text = "Ignore Case";
            this.IgnoreCaseCheckBox.UseVisualStyleBackColor = true;
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(1060, 109);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(427, 45);
            this.SearchButton.TabIndex = 4;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.Search);
            // 
            // MainWindowBackend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1499, 655);
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
    }
}