using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.App.Cloud.Words
{
    public interface IWordCounter
    {
        Result<List<GraphicWord>> Count(string row);
        Font Font { get; set; }
    }
}
