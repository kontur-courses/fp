using System.Drawing;

namespace TagCloudContainer.Api
{
    [CliRole]
    public interface IWordBrushProvider
    {
        Brush CreateBrushForWord(string word, int occurrenceCount);
    }
}