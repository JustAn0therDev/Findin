namespace Findin
{
    internal record FormState(
        string Path, 
        string FilePatterns, 
        string Search, 
        string DefaultProgramPath,
        string IgnoredDirectories);
}
