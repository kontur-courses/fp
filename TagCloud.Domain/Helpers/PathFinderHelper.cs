public static class PathFinderHelper
{
    public static string GetPathToFile(string fileName)
    {
#if DEBUG
        var projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;

        return Directory.GetFiles(
                Directory.GetParent(projectPath).FullName,
                fileName, SearchOption.AllDirectories)[0];
#else
        return fileName; 
#endif
    }
}

