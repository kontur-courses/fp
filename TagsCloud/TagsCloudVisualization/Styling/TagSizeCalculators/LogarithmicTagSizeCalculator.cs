using System;
using TagCloudResult;

namespace TagsCloudVisualization.Styling.TagSizeCalculators
{
    public class LogarithmicTagSizeCalculator : TagSizeCalculator
    {
        public override Result<float> GetScaleFactor(int wordCount, int minFontSize)
        {
            return Result.Ok((float) (Math.Log(wordCount + 1) * minFontSize));
        }
    }
}