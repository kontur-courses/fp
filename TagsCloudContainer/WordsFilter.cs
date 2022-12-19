using System.Text.RegularExpressions;
using TagsCloudContainer.Interfaces;
using Result;

namespace TagsCloudContainer;

public class WordsFilter : IWordsFilter
{
    public Result<List<string>> FilterWords(List<string> taggedWords, Result<ICustomOptions> options,
        HashSet<string>? boringWords = null)
    {
        //PoS - Part of Speech; grammemes - grammatical number etc, including PoS
        var excludedPoS =
            options.Value.ExcludedParticals.Split(", ", StringSplitOptions.RemoveEmptyEntries);

        var jointPos = string.Join('|', excludedPoS
            .Select(x => "=(,|=)"
                .Insert(1, x))
            .ToArray());
        jointPos = jointPos.Length == 0 ? "= (,|=)" : jointPos;
        var regexString =
            "^(\\w+){((?!).)*$".Insert(11, jointPos);
        // something like that ^(\w+){((?!=SPRO(,|=)|=PR(,|=)|=PART(,|=)|=CONJ(,|=)).)*$
        var regex = new Regex(regexString);

        var inputWords = taggedWords
            .Where(x => regex.IsMatch(x))
            .Select(x =>
            {
                var match = regex.Match(x);
                return match.Groups[1].Value;
            }).ToList();
        return boringWords is null
            ? new Result<List<string>>(inputWords)
            : new Result<List<string>>(inputWords.Where(x => !boringWords.Contains(x)).ToList());
    }
}