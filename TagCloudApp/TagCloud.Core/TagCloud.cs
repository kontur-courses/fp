using System.Drawing;
using System.Drawing.Imaging;
using TagCloud.Core.Settings.Interfaces;
using TagCloud.Core.Util;
using TagCloud.Core.Visualizers;
using TagCloud.Core.WordsParsing;

namespace TagCloud.Core
{
    public class TagCloud
    {
        private readonly WordsParser wordsParser;
        private readonly ITagCloudVisualizer visualizer;
        private readonly ITagCloudSettings settings;

        public TagCloud(WordsParser wordsParser, ITagCloudVisualizer visualizer, ITagCloudSettings settings)
        {
            this.wordsParser = wordsParser;
            this.visualizer = visualizer;
            this.settings = settings;
        }

        public Result<None> MakeTagCloudAndSave()
        {
            return MakeTagCloud()
                .Then(Save);
        }

        public Result<Bitmap> MakeTagCloud()
        {
            return wordsParser.Parse(settings.PathToWords, settings.PathToBoringWords)
                .Then(tagStats => visualizer.Render(tagStats));
        }

        public void Save(Image image)
        {
            ImageFormatResolver.TryResolveFromFileName(settings.PathForResultImage)
                .Then(imageFormat => image.Save(settings.PathForResultImage, imageFormat))
                .OnFail(err => image.Save(settings.PathForResultImage, ImageFormat.Png));
        }
    }
}
