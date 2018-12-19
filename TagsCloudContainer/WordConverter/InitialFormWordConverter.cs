using System.IO;
using NHunspell;

namespace TagsCloudContainer.WordConverter
{
    public class InitialFormWordConverter : IWordConverter
    {
        public Result<string> Convert(string word)
        {
            var ruAff = "../../ru_RU.aff";
            var ruDic = "../../ru_RU.dic";
            if (!File.Exists(ruAff) || !File.Exists(ruDic))
                return Result.Fail<string>($"file not found: {ruAff} or {ruDic}");

            using (var hunspell = new Hunspell(ruAff, ruDic))
            {
                var stems = hunspell.Stem(word);
                return stems.Count > 0 ? stems[0] : word;
            }
        }
    }
}