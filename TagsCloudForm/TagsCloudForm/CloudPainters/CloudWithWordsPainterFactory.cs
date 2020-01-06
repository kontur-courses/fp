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
            var layouter = circularCloudLayouterFactory.Invoke(new Point(settings.CenterX, settings.CenterY));
            var filterFuncs = CreateFuncsFromFilters(filters);
            return Result
                .Of(() => textReader.ReadLines(settings.WordsSource), new List<string>())
                .Then(x => UseCaseSettings(x, settings))
                .Then(filterFuncs)
                .Then(x => parser.GetWordsFrequency(x, settings.Language))
                .Then(x=> new CloudWithWordsPainter(imageHolder, settings, palette, layouter, x))
                .Then(x=> (ICloudPainter)x);
        }

        private IEnumerable<string> UseCaseSettings(IEnumerable<string> input, ICircularCloudLayouterWithWordsSettings settings)
        {
            if (settings.UpperCase)
                return input.Select(y => y.ToUpper());
            return input.Select(y => y.ToLower());
        }

        private IEnumerable<Func<IEnumerable<string>, Result<IEnumerable<string>>>> CreateFuncsFromFilters(
            IEnumerable<IWordsFilter> filters)
        {
            return filters.Select<IWordsFilter, Func<IEnumerable<string>, Result<IEnumerable<string>>>>(
                x => a => x.Filter(settings, a)
            );
        }
    }
}
