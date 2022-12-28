using TagCloudPainter.Extensions;
using TagCloudPainter.ResultOf;

namespace TagCloudPainter.FileReader;

public class TxtReader : IFileReader
{
    public Result<IEnumerable<string>> ReadFile(string path)
    {
        return path.ValidatePath("txt").
            Then(path => File.ReadAllLines(path).Where(x => x != ""));
    }
}