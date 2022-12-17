using System.Collections.Generic;

namespace TagsCloud.Interfaces
{
    public interface ITextSplitter
    {
        public IEnumerable<string> SplitTextOnWords(string text);
    }
}