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
            this.RegExpLabel = new System.Windows.Forms.Label();
            this.RegExpTextBox = new System.Windows.Forms.TextBox();
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.SearchLabel = new System.Windows.Forms.Label();
            this.ResultsListBox = new System.Windows.Forms.ListBox();
            this.FileTypeLabel = new System.Windows.Forms.Label();
            this.FileTypeTextBox = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PathTextBox
            // 
            this.PathTextBox.Location = new System.Drawing.Point(157, 12);
            this.PathTextBox.Name = "PathTextBox";
            this.PathTextBox.Size = new System.Drawing.Size(631, 23);
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
            // RegExpLabel
            // 
            this.RegExpLabel.AutoSize = true;
            this.RegExpLabel.Location = new System.Drawing.Point(12, 73);
            this.RegExpLabel.Name = "RegExpLabel";
            this.RegExpLabel.Size = new System.Drawing.Size(106, 15);
            this.RegExpLabel.TabIndex = 2;
            this.RegExpLabel.Text = "Regular Expression";
            // 
            // RegExpTextBox
            // 
            this.RegExpTextBox.Location = new System.Drawing.Point(157, 70);
            this.RegExpTextBox.Name = "RegExpTextBox";
            this.RegExpTextBox.Size = new System.Drawing.Size(631, 23);
            this.RegExpTextBox.TabIndex = 2;
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(157, 41);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(631, 23);
            this.SearchTextBox.TabIndex = 1;
            // 
            // SearchLabel
            // 
            this.SearchLabel.AutoSize = true;
            this.SearchLabel.Location = new System.Drawing.Point(12, 44);
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
            this.ResultsListBox.Size = new System.Drawing.Size(776, 259);
            this.ResultsListBox.TabIndex = 4;
            // 
            // FileTypeLabel
            // 
            this.FileTypeLabel.AutoSize = true;
            this.FileTypeLabel.Location = new System.Drawing.Point(12, 102);
            this.FileTypeLabel.Name = "FileTypeLabel";
            this.FileTypeLabel.Size = new System.Drawing.Size(164, 15);
            this.FileTypeLabel.TabIndex = 8;
            this.FileTypeLabel.Text = "In File Types (separated by \";\")";
            // 
            // FileTypeTextBox
            // 
            this.FileTypeTextBox.Location = new System.Drawing.Point(182, 99);
            this.FileTypeTextBox.Name = "FileTypeTextBox";
            this.FileTypeTextBox.Size = new System.Drawing.Size(606, 23);
            this.FileTypeTextBox.TabIndex = 3;
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(471, 128);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(317, 41);
            this.SearchButton.TabIndex = 9;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // MainWindowBackend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.FileTypeTextBox);
            this.Controls.Add(this.FileTypeLabel);
            this.Controls.Add(this.ResultsListBox);
            this.Controls.Add(this.SearchLabel);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.RegExpTextBox);
            this.Controls.Add(this.RegExpLabel);
            this.Controls.Add(this.PathLabel);
            this.Controls.Add(this.PathTextBox);
            this.Name = "MainWindowBackend";
            this.Text = "Find In";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox PathTextBox;
        private Label PathLabel;
        private Label RegExpLabel;
        private TextBox RegExpTextBox;
        private TextBox SearchTextBox;
        private Label SearchLabel;
        private ListBox ResultsListBox;
        private Label FileTypeLabel;
        private TextBox FileTypeTextBox;
        private Button SearchButton;
    }
}