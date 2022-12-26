using System;

namespace TagsCloudVisualization.WordProcessors.WordProcessingSettings
{
    public class ProcessingSettings : IProcessingSettings
    {
        private const int defaultMinWordLength = 3;
        private const int defaultMaxWordLength = 30;
        public int MinWordLength { get; set; }
        public int MaxWordLength { get; set; }
        public string[] ExcludedWords { get; set; }

        public ProcessingSettings(
            string excludedWords,
            int minLength = defaultMinWordLength,
            int maxLength = defaultMaxWordLength)
        {

            ExcludedWords = ParseWord(excludedWords).Value;
            MinWordLength = minLength;
            MaxWordLength = maxLength;
        }

        private Result<string[]> ParseWord(string words)
        {
            return Result.Of(() => words.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
