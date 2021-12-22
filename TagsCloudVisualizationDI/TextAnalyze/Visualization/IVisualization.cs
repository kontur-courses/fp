using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualizationDI.TextAnalyze.Visualization
{
    public interface IVisualization : IDisposable
    {
        Result<None> DrawAndSaveImage(List<RectangleWithWord> elements, Result<string> savePathResult, ImageFormat format, Font font);
        Result<List<RectangleWithWord>> FindSizeForElements(Dictionary<string, RectangleWithWord> formedElements, Font font);
    }
}
