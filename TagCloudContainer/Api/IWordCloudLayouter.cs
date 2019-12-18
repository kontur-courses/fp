using System.Collections.Generic;
using System.Drawing;
using TagCloudContainer.ResultMonad;

namespace TagCloudContainer.Api
{
    [CliRole]
    public interface IWordCloudLayouter
    {
        Result<IReadOnlyDictionary<string, Rectangle>> AddWords(IReadOnlyDictionary<string, int> words,
            List<Rectangle> containter);
    }
}