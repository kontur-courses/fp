﻿using System.Text.RegularExpressions;
using MoreLinq.Extensions;
using ResultOfTask;

namespace TagCloudResult.Words;

public class TextFormatter
{
    public Result<IList<Word>> Format(string text)
    {
        return Result.Ok(text)
            .Then(RemovePunctuations)
            .Then(ConvertToLowerCase)
            .Then(GetAllWordsFromText)
            .Then(RemoveEmptyWords)
            .Then(StripAllWords)
            .RefineError("Cannot format given text");
    }

    private string RemovePunctuations(string text)
    {
        return Regex.Replace(text, "\\p{P}", string.Empty);
    }

    private string ConvertToLowerCase(string text)
    {
        return text.ToLower();
    }

    private IList<Word> GetAllWordsFromText(string text)
    {
        var allWords = text.Split(Environment.NewLine);

        var words = allWords
            .CountBy(word => word)
            .Select(wordsWithAmount =>
                new Word(wordsWithAmount.Key, (float)wordsWithAmount.Value / allWords.Length))
            .ToList();
        return words;
    }

    private IList<Word> RemoveEmptyWords(IList<Word> words)
    {
        return words.Where(word => string.IsNullOrWhiteSpace(word.Value) == false).ToList();
    }

    private IList<Word> StripAllWords(IList<Word> words)
    {
        foreach (var word in words)
            word.Value = word.Value.Trim();
        return words;
    }
}