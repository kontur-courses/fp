using System.Collections.Generic;
using CTV.Common;

namespace CTV.Common
{
    public interface IWordSizer
    {
        public List<SizedWord> Convert(string[] words, float maxFontSize);
    }
}