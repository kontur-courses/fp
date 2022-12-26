using TagCloudContainer.Core.Interfaces;
using TagCloudContainer.Core.Models;
using TagCloudContainer.Core.Utils;

namespace TagCloudContainer.Core;

public class TagCloudProvider : ITagCloudProvider
{
    private readonly ITagCloudPlacer _tagCloudPlacer;
    private readonly IEnumerable<Word> _words;
    private readonly ISelectedValues _selectedValues;

    public TagCloudProvider(
        ITagCloudPlacer tagCloudPlacer, 
        IWordsReader wordsReader, 
        ITagCloudContainerConfig tagCloudContainerConfig,
        ISelectedValues selectValues)
    {
        _tagCloudPlacer = tagCloudPlacer;
        _selectedValues = selectValues;
        _words = wordsReader.GetWordsFromFile(tagCloudContainerConfig.WordsFilePath);
    }

    public Result<List<Word>> GetPreparedWords()
    {
        var result = (new List<Word>()).AsResult();
        var arrangedWords = WordsArranger.ArrangeWords(_words.ToList(), _selectedValues.PlaceWordsRandomly);

        if (!arrangedWords.IsSuccess)
            return Result.Fail<List<Word>>(arrangedWords.Error);

        foreach (var word in arrangedWords.GetValueOrThrow())
        {
            var wordResult = _tagCloudPlacer.PlaceInCloud(word);
            if (!wordResult.IsSuccess)
                return Result.Fail<List<Word>>(wordResult.Error);
            result.Value.Add(wordResult.Value);
        }

        return result;
    }
}