using System.Text.RegularExpressions;
using TagsCloudContainer.Interfaces;
using ResultOfTask;

namespace TagsCloudContainer;

public class WordsFilter : IWordsFilter
{
    private const string template = "= (,|=)";
    public Result<List<string>> FilterWords(List<string> taggedWords, ICustomOptions options,
        HashSet<string>? boringWords = null)
    {
        var excludedPoS =
            options.ExcludedParticals.Split(", ", StringSplitOptions.RemoveEmptyEntries);

        var jointPos = string.Join('|', excludedPoS
            .Select(x => template.Replace(" ", x))
            .ToArray());

        var innerExpression = jointPos.Length == 0 ? template : jointPos;

        var regexString =
            $"^(\\w+){{((?!{innerExpression}).)*$";
        // something like that ^(\w+){((?!=SPRO(,|=)|=PR(,|=)|=PART(,|=)|=CONJ(,|=)).)*$
        var regex = new Regex(regexString);

        var inputWords = taggedWords
            .Where(x => regex.IsMatch(x))
            .Select(x =>
            {
                var match = regex.Match(x);
                return match.Groups[1].Value;
            }).ToList();
        inputWords = boringWords is null ? inputWords : inputWords.Where(x => !boringWords.Contains(x)).ToList();
        return inputWords.AsResult();
    }
}