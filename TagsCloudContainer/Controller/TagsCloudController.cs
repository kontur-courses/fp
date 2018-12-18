using System.IO;
using TagsCloudContainer.Configuration;
using TagsCloudContainer.DataReader;
using TagsCloudContainer.ImageWriter;
using TagsCloudContainer.Preprocessor;
using TagsCloudContainer.ResultOf;
using TagsCloudContainer.TagsGenerator;
using TagsCloudContainer.Visualizer;
using TagsCloudContainer.WordsCounter;

namespace TagsCloudContainer.Controller
{
    public class TagsCloudController : ITagsCloudController
    {
        private readonly IConfiguration configuration;
        private readonly IDataReader reader;
        private readonly IPreprocessor preprocessor;
        private readonly IWordsCounter wordsCounter;
        private readonly ITagsGenerator tagsGenerator;
        private readonly IVisualizer visualizer;
        private readonly IImageWriter imageWriter;

        public TagsCloudController(
            IConfiguration configuration,
            IDataReader reader,
            IPreprocessor preprocessor,
            IWordsCounter wordsCounter,
            ITagsGenerator tagsGenerator,
            IVisualizer visualizer,
            IImageWriter imageWriter)
        {
            this.configuration = configuration;
            this.reader = reader;
            this.preprocessor = preprocessor;
            this.wordsCounter = wordsCounter;
            this.tagsGenerator = tagsGenerator;
            this.visualizer = visualizer;
            this.imageWriter = imageWriter;
        }

        public Result<None> Save()
        {
            var pathToSave = Path.Combine(configuration.DirectoryToSave,
                $"{configuration.OutFileName}.{configuration.ImageFormat}");

            return reader.Read(configuration.PathToWordsFile)
                .Then(words => preprocessor.PrepareWords(words))
                .Then(preparedWords => wordsCounter.GetWordsFrequency(preparedWords))
                .Then(wordsFrequency => tagsGenerator.GenerateTags(wordsFrequency))
                .Then(tags => visualizer.Visualize(tags))
                .Then(image => imageWriter.Write(
                    image, pathToSave)
                .RefineError("Failed to save file"));
        }
    }
}