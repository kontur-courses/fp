#region

using System.Drawing;
using System.Reflection;
using TagsCloudVisualization.Interfaces;

#endregion

namespace TagsCloudVisualization
{
    public class TagCloudCreator : ITagCloudCreator
    {
        private readonly IFileReader fileReader;
        private readonly IFrequencyCounter frequencyCounter;
        private readonly IImageGenerator imageGenerator;
        private readonly ITagParser tagParser;
        private readonly IWordPreparator wordPreparator;

        public TagCloudCreator(IFileReader fileReader,
            IWordPreparator wordPreparator,
            IImageGenerator imageGenerator,
            IFrequencyCounter frequencyCounter,
            ITagParser tagParser)
        {
            this.fileReader = fileReader;
            this.wordPreparator = wordPreparator;
            this.imageGenerator = imageGenerator;
            this.frequencyCounter = frequencyCounter;
            this.tagParser = tagParser;
        }

        public Result<Bitmap> CreateAndSaveCloudFromTextFile(string inputPath)
        {
            var result = fileReader.GetWordsFromFile(inputPath, new[] { ' ' })
                .Then(wordPreparator.GetPreparedWords)
                .Then(frequencyCounter.GetFrequencyDictionary)
                .Then(tagParser.ParseTags)
                .Then(imageGenerator.DrawTagCloudBitmap);

            return result;
        }

        public static Assembly GetAssemblyInfo()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}