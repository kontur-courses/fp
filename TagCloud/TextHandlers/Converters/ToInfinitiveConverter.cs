using System.Linq;
using NHunspell;

namespace TagCloud.TextHandlers.Converters;

public class ToInfinitiveConverter : IConverter
{
    private readonly Hunspell hunspell = new("ru_ru.aff", "ru_ru.dic");

    public Result<string> Convert(string original)
    {
        return original.AsResult()
            .Then(w => hunspell.Stem(w).FirstOrDefault() ?? w)
            .RefineError("Hunspell error");
    }
}