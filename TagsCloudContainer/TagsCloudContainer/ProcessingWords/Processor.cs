using CloudDrawing;
using ResultOf;
using TagsCloudContainer.PreprocessingWords;
using TagsCloudContainer.Reader;

namespace TagsCloudContainer.ProcessingWords
{
    public class Processor : IProcessor
    {
        private readonly IReaderFromFile readerFromFile;
        private readonly IPreprocessingWords preprocessingWords;
        private readonly ICircularCloudDrawing circularCloudDrawing;

        public Processor(
            IReaderFromFile readerFromFile,
            IPreprocessingWords preprocessingWords,
            ICircularCloudDrawing circularCloudDrawing)
        {
            this.readerFromFile = readerFromFile;
            this.preprocessingWords = preprocessingWords;
            this.circularCloudDrawing = circularCloudDrawing;
        }

        public Result<None> Run(string pathToFile, string pathToSaveFile, ImageSettings imageSettings,
            WordDrawSettings wordDrawSettings)
        {
            circularCloudDrawing.SetOptions(imageSettings);

            return pathToFile.AsResult()
                .Then(readerFromFile.GetWordsSet)
                .Then(preprocessingWords.Preprocessing)
                .Then(CountingWords.GetWordAndNumberOfRepetitions)
                .Then(e => circularCloudDrawing.DrawWords(e, wordDrawSettings))
                .Then((_) => circularCloudDrawing.SaveImage(pathToSaveFile));
            ;
        }
    }
}