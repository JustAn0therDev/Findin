using System.Text;

namespace Findin
{
    internal class FileDictionaryWrapper : IDisposable
    {
        public static Dictionary<string, StringBuilder> FileNamesToContent { get; } = new();
        private FileSystemWatcher Watcher { get; set; }

        /// <summary>
        /// Disposes an old FileSystemWatcher object and creates a new one to watch the path
        /// </summary>
        /// <param name="path">The path to watch for changes</param>
        public void Watch(string path, string fileTypes, string[] ignoredDirectories)
        {
            if (Watcher != null)
                Watcher.Dispose();

            FileNamesToContent.Clear();

            PopulateDictionary(path, fileTypes, ignoredDirectories);

            Watcher = new(path);

            Watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            foreach (var fileType in fileTypes.Split(';'))
            {
                Watcher.Filters.Add("*." + fileType);
            }

            Watcher.IncludeSubdirectories = true;
            Watcher.EnableRaisingEvents = true;
            
            Watcher.Changed += OnFileChanged;
            Watcher.Deleted += OnFileDeleted;
            Watcher.Created += OnFileCreated;
            Watcher.Renamed += OnFileRenamed;
        }

        private static void PopulateDictionary(string path, string fileTypes, string[] ignoredDirectories)
        {
            string[] fileTypeArray = fileTypes.Split(';');
            string[] allFileNames = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

            foreach (var filePath in allFileNames)
            {
                foreach (var fileType in fileTypeArray)
                {
                    if (filePath.EndsWith(fileType) && !InIgnoredDirectories(ignoredDirectories, filePath))
                    {
                        FileNamesToContent.Add(filePath, new StringBuilder(File.ReadAllText(filePath)));
                    }
                }
            }
        }

        public static bool InIgnoredDirectories(string[] directoriesToIgnore, string directory)
        {
            foreach (var dir in directoriesToIgnore)
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

        private static void OnFileChanged(object sender, FileSystemEventArgs fileSystemEvent)
        {
            if (FileNamesToContent.ContainsKey(fileSystemEvent.FullPath))
                FileNamesToContent[fileSystemEvent.FullPath] = new StringBuilder(File.ReadAllText(fileSystemEvent.FullPath));
        }

        private static void OnFileDeleted(object sender, FileSystemEventArgs fileSystemEvent)
        {
            if (FileNamesToContent.ContainsKey(fileSystemEvent.FullPath))
                FileNamesToContent.Remove(fileSystemEvent.FullPath);
        }

        private static void OnFileCreated(object sender, FileSystemEventArgs fileSystemEvent)
        {
            FileNamesToContent.Add(fileSystemEvent.FullPath, new StringBuilder(File.ReadAllText(fileSystemEvent.FullPath)));
        }

        private static void OnFileRenamed(object sender, RenamedEventArgs renamedEvent)
        {
            if (FileNamesToContent.ContainsKey(renamedEvent.OldFullPath))
            {
                FileNamesToContent.Add(renamedEvent.FullPath, FileNamesToContent[renamedEvent.OldFullPath]);
                FileNamesToContent.Remove(renamedEvent.OldFullPath);
            }
        }

        public void Dispose()
        {
            Watcher.Dispose();
        }
    }
}
