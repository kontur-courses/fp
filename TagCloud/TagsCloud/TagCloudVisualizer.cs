using System.Drawing;
using TagsCloud.ErrorHandling;
using TagsCloud.Interfaces;
using TagsCloud.WordStreams;
using TagsCloud.WordValidators;

namespace TagsCloud
{
    public class TagCloudVisualizer
    {
        private readonly BoringWordStream boringWordStream;
        private bool boringWordsWasRead;
        private readonly ICloudDrawer cloudDrawer;
        private readonly IImageSaver imageSaver;
        private readonly ITagCloudGenerator tagCloudGenerator;
        private readonly TagCloudSettings tagCloudSettings;
        private readonly ITagGenerator tagGenerator;
        private readonly IWordCounter wordCounter;
        private readonly WordStream wordStream;

        public TagCloudVisualizer(WordStream wordStream,
            ITagGenerator tagGenerator,
            ITagCloudGenerator tagCloudGenerator,
            ICloudDrawer cloudDrawer,
            IImageSaver imageSaver,
            IWordCounter wordCounter,
            TagCloudSettings tagCloudSettings,
            BoringWordStream boringWordStream)
        {
            this.wordStream = wordStream;
            this.tagGenerator = tagGenerator;
            this.tagCloudGenerator = tagCloudGenerator;
            this.cloudDrawer = cloudDrawer;
            this.imageSaver = imageSaver;
            this.wordCounter = wordCounter;
            this.tagCloudSettings = tagCloudSettings;
            this.boringWordStream = boringWordStream;
            boringWordsWasRead = false;
        }

        public Result<None> ReadBoringWords(string path)
        {
            return path == string.Empty
                ? new Result<None>()
                : boringWordStream.GetWords(path)
                    .Then(words => new BoringWordsValidator(words))
                    .Then(wordStream.AddNewValidator)
                    .RefineError("Failed to initialize boring words filter");
        }

        public Result<None> GenerateTagCloud()
        {
            return (boringWordsWasRead ? new Result<None>() : ReadBoringWords(tagCloudSettings.pathToBoringWords))
                .Then(_ => boringWordsWasRead = true)
                .Then(_ => wordStream.GetWords(tagCloudSettings.pathToInput))
                .Then(wordCounter.GetWordsStatistics)
                .Then(tagGenerator.GenerateTags)
                .Then(tagCloudGenerator.GenerateTagCloud)
                .Then(tagCloud => cloudDrawer.Paint(tagCloud,
                    new Size(tagCloudSettings.widthOutputImage, tagCloudSettings.heightOutputImage),
                    tagCloudSettings.backgroundColor, 15))
                .Then(image =>
                    imageSaver.SaveImage(image, tagCloudSettings.pathToOutput, tagCloudSettings.imageFormat));
        }
    }
}