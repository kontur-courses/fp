using ResultOf;
using TagsCloudVisualization.CloudGenerating;
using TagsCloudVisualization.ImageSaving;
using TagsCloudVisualization.Preprocessing;
using TagsCloudVisualization.Utils;
using TagsCloudVisualization.Visualizing;
using TagsCloudVisualization.WordsFileReading;

namespace TagsCloudConsole
{
    class App
    {
        private readonly Preprocessor preprocessor;
        private readonly ITagsCloudGenerator tagsCloudGenerator;
        private readonly TagsCloudVisualizer cloudVisualizer;
        private readonly FileReaderSelector fileReaderSelector;
        private readonly ParserSelector parserSelector;
        private readonly ImageSaverSelector imageSaverSelector;

        public App(
            FileReaderSelector fileReaderSelector, 
            ParserSelector parserSelector,
            Preprocessor preprocessor,
            ITagsCloudGenerator tagsCloudGenerator,
            TagsCloudVisualizer cloudVisualizer,
            ImageSaverSelector imageSaverSelector)
        {
            this.fileReaderSelector = fileReaderSelector;
            this.parserSelector = parserSelector;
            this.preprocessor = preprocessor;
            this.tagsCloudGenerator = tagsCloudGenerator;
            this.cloudVisualizer = cloudVisualizer;
            this.imageSaverSelector = imageSaverSelector;
        }

        public Result<None> Run(string imageFileName, string wordsFileName, string mode)
        {
            return fileReaderSelector.SelectFileReader(wordsFileName)
                .Then(reader => reader.ReadText(wordsFileName))
                .Then(words => parserSelector.SelectParser(mode)
                    .Then(parser => parser.ParseText(words))
                    .Then(parsedWords => preprocessor.Preprocess(parsedWords))
                    .Then(preprocessedWords => new StatisticsCalculator().CalculateStatistics(preprocessedWords))
                    .Then(stat => tagsCloudGenerator.GenerateTagsCloud(stat))
                    .Then(tagCloud => cloudVisualizer.GetPictureOfRectangles(tagCloud))
                    .Then(picture => imageSaverSelector.SelectImageSaver(imageFileName)
                        .Then(imageSaver => imageSaver.SaveImage(picture, imageFileName))
                    )
                );
        }
    }
}
