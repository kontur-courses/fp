using Mono.Options;
using ResultExtensions;
using ResultOf;
using TagsCloudContainer.Abstractions;
using TagsCloudContainer.Defaults.MyStem;

namespace TagsCloudContainer.Defaults.SettingsProviders;

public class SpeechPartFilterSettings : ICliSettingsProvider
{
    private static readonly HashSet<SpeechPart> defaultFilter = new()
    {
        SpeechPart.CONJ,
        SpeechPart.INTJ,
        SpeechPart.PART,
        SpeechPart.PR,
    };

    private readonly HashSet<SpeechPart> toFilterOut = defaultFilter.ToHashSet();

    public IReadOnlySet<SpeechPart> ToFilterOut => toFilterOut;

    public Result State { get; private set; } = Result.Ok();

    public OptionSet GetCliOptions()
    {
        var options = new OptionSet()
        {
            { "add-parts=", $"Add speech parts to exclusion filter. Defaults to {string.Join(", ", toFilterOut)}", v => State = AddParts(v) },
            { "remove-parts=", $"Remove speech parts from exclusion filter. Defaults to {string.Join(", ", toFilterOut)}", v => State = RemoveParts(v) }
        };

        return options;
    }

    private Result AddParts(string v)
    {
        foreach (var part in v.Split())
        {
            var result = ParsePart(part).Then(p => toFilterOut.Add(p));
            if (!result.IsSuccess)
                return result;
        }

        return Result.Ok();
    }

    private Result RemoveParts(string v)
    {
        foreach (var part in v.Split())
        {
            var result = ParsePart(part).Then(p => toFilterOut.Remove(p));
            if (!result.IsSuccess)
                return result;
        }

        return Result.Ok();
    }

    private static Result<SpeechPart> ParsePart(string part)
    {
        return Enum.TryParse<SpeechPart>(part, true, out var sp) ? sp : Result.Fail<SpeechPart>($"Could not parse {part} as {nameof(SpeechPart)}");
    }
}
