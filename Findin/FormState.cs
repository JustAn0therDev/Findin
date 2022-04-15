namespace Findin
{
    internal record FormState(
        string Path, 
        string FileTypes, 
        string Search, 
        bool IgnoreCaseIsChecked, 
        string DefaultProgramPath,
        string IgnoredDirectories);
}
