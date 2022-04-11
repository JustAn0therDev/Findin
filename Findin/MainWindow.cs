using System.Text.RegularExpressions;
using System.Text;

namespace Findin
{
    public partial class MainWindowBackend : Form
    {
        public MainWindowBackend()
        {
            InitializeComponent();
        }

        public void SearchTextBoxTextChanged(object sender, EventArgs e)
        {
            ResultsListBox.Items.Clear();

            string fileTypes = CleanFileTypesString(FileTypeTextBox.Text);

            if (!TextBoxHasValue(PathTextBox))
            {
                ShowEmptyFieldAlert(PathTextBox);
                return;
            }

            if (!TextBoxHasValue(SearchTextBox) || string.IsNullOrEmpty(fileTypes)) return;

            string path = PathTextBox.Text;

            string search = SearchTextBox.Text;

            List<string> directories = new() { Directory.GetCurrentDirectory() };

            PopulateListOfSubDirectoriesInPath(directories, path);

            List<string> fileNames;

            if (string.IsNullOrEmpty(fileTypes))
            {
                fileNames = GetAllFileNames(directories);
            }
            else
            {
                fileNames = GetAllFileNamesFilteredByFileTypes(directories, fileTypes);
            }

            foreach (var fileName in fileNames)
            {
                var sb = new StringBuilder(File.ReadAllText(fileName));

                MatchCollection matches = Regex.Matches(sb.ToString(), search);

                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                        ResultsListBox.Items.Add($"{fileName}: \"{ReadWholeLine(sb.ToString(), match.Index)}\"");
                }
            }
        }

        private static string ReadWholeLine(string input, int matchIndex)
        {
            StringBuilder result = new();
            StringBuilder backwardResult = new();
            StringBuilder forwardResult = new();
            int forwardIndex = matchIndex, backwardIndex = matchIndex - 1;

            // Going forward
            while (forwardIndex < input.Length - 1)
            {
                if (input[forwardIndex] == '\r')
                    break;

                forwardResult.Append(input[forwardIndex]);
                forwardIndex++;
            }

            // Going backwards
            while (backwardIndex >= 0)
            {
                if (input[backwardIndex] == '\n')
                    break;

                backwardResult.Append(input[backwardIndex]);
                backwardIndex--;
            }

            result.Append(string.Join("", backwardResult.ToString().Reverse()));
            result.Append(forwardResult);

            return result.ToString().Trim();
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

            foreach (var directory in directories)
            {
                fileNames.AddRange(FilterFilesByFileTypes(directory, fileTypes.Split(';')));
            }

            return fileNames;
        }

        private static string CleanFileTypesString(string fileTypes) => new Regex(";{2,}|;$").Replace(fileTypes, "");

        private static List<string> FilterFilesByFileTypes(string directory, IEnumerable<string> fileTypes)
        {
            List<string> filtered = new();
            string[] filesInDirectory = Directory.GetFiles(directory);

            foreach (var fileName in filesInDirectory)
            {
                foreach (var fileType in fileTypes)
                {
                    if (fileName.EndsWith(fileType))
                    {
                        filtered.Add(fileName);
                    }
                }
            }

            return filtered;
        }

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