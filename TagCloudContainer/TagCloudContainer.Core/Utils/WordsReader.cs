using TagCloudContainer.Core.Interfaces;
using TagCloudContainer.Core.Models;

namespace TagCloudContainer;

public class WordsReader : IWordsReader
{
    private readonly Dictionary<string, Word> _words;
    private readonly ILinesValidator _linesValidator;

    public WordsReader(ILinesValidator linesValidator)
    {
        _linesValidator = linesValidator;
        _words = new Dictionary<string, Word>();
    }
    
    public IEnumerable<Word> GetWordsFromFile(string filePath)
    {
        Read(filePath);
        return _words.Values.ToList().OrderByDescending(w => w.Weight);
    }

    private void Read(string filePath)
    {
        _linesValidator
            .Validate(File.ReadLines(filePath).Distinct())
            .ToList()
            .ForEach(AddWord);
    }

    private void AddWord(string wordValue)
    {
        if (_words.ContainsKey(wordValue))
        {
            _words[wordValue].Weight++;
            return;
        }
        
        var word = new Word() { Value = wordValue, Weight = 1 };
        _words.Add(wordValue, word);
    }
}