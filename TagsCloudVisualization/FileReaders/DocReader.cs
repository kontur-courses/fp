using Results;
using Spire.Doc;

namespace TagsCloudVisualization.FileReaders;

public class DocReader : IFileReader
{
    public bool CanRead(string path)
    {
        return path.Split('.')[^1] == "doc";
    }

    public Result<string> ReadText(string path)
    {
        try
        {
            var doc = new Document();
            doc.LoadFromFile(path);
            var text = doc.GetText();
            return Result.Ok(string.Join(" ", text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Skip(1)));
        }
        catch (Exception e)
        {
            return Result.Fail<string>(e.Message);
        }
    }
}