using System.Collections.Generic;
using ResultOf;

namespace CloudDrawing
{
    public interface ICircularCloudDrawing
    {
        Result<None> SetOptions(ImageSettings imageSettings);
        Result<None>  DrawWords(IEnumerable<(string, int)> wordsFontSize, WordDrawSettings settings);
        Result<None> SaveImage(string filename);
    }
}