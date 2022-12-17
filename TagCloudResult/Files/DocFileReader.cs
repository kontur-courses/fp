using Microsoft.Office.Interop.Word;

namespace TagCloudResult.Files;

public class DocFileReader : IFileReader
{
    public string Extension => ".docx";

    public string ReadAll(string filename)
    {
        var app = new Application();
        var doc = app.Documents.Open(filename);
        var range = doc.Range();
        return range.Text;
    }
}