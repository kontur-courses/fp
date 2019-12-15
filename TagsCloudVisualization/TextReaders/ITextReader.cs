using System.Text;
using TagsCloudVisualization.ErrorHandling;

namespace TagsCloudVisualization.TextReaders
{
    public interface ITextReader
    {
        Result<string> ReadText(string filePath, Encoding encoding);
    }
}