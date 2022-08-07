using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Findin
{
    public partial class MainWindowBackend : Form
    {
        public MainWindowBackend()
        {
            InitializeComponent();
        }

        private const string FormStateFileName = "state.bin";
        private const string ResultsFoundFormat = "Occurrences found: {0}. Showing {1} lines.";
        private const string RegexTestString = "S";
        private const int MaxLineSize = 250;
        private const int MaxItemsInResultListView = 500;
        private const int NumberOfBreakLineCharsToSkip = 2;

        private string DefaultProgramPath { get; set; }

        private async void Search(object sender, EventArgs e)
        {
            if (!TextBoxHasValue(FilePatternsTextBox))
            {
                ShowEmptyFieldAlert(FilePatternsTextBox);
                return;
            }

            string filePatterns = CleanSemiColonString(FilePatternsTextBox.Text);

            if (!TextBoxHasValue(PathTextBox))
            {
                ShowEmptyFieldAlert(PathTextBox);
                return;
            }

            if (!TextBoxHasValue(SearchTextBox))
            {
                ShowEmptyFieldAlert(SearchTextBox);
                return;
            }

            if (!RegexPatternIsValid(SearchTextBox.Text))
            {
                MessageBox.Show("Please search for a valid Regex pattern.", "Warning");
                return;
            }

            if (string.IsNullOrEmpty(filePatterns) || filePatterns.Contains("*.*"))
                return;

            if (!Directory.Exists(PathTextBox.Text))
                return;

            await Search(SearchTextBox.Text);
        }

        public static bool RegexPatternIsValid(string pattern)
        {
            try
            {
                _ = Regex.IsMatch(RegexTestString, pattern);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TextBoxHasValue(TextBox element) => !string.IsNullOrEmpty(element.Text) && !string.IsNullOrWhiteSpace(element.Text);

        public static void ShowEmptyFieldAlert(TextBox textBox) => MessageBox.Show($"The {textBox.Name.Replace("TextBox", "")} field must have a value.", "Warning");

        public static string CleanSemiColonString(string str) => new Regex("^;{1,}|;{2,}|;$").Replace(str, "");

        private async Task Search(string search)
        {
            ResultsFoundLabel.Visible = false;
            SearchingLabel.Visible = true;
            ResultListView.Items.Clear();

            int totalOccurrences = 0;

            bool reachedLimit = false;

            List<FileMatchInformation> listOfFileMatches = new();

            try
            {
                (Dictionary<string, FileOccurrence> fileSearchResults, totalOccurrences) = await Task.Run(async () => await GetFileContents(PathTextBox.Text, search));

                foreach (KeyValuePair<string, FileOccurrence> file in fileSearchResults)
                {
                    if (reachedLimit)
                        break;

                    Dictionary<string, List<int>> fileNameToLineNumber = new();

                    fileNameToLineNumber.TryAdd(file.Key, new List<int>());

                    foreach (int idx in file.Value.MatchIndexes)
                    {
                        if (listOfFileMatches.Count == MaxItemsInResultListView)
                        {
                            reachedLimit = true;
                            break;
                        }

                        int lineNumber = default;
                        string lineContent = string.Empty;

                        if (!await Task.Run(() => TryReadWholeLine(file.Value.FileContent, idx, out lineNumber, out lineContent)))
                            break;

                        if (fileNameToLineNumber[file.Key].Contains(lineNumber))
                            continue;

                        listOfFileMatches.Add(new(file.Key, lineNumber, lineContent));

                        fileNameToLineNumber[file.Key].Add(lineNumber);
                    }
                }
            }
            finally
            {
                UpdateResultListView(listOfFileMatches);

                ResultListView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
                ResultListView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.HeaderSize);

                SearchingLabel.Visible = false;
                ResultsFoundLabel.Text = string.Format(ResultsFoundFormat, totalOccurrences, ResultListView.Items.Count.ToString());
                ResultsFoundLabel.Visible = true;
            }
        }

        private void UpdateResultListView(List<FileMatchInformation> listLineNumberAndContent)
        {
            foreach (var file in listLineNumberAndContent)
            {
                ListViewItem item = new(file.FileName);

                item.SubItems.Add(file.LineNumber.ToString());

                item.SubItems.Add(file.LineContent);

                ResultListView.Items.Add(item);
            }
        }

        private async Task<(Dictionary<string, FileOccurrence>, int)> GetFileContents(string path, string regexSearchPattern)
        {
            Dictionary<string, FileOccurrence> fileContents = new();

            string[] filePatterns = CleanSemiColonString(FilePatternsTextBox.Text).Split(';');

            string ignoredDirectories = string.Join("|", CleanSemiColonString(IgnoreDirectoriesTextBox.Text).Split(';'));

            List<string> allFileNames = new();

            foreach (string pattern in filePatterns)
            {
                allFileNames.AddRange(Directory.GetFiles(path, pattern, SearchOption.AllDirectories));
            }

            int totalOccurrences = 0;

            foreach (string filePath in allFileNames)
            {
                if (ContainsIgnoredDirectories(filePath, ignoredDirectories) || string.IsNullOrEmpty(filePath))
                    continue;

                string fileContent = await File.ReadAllTextAsync(filePath);

                MatchCollection matches = Regex.Matches(fileContent, regexSearchPattern);

                totalOccurrences += matches.Count;

                if (matches.Count > 0)
                {
                    List<int> indexes = new();

                    foreach (Match match in matches)
                    {
                        indexes.Add(match.Index);
                    }

                    fileContents.Add(filePath, new FileOccurrence(fileContent, indexes));
                }
            }

            return (fileContents, totalOccurrences);
        }

        public static bool ContainsIgnoredDirectories(string filePath, string ignoredDirectories)
        {
            return Regex.IsMatch(filePath, string.Concat(@"\\", ignoredDirectories, @"\\"));
        }

        public static bool TryReadWholeLine(string input, int matchIndex, out int lineNumber, out string lineContent)
        {
            try
            {
                lineNumber = 1;

                int idx = 0;
                int matchLineIndex = 0;

                while (idx != matchIndex)
                {
                    if (input[idx] == '\r')
                    {
                        lineNumber++;
                        matchLineIndex = idx + NumberOfBreakLineCharsToSkip;
                    }

                    idx++;
                }

                StringBuilder lineContentStringBuilder = new();

                while (input[matchLineIndex] != '\r')
                {
                    if (lineContentStringBuilder.Length == MaxLineSize)
                        break;

                    if (matchLineIndex == input.Length - 1)
                    {
                        lineContentStringBuilder.Append(input[matchLineIndex]);
                        break;
                    }

                    lineContentStringBuilder.Append(input[matchLineIndex]);

                    matchLineIndex++;
                }

                lineContent = lineContentStringBuilder.ToString().TrimStart();

                return true;
            }
            catch
            {
                lineNumber = default;
                lineContent = string.Empty;
                return false;
            }
        }

        #region Events

        private async void OnFormLoad(object sender, EventArgs e)
        {
            ResultListView.View = View.Details;
            ResultListView.FullRowSelect = true;
            ResultListView.GridLines = true;

            ResultListView.Columns.Add("File", 400, HorizontalAlignment.Left);
            ResultListView.Columns.Add("Line", 100, HorizontalAlignment.Left);
            ResultListView.Columns.Add("Match", 1200, HorizontalAlignment.Left);

            ContextMenuStrip = new();

            AddToolStripMenuItemToContextMenuStrip("Copy File Name", CopyFileNameToClipboard_Click);
            AddToolStripMenuItemToContextMenuStrip("Copy Line Number", CopyLineNumberToClipboard_Click);
            AddToolStripMenuItemToContextMenuStrip("Copy Line Content", CopyLineContentToClipboard_Click);

            if (!File.Exists(FormStateFileName))
                return;

            await using Stream bytesFromStateFile = File.OpenRead(FormStateFileName);
            FormState? formState = JsonSerializer.Deserialize<FormState>(bytesFromStateFile);

            if (formState == null)
                return;

            PathTextBox.Text = formState.Path;
            FilePatternsTextBox.Text = formState.FilePatterns;
            SearchTextBox.Text = formState.Search;
            DefaultProgramPath = formState.DefaultProgramPath;
            IgnoreDirectoriesTextBox.Text = formState.IgnoredDirectories;
        }

        private void AddToolStripMenuItemToContextMenuStrip(string text, EventHandler clickEvent)
        {
            ToolStripMenuItem menuItem = new(text);

            menuItem.Click += clickEvent;

            ContextMenuStrip.Items.Add(menuItem);
        }

        private void CopyFileNameToClipboard_Click(object? _, EventArgs e)
        {
            if (ResultListView.SelectedItems.Count > 0)
                Clipboard.SetText(ResultListView.SelectedItems[0].SubItems[0]?.Text?.Split("\\")[^1] ?? string.Empty);
        }

        private void CopyLineNumberToClipboard_Click(object? _, EventArgs e)
        {
            if (ResultListView.SelectedItems.Count > 0)
                Clipboard.SetText(ResultListView.SelectedItems[0].SubItems[1]?.Text ?? string.Empty);
        }

        private void CopyLineContentToClipboard_Click(object? _, EventArgs e)
        {
            if (ResultListView.SelectedItems.Count > 0)
                Clipboard.SetText(ResultListView.SelectedItems[0].SubItems[2]?.Text ?? string.Empty);
        }

        private void SearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                Search(sender, e);
            }
        }

        private void MainWindowBackend_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormState formState = new(
                PathTextBox.Text,
                FilePatternsTextBox.Text,
                SearchTextBox.Text,
                DefaultProgramPath,
                IgnoreDirectoriesTextBox.Text);

            string serializedFormState = JsonSerializer.Serialize(formState);
            File.WriteAllBytes(FormStateFileName, Encoding.UTF8.GetBytes(serializedFormState));
        }

        private void SelectFolderButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = PathFolderBrowser.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                PathTextBox.Text = PathFolderBrowser.SelectedPath;
            }
        }

        private void SetDefaultProgramPathButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DefaultProgramFileDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                DefaultProgramPath = DefaultProgramFileDialog.FileName;
            }
        }

        #endregion
    }
}
