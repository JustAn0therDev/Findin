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
            }
        }

        private void SearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                Search(sender, e);
            }
        }

        public void Search(object sender, EventArgs e)
        {
            if (!TextBoxHasValue(FileTypeTextBox))
            {
                ShowEmptyFieldAlert(FileTypeTextBox);
                return;
            }

            string fileTypes = CleanFileTypesString(FileTypeTextBox.Text);

            if (!TextBoxHasValue(PathTextBox))
            {
                ShowEmptyFieldAlert(PathTextBox);
                return;
            }

            if (!TextBoxHasValue(SearchTextBox) || string.IsNullOrEmpty(fileTypes)) return;

            string path = PathTextBox.Text;
            string search = SearchTextBox.Text;

            ShowResults(path, search, fileTypes);
        }

        private async void ShowResults(string path, string search, string fileTypes)
        {
            ResultsListBox.Items.Clear();

            List<string> directories = new() { Directory.GetCurrentDirectory() };

            PopulateListOfSubDirectoriesInPath(directories, path);

            List<string> fileNames = string.IsNullOrEmpty(fileTypes) ? GetAllFileNames(directories) : GetAllFileNamesFilteredByFileTypes(directories, fileTypes);

            string regexSearchPattern = IgnoreCaseCheckBox.Checked ? $"(?i){search}" : search;

            foreach (var fileName in fileNames)
            {
                StringBuilder sb = new(await File.ReadAllTextAsync(fileName));

                string fileContent = sb.ToString();

                MatchCollection matches = Regex.Matches(fileContent, regexSearchPattern);

                foreach (Match match in matches)
                    ResultsListBox.Items.Add($"{fileName} at line {GetLineNumber(fileContent, match.Index)}: \"{ReadWholeLine(fileContent, match.Index)}\"");
            }
        }

        private static int GetLineNumber(string input, int matchIndex)
        {
            int lineNumber = 1;

            int idx = 0;

            while (idx != matchIndex)
            {
                if (input[idx] == '\r') lineNumber++;
                idx++;
            }

            return lineNumber;
        }

        private static string ReadWholeLine(string input, int matchIndex)
        {
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

            return (string.Join("", backwardResult.ToString().Reverse()) + forwardResult.ToString()).Trim();
        }

        private static List<string> GetAllFileNames(List<string> directories)
        {
            List<string> fileNames = new();

            foreach (var directory in directories)
            {
                fileNames.AddRange(Directory.GetFiles(directory));
            }

            return fileNames;
        }

        private static List<string> GetAllFileNamesFilteredByFileTypes(List<string> directories, string fileTypes)
        {
            List<string> fileNames = new();

            string[] fileTypeArray = fileTypes.Split(';');

            foreach (var directory in directories)
            {
                foreach (var fileType in fileTypeArray)
                    fileNames.AddRange(Directory.GetFiles(directory, fileType));
            }

            return fileNames;
        }

        private static string CleanFileTypesString(string fileTypes) => new Regex(";{2,}|;$").Replace(fileTypes, "");

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
            var formState = new FormState(PathTextBox.Text, FileTypeTextBox.Text, SearchTextBox.Text, IgnoreCaseCheckBox.Checked);
            string serializedFormState = JsonSerializer.Serialize(formState);
            File.WriteAllBytes(FormStateFileName, Encoding.UTF8.GetBytes(serializedFormState));
        }

        private void SelectFolderButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = FolderBrowser.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                PathTextBox.Text = FolderBrowser.SelectedPath;
            }
        }

        private void ResultsListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int itemIndex = ResultsListBox.IndexFromPoint(e.Location);

            if (itemIndex != ListBox.NoMatches)
            {
                string? selectedFile = ResultsListBox.Items[itemIndex] as string;

                if (!string.IsNullOrEmpty(selectedFile))
                {
                    string fileName = selectedFile.Split(' ')[0];

                    Process.Start("notepad.exe", fileName);
                }
            }
        }
    }
}