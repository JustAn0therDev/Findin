namespace Findin
{
    public partial class MainWindowBackend : Form
    {
        public MainWindowBackend()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            ResultsListBox.Items.Clear();

            string fileTypes = string.Empty;
            string path, search;

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

            path = PathTextBox.Text;

            search = SearchTextBox.Text;

            List<string> directories = new();

            PopulateListOfSubDirectoriesInPath(path, directories);

            // TODO: populate "files" with bitwise operators that result in a number, which is the key of a dictionary in the class:
            /*
                Dictionary<int, Func<string, string, List<string>>> { [1] = x, [2] = y, [4] = z }
            */
            // TODO: also check if a delegate can handle an arbitrary number of arguments

            List<string> files = new List<string>();

            foreach (var fileName in files)
            {
                if (fileName.Contains(search))
                    ResultsListBox.Items.Add(fileName);
            }
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
                fileNames.AddRange(FilterFilesByFileTypes(directory, fileTypes));
            }

            return fileNames;
        }

        private static List<string> GetAllFileNamesBySearchPatternFilteredByFileTypes(List<string> directories, string searchPattern, string fileTypes)
        {
            List<string> fileNames = new();

            foreach (var directory in directories)
            {
                fileNames.AddRange(FilterFilesByFileTypesAndSearchPattern(directory, searchPattern, fileTypes));
            }

            return fileNames;
        }

        private static List<string> GetAllFileNamesBySearchPattern(List<string> directories, string searchPattern)
        {
            List<string> fileNames = new();

            foreach (var directory in directories)
            {
                fileNames.AddRange(Directory.GetFiles(directory, searchPattern));
            }

            return fileNames;
        }

        private static List<string> FilterFilesByFileTypes(string directory, string fileTypes)
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

        private static List<string> FilterFilesByFileTypesAndSearchPattern(string directory, string fileTypes, string searchPattern)
        {
            List<string> filtered = new();
            string[] filesInDirectory = Directory.GetFiles(directory, searchPattern);

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

        private static void PopulateListOfSubDirectoriesInPath(string path, List<string> subDirectories)
        {
            string[] directories = Directory.GetDirectories(path);

            foreach (var directoryPath in directories)
            {
                subDirectories.Add(directoryPath);
                PopulateListOfSubDirectoriesInPath(directoryPath, subDirectories);
            }
        }

        private static bool TextBoxHasValue(TextBox element) => !string.IsNullOrEmpty(element.Text) && !string.IsNullOrWhiteSpace(element.Text);

        private static void ShowEmptyFieldAlert(TextBox textBox)
        {
            MessageBox.Show($"The {textBox.Name.Replace("TextBox", "")} field must have a value.");
        }
    }
}