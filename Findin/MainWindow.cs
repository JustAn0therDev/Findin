using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Findin
{
    public partial class MainWindowBackend : Form
    {
        private readonly FileDictionaryWrapper FileDictionaryWrapper = new();
        private bool IsLoadingDirectory { get; set; }

        public MainWindowBackend()
        {
            InitializeComponent();
        }

        private const string FormStateFileName = "state.bin";
        private const string ResultsFoundFormat = "Matches found: {0}";
        private const int ResultLimit = 250;
        private const int MaxLinePreviewSize = 120;

        private string DefaultProgramPath { get; set; }
        private string LastSearchedPath { get; set; }

        public async void Search(object sender, EventArgs e)
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
                fileTypes.Contains("*.*") ||
                IsLoadingDirectory)
                return;

            if (!Directory.Exists(PathTextBox.Text))
                return;
            
            if (PathTextBox.Text != LastSearchedPath)
            {
                LastSearchedPath = PathTextBox.Text;
                await UpdateFileDictionary();
            }

            ShowResults(SearchTextBox.Text);
        }

        private static bool TextBoxHasValue(TextBox element) => !string.IsNullOrEmpty(element.Text) && !string.IsNullOrWhiteSpace(element.Text);

        private static void ShowEmptyFieldAlert(TextBox textBox) => MessageBox.Show($"The {textBox.Name.Replace("TextBox", "")} field must have a value.");

        private static string CleanSemiColonString(string fileTypes) => new Regex(";{2,}|;$").Replace(fileTypes, "");

        private void ShowResults(string search)
        {
            try
            {
                Dictionary<string, List<int>> fileNameToLineNumber = new();

                ResultsListBox.Items.Clear();
                ResultsFoundLabel.Visible = false;
                SearchingLabel.Visible = true;

                search = FixSearchPattern(search);

                string regexSearchPattern = IgnoreCaseCheckBox.Checked ? $@"(?i){search}" : search;

                foreach (KeyValuePair<string, StringBuilder> keyValuePair in FileDictionaryWrapper.FileNamesToContent)
                {
                    if (ResultsListBox.Items.Count == ResultLimit)
                    {
                        break;
                    }

                    fileNameToLineNumber.Add(keyValuePair.Key, new List<int>());

                    string fileContent = keyValuePair.Value.ToString();

                    MatchCollection matches = Regex.Matches(fileContent, regexSearchPattern);

                    foreach (Match match in matches)
                    {
                        (int lineNumber, string lineContent) = ReadWholeLine(fileContent, match.Index);

                        if (ResultsListBox.Items.Count < ResultLimit && !fileNameToLineNumber[keyValuePair.Key].Contains(lineNumber))
                        {
                            ResultsListBox.Items.Add($"{keyValuePair.Key} at line {lineNumber.ToString()}: \"{lineContent}\"");
                            fileNameToLineNumber[keyValuePair.Key].Add(lineNumber);
                        }
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

            for (int i = 0; i < search.Length; i++)
            {
                if (Regex.IsMatch(search[i].ToString(), @"[^A-Za-z0-9_\s\u0000-\u024F]|[\W]+"))
                {
                    fixedPattern.Append($"\\{search[i]}");
                }
                else if (search[i] == ' ')
                {
                    fixedPattern.Append(@"\s");
                }
                else
                {
                    fixedPattern.Append(search[i]);
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

            StringBuilder backwardResult = new();
            StringBuilder forwardResult = new();
            int forwardIndex = matchIndex, backwardIndex = matchIndex - 1;
            int charCountFromIndex = 0;

            // Going forward
            while (forwardIndex < input.Length - 1)
            {
                if (input[forwardIndex] == '\r' || charCountFromIndex == MaxLinePreviewSize)
                {
                    break;
                }

                forwardResult.Append(input[forwardIndex]);
                forwardIndex++;
                charCountFromIndex++;
            }

            // Going backwards
            while (backwardIndex >= 0)
            {
                if (input[backwardIndex] == '\n')
                {
                    break;
                }

                backwardResult.Append(input[backwardIndex]);
                backwardIndex--;
            }

            return (lineNumber, (string.Join("", backwardResult.ToString().Reverse()) + forwardResult.ToString()).Trim());
        }

        private void SetResultsFoundLabelText()
        {
            if (ResultsListBox.Items.Count == ResultLimit)
            {
                ResultsFoundLabel.Text = $"Matches reached limit. Showing top {ResultLimit} matches.";
                return;
            }

            ResultsFoundLabel.Text = string.Format(ResultsFoundFormat, ResultsListBox.Items.Count.ToString());
        }

        #region Events

        private async void OnFormLoad(object sender, EventArgs e)
        {
            if (File.Exists(FormStateFileName))
            {
                using Stream bytesFromStateFile = File.OpenRead(FormStateFileName);
                FormState? formState = JsonSerializer.Deserialize<FormState>(bytesFromStateFile);

                if (formState == null) return;

                PathTextBox.Text = formState.Path;
                FileTypeTextBox.Text = formState.FileTypes;
                SearchTextBox.Text = formState.Search;
                IgnoreCaseCheckBox.Checked = formState.IgnoreCaseIsChecked;
                DefaultProgramPath = formState.DefaultProgramPath;
                IgnoreDirectoriesTextBox.Text = formState.IgnoredDirectories;
                LastSearchedPath = formState.Path;

                await UpdateFileDictionary();
            }
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
            var formState = new FormState(
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

            if (itemIndex != ListBox.NoMatches)
            {
                string? selectedFile = ResultsListBox.Items[itemIndex] as string;

                if (!string.IsNullOrEmpty(selectedFile))
                {
                    string fileName = selectedFile.Split(" at")[0];

                    Process.Start(DefaultProgramPath, $"\"{fileName}\"");
                }
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

        private async Task UpdateFileDictionary()
        {
            if (IsLoadingDirectory || !TextBoxHasValue(FileTypeTextBox)) return;

            try
            {
                if (!Directory.Exists(LastSearchedPath))
                    return;
                
                IsLoadingDirectory = true;
                LoadingDirectoryLabel.Visible = true;

                string[] ignoredDirectoriesArray = CleanSemiColonString(IgnoreDirectoriesTextBox.Text).Split(';');
                await Task.Run(() => FileDictionaryWrapper.Watch(PathTextBox.Text, FileTypeTextBox.Text, ignoredDirectoriesArray));
            }
            finally
            {
                IsLoadingDirectory = false;
                LoadingDirectoryLabel.Visible = false;
            }
        }

        #endregion
    }
}