using Results;

namespace TagsCloudVisualization.FileReaders;

public class TxtReader : IFileReader
{
    public bool CanRead(string path)
    {
        return path.Split('.')[^1] == "txt";
    }

    public Result<string> ReadText(string path)
    {
        if (!File.Exists(path))
            return Result.Fail<string>($"Can't find file with path {Path.GetFullPath(path)}");
        return Result.Ok(File.ReadAllText(path));
    }
}