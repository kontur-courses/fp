using System.Drawing;
using TagsCloudVisualization;

namespace TagCloud;

public class WordsForCloudGenerator: IWordsForCloudGenerator
{
    private readonly string fontName;
        private readonly int maxFontSize;
        private readonly ITagCloudLayouter tagCloudLayouter;
        private readonly IColorGenerator colorGenerator;

        public WordsForCloudGenerator(string fontName, int maxFontSize, ITagCloudLayouter tagCloudLayouter,
                                      IColorGenerator colorGenerator)
        {
            this.tagCloudLayouter = tagCloudLayouter;
            this.fontName = fontName;
            this.maxFontSize = maxFontSize;
            this.colorGenerator = colorGenerator;
        }

        public Result<IEnumerable<WordForCloud>> Generate(Result<List<string>> wordsResult)
        {
            return wordsResult.Then((words) => GetWordsFrequency(words)
                    .OrderByDescending(x => x.Value)
                    .ToList())
                .Then(frequency => {
                    var maxFrequency = frequency.FirstOrDefault().Value;
                    return frequency.Select(x =>
                        GetWordForCloud(fontName,
                            maxFontSize,
                            colorGenerator.GetNextColor(),
                            x.Key,
                            x.Value,
                            maxFrequency));
                });
        }

        private static Dictionary<string, int> GetWordsFrequency(List<string> words) =>
            words.GroupBy(x => x)
                 .ToDictionary(group => group.Key,
                               group => group.Count());

        private WordForCloud GetWordForCloud(string font, int maxWordSize, Color color, string word,
                                             int wordFrequency, int maxFrequency)
        {
            var wordFontSize = (int) Math.Round(maxWordSize * (double) wordFrequency / maxFrequency);
            var wordSize = new Size((int) (word.Length * (wordFontSize * 0.8)), wordFontSize + 12);

            return new WordForCloud(font, wordFontSize, word, tagCloudLayouter.PutNextRectangle(wordSize), color);
        }
}