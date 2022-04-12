using System.Text;
using System.Text.RegularExpressions;

namespace Findin
{
    public partial class MainWindowBackend : Form
    {
        public MainWindowBackend() => InitializeComponent();

        public void Search(object sender, EventArgs e)
        {
            string fileTypes = CleanFileTypesString(FileTypeTextBox.Text);

            if (!TextBoxHasValue(PathTextBox))
            {
                ShowEmptyFieldAlert(PathTextBox);
                return;
            }

            if (!TextBoxHasValue(SearchTextBox) || string.IsNullOrEmpty(fileTypes)) return;

            string path = PathTextBox.Text;
            string search = SearchTextBox.Text;

            Task.Run(() => ShowResults(path, search, fileTypes));
        }

        private void ShowResults(string path, string search, string fileTypes)
        {
            ResultsListBox.Items.Clear();

            List<string> directories = new() { Directory.GetCurrentDirectory() };

            PopulateListOfSubDirectoriesInPath(directories, path);

            List<string> fileNames = string.IsNullOrEmpty(fileTypes) ? GetAllFileNames(directories) : GetAllFileNamesFilteredByFileTypes(directories, fileTypes);

            List<string> results = new();

            string regexSearchPattern = IgnoreCaseCheckBox.Checked ? $"(?i){search}" : search;

            Parallel.ForEach(fileNames, fileName =>
            {
                StringBuilder sb = new(File.ReadAllText(fileName));

                string fileContent = sb.ToString();

                MatchCollection matches = Regex.Matches(fileContent, regexSearchPattern);

                if (matches.Count > 0)
                    foreach (Match match in matches)
                        results.Add($"{fileName}: \"{ReadWholeLine(fileContent, match.Index)}\"");
            });

            foreach (var result in results) ResultsListBox.Items.Add(result);
        }

        private static string ReadWholeLine(string input, int matchIndex)
        {
            StringBuilder backwardResult = new();
            StringBuilder forwardResult = new();
            int forwardIndex = matchIndex, backwardIndex = matchIndex - 1;

            // Going forward
            while (forwardIndex < input.Length - 1)
            {
                // TODO: make this better. Not every time the line breaks will be this consistent.
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
                // TODO: make this better. Not every time the line breaks will be this consistent.
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
    }
}