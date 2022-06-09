using System.Collections.Concurrent;
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
                Regex.IsMatch(RegexTestString, pattern);
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

        private async void Search(string search)
        {
            ResultsFoundLabel.Visible = false;
            SearchingLabel.Visible = true;
            ResultListView.Items.Clear();

            ConcurrentBag<ListViewItem> allListViewItemOccurrences = new();

            try
            {
                ConcurrentDictionary<string, string> fileSearchResults = await GetFileContents(PathTextBox.Text, search);

                Parallel.ForEach(fileSearchResults, keyValuePair =>
               {
                   ConcurrentDictionary<string, List<int>> fileNameToLineNumber = new();

                   fileNameToLineNumber.TryAdd(keyValuePair.Key, new List<int>());

                   MatchCollection matches = Regex.Matches(keyValuePair.Value, search);

                   foreach (Match match in matches)
                   {
                       if (!TryReadWholeLine(keyValuePair.Value, match.Index, out int lineNumber, out string lineContent))
                           break;

                       if (fileNameToLineNumber[keyValuePair.Key].Contains(lineNumber))
                           continue;

                       ListViewItem item = new(keyValuePair.Key);

                       item.SubItems.Add(lineNumber.ToString());

                       item.SubItems.Add(lineContent);

                       allListViewItemOccurrences.Add(item);

                       fileNameToLineNumber[keyValuePair.Key].Add(lineNumber);
                   }
               });
            }
            finally
            {
                int numberOfOccurrencesFound = 0;

                foreach (ListViewItem item in allListViewItemOccurrences)
                {
                    if (numberOfOccurrencesFound == MaxItemsInResultListView)
                        break;

                    ResultListView.Items.Add(item);
                    numberOfOccurrencesFound++;
                }

                ResultListView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
                ResultListView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.HeaderSize);

                SearchingLabel.Visible = false;
                SetResultsFoundLabelText(allListViewItemOccurrences.Count);
                ResultsFoundLabel.Visible = true;
            }
        }

        private void SetResultsFoundLabelText(int totalRecordsFound)
        {
            if (totalRecordsFound > MaxItemsInResultListView)
            {
                ResultsFoundLabel.Text = string.Format(TooManyResultsFoundFormat, totalRecordsFound.ToString(), MaxItemsInResultListView.ToString());
                return;
            }
                
            ResultsFoundLabel.Text = string.Format(ResultsFoundFormat, totalRecordsFound.ToString());
        }

        private Task<ConcurrentDictionary<string, string>> GetFileContents(string path, string regexSearchPattern)
        {
            ConcurrentDictionary<string, string> fileContents = new();
            
            string[] filePatterns = CleanSemiColonString(FilePatternsTextBox.Text).Split(';');
            
            string ignoredDirectories = string.Join("|", CleanSemiColonString(IgnoreDirectoriesTextBox.Text).Split(';'));

            List<string> allFileNames = new();

            foreach (string pattern in filePatterns)
            {
                allFileNames.AddRange(Directory.GetFiles(path, pattern, SearchOption.AllDirectories));
            }

            Parallel.ForEach(allFileNames, filePath =>
            {
               if (Regex.IsMatch(filePath, $"\\\\{ignoredDirectories}\\\\") ||
                   string.IsNullOrEmpty(filePath))
                   return;

               string fileContent = File.ReadAllText(filePath);

               bool hasMatch = Regex.IsMatch(fileContent, regexSearchPattern);

               if (hasMatch && !fileContents.ContainsKey(filePath))
               {
                   fileContents.TryAdd(filePath, fileContent);
               }
           });

            return Task.FromResult(fileContents);
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
                        // + 2 to skip the "\r\n" chars
                        matchLineIndex = idx + 2;
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