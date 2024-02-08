using StopWord;
using TagsCloudContainer.Infrastucture;

namespace TagsCloudContainer.Algorithm
{
    public class WordProcessor : IWordProcessor
    {

        private readonly IFileParser parser;

        public WordProcessor(IFileParser parser)
        {
            this.parser = parser;
        }

        public Result<Dictionary<string, int>> CalculateFrequencyInterestingWords(string sourceFilePath, string boringFilePath)
        {
            var wordFrequencies = new Dictionary<string, int>();
            var interestingWords = GetInterestingWords(sourceFilePath, boringFilePath);

            if (!interestingWords.IsSuccess)
                return Result.Fail<Dictionary<string, int>>(interestingWords.Error);

            wordFrequencies = interestingWords.Value.GroupBy(word => word).ToDictionary(group => group.Key, group => group.Count());

            return Result.Ok(wordFrequencies);
        }

        public Result<List<string>> GetInterestingWords(string sourceFilePath, string boringFilePath)
        {
            var boringWords = parser.ReadWordsInFile(boringFilePath).RefineError("The source of \"boring\" words");
            var sourceWords = parser.ReadWordsInFile(sourceFilePath).RefineError("The source of the words for the tag cloud");
            var stopWords = StopWords.GetStopWords("ru");

            if (!boringWords.IsSuccess || !sourceWords.IsSuccess)
                return Result.Fail<List<string>>(boringWords.Error + '\n' + sourceWords.Error);

            var interestingWords = sourceWords.Value.Where(word => !boringWords.Value.Contains(word) 
                && !stopWords.Contains(word)).ToList();

            return Result.Ok(interestingWords);
        }
    }
}
