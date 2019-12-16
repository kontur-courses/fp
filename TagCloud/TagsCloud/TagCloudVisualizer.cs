using System.Drawing;
using TagsCloud.Interfaces;
using TagsCloud.ErrorHandling;

namespace TagsCloud
{
    public class TagCloudVisualizer
    {
        private readonly IWordStream wordStream;
        private readonly ITagGenerator tagGenerator;
        private readonly ITagCloudGenerator tagCloudGenerator;
        private readonly ICloudDrawer cloudDrawer;
        private readonly IImageSaver imageSaver;
        private readonly IWordCounter wordCounter;
        private readonly TagCloudSettings tagCloudSettings;

        public TagCloudVisualizer(IWordStream wordStream,
            ITagGenerator tagGenerator,
            ITagCloudGenerator tagCloudGenerator,
            ICloudDrawer cloudDrawer,
            IImageSaver imageSaver,
            IWordCounter wordCounter,
            TagCloudSettings tagCloudSettings)
        {
            this.wordStream = wordStream;
            this.tagGenerator = tagGenerator;
            this.tagCloudGenerator = tagCloudGenerator;
            this.cloudDrawer = cloudDrawer;
            this.imageSaver = imageSaver;
            this.wordCounter = wordCounter;
            this.tagCloudSettings = tagCloudSettings;
        }

        public Result<None> GenerateTagCloud()
        {
            return wordStream.GetWords(tagCloudSettings.pathToInput)
                 .Then(words => wordCounter.GetWordsStatistics(words))
                 .Then(wordsStatistics => tagGenerator.GenerateTag(wordsStatistics))
                 .Then(tags => tagCloudGenerator.GenerateTagCloud(tags))
                 .Then(tagCloud => cloudDrawer.Paint(tagCloud, new Size(tagCloudSettings.widthOutputImage, tagCloudSettings.heightOutputImage), tagCloudSettings.backgroundColor, 15))
                 .Then(image => imageSaver.SaveImage(image, tagCloudSettings.pathToOutput, tagCloudSettings.imageFormat));
        }
    }
}
