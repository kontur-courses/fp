using Result;
namespace TagCloud;

public class TxtFileLoader : IFileLoader
{
    public Result<string> Load(string path)
    {
        if (string.IsNullOrEmpty(path))
            return new Result<string>(null, "Path is null or empty");
        
        if (!File.Exists(path))
            return new Result<string>(null, $"File with path \"{path}\" isn't found: ");
        
        if (Path.GetExtension(path) != ".txt")
            return new Result<string>(null, "File format is not .txt");
        
        return new Result<string>(File.ReadAllText(path));
    }
}