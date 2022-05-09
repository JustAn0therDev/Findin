using System.Text;

namespace Findin
{
    internal class FileDictionaryWrapper
    {
        public Dictionary<string, StringBuilder> FileNamesToContent { get; private set; } = new();
        private FileSystemWatcher Watcher { get; set; }
        private string Path { get; set; }
        private string[] FileTypes { get; set; }
        private string[] IgnoredDirectories { get; set; }

        public void Watch(string path, string fileTypes, string[] ignoredDirectories)
        {
            if (Watcher != null)
            {
                Watcher.Dispose();
            }

            FileNamesToContent.Clear();

            Path = path;
            FileTypes = fileTypes.Split(';');
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

            Watcher.Changed += OnFileChanged;
            Watcher.Deleted += OnFileDeleted;
            Watcher.Created += OnFileCreated;
            Watcher.Renamed += OnFileRenamed;

            Watcher.EnableRaisingEvents = true;
        }

        private void PopulateDictionary()
        {
            string[] allFileNames = Directory.GetFiles(Path, "*.*", SearchOption.AllDirectories);

            foreach (var filePath in allFileNames)
            {
                if (FileTypeIsInDesiredFileTypes(filePath) && !InIgnoredDirectories(filePath))
                {
                    FileNamesToContent.Add(filePath, new StringBuilder(File.ReadAllText(filePath)));
                }
            }
        }

        private bool FileTypeIsInDesiredFileTypes(string filePath)
        {
            foreach (var fileType in FileTypes)
            {
                if (filePath.EndsWith(fileType))
                    return true;
            }

            return false;
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

        private void OnFileChanged(object sender, FileSystemEventArgs fileSystemEvent)
        {
            try
            {
                if (FileNamesToContent.ContainsKey(fileSystemEvent.FullPath) && FileTypeIsInDesiredFileTypes(fileSystemEvent.FullPath))
                {
                    FileNamesToContent[fileSystemEvent.FullPath] = new StringBuilder(File.ReadAllText(fileSystemEvent.FullPath));
                }
            }
            catch (IOException)
            {
                // File is being used by another process.
            }
        }

        private void OnFileDeleted(object sender, FileSystemEventArgs fileSystemEvent)
        {
            try
            {
                if (FileNamesToContent.ContainsKey(fileSystemEvent.FullPath))
                {
                    FileNamesToContent.Remove(fileSystemEvent.FullPath);
                }
            }
            catch (IOException)
            {
                // File is being used by another process.
            }
        }

        private void OnFileCreated(object sender, FileSystemEventArgs fileSystemEvent)
        {
            try
            {
                if (FileTypeIsInDesiredFileTypes(fileSystemEvent.FullPath))
                    FileNamesToContent.Add(fileSystemEvent.FullPath, new StringBuilder(File.ReadAllText(fileSystemEvent.FullPath)));
            }
            catch (IOException)
            {
                // File is being used by another process.
            }
        }

        private void OnFileRenamed(object sender, RenamedEventArgs renamedEvent)
        {
            try
            {
                // @Important: This is a "Visual Studio-only" operation, since the IDE first renames the file to a ".TMP" file
                // before applying any changes.
                if (renamedEvent.FullPath.EndsWith("TMP") && FileNamesToContent.ContainsKey(renamedEvent.OldFullPath))
                {
                    FileNamesToContent[renamedEvent.OldFullPath] = new StringBuilder(File.ReadAllText(renamedEvent.FullPath));
                    return;
                }

                if (FileNamesToContent.ContainsKey(renamedEvent.OldFullPath) && FileTypeIsInDesiredFileTypes(renamedEvent.FullPath))
                {
                    FileNamesToContent.Add(renamedEvent.FullPath, FileNamesToContent[renamedEvent.OldFullPath]);
                    FileNamesToContent.Remove(renamedEvent.OldFullPath);
                }
            }
            catch (IOException)
            {
                // File is being used by another process.
            }
        }
    }
}
