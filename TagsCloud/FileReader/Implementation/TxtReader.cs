using TagCloud.ResultImplementation;

namespace TagCloud.FileReader.Implementation;

public class TxtReader : IFileReader
{
    public Result<string[]> Read(string path)
    {
        try
        {
            return File.ReadAllLines(path);
        }
        catch (Exception e)
        {
            return Result.Fail<string[]>($"{e.Message}");
        }
    }
}