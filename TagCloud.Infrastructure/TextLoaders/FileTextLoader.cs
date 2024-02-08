public class FileTextLoader : ITextLoader
{
    public Result<string> Load(string path)
    {
        return Result.Of(() => File.ReadAllText(path))
            .RefineError($"Can`t read text file with path {path}"); ;
    }
}
