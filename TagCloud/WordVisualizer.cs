using System;
using System.Drawing;
using System.Linq;
using ResultOf;

namespace TagCloud
{
    public class WordVisualizer
    {
        private ITextAnalyzer Analyzer { get; }
        private ICloudLayouter Layouter { get; }
        private ICloudVisualizer Visualizer { get; }

        public WordVisualizer(ITextAnalyzer analyzer, ICloudLayouter layouter, ICloudVisualizer visualizer)
        {
            Layouter = layouter;
            Visualizer = visualizer;
            Analyzer = analyzer;
        }

        public Result<Bitmap> CreateCloudImage(IImageSettings settings)
        {
            return ConfigureVisualizer(settings)
                .Then(n => settings.GetTextColor())
                .Then(c => settings.GetBackgroundColor())
                .Then(c => settings.GetSize())
                .Then(s => Visualizer.CreateImage(
                    settings.GetTextColor().GetValueOrThrow(),
                    settings.GetBackgroundColor().GetValueOrThrow(),
                    settings.GetSize().GetValueOrThrow()))
                .RefineError("Creating image failed");
        }

        private Result<None> ConfigureVisualizer(IImageSettings settings)
        {
            var analyzerResult = Analyzer.GetWordList()
                .Then(l => Analyzer.GetMaxFrequency());
            if (!analyzerResult.IsSuccess)
                return Result.Fail<None>(analyzerResult.Error);
            
            foreach (var word in Analyzer.GetWordList().GetValueOrThrow().OrderByDescending(w => w.Frequency))
            {
                const double scalingParameter = 0.65;
                var fontSize =
                    100f * (float) Math.Pow((double) word.Frequency / Analyzer.GetMaxFrequency().GetValueOrThrow(),
                        scalingParameter);
                var result = settings.GetFont(fontSize)
                    .Then(font => MeasureString(word.Value, font))
                    .Then(size => Layouter.PutNextRectangle(size.ToSize()))
                    .Then(position => Visualizer.AddWord(word, position, settings.GetFont(fontSize).GetValueOrThrow()));
                
                if (!result.IsSuccess)
                    return result;
            }

            return Result.Ok();
        }

        private static SizeF MeasureString(string str, Font font)
        {
            var image = new Bitmap(1, 1);
            var graphics = Graphics.FromImage(image);
            return graphics.MeasureString(str, font);
        }
    }
}
