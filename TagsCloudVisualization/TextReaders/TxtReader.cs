using System.IO;
using System.Text;
using TagsCloudVisualization.ErrorHandling;

namespace TagsCloudVisualization.TextReaders
{
    public class TxtReader : ITextReader
    {
        public Result<string> ReadText(string filePath, Encoding encoding)
        {
            return Result.Of(() => new FileStream(filePath, FileMode.Open))
                .RefineError($"Could not open file {filePath}")
                .Then(stream => new StreamReader(stream, encoding))
                .Then(ReadStream)
                .OnFail(new ConsoleErrorHandler().HandleError);
        }

        private string ReadStream(StreamReader streamReader)
        {
            using (streamReader)
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}