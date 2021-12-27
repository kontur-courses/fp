using System;
using System.Collections.Generic;
using System.Drawing;

namespace CTV.Common
{
    public interface IWordSizer
    {
        public List<SizedWord> Convert(string[] words, Font font, Func<string, Font, Size> getWordSize);
    }
}