using System.Drawing;
using System.Linq;
using TagCloud.CloudLayouter;
using TagCloud.CloudVisualizerSpace;
using TagCloud.CloudVisualizerSpace.CloudViewConfigurationSpace;
using TagCloud.WordsPreprocessing.DocumentParsers;

namespace TagCloud.Interfaces
{
    public class BaseApplication
    {
        public ApplicationSettings AppSettings { get; }
        private readonly CloudVisualizer visualizer;
        public CloudViewConfiguration CloudViewConfiguration { get; }
        public CloudConfiguration CloudConfiguration { get; }
        public IDocumentParser[] Parsers { get; }

        public BaseApplication(CloudVisualizer visualizer, CloudViewConfiguration cloudViewConfiguration, CloudConfiguration cloudConfiguration,
            IDocumentParser[] parsers, ApplicationSettings settings)
        {
            AppSettings = settings;
            this.visualizer = visualizer;
            CloudViewConfiguration = cloudViewConfiguration;
            Parsers = parsers;
            CloudConfiguration = cloudConfiguration;
        }

        public Result<Image> GetImage()
        {
            var format = $".{AppSettings.FilePath.Split('.').Last()}";
            return Result.Of(() => Parsers.First(p => p.AllowedTypes.Contains(format)))
                .Then(p =>
                {
                    using (p)
                    {
                        return p.GetWords(AppSettings)
                            .Then(e => AppSettings.CurrentTextAnalyzer
                                .GetWords(e, CloudConfiguration.WordsCount))
                            .Then(visualizer.GetCloud)
                            .Then(x => (Image) x);
                    }
                });

        }

        public void Close()
        {
            foreach (var parser in Parsers)
            {
                parser.Dispose();
            }
        }

        public void SetFontFamily(string fontFamily)
        {
            CloudViewConfiguration.FontFamily = new FontFamily(fontFamily);
        }
    }
}
