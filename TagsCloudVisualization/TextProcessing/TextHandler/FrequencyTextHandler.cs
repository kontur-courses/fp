using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Words;
using TagsCloudVisualization.WordsProcessing.WordsFilters;
using TagsCloudVisualization.WordsProcessing.WordsWeighers;

namespace TagsCloudVisualization.TextProcessing.TextHandler
{
    public class FrequencyTextHandler : ITextHandler
    {
        private static readonly char[] Separators =
        {
            ' ', ',', '.', ';', ':',
            '\n', '\t', '\r',
            '[', ']',
            '<', '>',
            '{', '}',
            '"', '\'',
            '(', ')',
            '+', '-', '=', '/', '*', '\\',
            '!', '?', '#', '$', '|', '_', '&', '^', '%',
            '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'
        };

        private readonly IEnumerable<IWordFilter> wordFilters;
        private readonly IWordsWeigher weigher;

        public FrequencyTextHandler(IEnumerable<IWordFilter> wordFilters, IWordsWeigher weigher)
        {
            this.wordFilters = wordFilters;
            this.weigher = weigher;
        }

        public Result<IEnumerable<Word>> GetHandledWords(string text)
        {
            return Result
                .Of(() => text
                    .Split(Separators, StringSplitOptions.RemoveEmptyEntries)
                    .Select(word => word.ToLower()))
                .Then(textWords => wordFilters
                    .Aggregate(textWords, (current, filter) => filter.FilterWords(current)))
                .Then(filteredWords => weigher.WeighWords(filteredWords));
        }
    }
}