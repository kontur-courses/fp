using System.Text;
using TagCloudContainer.Core.Interfaces;

namespace TagCloudContainer.Core.Utils;

public class LinesValidator : ILinesValidator
{
    private StringBuilder _word;
    private HashSet<string> _boringWords;
    private readonly Dictionary<bool, Func<IEnumerable<string>, IEnumerable<string>>> _validators;

    private readonly ITagCloudContainerConfig _tagCloudContainerConfig;

    public string Result
    {
        get => _word.ToString();
    }

    public LinesValidator(ITagCloudContainerConfig tagCloudContainerConfig)
    {
        _tagCloudContainerConfig = tagCloudContainerConfig;
        _validators = new Dictionary<bool, Func<IEnumerable<string>, IEnumerable<string>>>()
        {
            { true, ReturnLinesAfterValidation },
            { false, ReturnLinesWithoutValidation }
        };
    }

    public IEnumerable<string> Validate(IEnumerable<string> lines) 
        => _validators[_tagCloudContainerConfig.NeedValidateWords](lines);

    private IEnumerable<string> ReturnLinesAfterValidation(IEnumerable<string> lines)
    {
        var filePath = _tagCloudContainerConfig.ExcludeWordsFilePath;
        _boringWords = File
            .ReadLines(filePath)
            .Distinct()
            .ToHashSet();

        return lines
            .Select(ValidateWord)
            .Where(line => !string.IsNullOrWhiteSpace(line));
    }

    private IEnumerable<string> ReturnLinesWithoutValidation(IEnumerable<string> lines) => lines;

    private string ValidateWord(string word)
    {
        if (string.IsNullOrEmpty(word))
            return "";
        
        return ForWord(word)
            .RemovePunctuationMarks()
            .RemoveIfItBoring()
            .Result;
    }

    private LinesValidator ForWord(string word)
    {
        _word = new StringBuilder(word.ToLower());
        return this;
    }

    private LinesValidator RemoveIfItBoring()
    {
        if (IsBoring())
            _word.Clear();
        
        return this;
    }

    private LinesValidator RemovePunctuationMarks()
    {
        for (var i = 0; i < _word.Length; i++)
            if (char.IsPunctuation(_word[i]))
            {
                _word.Remove(i, 1);
                i--;
            }

        return this;
    }
    
    private bool IsBoring() => _boringWords.Contains(_word.ToString());
}
