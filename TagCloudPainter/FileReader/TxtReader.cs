using TagCloudPainter.ResultOf;

namespace TagCloudPainter.FileReader;

public class TxtReader : IFileReader
{
    public Result<IEnumerable<string>> ReadFile(string path)
    {
        if (!path.EndsWith("txt"))
            return Result.Fail<IEnumerable<string>>("file is not in txt format");

        if (!File.Exists(path))
            return Result.Fail<IEnumerable<string>>($"path {path} does not exist ");

        return Result.Of(()=>File.ReadAllLines(path).Where(x => x != ""), "Cannot read file");
    }
}