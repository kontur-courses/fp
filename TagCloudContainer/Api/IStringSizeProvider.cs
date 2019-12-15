using System.Drawing;

namespace TagCloudContainer.Api
{
    [CliRole]
    public interface IStringSizeProvider
    {
        Size GetStringSize(string word, int occurrenceCount);
    }
}