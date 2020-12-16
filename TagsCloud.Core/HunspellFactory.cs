using NHunspell;
using TagsCloud.ResultPattern;

namespace TagsCloud.Core
{
    public class HunspellFactory
    {
        private readonly Result<Hunspell> hunspell;

        public HunspellFactory(string pathToAffix, string pathToDictionary)
        {
            hunspell = Result.Of(() => new Hunspell(pathToAffix, pathToDictionary));
        }

        public Result<Hunspell> CreateHunspell() => hunspell;
    }
}
