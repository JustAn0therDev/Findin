using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Findin
{
    [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
    public partial class MainWindowBackend : Form
    {
        public MainWindowBackend()
        {
            InitializeComponent();
        }

        private const string FormStateFileName = "state.bin";
        private const string ResultsFoundFormat = "Matches found: {0}";
        private const string TooManyResultsFoundFormat = "Matches found: {0}. Showing top {1}.";
        private const string RegexTestString = "S";
        private const int MaxLineSize = 250;
        private const int MaxItemsInResultListView = 200;
        private const int NumberOfBreakLineCharsToSkip = 2;

        private string DefaultProgramPath { get; set; }

        private void Search(object sender, EventArgs e)
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

            if (string.IsNullOrEmpty(filePatterns) ||
                filePatterns.Contains("*.*"))
                return;

            if (!Directory.Exists(PathTextBox.Text))
                return;

            Search(SearchTextBox.Text);
        }

        private static bool RegexPatternIsValid(string pattern)
        {
            try
            {
                bool _ = Regex.IsMatch(RegexTestString, pattern);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool TextBoxHasValue(TextBox element) => !string.IsNullOrEmpty(element.Text) && !string.IsNullOrWhiteSpace(element.Text);

        private static void ShowEmptyFieldAlert(TextBox textBox) => MessageBox.Show($"The {textBox.Name.Replace("TextBox", "")} field must have a value.", "Warning");

        private static string CleanSemiColonString(string str) => new Regex(";{2,}|;$").Replace(str, "");

        private void Search(string search)
        {
            ResultsFoundLabel.Visible = false;
            SearchingLabel.Visible = true;
            ResultListView.Items.Clear();

            int totalOccurrences = 0;

            bool reachedLimit = false;

            try
            {
                (Dictionary<string, FileOccurrence> fileSearchResults, totalOccurrences) = GetFileContents(PathTextBox.Text, search);

                foreach (KeyValuePair<string, FileOccurrence> file in fileSearchResults)
                {
                    if (reachedLimit)
                        break;

                    Dictionary<string, List<int>> fileNameToLineNumber = new();

                    fileNameToLineNumber.TryAdd(file.Key, new List<int>());

                    foreach (int idx in file.Value.MatchIndexes)
                    {
                        if (ResultListView.Items.Count == MaxItemsInResultListView)
                        {
                            reachedLimit = true;
                            break;
                        }

                        if (!TryReadWholeLine(file.Value.FileContent, idx, out int lineNumber, out string lineContent))
                            break;

                        if (fileNameToLineNumber[file.Key].Contains(lineNumber))
                            break;

                        ListViewItem item = new(file.Key);

                        item.SubItems.Add(lineNumber.ToString());

                        item.SubItems.Add(lineContent);

                        ResultListView.Items.Add(item);

                        fileNameToLineNumber[file.Key].Add(lineNumber);
                    }
                }
            }
            finally
            {
                ResultListView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
                ResultListView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.HeaderSize);

                SearchingLabel.Visible = false;
                SetResultsFoundLabelText(totalOccurrences);
                ResultsFoundLabel.Visible = true;
            }
        }

        private void SetResultsFoundLabelText(int occurrences)
        {
            if (occurrences > MaxItemsInResultListView)
            {
                ResultsFoundLabel.Text = string.Format(TooManyResultsFoundFormat,
                    occurrences.ToString(),
                    MaxItemsInResultListView.ToString());
                return;
            }

            ResultsFoundLabel.Text = string.Format(ResultsFoundFormat, ResultListView.Items.Count.ToString());
        }

        private (Dictionary<string, FileOccurrence>, int) GetFileContents(string path, string regexSearchPattern)
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
                if (Regex.IsMatch(filePath, $"\\\\{ignoredDirectories}\\\\") || string.IsNullOrEmpty(filePath))
                    continue;

                string fileContent = File.ReadAllText(filePath);

                MatchCollection matches = Regex.Matches(fileContent, regexSearchPattern);

                totalOccurrences += matches.Count;

                if (matches.Count > 0 && fileContents.Count < MaxItemsInResultListView)
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

        private static bool TryReadWholeLine(string input, int matchIndex, out int lineNumber, out string lineContent)
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