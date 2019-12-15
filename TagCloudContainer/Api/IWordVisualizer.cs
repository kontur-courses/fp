using System.Collections.Generic;
using System.Drawing;

namespace TagCloudContainer.Api
{
    [CliRole]
    public interface IWordVisualizer
    {
        Image CreateImageWithWords(IEnumerable<string> words);
    }
}