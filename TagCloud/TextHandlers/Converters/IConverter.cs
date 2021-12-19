using System.Collections.Generic;

namespace TagCloud.TextHandlers.Converters
{
    public interface IConverter
    {
       Result<string> Convert(string word);
    }
}