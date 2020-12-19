using NHunspell;
using TagsCloud.ResultPattern;

namespace TagsCloud.Core
{
    public class HunspellFactory
    {
        public Result<Hunspell> CreateHunspell(PathSettings settings) 
            => Result.Of(() => new Hunspell(settings.PathToAffix, settings.PathToDictionary));
    }
}