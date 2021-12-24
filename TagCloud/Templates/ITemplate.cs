using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.Templates;

public interface ITemplate
{
    IEnumerable<WordParameter> GetWordParameters();
    Size ImageSize { get; }
    Color BackgroundColor { get; }
}