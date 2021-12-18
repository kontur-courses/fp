using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudDrawer.ImageSaveService;
using TagsCloudDrawer.ImageSettings;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.CloudLayouter.VectorsGenerator;
using TagsCloudVisualization.Drawable.Tags.Settings;
using TagsCloudVisualization.WordsPreprocessor;
using TagsCloudVisualization.WordsProvider;

namespace TagsCloudVisualization
{
    public class TagsCloudVisualisationSettings
    {
        public ImageSettingsProvider ImageSettingsProvider { get; init; } = ImageSettingsProvider.Default;

        public TagDrawableSettingsProvider TagDrawableSettingsProvider { get; init; } =
            TagDrawableSettingsProvider.Default;

        public ILayouter Layouter { get; init; } =
            new NonIntersectedLayouter(Point.Empty, new CircularVectorsGenerator(0.005, 360));

        public IEnumerable<string> BoringWords { get; init; } = Array.Empty<string>();

        public IImageSaveService ImageSaveService { get; init; } = new PngSaveService();

        public IEnumerable<IWordsPreprocessor> WordsPreprocessors { get; init; } =
            Enumerable.Empty<IWordsPreprocessor>();

        public IWordsProvider WordsProvider { get; init; }
    }
}