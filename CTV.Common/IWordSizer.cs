using System.Collections.Generic;
using System.Drawing;
using CTV.Common;

namespace CTV.Common
{
    public interface IWordSizer
    {
        public List<SizedWord> Convert(string[] words, Font font, Graphics g);
    }
}