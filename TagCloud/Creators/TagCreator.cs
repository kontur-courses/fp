using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.ResultMonad;
using TagCloud.Settings;

namespace TagCloud.Creators
{
    public class TagCreator : ITagCreator
    {
        private readonly ITagCreatorSettings drawingSettings;

        public TagCreator(ITagCreatorSettings settings)
        {
            drawingSettings = settings;
        }

        public Tag Create(string value, int frequency, Graphics graphics)
        {
            var font = new Font(drawingSettings.FontName,
                drawingSettings.FontSize * frequency + 2);
            var size = graphics.MeasureString(value, font).ToSize();
            return new Tag(value, frequency, size);
        }

        public Result<IEnumerable<Tag>> Create(Dictionary<string, int> wordsWithFrequency)
        {
            using var graphics = Graphics.FromHwnd(new IntPtr());
            if (drawingSettings.FontSize <= 0)
                return Result.Fail<IEnumerable<Tag>>("Non-positive font size");
            var renderFont = new Font(drawingSettings.FontName, drawingSettings.FontSize);
            if (renderFont.Name != drawingSettings.FontName)
                return Result.Fail<IEnumerable<Tag>>("Font not found");
            return wordsWithFrequency.Select(pair => Create(pair.Key, pair.Value, graphics))
                .ToArray();
        }
    }
}
