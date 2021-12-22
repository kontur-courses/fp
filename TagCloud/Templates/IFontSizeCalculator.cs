using System.Collections.Generic;

namespace TagCloud.Templates;

public interface IFontSizeCalculator
{
    Dictionary<string, float> GetFontSizes(IEnumerable<string> words);
}