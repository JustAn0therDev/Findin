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

            string path = PathTextBox.Text;

            string search = SearchTextBox.Text;

            string fileTypes = FileTypeTextBox.Text;

            List<string> directories = new() { Directory.GetCurrentDirectory() };

            PopulateListOfSubDirectoriesInPath(directories, path);

            // TODO: populate "files" with bitwise operators that result in a number, which is the key of a dictionary in the class:
            /*
                Dictionary<int, Func<string, string, List<string>>> { [1] = x, [2] = y, [4] = z }
            */
            // TODO: also check if a delegate can handle an arbitrary number of arguments

            List<string> files;

            if (string.IsNullOrEmpty(fileTypes))
            {
                files = GetAllFileNames(directories);
            }
            else
            {
                files = GetAllFileNamesFilteredByFileTypes(directories, fileTypes);
            }

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
                fileNames.AddRange(FilterFilesByFileTypes(directory, fileTypes.Split(';')));
            }

            return fileNames;
        }

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