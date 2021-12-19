using System.Collections.Generic;
using ResultOf;
using TagCloud.Drawing;

namespace TagCloud.PreLayout
{
    public interface IWordLayouter
    {
        List<Result<Word>> Layout(IDrawerOptions options, Dictionary<string, int> wordsWithFrequency);
    }
}