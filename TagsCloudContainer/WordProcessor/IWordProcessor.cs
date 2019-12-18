using System;
using System.Collections.Generic;
using System.Text;
using TagsCloudContainer.UI;

namespace TagsCloudContainer.WordProcessor
{
    public interface IWordProcessor
    {
        Result<IEnumerable<WordWithCount>> ProcessWords(IEnumerable<string> words);
    }
}
