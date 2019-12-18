using TagCloudResult;

namespace TagsCloudTextProcessing.Readers
{
    public interface ITextReader
    {
        Result<string> ReadText();
    }
}