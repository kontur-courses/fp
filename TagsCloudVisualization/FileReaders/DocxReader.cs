using Results;
using Xceed.Words.NET;

namespace TagsCloudVisualization.FileReaders;

public class DocxReader : IFileReader
{
    public bool CanRead(string path)
    {
        return path.Split('.')[^1] == "docx";
    }

    public Result<string> ReadText(string path)
    {
        if (!File.Exists(path))
            return Result.Fail<string>($"Can't find file with path {Path.GetFullPath(path)}");
        var doc = DocX.Load(path);
        return string.Join(" ", doc.Paragraphs.Select(p => p.Text));
    }
}