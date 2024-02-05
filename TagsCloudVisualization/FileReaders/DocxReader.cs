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
        try
        {
            var doc = DocX.Load(path);
            return string.Join(" ", doc.Paragraphs.Select(p => p.Text));
        }
        catch (Exception e)
        {
            return Result.Fail<string>(e.Message);
        }
    }
}