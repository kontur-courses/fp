using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.Templates;

public interface ITemplate
{
    IEnumerable<WordParameter> GetWordParameters();
    Size Size { get; }
    PointF Center { get; }
    Color BackgroundColor { get; }
}