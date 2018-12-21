using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudResult;

namespace TagsCloudContainer.WordsFilter
{
    public static class FilteredWordsExtensions
    {

        public static Dictionary<string, int> RemoveWord(this Dictionary<string, int> words, string boringWord)
        {
            return words
                .Where(word => word.Key != boringWord.ToLower())
                .ToDictionary(word => word.Key, word => word.Value);
        }

        public static Dictionary<string, int> RemoveWords(this Dictionary<string, int> words, List<string> boringWords)
        {
            var lowerBoringWords = boringWords.Select(word => word.ToLower());

            return words
                .Where(word => !lowerBoringWords.Contains(word.Key))
                .ToDictionary(word => word.Key, word => word.Value);
        }


        public static Result<Dictionary<string, int>> RemoveWordsOutOfLengthRange(this Dictionary<string, int> words,
            int leftBound, int rightBound = int.MaxValue)
        {
            if (leftBound > rightBound || Math.Min(leftBound, rightBound) < 0)
                return Result.Fail<Dictionary<string, int>>(
                    $"Right bound should be greater than left and both are greater than zero.");

            return words
                .Where(word => word.Key.Length >= leftBound && word.Key.Length <= rightBound)
                .ToDictionary(word => word.Key, word => word.Value);
        }
    }
}
