using System.Linq;
using TagsCloud.Visualization.Models;
using TagsCloud.Visualization.WordsParser;
using TagsCloud.Visualization.WordsReaders;

namespace TagsCloud.Visualization
{
    public class WordsService : IWordsService
    {
        private readonly IWordsParser wordsParser;

        public WordsService(IWordsParser wordsParser) => this.wordsParser = wordsParser;

        public Word[] GetWords(IWordsReadService wordsReadService)
        {
            var text = wordsReadService.Read();

            var parsed = wordsParser.CountWordsFrequency(text);

            return parsed
                .Where(x => x.Value > 1)
                .OrderByDescending(x => x.Value)
                .Select(x => new Word(x.Key, x.Value))
                .ToArray();
        }
    }
}