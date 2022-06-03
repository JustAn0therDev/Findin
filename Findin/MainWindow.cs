using System.Collections.Concurrent;
using System.Diagnostics;
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
        private const int ResultLimit = 250;
        private const int MaxLinePreviewSize = 80;
        private const int MaxLineSize = 1000;

        private string DefaultProgramPath { get; set; }

        private async void Search(object sender, EventArgs e)
        {
            if (!TextBoxHasValue(FileTypeTextBox))
            {
                ShowEmptyFieldAlert(FileTypeTextBox);
                return;
            }

            string fileTypes = CleanSemiColonString(FileTypeTextBox.Text);

            if (!TextBoxHasValue(PathTextBox))
            {
                ShowEmptyFieldAlert(PathTextBox);
                return;
            }

            if (!TextBoxHasValue(SearchTextBox) ||
                string.IsNullOrEmpty(fileTypes) ||
                fileTypes.Contains("*.*"))
                return;

            if (!Directory.Exists(PathTextBox.Text))
                return;
            
            await Search(SearchTextBox.Text);
        }

        private static bool TextBoxHasValue(TextBox element) => !string.IsNullOrEmpty(element.Text) && !string.IsNullOrWhiteSpace(element.Text);

        private static void ShowEmptyFieldAlert(TextBox textBox) => MessageBox.Show($"The {textBox.Name.Replace("TextBox", "")} field must have a value.");

        private static string CleanSemiColonString(string fileTypes) => new Regex(";{2,}|;$").Replace(fileTypes, "");

        // TODO: Update the README.md on how the search for files is done.
        // TODO: Change the way the results are "created" in the ResultListBox.
        // First, created the lines in parallel THEN populate the ResultListBox.
        // The new Dictionary containing information about the lines found should be
        // limited to {ResultLimit} results.
        private async Task Search(string search)
        {
            try
            {
                Dictionary<string, List<int>> fileNameToLineNumber = new();

                ResultsListBox.Items.Clear();
                ResultsFoundLabel.Visible = false;
                SearchingLabel.Visible = true;

                search = FixSearchPattern(search);

                string regexSearchPattern = IgnoreCaseCheckBox.Checked ? $@"(?i){search}" : search;

                ConcurrentDictionary<string, string> fileSearchResults = await GetFileContents(PathTextBox.Text, regexSearchPattern);

                foreach (KeyValuePair<string, string> keyValuePair in fileSearchResults)
                {
                    if (ResultsListBox.Items.Count == ResultLimit)
                    {
                        break;
                    }

                    fileNameToLineNumber.Add(keyValuePair.Key, new List<int>());

                    MatchCollection matches = Regex.Matches(keyValuePair.Value, regexSearchPattern);

                    foreach (Match match in matches)
                    {
                        (int lineNumber, string lineContent) = ReadWholeLine(keyValuePair.Value, match.Index);

                        if (fileNameToLineNumber[keyValuePair.Key].Contains(lineNumber))
                            continue;

                        if (ResultsListBox.Items.Count == ResultLimit)
                            break;

                        ResultsListBox.Items.Add(string.Format("{0} at line {1}: {2}", keyValuePair.Key, lineNumber.ToString(), lineContent));
                        
                        fileNameToLineNumber[keyValuePair.Key].Add(lineNumber);
                    }
                }
            }
            finally
            {
                SearchingLabel.Visible = false;
                SetResultsFoundLabelText();
                ResultsFoundLabel.Visible = true;
            }
        }

        private static string FixSearchPattern(string search)
        {
            StringBuilder fixedPattern = new();

            foreach (char character in search)
            {
                if (Regex.IsMatch(character.ToString(), @"[^A-Za-z0-9_\s\u0000-\u024F]|[\W]+"))
                {
                    fixedPattern.Append($"\\{character}");
                }
                else if (character == ' ')
                {
                    fixedPattern.Append(@"\s");
                }
                else
                {
                    fixedPattern.Append(character);
                }
            }

            return fixedPattern.ToString();
        }

        private static (int, string) ReadWholeLine(string input, int matchIndex)
        {
            int lineNumber = 1;

            int idx = 0;

            while (idx != matchIndex)
            {
                if (input[idx] == '\r')
                    lineNumber++;

                idx++;
            }

            StringBuilder charsBeforeMatchIndexReversed = new();
            StringBuilder charsFromMatchIndex = new();
            int forwardIndex = matchIndex, backwardIndex = matchIndex - 1;
            int charCountFromIndex = 0;

            // Going forward
            while (forwardIndex < input.Length - 1)
            {
                if (input[forwardIndex] == '\r' || charCountFromIndex is MaxLinePreviewSize or > MaxLineSize)
                {
                    break;
                }

                charsFromMatchIndex.Append(input[forwardIndex]);
                forwardIndex++;
                charCountFromIndex++;
            }

            // Reset number of chars read so we can count backwards and don't exceed the char limit.
            charCountFromIndex = 0;

            // Going backwards
            while (backwardIndex >= 0)
            {
                if (input[backwardIndex] == '\n' || charCountFromIndex is MaxLinePreviewSize or > MaxLineSize)
                {
                    break;
                }

                charsBeforeMatchIndexReversed.Append(input[backwardIndex]);
                backwardIndex--;
                charCountFromIndex++;
            }

            StringBuilder charsBeforeMatchIndex = new();

            for (int i = charsBeforeMatchIndexReversed.Length - 1; i >= 0; i--)
            {
                charsBeforeMatchIndex.Append(charsBeforeMatchIndexReversed[i]);
            }

            return (lineNumber, string.Concat(charsBeforeMatchIndex.ToString(), charsFromMatchIndex).TrimStart());
        }

        private void SetResultsFoundLabelText()
        {
            ResultsFoundLabel.Text = string.Format(ResultsFoundFormat, ResultsListBox.Items.Count.ToString());
        }
        
        private Task<ConcurrentDictionary<string, string>> GetFileContents(string path, string regexSearchPattern)
        {
            ConcurrentDictionary<string, string> fileContents = new();
            string[] allFileNames = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            string[] ignoredDirectories = CleanSemiColonString(IgnoreDirectoriesTextBox.Text).Split(';');
            string[] fileTypes = CleanSemiColonString(FileTypeTextBox.Text).Split(';');
            
            Parallel.ForEach(allFileNames, new ParallelOptions { MaxDegreeOfParallelism = ResultLimit }, filePath =>
            {
                if (!FileTypeIsInDesiredFileTypes(filePath, fileTypes) ||
                    InIgnoredDirectories(filePath, ignoredDirectories) ||
                    string.IsNullOrEmpty(filePath)                     ||
                    fileContents.Count >= ResultLimit)
                    return;

                string fileContent = File.ReadAllText(filePath);
                
                bool hasMatch = Regex.IsMatch(fileContent, regexSearchPattern);

                if (hasMatch)
                {
                    fileContents.TryAdd(filePath, fileContent);
                }
            });

            return Task.FromResult(fileContents);
        }
        
        private static bool FileTypeIsInDesiredFileTypes(string filePath, string[] fileTypes)
        {
            foreach (string fileType in fileTypes)
            {
                if (filePath.EndsWith(fileType))
                    return true;
            }

            return false;
        }

        private static bool InIgnoredDirectories(string directory, string[] ignoredDirectories)
        {
            foreach (string dir in ignoredDirectories)
            {
                foreach (string path in directory.Split('\\'))
                {
                    if (path == dir)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #region Events

        private async void OnFormLoad(object sender, EventArgs e)
        {
            if (!File.Exists(FormStateFileName))
                return;
            
            await using Stream bytesFromStateFile = File.OpenRead(FormStateFileName);
            FormState? formState = JsonSerializer.Deserialize<FormState>(bytesFromStateFile);

            if (formState == null) 
                return;

            PathTextBox.Text = formState.Path;
            FileTypeTextBox.Text = formState.FileTypes;
            SearchTextBox.Text = formState.Search;
            IgnoreCaseCheckBox.Checked = formState.IgnoreCaseIsChecked;
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
                FileTypeTextBox.Text,
                SearchTextBox.Text,
                IgnoreCaseCheckBox.Checked,
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

        private void ResultsListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (string.IsNullOrEmpty(DefaultProgramPath))
            {
                MessageBox.Show("You do not have a default program to open the file when clicking on a search result.\nIf you want to open the file in a program of your preference " +
                    "you can do that by clicking the \"Set Default Program Path\" button and selecting a program!\nIt can be a text editor or an IDE.", "Warning!", MessageBoxButtons.OK);
                return;
            }

            int itemIndex = ResultsListBox.IndexFromPoint(e.Location);

            if (itemIndex == ListBox.NoMatches)
                return;
            
            string? selectedFile = ResultsListBox.Items[itemIndex] as string;

            if (string.IsNullOrEmpty(selectedFile))
                return;
            
            string fileName = selectedFile.Split(" at")[0];

            Process.Start(DefaultProgramPath, $"\"{fileName}\"");
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