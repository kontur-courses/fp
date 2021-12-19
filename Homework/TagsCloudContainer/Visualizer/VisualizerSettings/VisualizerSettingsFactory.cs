using System.Drawing;
using TagsCloudContainer.Visualizer.ColorGenerators;

namespace TagsCloudContainer.Visualizer.VisualizerSettings
{
    public class VisualizerSettingsFactory : IFactory<IVisualizerSettings>
    {
        private readonly IResolver<PalleteType, IColorGenerator> colorsGeneratorResolver;
        private readonly ITagCloudSettings settings;

        public VisualizerSettingsFactory(IResolver<PalleteType, IColorGenerator> colorsGeneratorResolver,
            ITagCloudSettings settings)
        {
            this.colorsGeneratorResolver = colorsGeneratorResolver;
            this.settings = settings;
        }

        public IVisualizerSettings Create()
        {
            var colorAlgorithm = colorsGeneratorResolver.Get(settings.ColoringAlgorythm).GetValueOrThrow();
            var backgroundColor = Color.FromName(settings.BackgroundColor);
            var fontFamily = new FontFamily(settings.FontName);
            var font = new Font(fontFamily, settings.FontSize);
            var imageSize = new Size(settings.ImageWidth, settings.ImageHeight);
            return new VisualizerSettings(colorAlgorithm, backgroundColor, font, imageSize);
        }
    }
}