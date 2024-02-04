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
        if (!File.Exists(path))
            return Result.Fail<string>($"Cant't find file with this path {Path.GetFullPath(path)}");
        var doc = new Document();
        doc.LoadFromFile(path);
        var text = doc.GetText();
        return string.Join(" ", text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Skip(1));
    }
}