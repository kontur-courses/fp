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
        try
        {
            return Result.Ok(File.ReadAllText(path));
        }
        catch (Exception e)
        {
            return Result.Fail<string>(e.Message);
        }
    }
}