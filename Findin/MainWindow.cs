using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Findin
{
    public partial class MainWindowBackend : Form
    {
        public MainWindowBackend() => InitializeComponent();

        private const string FormStateFileName = "state.bin";

        private string DefaultProgramPath { get; set; }

        private void OnFormLoad(object sender, EventArgs e)
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
            }
        }

        private void SearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                Search(sender, e);
            }
        }

        public async void Search(object sender, EventArgs e)
        {
            if (!TextBoxHasValue(FileTypeTextBox))
            {
                ShowEmptyFieldAlert(FileTypeTextBox);
                return;
            }

            string fileTypes = CleanSemiColonString(FileTypeTextBox.Text);
            string ignoreDirectories = CleanSemiColonString(IgnoreDirectoriesTextBox.Text);

            if (!TextBoxHasValue(PathTextBox))
            {
                ShowEmptyFieldAlert(PathTextBox);
                return;
            }

            if (!TextBoxHasValue(SearchTextBox) || string.IsNullOrEmpty(fileTypes) || fileTypes.Contains("*.*")) return;

            ShowResults(PathTextBox.Text, SearchTextBox.Text, fileTypes, ignoreDirectories);
        }

        private async void ShowResults(string path, string search, string fileTypes, string ignoredDirectories)
        {
            ResultsListBox.Items.Clear();

            List<string> directories = new() { Directory.GetCurrentDirectory() };

            PopulateListOfSubDirectoriesInPath(directories, path);

            List<string> fileNames = GetAllFileNamesFilteredByFileTypes(directories, ignoredDirectories, fileTypes);

            search = FixSearchPattern(search);

            string regexSearchPattern = IgnoreCaseCheckBox.Checked ? $@"(?i){search}" : search;

            foreach (var fileName in fileNames)
            {
                StringBuilder sb = new(await File.ReadAllTextAsync(fileName, encoding: Encoding.UTF8));

                string fileContent = sb.ToString();

                MatchCollection matches = Regex.Matches(fileContent, regexSearchPattern);

                foreach (Match match in matches)
                {
                    (int lineNumber, string lineContent) = ReadWholeLine(fileContent, match.Index);
                    ResultsListBox.Items.Add($"{fileName} at line {lineNumber}: \"{lineContent}\"");
                }
            }
        }

        private static string FixSearchPattern(string search)
        {
            MatchCollection matches = Regex.Matches(search, @$"\W");

            foreach (Match match in matches)
            {
                search = search.Replace(match.Value, $@"\{match.Value}");
            }

            return search;
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

            // Going forward
            while (forwardIndex < input.Length - 1)
            {
                if (input[forwardIndex] == '\r')
                {
                    break;
                }

                forwardResult.Append(input[forwardIndex]);
                forwardIndex++;
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

        private static List<string> GetAllFileNamesFilteredByFileTypes(List<string> directories, string directoriesToIgnore, string fileTypes)
        {
            List<string> fileNames = new();

            string[] fileTypeArray = fileTypes.Split(';');
            string[] ignoredDirectoriesArray = directoriesToIgnore.Split(';');

            foreach (var directory in directories)
            {
                if (InIgnoredDirectories(ignoredDirectoriesArray, directory))
                    continue;

                foreach (var fileType in fileTypeArray)
                {
                    fileNames.AddRange(Directory.EnumerateFiles(directory, fileType));
                }
            }

            return fileNames;
        }

        public static bool InIgnoredDirectories(string[] directoriesToIgnore, string directory)
        {
            foreach (var dir in directoriesToIgnore)
            {
                foreach (var path in directory.Split('\\'))
                {
                    if (path == dir) return true;
                }
            }

            return false;
        }

        private static string CleanSemiColonString(string fileTypes) => new Regex(";{2,}|;$").Replace(fileTypes, "");

        private static void PopulateListOfSubDirectoriesInPath(List<string> subDirectoriesListToPopulate, string path)
        {
            string[] directories = Directory.GetDirectories(path);

            foreach (var directoryPath in directories)
            {
                subDirectoriesListToPopulate.Add(directoryPath);
                PopulateListOfSubDirectoriesInPath(subDirectoriesListToPopulate, directoryPath);
            }
        }

        private static bool TextBoxHasValue(TextBox element) => !string.IsNullOrEmpty(element.Text) && !string.IsNullOrWhiteSpace(element.Text);

        private static void ShowEmptyFieldAlert(TextBox textBox)
        {
            MessageBox.Show($"The {textBox.Name.Replace("TextBox", "")} field must have a value.");
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
    }
}