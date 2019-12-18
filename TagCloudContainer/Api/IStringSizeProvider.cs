using System.Drawing;
using TagCloudContainer.ResultMonad;

namespace TagCloudContainer.Api
{
    [CliRole]
    public interface IStringSizeProvider
    {
        Result<Size> GetStringSize(string word, int occurrenceCount);
    }
}