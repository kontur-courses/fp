using System;
using System.Collections.Generic;
using System.Drawing.Imaging;

namespace TagsCloudVisualizationDI.TextAnalyze.Visualization
{
    public interface IVisualization : IDisposable
    {
        Result<None> DrawAndSaveImage(List<RectangleWithWord> elements, string savePath, ImageFormat format);
        List<RectangleWithWord> FindSizeForElements(Dictionary<string, RectangleWithWord> formedElements);
    }
}
