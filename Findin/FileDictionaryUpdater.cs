using System.Text;

namespace Findin
{
    internal class FileDictionaryWrapper : IDisposable
    {
        public Dictionary<string, StringBuilder> FileNamesToContent { get; private set; } = new();
        private FileSystemWatcher Watcher { get; set; }
        private string Path { get; set; }
        private string FileTypes { get; set; }
        private string[] IgnoredDirectories { get; set; }

        public void Watch(string path, string fileTypes, string[] ignoredDirectories)
        {
            if (Watcher != null)
                Watcher.Dispose();

            FileNamesToContent.Clear();

            Path = path;
            FileTypes = fileTypes;
            IgnoredDirectories = ignoredDirectories;

            PopulateDictionary();

            Watcher = new(path);

            Watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Size;

            foreach (var fileType in fileTypes.Split(';'))
            {
                Watcher.Filters.Add("*." + fileType);
            }

            Watcher.IncludeSubdirectories = true;

            Watcher.Changed += OnFileChange;
            Watcher.Deleted += OnFileChange;
            Watcher.Created += OnFileChange;
            Watcher.Renamed += OnFileChange;
            
            Watcher.EnableRaisingEvents = true;
        }

        private void PopulateDictionary()
        {
            string[] fileTypeArray = FileTypes.Split(';');
            string[] allFileNames = Directory.GetFiles(Path, "*.*", SearchOption.AllDirectories);

            foreach (var filePath in allFileNames)
            {
                foreach (var fileType in fileTypeArray)
                {
                    if (filePath.EndsWith(fileType) && !InIgnoredDirectories(filePath))
                    {
                        FileNamesToContent.Add(filePath, new StringBuilder(File.ReadAllText(filePath)));
                    }
                }
            }
        }

        public bool InIgnoredDirectories(string directory)
        {
            foreach (var dir in IgnoredDirectories)
            {
                foreach (var path in directory.Split('\\'))
                {
                    if (path == dir)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void OnFileChange(object sender, FileSystemEventArgs fileSystemEvent) => Watch(Path, FileTypes, IgnoredDirectories);

        public void Dispose()
        {
            Watcher.Dispose();
        }
    }
}
