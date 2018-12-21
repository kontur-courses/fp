using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudBuilder;
using TagsCloudContainer.WordsFilter.BoringWords;
using TagsCloudResult;

namespace TagsCloudContainer.WordsFilter
{
    public class FilteredWords : IFilteredWords
    {
        public Dictionary<string, int> FilteredWordsList => new Dictionary<string, int>(words);
        private readonly int leftBound;
        private readonly int rightBound;
        private HashSet<string> boringWords;
        private Dictionary<string, int> words;

        public FilteredWords(IBoringWords boringWords, IWordsPreparer inputWords,
            int leftBound = 5, int rightBound = int.MaxValue)
        {
            var boringWordsResult = boringWords.GetBoringWords;
            if (!boringWordsResult.IsSuccess)
                throw new ArgumentException(boringWordsResult.Error);

            this.boringWords = boringWords.GetBoringWords
                .GetValueOrThrow();
            var prepareWordsResult = inputWords.GetPreparedWords();
            if (!prepareWordsResult.IsSuccess)
                throw new ArgumentException(prepareWordsResult.Error);

            words = prepareWordsResult.GetValueOrThrow();
            this.leftBound = leftBound;
            this.rightBound = rightBound;

            RemoveBoringWords();
            var removeWordsResult = RemoveWordsOutOfLengthRange();
            if (!removeWordsResult.IsSuccess)
                throw new ArgumentException(removeWordsResult.Error);
        }

        private void RemoveBoringWords()
        {
            var lowerBoringWords = boringWords.Select(word => word.ToLower());

            words = words
                .Where(word => !lowerBoringWords.Contains(word.Key))
                .ToDictionary(word => word.Key, word => word.Value);
        }

        private Result<None> RemoveWordsOutOfLengthRange()
        {
            if (leftBound > rightBound || Math.Min(leftBound, rightBound) < 0)
                return Result.Fail<None>(
                    $"Right bound should be greater than left and both are greater than zero.");

            words = words
                .Where(word => word.Key.Length >= leftBound && word.Key.Length <= rightBound)
                .ToDictionary(word => word.Key, word => word.Value);
            return Result.Ok();
        }
    }
}
