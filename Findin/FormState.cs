namespace Findin
{
    internal record FormState(
        string Path, 
        string FileTypes, 
        string Search, 
        string DefaultProgramPath,
        string IgnoredDirectories);
}
