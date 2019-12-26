using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CircularCloudLayouter;
using TagsCloudForm.CircularCloudLayouterSettings;
using TagsCloudForm.Common;
using TagsCloudForm.WordFilters;

namespace TagsCloudForm.CloudPainters
{
    public class CloudWithWordsPainterFactory : IPainterFactory
    {
        private readonly IImageHolder imageHolder;
        private readonly IPalette palette;
        private readonly Func<Point, ICircularCloudLayouter> circularCloudLayouterFactory;
        private readonly ICircularCloudLayouterWithWordsSettings settings;
        private readonly IWordsFilter[] filters;
        private readonly IWordsFrequencyParser parser;
        private readonly ITextReader textReader;
        public CloudWithWordsPainterFactory(IImageHolder imageHolder,
            IPalette palette,
            ICircularCloudLayouterWithWordsSettings settings,
            ITextReader textReader,
            Func<Point, ICircularCloudLayouter> circularCloudLayouterFactory, IWordsFilter[] filters, IWordsFrequencyParser parser)
        {
            this.imageHolder = imageHolder;
            this.palette = palette;
            this.circularCloudLayouterFactory = circularCloudLayouterFactory;
            this.settings = settings;
            this.filters = filters;
            this.parser = parser;
            this.textReader = textReader;
        }

        public Result<ICloudPainter> Create()
        {
            var errors = new StringBuilder();
            IEnumerable<string> lines;
            var linesResult = textReader.ReadLines(settings.WordsSource);
            if (!linesResult.IsSuccess)
            {
                errors.Append("\n");
                errors.Append(linesResult.Error);
                lines = new List<string>();
            }
            else
            {
                lines = linesResult.Value;
            }
            if (settings.UpperCase)
                lines = lines.Select(x => x.ToUpper());
            else
                lines = lines.Select(x => x.ToLower());
            foreach (var filter in filters)
            {
                Result<IEnumerable<string>> filtered;
                filtered = filter.Filter(settings, lines);
                if (filtered.IsSuccess)
                    lines = filtered.Value;
                else
                {
                    errors.Append("\n");
                    errors.Append(filtered.Error);
                }
            }
            var wordsWithFrequency = parser.GetWordsFrequency(lines, settings.Language);
            var layouter = circularCloudLayouterFactory.Invoke(new Point(settings.CenterX, settings.CenterY));
            var painter = new CloudWithWordsPainter(imageHolder, settings, palette, layouter, wordsWithFrequency);
            return errors.Length == 0 ? 
                Result.Ok<ICloudPainter>(painter) : 
                new Result<ICloudPainter>(errors.ToString(), painter);
        }
    }
}
